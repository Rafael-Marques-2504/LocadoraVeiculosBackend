using LocadoraVeiculos.API.Data;
using LocadoraVeiculos.API.Dtos;
using LocadoraVeiculos.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly LocadoraContext _context;
    public ClientesController(LocadoraContext context) => _context = context;

    /// <summary>Lista todos os clientes cadastrados.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetAll()
    {
        return Ok(await _context.Clientes.AsNoTracking().ToListAsync());
    }

    /// <summary>Busca um cliente pelo Id.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Cliente>> GetById(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null)
            return NotFound(new { mensagem = $"Cliente {id} não encontrado." });

        return Ok(cliente);
    }

    /// <summary>Cadastra um novo cliente.</summary>
    [HttpPost]
    public async Task<ActionResult<Cliente>> Create(ClienteDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var cpfExiste = await _context.Clientes.AnyAsync(c => c.CPF == dto.CPF);
        if (cpfExiste)
            return BadRequest(new { mensagem = "Já existe um cliente cadastrado com este CPF." });

        var emailExiste = await _context.Clientes.AnyAsync(c => c.Email == dto.Email);
        if (emailExiste)
            return BadRequest(new { mensagem = "Já existe um cliente cadastrado com este e-mail." });

        var cliente = new Cliente
        {
            Nome = dto.Nome,
            CPF = dto.CPF,
            Email = dto.Email,
            Telefone = dto.Telefone
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
    }

    /// <summary>Atualiza um cliente existente.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ClienteDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null)
            return NotFound(new { mensagem = $"Cliente {id} não encontrado." });

        var cpfEmUso = await _context.Clientes.AnyAsync(c => c.CPF == dto.CPF && c.Id != id);
        if (cpfEmUso)
            return BadRequest(new { mensagem = "Já existe outro cliente cadastrado com este CPF." });

        cliente.Nome = dto.Nome;
        cliente.CPF = dto.CPF;
        cliente.Email = dto.Email;
        cliente.Telefone = dto.Telefone;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Remove um cliente (somente se não possuir aluguéis ou reservas vinculados).</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null)
            return NotFound(new { mensagem = $"Cliente {id} não encontrado." });

        var possuiVinculo = await _context.Alugueis.AnyAsync(a => a.ClienteId == id)
            || await _context.Reservas.AnyAsync(r => r.ClienteId == id);

        if (possuiVinculo)
            return BadRequest(new { mensagem = "Não é possível excluir: existem aluguéis ou reservas vinculados a este cliente." });

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
