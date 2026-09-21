using LocadoraVeiculos.API.Data;
using LocadoraVeiculos.API.Dtos;
using LocadoraVeiculos.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlugueisController : ControllerBase
{
    private readonly LocadoraContext _context;
    public AlugueisController(LocadoraContext context) => _context = context;

    // =====================================================================
    // CRUD
    // =====================================================================

    /// <summary>Lista todos os aluguéis, incluindo cliente e veículo.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Aluguel>>> GetAll()
    {
        return Ok(await _context.Alugueis
            .Include(a => a.Cliente)
            .Include(a => a.Veiculo)
            .AsNoTracking()
            .ToListAsync());
    }

    /// <summary>Busca um aluguel pelo Id.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Aluguel>> GetById(int id)
    {
        var aluguel = await _context.Alugueis
            .Include(a => a.Cliente)
            .Include(a => a.Veiculo)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (aluguel == null)
            return NotFound(new { mensagem = $"Aluguel {id} não encontrado." });

        return Ok(aluguel);
    }

    /// <summary>Registra um novo aluguel (locação de um veículo por um cliente).</summary>
    [HttpPost]
    public async Task<ActionResult<Aluguel>> Create(AluguelCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (dto.DataFimPrevista <= dto.DataInicio)
            return BadRequest(new { mensagem = "A data de fim prevista deve ser posterior à data de início." });

        var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
        if (!clienteExiste) return BadRequest(new { mensagem = "Cliente informado não existe." });

        var veiculoExiste = await _context.Veiculos.AnyAsync(v => v.Id == dto.VeiculoId);
        if (!veiculoExiste) return BadRequest(new { mensagem = "Veículo informado não existe." });

        var veiculoOcupado = await _context.Alugueis
            .AnyAsync(a => a.VeiculoId == dto.VeiculoId && a.DataDevolucao == null);
        if (veiculoOcupado)
            return BadRequest(new { mensagem = "Este veículo já está em um aluguel em aberto." });

        var aluguel = new Aluguel
        {
            ClienteId = dto.ClienteId,
            VeiculoId = dto.VeiculoId,
            DataInicio = dto.DataInicio,
            DataFimPrevista = dto.DataFimPrevista,
            QuilometragemInicial = dto.QuilometragemInicial,
            ValorDiaria = dto.ValorDiaria
        };

        _context.Alugueis.Add(aluguel);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = aluguel.Id }, aluguel);
    }

    /// <summary>
    /// Registra a devolução de um veículo: grava data de devolução, quilometragem final
    /// e calcula automaticamente o valor total do aluguel (dias x valor da diária).
    /// </summary>
    [HttpPut("{id:int}/devolucao")]
    public async Task<IActionResult> RegistrarDevolucao(int id, AluguelDevolucaoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var aluguel = await _context.Alugueis.FindAsync(id);
        if (aluguel == null)
            return NotFound(new { mensagem = $"Aluguel {id} não encontrado." });

        if (aluguel.DataDevolucao != null)
            return BadRequest(new { mensagem = "Este aluguel já foi finalizado anteriormente." });

        if (dto.QuilometragemFinal < aluguel.QuilometragemInicial)
            return BadRequest(new { mensagem = "A quilometragem final não pode ser menor que a inicial." });

        aluguel.DataDevolucao = dto.DataDevolucao;
        aluguel.QuilometragemFinal = dto.QuilometragemFinal;

        var dias = Math.Max(1, (dto.DataDevolucao.Date - aluguel.DataInicio.Date).Days);
        aluguel.ValorTotal = dias * aluguel.ValorDiaria;

        var veiculo = await _context.Veiculos.FindAsync(aluguel.VeiculoId);
        if (veiculo != null) veiculo.Quilometragem = dto.QuilometragemFinal;

        await _context.SaveChangesAsync();
        return Ok(aluguel);
    }

    /// <summary>Remove um registro de aluguel.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var aluguel = await _context.Alugueis.FindAsync(id);
        if (aluguel == null)
            return NotFound(new { mensagem = $"Aluguel {id} não encontrado." });

        _context.Alugueis.Remove(aluguel);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // =====================================================================
    // FILTROS (rotas de consulta com JOIN entre tabelas)
    // =====================================================================

    /// <summary>
    /// FILTRO 4 — INNER JOIN entre Aluguel, Cliente e Veiculo.
    /// Lista o histórico de aluguéis de um cliente específico.
    /// </summary>
    [HttpGet("por-cliente/{clienteId:int}")]
    public async Task<IActionResult> GetPorCliente(int clienteId)
    {
        var resultado = await (
            from a in _context.Alugueis
            join c in _context.Clientes on a.ClienteId equals c.Id
            join v in _context.Veiculos on a.VeiculoId equals v.Id
            where c.Id == clienteId
            select new
            {
                a.Id,
                Cliente = c.Nome,
                Veiculo = v.Modelo,
                v.Placa,
                a.DataInicio,
                a.DataFimPrevista,
                a.DataDevolucao,
                a.ValorTotal
            }
        ).ToListAsync();

        return Ok(resultado);
    }

    /// <summary>
    /// FILTRO 5 — INNER JOIN entre Aluguel, Cliente e Veiculo, com filtro por período.
    /// Lista todos os aluguéis iniciados dentro de um intervalo de datas.
    /// </summary>
    [HttpGet("periodo")]
    public async Task<IActionResult> GetPorPeriodo([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
    {
        if (fim < inicio)
            return BadRequest(new { mensagem = "A data final deve ser maior ou igual à data inicial." });

        var resultado = await (
            from a in _context.Alugueis
            join c in _context.Clientes on a.ClienteId equals c.Id
            join v in _context.Veiculos on a.VeiculoId equals v.Id
            where a.DataInicio >= inicio && a.DataInicio <= fim
            select new
            {
                a.Id,
                Cliente = c.Nome,
                Veiculo = v.Modelo,
                a.DataInicio,
                a.DataFimPrevista,
                a.ValorTotal
            }
        ).ToListAsync();

        return Ok(resultado);
    }
}
