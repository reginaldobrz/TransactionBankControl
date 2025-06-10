using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionBankControl.API.Controllers;
using TransactionBankControl.Application.DTOs;
using TransactionBankControl.Application.Interfaces;
using TransactionBankControl.Domain.Entities;
using Xunit;

namespace TransactionBankControl.Tests.Controller
{
    public class ContaControllerTests
    {
        private readonly Mock<IContaService> _serviceMock;
        private readonly ContaController _controller;

        public ContaControllerTests()
        {
            _serviceMock = new Mock<IContaService>();
            _controller = new ContaController(_serviceMock.Object);
        }

        [Fact]
        public async Task Post_ShouldReturnOk_WhenContaIsCreated()
        {
            // Arrange
            var request = new ContaRequest { NomeCliente = "John Doe", Documento = "123456789" };
            _serviceMock.Setup(s => s.CadastrarConta(request)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(request);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.CadastrarConta(request), Times.Once);
        }

        [Fact]
        public async Task Get_ShouldReturnOkWithContas_WhenContasAreFound()
        {
            // Arrange
            var contas = new List<ContaBancaria>
               {
                   new ContaBancaria("John Doe", "60720484049", 1000),
                   new ContaBancaria("Jane Doe", "07471394072", 2000)
               };
            _serviceMock.Setup(s => s.ConsultarContas(null, null)).ReturnsAsync(contas);

            // Act
            var result = await _controller.Get(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(contas, okResult.Value);
            _serviceMock.Verify(s => s.ConsultarContas(null, null), Times.Once);
        }

        [Fact]
        public async Task Inativar_ShouldReturnOk_WhenContaIsInactivated()
        {
            // Arrange
            var documento = "123456789";
            _serviceMock.Setup(s => s.InativarConta(documento)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Inativar(documento);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.InativarConta(documento), Times.Once);
        }

        [Fact]
        public async Task Ativar_ShouldReturnOk_WhenContaIsActivated()
        {
            // Arrange
            var documento = "123456789";
            _serviceMock.Setup(s => s.AtivarConta(documento)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Ativar(documento);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.AtivarConta(documento), Times.Once);
        }

        [Fact]
        public async Task Transferir_ShouldReturnOk_WhenTransferIsSuccessful()
        {
            // Arrange
            var request = new TransferenciaRequest
            {
                DocumentoOrigem = "123456789",
                DocumentoDestino = "987654321",
                Valor = 500
            };
            _serviceMock.Setup(s => s.Transferir(request)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Transferir(request);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.Transferir(request), Times.Once);
        }
    }
}
