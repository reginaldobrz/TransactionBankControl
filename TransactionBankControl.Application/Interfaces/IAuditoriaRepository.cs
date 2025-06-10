using System.Threading.Tasks;
using TransactionBankControl.Domain.Entities;

namespace TransactionBankControl.Application.Interfaces
{
    public interface IAuditoriaRepository
    {
        Task Cadastrar(Auditoria auditoria);
    }
}
