using LocadoraVeiculos.API.Data;
using LocadoraVeiculos.API.Dtos;
using LocadoraVeiculos.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FabricantesController : ControllerBase
{
    private readonly LocadoraContext _context;
    public FabricantesController(LocadoraContext context) => _context = context;

    /// <summary>Lista todos os fabricantes cadastrados.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Fabricante>>> GetAll()
    {
        return Ok(await _context.Fabricantes.AsNoTracking().ToListAsync());
    }

    /// <summary>Busca um fabricante pelo Id.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Fabricante>> GetById(int id)
    {
        var fabricante = await _context.Fabricantes.FindAsync(id);
        if (fabricante == null)
            return NotFound(new { mensagem = $"Fabricante {id} não encontrado." });

        return Ok(fabricante);
    }

    /// <summary>Cadastra um novo fabricante.</summary>
    [HttpPost]
    public async Task<ActionResult<Fabricante>> Create(FabricanteDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var fabricante = new Fabricante { Nome = dto.Nome, PaisOrigem = dto.PaisOrigem };
        _context.Fabricantes.Add(fabricante);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = fabricante.Id }, fabricante);
    }

    /// <summary>Atualiza um fabricante existente.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, FabricanteDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var fabricante = await _context.Fabricantes.FindAsync(id);
        if (fabricante == null)
            return NotFound(new { mensagem = $"Fabricante {id} não encontrado." });

        fabricante.Nome = dto.Nome;
        fabricante.PaisOrigem = dto.PaisOrigem;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Remove um fabricante (somente se não possuir veículos vinculados).</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var fabricante = await _context.Fabricantes.FindAsync(id);
        if (fabricante == null)
            return NotFound(new { mensagem = $"Fabricante {id} não encontrado." });

        var possuiVeiculos = await _context.Veiculos.AnyAsync(v => v.FabricanteId == id);
        if (possuiVeiculos)
            return BadRequest(new { mensagem = "Não é possível excluir: existem veículos vinculados a este fabricante." });

        _context.Fabricantes.Remove(fabricante);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
