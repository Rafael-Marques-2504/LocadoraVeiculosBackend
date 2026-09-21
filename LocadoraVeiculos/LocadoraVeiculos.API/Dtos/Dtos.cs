using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.API.Dtos;

public class FabricanteDto
{
    [Required, MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(60)]
    public string? PaisOrigem { get; set; }
}

public class VeiculoDto
{
    [Required, MaxLength(60)]
    public string Modelo { get; set; } = string.Empty;

    [Required, MaxLength(10)]
    public string Placa { get; set; } = string.Empty;

    [Range(1950, 2100)]
    public int AnoFabricacao { get; set; }

    [Range(0, int.MaxValue)]
    public int Quilometragem { get; set; }

    [Required]
    public int FabricanteId { get; set; }
}

public class ClienteDto
{
    [Required, MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(14)]
    public string CPF { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(120)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefone { get; set; }
}

public class AluguelCreateDto
{
    [Required]
    public int ClienteId { get; set; }

    [Required]
    public int VeiculoId { get; set; }

    [Required]
    public DateTime DataInicio { get; set; }

    [Required]
    public DateTime DataFimPrevista { get; set; }

    [Range(0, int.MaxValue)]
    public int QuilometragemInicial { get; set; }

    [Range(0, double.MaxValue)]
    public decimal ValorDiaria { get; set; }
}

public class AluguelDevolucaoDto
{
    [Required]
    public DateTime DataDevolucao { get; set; }

    [Required, Range(0, int.MaxValue)]
    public int QuilometragemFinal { get; set; }
}

public class ReservaDto
{
    [Required]
    public int ClienteId { get; set; }

    [Required]
    public int VeiculoId { get; set; }

    [Required]
    public DateTime DataInicioPrevista { get; set; }

    [Required]
    public DateTime DataFimPrevista { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Pendente";
}
