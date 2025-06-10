using Microsoft.EntityFrameworkCore;
using TransactionBankControl.Domain.Entities;

namespace TransactionBankControl.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ContaBancaria> ContasBancarias { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContaBancaria>()
                .HasMany(c => c.Transacoes)
                .WithOne(t => t.ContaBancaria)
                .HasForeignKey(t => t.ContaBancariaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Auditoria>()
                .HasOne(c => c.ContaBancaria)
                .WithOne()
                .HasForeignKey<Auditoria>(a => a.ContaBancariaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}