using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionBankControl.Application.DTOs;
using TransactionBankControl.Application.Interfaces;
using TransactionBankControl.Domain.Entities;
using TransactionBankControl.Domain.Enums;

namespace TransactionBankControl.Application.Services
{
    public class ContaService(IContaRepository repo, IAuditoriaRepository aud) : IContaService
    {
        public async Task CadastrarConta(ContaRequest request)
        {
            if (await repo.ContaExiste(request.Documento))
                throw new InvalidOperationException("Conta já existente para esse documento.");

            var conta = new ContaBancaria(request.NomeCliente, request.Documento, 1000);

            await repo.Cadastrar(conta);
        }

        public async Task<IEnumerable<ContaBancaria>> ConsultarContas(string? nome, string? documento)
        {
            return await repo.Consultar(nome, documento);
        }

        public async Task InativarConta(string documento)
        {
            var conta = await repo.ObterPorDocumento(documento);
            if (conta == null || !conta.Ativa)
                throw new InvalidOperationException("Conta não encontrada ou já inativa.");

            conta.Ativa = false;
            await repo.Atualizar(conta);
            await repo.SaveChangesAsync();

            await aud.Cadastrar(new Auditoria(conta.Id, conta.Documento, TipoAuditoria.Inativacao));
        }

        public async Task AtivarConta(string documento)
        {
            var conta = await repo.ObterPorDocumento(documento);
            if (conta == null || conta.Ativa)
                throw new InvalidOperationException("Conta não encontrada ou já ativa.");

            conta.Ativa = true;
            await repo.Atualizar(conta);
            await repo.SaveChangesAsync();

            await aud.Cadastrar(new Auditoria(conta.Id, conta.Documento, TipoAuditoria.Ativacao));

        }

        public async Task Transferir(TransferenciaRequest request)
        {
            if (request.Valor <= 0)
                throw new ArgumentException("Valor inválido");


            var origem = await repo.ObterPorDocumento(request.DocumentoOrigem);
            var destino = await repo.ObterPorDocumento(request.DocumentoDestino);

            if (origem == null || destino == null || !origem.Ativa || !destino.Ativa)
                throw new InvalidOperationException("Contas inválidas ou inativas.");

            if (origem.Saldo < request.Valor)
                throw new InvalidOperationException("Saldo insuficiente.");

            origem.AdicionarTransacao(new Transacao(origem.Id, request.Valor, TipoTransacao.Debito));
            destino.AdicionarTransacao(new Transacao(destino.Id, request.Valor, TipoTransacao.Credito));

            await repo.SaveChangesAsync();
        }
    }
}


