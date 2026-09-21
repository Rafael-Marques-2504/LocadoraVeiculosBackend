using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.API.Models;

/// <summary>
/// Representa um veículo da frota da locadora.
/// </summary>
public class Veiculo
{
    public int Id { get; set; }

    [Required, MaxLength(60)]
    public string Modelo { get; set; } = string.Empty;

    [Required, MaxLength(10)]
    public string Placa { get; set; } = string.Empty;

    [Range(1950, 2100)]
    public int AnoFabricacao { get; set; }

    [Range(0, int.MaxValue)]
    public int Quilometragem { get; set; }

    // Chave estrangeira -> todo veículo pertence a um fabricante
    [Required]
    public int FabricanteId { get; set; }
    public Fabricante? Fabricante { get; set; }

    public ICollection<Aluguel> Alugueis { get; set; } = new List<Aluguel>();
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
