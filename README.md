# 💸 TransactionBankControl

Projeto em .NET 8 com arquitetura em camadas (API, Application, Infrastructure, Domain) para controle de contas bancárias e transações. Utiliza Entity Framework Core com SQL Server e documentação via Swagger.

## 📚 Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com)
- Entity Framework Core
- SQL Server
- Swagger (Swashbuckle)
- RESTful APIs
- Arquitetura em camadas
- Docker (opcional)

---

## ⚙️ Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- Visual Studio 2022+ ou VS Code
- (Opcional) [Docker Desktop](https://www.docker.com/)

---

## 🚀 Como executar o projeto

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/TransactionBankControl.git
cd TransactionBankControl
```

### 2. Configure a connection string

Edite o arquivo `TransactionBankControl.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TransactionBankControlDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

> ✅ Certifique-se de que o SQL Server está em execução e você tem permissões para criar o banco.

---

### 3. Crie o banco de dados

Abra o terminal na raiz do projeto e execute:

```bash
dotnet ef database update --project TransactionBankControl.Infrastructure --startup-project TransactionBankControl.API
```

---

### 4. Rode a aplicação

```bash
cd TransactionBankControl.API
dotnet run
```

A API estará disponível em:

```
https://localhost:5001
http://localhost:5000
```

---

### 5. Acesse a documentação Swagger

Abra no navegador:

```
https://localhost:5001/swagger
```

---

## ✅ Funcionalidades

- Cadastro de contas bancárias
- Listagem de contas
- Transferência entre contas
- Histórico de transações
- Validações de saldo, estado da conta e duplicidade

---

## 🧪 Testes

> (Adicione aqui quando houver testes unitários ou de integração)

---

## 📂 Estrutura do Projeto

```
TransactionBankControl
│
├── TransactionBankControl.API            # Camada de apresentação (Controllers, Program.cs)
├── TransactionBankControl.Application    # Regras de negócio (Services, DTOs, Interfaces)
├── TransactionBankControl.Infrastructure # Persistência (DbContext, Migrations, Repositories)
├── TransactionBankControl.Domain         # Entidades e enums
```

---

## 🛠️ Scripts úteis

Criar nova migration:

```bash
dotnet ef migrations add NomeDaMigration --project TransactionBankControl.Infrastructure --startup-project TransactionBankControl.API
```

Remover migration:

```bash
dotnet ef migrations remove --project TransactionBankControl.Infrastructure
```

---

## 🤝 Contribuições

Sinta-se à vontade para abrir issues e pull requests! Este projeto está em constante evolução.

---

## 📄 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
