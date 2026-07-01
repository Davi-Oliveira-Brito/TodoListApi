# TodoListApi

API RESTful de gerenciamento de tarefas desenvolvida como desafio técnico para a vaga de Desenvolvedor C# na **PWI Sistemas**.

---

## Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Tecnologias](#tecnologias)
- [Arquitetura](#arquitetura)
- [Pré-requisitos](#pré-requisitos)
- [Como Rodar](#como-rodar)
- [Endpoints](#endpoints)
- [Testes](#testes)
- [Estrutura de Pastas](#estrutura-de-pastas)

---

## Sobre o Projeto

API construída com ASP.NET Core que permite gerenciar uma lista de tarefas (To Do List), com suporte a criação, leitura, atualização e exclusão de tarefas. Cada tarefa possui um tipo associado - **Normal** ou **Urgente** - que é pré-cadastrado no banco via Seed na migration.

---

## Tecnologias

- .NET 10.0
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server
- Swagger / OpenAPI
- xUnit (testes unitários)
- Microsoft.EntityFrameworkCore.InMemory (banco em memória para testes)

---

## Arquitetura

O projeto segue uma arquitetura em camadas com responsabilidades bem definidas:

**Controllers** - recebem as requisições HTTP e delegam para os Repositories. Não contêm lógica de negócio.

**Repositories** - responsáveis pelo acesso ao banco de dados via Entity Framework. Implementam interfaces, facilitando testes e extensões futuras.

**Models** - representam as entidades do banco de dados (`Tarefa` e `TipoTarefa`).

**DTOs (Data Transfer Objects)** - separam os objetos de entrada e saída da API dos modelos internos, evitando exposição desnecessária de dados.

**Data** - contém o `AppDbContext`, responsável pela configuração do ORM, mapeamento das entidades e Seed de dados iniciais.

Essa separação garante que cada camada tenha uma única responsabilidade, tornando o código fácil de ler, manter e estender.

---

## Pré-requisitos

Antes de rodar o projeto, certifique-se de ter instalado:

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (qualquer versão - Express recomendado)
- dotnet-ef instalado globalmente:

```bash
dotnet tool install --global dotnet-ef
```

---

## Como Rodar

### 1. Clone o repositório

```bash
git clone https://github.com/Davi-Oliveira-Brito/TodoListApi.git
cd TodoListApi
```

### 2. Configure a connection string

Abra o arquivo `TodoListApi/appsettings.json` e ajuste a connection string com as informações do seu SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TodoListDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Se o seu SQL Server usar autenticação por usuário e senha:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TodoListDb;User Id=seu_usuario;Password=sua_senha;TrustServerCertificate=True;"
  }
}
```

### 3. Restaure as dependências

```bash
cd TodoListApi
dotnet restore
```

### 4. Crie o banco e execute as migrations

```bash
dotnet ef database update
```

Este comando cria o banco `TodoListDb` no SQL Server e popula automaticamente os tipos de tarefa (Normal e Urgente) via Seed da migration.

### 5. Faça o build

```bash
dotnet build
```

### 6. Rode a aplicação

```bash
dotnet run
```

### 7. Acesse o Swagger

Abra no navegador:

```
https://localhost:7100/swagger
```

---

## Endpoints

### Tarefas

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/tarefas` | Lista todas as tarefas |
| GET | `/api/tarefas/{id}` | Busca uma tarefa por ID |
| POST | `/api/tarefas` | Cria uma nova tarefa |
| PUT | `/api/tarefas/{id}` | Atualiza uma tarefa existente |
| DELETE | `/api/tarefas/{id}` | Remove uma tarefa |

### Tipos de Tarefa

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/tipotarefas` | Lista os tipos disponíveis (Normal / Urgente) |
| GET | `/api/tipotarefas/{id}` | Busca um tipo por ID |

### Exemplo de payload para criação de tarefa (POST /api/tarefas)

```json
{
  "titulo": "Revisar documentação",
  "descricao": "Revisar o README do projeto",
  "tipoTarefaId": 1
}
```

### Exemplo de resposta (GET /api/tarefas)

```json
[
  {
    "id": 1,
    "titulo": "Revisar documentação",
    "descricao": "Revisar o README do projeto",
    "concluida": false,
    "dataInclusao": "2026-07-01T14:25:11.934503",
    "tipoTarefaId": 1,
    "tipoTarefaDescricao": "Normal"
  }
]
```

---

## Testes

O projeto conta com testes unitários cobrindo todos os métodos do `TarefaRepository`, utilizando banco de dados em memória (InMemory) para isolamento total do ambiente.

Para rodar os testes:

```bash
cd TodoListApi.Tests
dotnet test
```

Resultado esperado:

```
total: 7 | falhou: 0 | bem-sucedido: 7
```

Casos de teste cobertos:

- Criar tarefa e verificar persistência
- Listar todas as tarefas
- Buscar tarefa por ID existente
- Buscar tarefa por ID inexistente (retorna null)
- Atualizar tarefa existente
- Deletar tarefa existente
- Deletar tarefa inexistente (retorna false)

---

## Estrutura de Pastas

```
TodoListApi/
├── Controllers/              -> Endpoints HTTP (GET, POST, PUT, DELETE)
├── Data/
│   ├── AppDbContext.cs       -> Configuração do ORM e Seed de dados
│   └── AppDbContextFactory.cs -> Factory para uso do EF em design-time
├── DTOs/
│   ├── TarefaCreateDto.cs    -> Objeto de entrada (POST e PUT)
│   └── TarefaResponseDto.cs  -> Objeto de saída (GET)
├── Migrations/               -> Histórico de versões do banco (gerado pelo EF Core)
├── Models/
│   ├── Tarefa.cs             -> Entidade principal
│   └── TipoTarefa.cs         -> Entidade de tipo (Normal / Urgente)
├── Repositories/
│   ├── ITarefaRepository.cs       -> Interface do repositório de tarefas
│   ├── TarefaRepository.cs        -> Implementação com acesso ao banco
│   ├── ITipoTarefaRepository.cs   -> Interface do repositório de tipos
│   └── TipoTarefaRepository.cs    -> Implementação com acesso ao banco
├── appsettings.json          -> Configurações da aplicação
└── Program.cs                -> Ponto de entrada e registro de dependências

TodoListApi.Tests/
└── TarefaRepositoryTests.cs  -> 7 testes unitários do TarefaRepository
```

---

Desenvolvido por [**Davi Oliveira**](https://www.linkedin.com/in/davi-oliveira-brito-b7267b252/) 