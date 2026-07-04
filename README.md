# TodoListApi

🎥 [Assista à demonstração em vídeo](https://youtu.be/xSI3pPvKvW0)

API RESTful de gerenciamento de tarefas desenvolvida como desafio técnico para a vaga de Desenvolvedor C# na **PWI Sistemas**.

Front-end: [TodoListFront](https://github.com/Davi-Oliveira-Brito/TodoListFront)

---

## Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Tecnologias](#tecnologias)
- [Arquitetura](#arquitetura)
- [Pré-requisitos](#pré-requisitos)
- [Como Rodar](#como-rodar)
- [Autenticação](#autenticação)
- [Endpoints](#endpoints)
- [Postman Collection](#postman-collection)
- [Testes](#testes)
- [Estrutura de Pastas](#estrutura-de-pastas)

---

## Sobre o Projeto

API construída com ASP.NET Core que permite gerenciar uma lista de tarefas (To Do List), com suporte a criação, leitura, atualização e exclusão de tarefas. Cada tarefa possui um tipo associado — **Normal** ou **Urgente** — que é pré-cadastrado no banco via Seed na migration.

A API conta com autenticação via **JWT (JSON Web Token)**. Cada usuário gerencia apenas suas próprias tarefas — os endpoints de tarefas são protegidos e exigem um token válido.

A API tem CORS habilitado para `http://localhost:3000`, permitindo o consumo direto pelo [front-end](https://github.com/Davi-Oliveira-Brito/TodoListFront) local.

---

## Tecnologias

- .NET 10.0
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server
- JWT (JSON Web Token) — autenticação
- BCrypt — hash de senha
- Swagger / OpenAPI
- xUnit (testes unitários)
- Microsoft.EntityFrameworkCore.InMemory (banco em memória para testes)

---

## Arquitetura

O projeto segue uma arquitetura em camadas com responsabilidades bem definidas:

**Controllers** — recebem as requisições HTTP e delegam para os Repositories. Não contêm lógica de negócio.

**Services** — contém a lógica de autenticação (`AuthService`): geração de token JWT, hash de senha com BCrypt e validação de credenciais.

**Repositories** — responsáveis pelo acesso ao banco de dados via Entity Framework. Implementam interfaces, facilitando testes e extensões futuras.

**Models** — representam as entidades do banco de dados (`Tarefa`, `TipoTarefa` e `Usuario`).

**DTOs (Data Transfer Objects)** — separam os objetos de entrada e saída da API dos modelos internos, evitando exposição desnecessária de dados.

**Data** — contém o `AppDbContext`, responsável pela configuração do ORM, mapeamento das entidades e Seed de dados iniciais.

Essa separação garante que cada camada tenha uma única responsabilidade, tornando o código fácil de ler, manter e estender.

---

## Pré-requisitos

Antes de rodar o projeto, certifique-se de ter instalado:

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (qualquer versão — Express recomendado)
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

Abra o arquivo `TodoListApi/appsettings.json` e ajuste com as informações do seu SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TodoListDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "TodoListApi@PWI#Sistemas$2026!SecretKey",
    "Issuer": "TodoListApi",
    "Audience": "TodoListApiUsers"
  }
}
```

Se o seu SQL Server usar autenticação por usuário e senha, substitua a connection string por:

```
Server=localhost;Database=TodoListDb;User Id=seu_usuario;Password=sua_senha;TrustServerCertificate=True;
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
http://localhost:5222/swagger
```

---

## Autenticação

A API utiliza autenticação via JWT. Os endpoints de tarefas são protegidos e exigem um token válido no header `Authorization`.

### Fluxo de autenticação

**1. Crie uma conta:**

```
POST /api/Auth/register
```

```json
{
  "nome": "Davi Oliveira",
  "email": "davi@email.com",
  "senha": "123456"
}
```

**2. Ou faça login:**

```
POST /api/Auth/login
```

```json
{
  "email": "davi@email.com",
  "senha": "123456"
}
```

**3. Use o token retornado:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "nome": "Davi Oliveira",
  "email": "davi@email.com"
}
```

**4. No Swagger:** clique no botão **Authorize** e cole o token.

**5. Em outras ferramentas (Postman, curl):** envie o token no header:

```
Authorization: Bearer {seu_token}
```

O token expira em **8 horas**.

---

## Endpoints

### Autenticação (público)

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/Auth/register` | Cadastra um novo usuário e retorna o token |
| POST | `/api/Auth/login` | Autentica e retorna o token |

### Tarefas (requer token JWT)

Cada usuário acessa apenas suas próprias tarefas.

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/Tarefas` | Lista todas as tarefas do usuário autenticado |
| GET | `/api/Tarefas/{id}` | Busca uma tarefa por ID |
| POST | `/api/Tarefas` | Cria uma nova tarefa |
| PUT | `/api/Tarefas/{id}` | Atualiza uma tarefa existente |
| DELETE | `/api/Tarefas/{id}` | Remove uma tarefa |

### Tipos de Tarefa (público)

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/TipoTarefas` | Lista os tipos disponíveis (Normal / Urgente) |
| GET | `/api/TipoTarefas/{id}` | Busca um tipo por ID |

### Exemplo de payload para criação de tarefa (POST /api/Tarefas)

```json
{
  "titulo": "Revisar documentação",
  "descricao": "Revisar o README do projeto",
  "tipoTarefaId": 1,
  "concluida": false
}
```

### Exemplo de resposta (GET /api/Tarefas)

```json
[
  {
    "id": 1,
    "titulo": "Revisar documentação",
    "descricao": "Revisar o README do projeto",
    "concluida": false,
    "dataInclusao": "2026-07-02T19:47:46.014Z",
    "tipoTarefaId": 1,
    "tipoTarefaDescricao": "Normal"
  }
]
```

> Datas são retornadas em **UTC**. O front-end deve converter para o horário local do usuário.

---

## Postman Collection

O repositório inclui uma collection do Postman com todos os endpoints prontos para teste.

### Como importar

1. Abra o Postman
2. Clique em **Import**
3. Selecione o arquivo `TodoListApi.postman_collection.json` na raiz do repositório

A collection já vem com a variável `baseUrl` (`http://localhost:5222`) configurada — não é necessário criar um environment.

### Como usar

1. Dê **Send** em `POST /api/Auth/register` (ou `/api/Auth/login`)
2. Um script de teste captura o token da resposta e preenche automaticamente a variável de collection `bearerToken` — sem precisar copiar ou colar nada
3. Todos os endpoints de Tarefas já estarão autenticados automaticamente, pois usam `{{bearerToken}}` como Bearer Token

---

## Testes

O projeto conta com testes unitários cobrindo todos os métodos do `TarefaRepository`, incluindo isolamento por usuário. Utiliza banco de dados em memória (InMemory) para isolamento total do ambiente.

Para rodar os testes:

```bash
cd TodoListApi.Tests
dotnet test
```

Resultado esperado:

```
total: 9 | falhou: 0 | bem-sucedido: 9
```

Casos de teste cobertos:

- Criar tarefa e verificar persistência
- Listar tarefas — retorna apenas as do usuário autenticado
- Buscar tarefa por ID existente
- Buscar tarefa por ID inexistente (retorna null)
- Buscar tarefa de outro usuário (retorna null)
- Atualizar tarefa existente
- Deletar tarefa existente
- Deletar tarefa inexistente (retorna false)
- Deletar tarefa de outro usuário (retorna false)

---

## Estrutura de Pastas

```
TodoListApi/
├── Controllers/
│   ├── AuthController.cs          -> Endpoints de registro e login
│   ├── TarefasController.cs       -> CRUD de tarefas (protegido por JWT)
│   └── TipoTarefasController.cs   -> Listagem de tipos (público)
├── Data/
│   ├── AppDbContext.cs            -> Configuração do ORM e Seed de dados
│   └── AppDbContextFactory.cs     -> Factory para uso do EF em design-time
├── DTOs/
│   ├── AuthDto.cs                 -> DTOs de registro, login e token
│   ├── TarefaCreateDto.cs         -> Objeto de entrada (POST e PUT)
│   └── TarefaResponseDto.cs       -> Objeto de saída (GET)
├── Migrations/                    -> Histórico de versões do banco (gerado pelo EF Core)
├── Models/
│   ├── Tarefa.cs                  -> Entidade principal
│   ├── TipoTarefa.cs              -> Entidade de tipo (Normal / Urgente)
│   └── Usuario.cs                 -> Entidade de usuário
├── Repositories/
│   ├── ITarefaRepository.cs       -> Interface do repositório de tarefas
│   ├── TarefaRepository.cs        -> Implementação com acesso ao banco
│   ├── ITipoTarefaRepository.cs   -> Interface do repositório de tipos
│   └── TipoTarefaRepository.cs    -> Implementação com acesso ao banco
├── Services/
│   └── AuthService.cs             -> Lógica de autenticação e geração de token
├── appsettings.json               -> Configurações da aplicação
└── Program.cs                     -> Ponto de entrada e registro de dependências

TodoListApi.Tests/
└── TarefaRepositoryTests.cs       -> 9 testes unitários do TarefaRepository
```

---

Desenvolvido por [**Davi Oliveira Brito**](https://www.linkedin.com/in/davi-oliveira-brito-b7267b252/)