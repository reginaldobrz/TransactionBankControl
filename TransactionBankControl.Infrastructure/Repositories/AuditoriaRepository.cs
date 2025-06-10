using System.Threading.Tasks;
using TransactionBankControl.Application.Interfaces;
using TransactionBankControl.Domain.Entities;
using TransactionBankControl.Infrastructure.Context;

namespace TransactionBankControl.Infrastructure.Repositories
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly AppDbContext _context;

        public AuditoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Cadastrar(Auditoria auditoria)
        {
            _context.Auditorias.Add(auditoria);
            await _context.SaveChangesAsync();
        }
    }
}
