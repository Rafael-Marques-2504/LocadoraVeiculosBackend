using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.API.Models;

/// <summary>
/// Representa um cliente da locadora.
/// </summary>
public class Cliente
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(14)]
    public string CPF { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(120)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefone { get; set; }

    public ICollection<Aluguel> Alugueis { get; set; } = new List<Aluguel>();
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
