using LocadoraVeiculos.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.API.Data;

public class LocadoraContext : DbContext
{
    public LocadoraContext(DbContextOptions<LocadoraContext> options) : base(options) { }

    public DbSet<Fabricante> Fabricantes => Set<Fabricante>();
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Aluguel> Alugueis => Set<Aluguel>();
    public DbSet<Reserva> Reservas => Set<Reserva>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---- Restrições de unicidade ----
        modelBuilder.Entity<Veiculo>()
            .HasIndex(v => v.Placa)
            .IsUnique();

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.CPF)
            .IsUnique();

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Email)
            .IsUnique();

        // ---- Relacionamentos (chaves estrangeiras) ----

        // Veiculo (N) -> Fabricante (1)
        modelBuilder.Entity<Veiculo>()
            .HasOne(v => v.Fabricante)
            .WithMany(f => f.Veiculos)
            .HasForeignKey(v => v.FabricanteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Aluguel (N) -> Cliente (1)
        modelBuilder.Entity<Aluguel>()
            .HasOne(a => a.Cliente)
            .WithMany(c => c.Alugueis)
            .HasForeignKey(a => a.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Aluguel (N) -> Veiculo (1)
        modelBuilder.Entity<Aluguel>()
            .HasOne(a => a.Veiculo)
            .WithMany(v => v.Alugueis)
            .HasForeignKey(a => a.VeiculoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Reserva (N) -> Cliente (1)
        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.Cliente)
            .WithMany(c => c.Reservas)
            .HasForeignKey(r => r.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Reserva (N) -> Veiculo (1)
        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.Veiculo)
            .WithMany(v => v.Reservas)
            .HasForeignKey(r => r.VeiculoId)
            .OnDelete(DeleteBehavior.Restrict);

        // ---- Tipos de coluna para valores monetários ----
        modelBuilder.Entity<Aluguel>()
            .Property(a => a.ValorDiaria)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<Aluguel>()
            .Property(a => a.ValorTotal)
            .HasColumnType("decimal(10,2)");
    }
}
