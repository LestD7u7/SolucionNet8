using BancaLafise.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace BancaLafise.Infrastructure.Context
{
    public class LafiseDbContext : DbContext
    {
        public LafiseDbContext(DbContextOptions<LafiseDbContext> options)
        : base(options) { }

        public DbSet<TipoMovimiento> TiposMovimiento { get; set; }
        public DbSet<TipoTransaccion> TiposTransaccion { get; set; }
        public DbSet<Sexo> Sexos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<TipoCuenta> TiposCuenta { get; set; }
        public DbSet<CuentaBancaria> CuentasBancarias { get; set; }
        public DbSet<ReglaCumplimiento> ReglasCumplimiento { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<EstadoUsuario> EstadosUsuario { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TipoTransaccion>()
                .HasOne<TipoMovimiento>()
                .WithMany()
                .HasForeignKey(t => t.TipoMovimiento);

            modelBuilder.Entity<Cliente>()
                .HasOne<Sexo>()
                .WithMany()
                .HasForeignKey(c => c.SexoId);

            modelBuilder.Entity<CuentaBancaria>()
                .HasOne<TipoCuenta>()
                .WithMany()
                .HasForeignKey(c => c.TipoCuenta);

            modelBuilder.Entity<CuentaBancaria>()
                .HasOne<Cliente>()
                .WithMany()
                .HasForeignKey(c => c.ClienteId);

            modelBuilder.Entity<Transaccion>()
                .HasOne<TipoTransaccion>()
                .WithMany()
                .HasForeignKey(t => t.TipoTransaccion);

            modelBuilder.Entity<Usuario>()
                .HasOne<Cliente>()
                .WithMany()
                .HasForeignKey(u => u.ClienteId);

            modelBuilder.Entity<Usuario>()
                .HasOne<EstadoUsuario>()
                .WithMany()
                .HasForeignKey(u => u.EstadoId);

            modelBuilder.Entity<ReglaCumplimiento>()
                .HasOne<TipoCuenta>()
                .WithMany()
                .HasForeignKey(u => u.TipoCuenta);
        }

        //public Task<int> SaveChangesAsync(CancellationToken cancellationToken) => base.SaveChangesAsync(cancellationToken);
    }
}
