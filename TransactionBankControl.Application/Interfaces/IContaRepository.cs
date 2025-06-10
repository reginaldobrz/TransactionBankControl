using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionBankControl.Domain.Entities;

namespace TransactionBankControl.Application.Interfaces
{
    public interface IContaRepository
    {
        Task<bool> ContaExiste(string documento);
        Task<ContaBancaria?> ObterPorDocumento(string documento);
        Task<IEnumerable<ContaBancaria>> Consultar(string? nome, string? documento);
        Task Cadastrar(ContaBancaria conta);
        Task Atualizar(ContaBancaria conta, bool commit = true);
        Task SaveChangesAsync();
    }
}