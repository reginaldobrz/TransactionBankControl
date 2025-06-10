using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionBankControl.Application.Interfaces;
using TransactionBankControl.Domain.Entities;
using TransactionBankControl.Infrastructure.Context;

namespace TransactionBankControl.Infrastructure.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly AppDbContext _context;

        public ContaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ContaExiste(string documento)
        {
            return await _context.ContasBancarias.AnyAsync(c => c.Documento == documento);
        }

        public async Task<ContaBancaria?> ObterPorDocumento(string documento)
        {
            return await _context.ContasBancarias.Include(c => c.Transacoes).FirstOrDefaultAsync(c => c.Documento == documento);
        }

        public async Task<IEnumerable<ContaBancaria>> Consultar(string? nome, string? documento)
        {
            return await _context.ContasBancarias
                .Where(c =>
                    (string.IsNullOrEmpty(nome) || c.NomeTitular.Contains(nome)) &&
                    (string.IsNullOrEmpty(documento) || c.Documento == documento))
                .Include(c => c.Transacoes)
                .ToListAsync();
        }

        public async Task Cadastrar(ContaBancaria conta)
        {
            _context.ContasBancarias.Add(conta);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(ContaBancaria conta, bool commit = true)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            _context.ContasBancarias.Update(conta);

            if (commit)
            {
                await transaction.CommitAsync();
                await SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();

        }
    }
}