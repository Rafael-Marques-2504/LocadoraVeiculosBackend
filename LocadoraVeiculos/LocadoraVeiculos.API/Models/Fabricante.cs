using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.API.Models;

/// <summary>
/// Representa o fabricante (marca) de um veículo. Ex: Volkswagen, Fiat, Toyota.
/// </summary>
public class Fabricante
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(60)]
    public string? PaisOrigem { get; set; }

    // Relacionamento 1:N -> um fabricante possui vários veículos
    public ICollection<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
}
