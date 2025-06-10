using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TransactionBankControl.Domain.Enums;
using TransactionBankControl.Domain.ValueObjects;

namespace TransactionBankControl.Domain.Entities
{
    public class ContaBancaria
    {
        [Key]
        public Guid Id { get; private set; }
        public string NomeTitular { get; private set; }
        public string Documento { get; private set; }
        public bool Ativa { get; set; }
        public decimal Saldo { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();

        protected ContaBancaria() { }

        public ContaBancaria(string nomeTitular, string documento, decimal saldo)
        {
            if (string.IsNullOrWhiteSpace(nomeTitular))
                throw new ArgumentException("Nome do titular é obrigatório");

            if (!DocumentoValido.Validar(documento))
                throw new ArgumentException("Documento inválido");

            Id = Guid.NewGuid();
            NomeTitular = nomeTitular;
            Documento = documento;
            Ativa = true;
            Saldo = saldo;
            DataCriacao = DateTime.UtcNow;
            Transacoes = new List<Transacao>();
        }

        public void AdicionarTransacao(Transacao transacao)
        {
            transacao.ContaBancariaId = Id;
            Transacoes.Add(transacao);

            Saldo += transacao.Tipo == TipoTransacao.Credito
                ? transacao.Valor
                : -transacao.Valor;
        }
    }
}