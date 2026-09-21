using LocadoraVeiculos.API.Data;
using LocadoraVeiculos.API.Dtos;
using LocadoraVeiculos.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservasController : ControllerBase
{
    private readonly LocadoraContext _context;
    public ReservasController(LocadoraContext context) => _context = context;

    /// <summary>Lista todas as reservas, com dados de cliente e veículo.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reserva>>> GetAll()
    {
        return Ok(await _context.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Veiculo)
            .AsNoTracking()
            .ToListAsync());
    }

    /// <summary>Busca uma reserva pelo Id.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Reserva>> GetById(int id)
    {
        var reserva = await _context.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Veiculo)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reserva == null)
            return NotFound(new { mensagem = $"Reserva {id} não encontrada." });

        return Ok(reserva);
    }

    /// <summary>Cria uma nova reserva de veículo.</summary>
    [HttpPost]
    public async Task<ActionResult<Reserva>> Create(ReservaDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (dto.DataFimPrevista <= dto.DataInicioPrevista)
            return BadRequest(new { mensagem = "A data de fim prevista deve ser posterior à data de início prevista." });

        var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
        if (!clienteExiste) return BadRequest(new { mensagem = "Cliente informado não existe." });

        var veiculoExiste = await _context.Veiculos.AnyAsync(v => v.Id == dto.VeiculoId);
        if (!veiculoExiste) return BadRequest(new { mensagem = "Veículo informado não existe." });

        var reserva = new Reserva
        {
            ClienteId = dto.ClienteId,
            VeiculoId = dto.VeiculoId,
            DataInicioPrevista = dto.DataInicioPrevista,
            DataFimPrevista = dto.DataFimPrevista,
            Status = string.IsNullOrWhiteSpace(dto.Status) ? "Pendente" : dto.Status,
            DataReserva = DateTime.Now
        };

        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = reserva.Id }, reserva);
    }

    /// <summary>Atualiza uma reserva existente (datas e status).</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ReservaDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null)
            return NotFound(new { mensagem = $"Reserva {id} não encontrada." });

        reserva.DataInicioPrevista = dto.DataInicioPrevista;
        reserva.DataFimPrevista = dto.DataFimPrevista;
        reserva.Status = dto.Status;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Remove (cancela) uma reserva.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null)
            return NotFound(new { mensagem = $"Reserva {id} não encontrada." });

        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
