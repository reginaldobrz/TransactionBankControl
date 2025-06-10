using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TransactionBankControl.Domain.Entities;
using TransactionBankControl.Domain.Enums;
using TransactionBankControl.Infrastructure.Context;
using TransactionBankControl.Infrastructure.Repositories;
using Xunit;

namespace TransactionBankControl.Tests.Infrastructure
{
    public class AuditoriaRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;

        public AuditoriaRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Cadastrar_ShouldAddAuditoriaToDatabase()
        {
            // Arrange  
            var context = new AppDbContext(_dbContextOptions);
            var repository = new AuditoriaRepository(context);
            var auditoria = new Auditoria(Guid.NewGuid(), "123456789", TipoAuditoria.Ativacao);

            // Act  
            await repository.Cadastrar(auditoria);

            // Assert  
            var savedAuditoria = await context.Auditorias.FirstOrDefaultAsync(a => a.Id == auditoria.Id);
            Assert.NotNull(savedAuditoria);
            Assert.Equal(auditoria.Id, savedAuditoria.Id);
            Assert.Equal(auditoria.Documento, savedAuditoria.Documento);
            Assert.Equal(auditoria.Tipo, savedAuditoria.Tipo);
        }
    }
}
