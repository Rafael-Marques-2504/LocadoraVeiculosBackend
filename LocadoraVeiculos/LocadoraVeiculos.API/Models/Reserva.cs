using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.API.Models;

/// <summary>
/// Entidade adicional (5ª entidade do sistema).
/// Representa a reserva antecipada de um veículo por um cliente, antes da
/// efetivação do aluguel.
/// </summary>
public class Reserva
{
    public int Id { get; set; }

    [Required]
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    [Required]
    public int VeiculoId { get; set; }
    public Veiculo? Veiculo { get; set; }

    [Required]
    public DateTime DataReserva { get; set; } = DateTime.Now;

    [Required]
    public DateTime DataInicioPrevista { get; set; }

    [Required]
    public DateTime DataFimPrevista { get; set; }

    // Pendente | Confirmada | Cancelada
    [Required, MaxLength(20)]
    public string Status { get; set; } = "Pendente";
}
