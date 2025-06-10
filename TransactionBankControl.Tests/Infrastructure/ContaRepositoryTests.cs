using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TransactionBankControl.Domain.Entities;
using TransactionBankControl.Infrastructure.Context;
using TransactionBankControl.Infrastructure.Repositories;
using Xunit;

namespace TransactionBankControl.Tests.Infrastructure
{
    public class ContaRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly AppDbContext _context;
        private readonly ContaRepository _repository;

        public ContaRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(_dbContextOptions);
            _repository = new ContaRepository(_context);
        }

        [Fact]
        public async Task ContaExiste_ShouldReturnTrue_WhenContaExists()
        {
            // Arrange  
            var conta = new ContaBancaria("John Doe", "07471394072", 1000);
            _context.ContasBancarias.Add(conta);
            await _context.SaveChangesAsync();

            // Act  
            var result = await _repository.ContaExiste("07471394072");

            // Assert  
            Assert.True(result);
        }

        [Fact]
        public async Task ContaExiste_ShouldReturnFalse_WhenContaDoesNotExist()
        {
            // Act  
            var result = await _repository.ContaExiste("987654321");

            // Assert  
            Assert.False(result);
        }

        [Fact]
        public async Task ObterPorDocumento_ShouldReturnConta_WhenContaExists()
        {
            // Arrange  
            var conta = new ContaBancaria("Jane Doe", "07471394072", 2000);
            _context.ContasBancarias.Add(conta);
            await _context.SaveChangesAsync();

            // Act  
            var result = await _repository.ObterPorDocumento("07471394072");

            // Assert  
            Assert.NotNull(result);
            Assert.Equal("Jane Doe", result.NomeTitular);
        }

        [Fact]
        public async Task ObterPorDocumento_ShouldReturnNull_WhenContaDoesNotExist()
        {
            // Act  
            var result = await _repository.ObterPorDocumento("987654321");

            // Assert  
            Assert.Null(result);
        }

        [Fact]
        public async Task Consultar_ShouldReturnFilteredContas()
        {
            // Arrange  
            var conta1 = new ContaBancaria("John Doe", "07471394072", 1000);
            var conta2 = new ContaBancaria("Jane Doe", "60720484049", 2000);
            _context.ContasBancarias.AddRange(conta1, conta2);
            await _context.SaveChangesAsync();

            // Act  
            var result = await _repository.Consultar("Jane", null);

            // Assert  
            Assert.Single(result);
            Assert.Equal("Jane Doe", result.First().NomeTitular);
        }

        [Fact]
        public async Task Cadastrar_ShouldAddContaToDatabase()
        {
            // Arrange  
            var conta = new ContaBancaria("John Doe", "60720484049", 1000);

            // Act  
            await _repository.Cadastrar(conta);

            // Assert  
            var result = await _context.ContasBancarias.FirstOrDefaultAsync(c => c.Documento == "60720484049");
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.NomeTitular);
        }
    }
}
