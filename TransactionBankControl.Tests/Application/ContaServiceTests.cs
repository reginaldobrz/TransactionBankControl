using Moq;
using System;
using System.Threading.Tasks;
using TransactionBankControl.Application.DTOs;
using TransactionBankControl.Application.Interfaces;
using TransactionBankControl.Application.Services;
using TransactionBankControl.Domain.Entities;
using Xunit;

namespace TransactionBankControl.Tests.Application;

public class ContaServiceTests
{
    [Fact]
    public async Task CadastrarConta_DeveCriarContaComSaldoInicial()
    {
        var repoMock = new Mock<IContaRepository>();
        var audMock = new Mock<IAuditoriaRepository>();
        repoMock.Setup(r => r.ContaExiste("07471394072")).ReturnsAsync(false);

        var service = new ContaService(repoMock.Object, audMock.Object);

        var request = new ContaRequest { NomeCliente = "João", Documento = "07471394072" };
        await service.CadastrarConta(request);

        repoMock.Verify(r => r.Cadastrar(It.Is<ContaBancaria>(c =>
            c.NomeTitular == "João" &&
            c.Documento == "07471394072" &&
            c.Saldo == 1000
        )), Times.Once);
    }

    private readonly Mock<IContaRepository> _repoMock;
    private readonly Mock<IAuditoriaRepository> _audMock;
    private readonly ContaService _service;

    public ContaServiceTests()
    {
        _repoMock = new Mock<IContaRepository>();
        _audMock = new Mock<IAuditoriaRepository>();
        _service = new ContaService(_repoMock.Object, _audMock.Object);
    }

    [Fact]
    public async Task CadastrarConta_ShouldThrowException_WhenContaAlreadyExists()
    {
        // Arrange  
        var request = new ContaRequest { NomeCliente = "John Doe", Documento = "123456789" };
        _repoMock.Setup(r => r.ContaExiste(request.Documento)).ReturnsAsync(true);

        // Act & Assert  
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CadastrarConta(request));
    }

    [Fact]
    public async Task CadastrarConta_ShouldCallCadastrar_WhenContaDoesNotExist()
    {
        // Arrange  
        var request = new ContaRequest { NomeCliente = "John Doe", Documento = "07471394072" };
        _repoMock.Setup(r => r.ContaExiste(request.Documento)).ReturnsAsync(false);

        // Act  
        await _service.CadastrarConta(request);

        // Assert  
        _repoMock.Verify(r => r.Cadastrar(It.IsAny<ContaBancaria>()), Times.Once);
    }

    [Fact]
    public async Task InativarConta_ShouldThrowException_WhenContaNotFoundOrAlreadyInactive()
    {
        // Arrange  
        var documento = "123456789";
        _repoMock.Setup(r => r.ObterPorDocumento(documento)).ReturnsAsync((ContaBancaria)null);

        // Act & Assert  
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.InativarConta(documento));
    }

    [Fact]
    public async Task InativarConta_ShouldUpdateContaAndLogAuditoria_WhenContaIsActive()
    {
        // Arrange  
        var documento = "07471394072";
        var conta = new ContaBancaria("John Doe", documento, 1000) { Ativa = true };
        _repoMock.Setup(r => r.ObterPorDocumento(documento)).ReturnsAsync(conta);

        // Act  
        await _service.InativarConta(documento);

        // Assert  
        _repoMock.Verify(r => r.Atualizar(conta, true), Times.Once);
        _audMock.Verify(a => a.Cadastrar(It.IsAny<Auditoria>()), Times.Once);
    }

    [Fact]
    public async Task Transferir_ShouldThrowException_WhenValorIsInvalid()
    {
        // Arrange  
        var request = new TransferenciaRequest { DocumentoOrigem = "123", DocumentoDestino = "456", Valor = -100 };

        // Act & Assert  
        await Assert.ThrowsAsync<ArgumentException>(() => _service.Transferir(request));
    }

    [Fact]
    public async Task Transferir_ShouldThrowException_WhenSaldoIsInsufficient()
    {
        // Arrange  
        var request = new TransferenciaRequest { DocumentoOrigem = "60720484049", DocumentoDestino = "07471394072", Valor = 1000 };
        var origem = new ContaBancaria("John Doe", "60720484049", 500) { Ativa = true };
        var destino = new ContaBancaria("Jane Doe", "07471394072", 1000) { Ativa = true };
        _repoMock.Setup(r => r.ObterPorDocumento(request.DocumentoOrigem)).ReturnsAsync(origem);
        _repoMock.Setup(r => r.ObterPorDocumento(request.DocumentoDestino)).ReturnsAsync(destino);

        // Act & Assert  
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Transferir(request));
    }

    [Fact]
    public async Task Transferir_ShouldPerformTransfer_WhenValid()
    {
        // Arrange  
        var request = new TransferenciaRequest { DocumentoOrigem = "07471394072", DocumentoDestino = "60720484049", Valor = 100 };
        var origem = new ContaBancaria("John Doe", "07471394072", 500) { Ativa = true };
        var destino = new ContaBancaria("Jane Doe", "60720484049", 1000) { Ativa = true };
        _repoMock.Setup(r => r.ObterPorDocumento(request.DocumentoOrigem)).ReturnsAsync(origem);
        _repoMock.Setup(r => r.ObterPorDocumento(request.DocumentoDestino)).ReturnsAsync(destino);

        // Act  
        await _service.Transferir(request);

        // Assert  
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}