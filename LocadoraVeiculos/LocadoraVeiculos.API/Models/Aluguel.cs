using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.API.Models;

/// <summary>
/// Representa a locação de um veículo por um cliente em um período.
/// </summary>
public class Aluguel
{
    public int Id { get; set; }

    [Required]
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    [Required]
    public int VeiculoId { get; set; }
    public Veiculo? Veiculo { get; set; }

    [Required]
    public DateTime DataInicio { get; set; }

    [Required]
    public DateTime DataFimPrevista { get; set; }

    // Nulo enquanto o veículo não for devolvido
    public DateTime? DataDevolucao { get; set; }

    [Range(0, int.MaxValue)]
    public int QuilometragemInicial { get; set; }

    // Preenchida somente na devolução
    public int? QuilometragemFinal { get; set; }

    [Range(0, double.MaxValue)]
    public decimal ValorDiaria { get; set; }

    // Calculado automaticamente na devolução (dias x valor da diária)
    public decimal? ValorTotal { get; set; }
}
