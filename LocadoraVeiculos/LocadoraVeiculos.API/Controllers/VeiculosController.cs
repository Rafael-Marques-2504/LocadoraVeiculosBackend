using LocadoraVeiculos.API.Data;
using LocadoraVeiculos.API.Dtos;
using LocadoraVeiculos.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculosController : ControllerBase
{
    private readonly LocadoraContext _context;
    public VeiculosController(LocadoraContext context) => _context = context;

    // =====================================================================
    // CRUD
    // =====================================================================

    /// <summary>Lista todos os veículos, incluindo os dados do fabricante.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Veiculo>>> GetAll()
    {
        return Ok(await _context.Veiculos
            .Include(v => v.Fabricante)
            .AsNoTracking()
            .ToListAsync());
    }

    /// <summary>Busca um veículo pelo Id.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Veiculo>> GetById(int id)
    {
        var veiculo = await _context.Veiculos
            .Include(v => v.Fabricante)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (veiculo == null)
            return NotFound(new { mensagem = $"Veículo {id} não encontrado." });

        return Ok(veiculo);
    }

    /// <summary>Cadastra um novo veículo.</summary>
    [HttpPost]
    public async Task<ActionResult<Veiculo>> Create(VeiculoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var fabricanteExiste = await _context.Fabricantes.AnyAsync(f => f.Id == dto.FabricanteId);
        if (!fabricanteExiste)
            return BadRequest(new { mensagem = "Fabricante informado não existe." });

        var placaEmUso = await _context.Veiculos.AnyAsync(v => v.Placa == dto.Placa);
        if (placaEmUso)
            return BadRequest(new { mensagem = "Já existe um veículo cadastrado com esta placa." });

        var veiculo = new Veiculo
        {
            Modelo = dto.Modelo,
            Placa = dto.Placa,
            AnoFabricacao = dto.AnoFabricacao,
            Quilometragem = dto.Quilometragem,
            FabricanteId = dto.FabricanteId
        };

        _context.Veiculos.Add(veiculo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = veiculo.Id }, veiculo);
    }

    /// <summary>Atualiza um veículo existente.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, VeiculoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var veiculo = await _context.Veiculos.FindAsync(id);
        if (veiculo == null)
            return NotFound(new { mensagem = $"Veículo {id} não encontrado." });

        var fabricanteExiste = await _context.Fabricantes.AnyAsync(f => f.Id == dto.FabricanteId);
        if (!fabricanteExiste)
            return BadRequest(new { mensagem = "Fabricante informado não existe." });

        veiculo.Modelo = dto.Modelo;
        veiculo.Placa = dto.Placa;
        veiculo.AnoFabricacao = dto.AnoFabricacao;
        veiculo.Quilometragem = dto.Quilometragem;
        veiculo.FabricanteId = dto.FabricanteId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Remove um veículo (somente se não possuir aluguéis vinculados).</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);
        if (veiculo == null)
            return NotFound(new { mensagem = $"Veículo {id} não encontrado." });

        var possuiAlugueis = await _context.Alugueis.AnyAsync(a => a.VeiculoId == id);
        if (possuiAlugueis)
            return BadRequest(new { mensagem = "Não é possível excluir: existem aluguéis vinculados a este veículo." });

        _context.Veiculos.Remove(veiculo);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // =====================================================================
    // FILTROS (rotas de consulta com JOIN entre tabelas)
    // =====================================================================

    /// <summary>
    /// FILTRO 1 — INNER JOIN entre Veiculo e Fabricante.
    /// Lista todos os veículos de um determinado fabricante.
    /// </summary>
    [HttpGet("por-fabricante/{fabricanteId:int}")]
    public async Task<IActionResult> GetPorFabricante(int fabricanteId)
    {
        var resultado = await (
            from v in _context.Veiculos
            join f in _context.Fabricantes on v.FabricanteId equals f.Id
            where f.Id == fabricanteId
            select new
            {
                v.Id,
                v.Modelo,
                v.Placa,
                v.AnoFabricacao,
                v.Quilometragem,
                Fabricante = f.Nome
            }
        ).ToListAsync();

        return Ok(resultado);
    }

    /// <summary>
    /// FILTRO 2 — LEFT JOIN (GroupJoin + DefaultIfEmpty) entre Veiculo e Aluguel.
    /// Lista os veículos que estão disponíveis, ou seja, sem nenhum aluguel em aberto.
    /// </summary>
    [HttpGet("disponiveis")]
    public async Task<IActionResult> GetDisponiveis()
    {
        var resultado = await (
            from v in _context.Veiculos
            join a in _context.Alugueis.Where(a => a.DataDevolucao == null)
                on v.Id equals a.VeiculoId into alugueisAbertos
            from a in alugueisAbertos.DefaultIfEmpty() // LEFT JOIN
            where a == null
            select new
            {
                v.Id,
                v.Modelo,
                v.Placa,
                v.Quilometragem,
                Fabricante = v.Fabricante!.Nome
            }
        ).ToListAsync();

        return Ok(resultado);
    }

    /// <summary>
    /// FILTRO 3 — LEFT JOIN (GroupJoin) + agregação.
    /// Retorna o ranking de veículos por número de aluguéis realizados.
    /// </summary>
    [HttpGet("mais-alugados")]
    public async Task<IActionResult> GetMaisAlugados()
    {
        var resultado = await (
            from v in _context.Veiculos
            join a in _context.Alugueis on v.Id equals a.VeiculoId into alugueisDoVeiculo
            from a in alugueisDoVeiculo.DefaultIfEmpty() // LEFT JOIN
            group a by new { v.Id, v.Modelo, v.Placa } into g
            select new
            {
                g.Key.Id,
                g.Key.Modelo,
                g.Key.Placa,
                TotalAlugueis = g.Count(a => a != null)
            }
        ).OrderByDescending(x => x.TotalAlugueis).ToListAsync();

        return Ok(resultado);
    }
}
