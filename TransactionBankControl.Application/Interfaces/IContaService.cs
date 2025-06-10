using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionBankControl.Application.DTOs;
using TransactionBankControl.Domain.Entities;

namespace TransactionBankControl.Application.Interfaces
{
    public interface IContaService
    {
        Task CadastrarConta(ContaRequest request);
        Task<IEnumerable<ContaBancaria>> ConsultarContas(string? nome, string? documento);
        Task InativarConta(string documento);
        Task AtivarConta(string documento);
        Task Transferir(TransferenciaRequest request);
    }
}
