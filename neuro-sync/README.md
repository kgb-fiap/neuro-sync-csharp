# 🧠 Neuro-Sync API (.NET)

API REST para inclusão produtiva de pessoas neurodivergentes em ambientes de trabalho, construída em ASP.NET Core Web API + EF Core + JWT + Oracle.  

Esta API é o backend oficial da solução Neuro-Sync (Global Solution 2025 – FIAP), consumida pelo app mobile, responsável por:

- Autenticação e autorização de usuários corporativos
- Cadastro de usuários, perfis e preferências sensoriais
- Gestão de estações de trabalho e zonas sensoriais
- Reservas de estações, avaliações de conforto e indicadores
- Consulta a funções/procedures Oracle para métricas de conforto e ocupação

---

## 📖 Sobre o Projeto

O objetivo da Neuro-Sync API é disponibilizar serviços REST seguros e bem estruturados para suportar o fluxo de inclusão sensorial em ambientes de trabalho híbridos e open-plan, respeitando:

- **ODS 8 – Trabalho decente e crescimento econômico**
- **ODS 10 – Redução das desigualdades**

A API consolida regras de negócio, domínio e integração com Oracle, expondo dados de:

- **Organização:** Empresa, Filial, Setor  
- **Usuários e papéis:** Usuário, Perfil, UsuárioPerfil  
- **Preferências sensoriais:** PreferenciaSensorial  
- **Ambiente físico:** ZonaSensorial, EstacaoTrabalho  
- **IoT:** TipoSensor, Sensor, LeituraSensor  
- **Reservas & bem-estar:** StatusReserva, ReservaEstacao, AvaliacaoEstacao  

---

## 🎯 Objetivo (ODS 8 e 10)

Fornecer uma camada de serviços em .NET para:

- Garantir que colaboradores neurodivergentes encontrem espaços sensorialmente adequados
- Expor indicadores de conforto, compatibilidade e ocupação para RH/Facilities
- Centralizar regras de negócio e validações no backend, com segurança (JWT) e padronização (ProblemDetails + Swagger)

---

## ✨ Funcionalidades Principais da API

### 🔐 Autenticação & Autorização (JWT)

- `POST /auth/login`  
  - Autentica um usuário pelo `email` e `senha` (suporta senha hash SHA-256 ou texto puro para cenários legados)
  - Gera JWT com:
    - `sub`: ID do usuário
    - `email`, `name`
    - `role`: perfis associados (`USUARIO_PERFIL` + `PERFIL`)
- Protege endpoints `/api/*` com Bearer JWT
- Swagger configurado com esquema de segurança `Bearer`

### 👤 Usuários & Perfis

- CRUD de usuários via `/api/usuarios`
- `/api/usuarios/search` com paginação e ordenação genérica
- Cada usuário pode possuir múltiplos perfis (roles)
- Campos como neurodivergência, observações de suporte e status ativo

### 🎧 Preferências Sensoriais

- `POST /api/preferencias` para registrar preferências por usuário
- `GET /api/preferencias/usuario/{usuarioId}` lista o histórico / preferências ativas
- Campos: ruído máximo, faixa de lux, tolerância visual, zona preferida

### 🪑 Estações de Trabalho & Zonas

- CRUD de estações via `/api/estacoes`
- `/api/estacoes/search` com:
  - `pageNumber`, `pageSize`
  - filtros por `zonaSensorialId`, `status`, `codigo`
  - ordenação por campo (`sortBy`, `sortDirection`)
- HATEOAS nas respostas de estação:
  - `self`, `reservas`, `indice_conforto`, `taxa_ocupacao`

### 📊 Reservas & Avaliações

- CRUD de reservas em `/api/reservas`
- `/api/reservas/search` com filtros por:
  - `usuarioId`, `estacaoTrabalhoId`, `statusReservaId`
  - intervalo de datas (`inicio`, `fim`)
- Endpoints específicos:
  - `PATCH /api/reservas/{id}/status` – atualização de status (ex.: cancelamento)
  - `GET /api/reservas/{id}/compatibilidade` – consulta função Oracle `FNC_RESERVA_COMPATIVEL`
  - `POST /api/reservas/{id}/avaliacao` – registra avaliação de conforto da reserva
- HATEOAS nas reservas:
  - `self`, `estacao`, `avaliacao`

### 📈 Integração com Oracle (Functions/Procedures)

Via repositórios e ADO.NET/EF Core:

- `GET /api/estacoes/{id}/indice-conforto`
  - Chama `FNC_CALC_INDICE_CONFORTO(p_id_estacao, p_data_inicio, p_data_fim)`
- `GET /api/estacoes/{id}/taxa-ocupacao`
  - Chama `FNC_TAXA_OCUPACAO_ESTACAO(p_id_estacao, p_data_inicio, p_data_fim)`
- `GET /api/reservas/{id}/compatibilidade`
  - Chama `FNC_RESERVA_COMPATIVEL(p_id_reserva)`

---

## 🛠️ Tecnologias Utilizadas

- **Backend:** .NET 7, ASP.NET Core Web API
- **Autenticação:** JWT (System.IdentityModel.Tokens.Jwt, JwtBearer)
- **ORM:** EF Core 7 + Oracle Entity Framework Core 7.21.12
- **Banco:** Oracle Database 
- **Documentação:** Swagger / Swashbuckle
- **Padrões & Arquitetura:**
  - Clean-ish Layered Architecture
  - Repository Pattern
  - DTO/ViewModel + Services de Aplicação
  - ProblemDetails + Middleware global de exceções
  - HATEOAS para recursos principais

---

## 📂 Estrutura da Solução

```text
NeuroSync.sln
└── src/
    ├── NeuroSync.Domain/
    │   ├── Entities/
    │   │   ├── Empresa, Filial, Setor
    │   │   ├── Usuario, Perfil, UsuarioPerfil
    │   │   ├── PreferenciaSensorial
    │   │   ├── ZonaSensorial, EstacaoTrabalho
    │   │   ├── TipoSensor, Sensor, LeituraSensor
    │   │   ├── StatusReserva, ReservaEstacao, AvaliacaoEstacao
    │   │   └── BaseEntity (Id, DataCadastro, Ativo)
    │   └── Repositories/
    │       ├── IRepository<T>
    │       ├── IUsuarioRepository
    │       ├── IEstacaoTrabalhoRepository
    │       └── IReservaEstacaoRepository
    │
    ├── NeuroSync.Application/
    │   ├── Common/
    │   │   ├── BusinessException
    │   │   ├── JwtSettings
    │   │   └── PasswordHasher (SHA-256)
    │   ├── DTOs/
    │   │   ├── Auth/ (LoginRequestDto, AuthResponseDto)
    │   │   ├── Usuarios/ (Create/Update/UsuarioDto)
    │   │   ├── Estacoes/ (Create, Search, EstacaoTrabalhoDto + HATEOAS)
    │   │   ├── Reservas/ (Create, Search, ReservaEstacaoDto + HATEOAS,
    │   │   │              CompatibilidadeReservaDto, AvaliacaoEstacaoDto)
    │   │   └── Preferencias/ (CreatePreferenciaSensorialDto, PreferenciaSensorialDto)
    │   ├── Responses/
    │   │   ├── PagedResult<T>
    │   │   └── LinkDto (HATEOAS)
    │   └── Services/
    │       ├── IAuthService, AuthService
    │       ├── IUsuarioService, UsuarioService
    │       ├── IEstacaoService, EstacaoService
    │       ├── IReservaService, ReservaService
    │       └── IPreferenciaSensorialService, PreferenciaSensorialService
    │
    ├── NeuroSync.Infrastructure/
    │   ├── Persistence/
    │   │   ├── NeuroSyncDbContext
    │   │   └── NeuroSyncDbContextFactory (design-time / migrations)
    │   ├── Repositories/
    │   │   ├── EfRepository<T>
    │   │   ├── UsuarioRepository
    │   │   ├── EstacaoTrabalhoRepository (functions Oracle)
    │   │   └── ReservaEstacaoRepository (functions Oracle)
    │   ├── Migrations/
    │   │   └── 20251125000100_InitialCreate.cs
    │   └── DependencyInjection.cs
    │
    ├── NeuroSync.Api/
    │   ├── Controllers/
    │   │   ├── AuthController
    │   │   ├── UsuariosController
    │   │   ├── EstacoesController
    │   │   ├── ReservasController
    │   │   └── PreferenciasController
    │   ├── Middleware/
    │   │   └── ExceptionHandlingMiddleware (ProblemDetails customizado)
    │   ├── appsettings.json / appsettings.Development.json
    │   └── Program.cs (minimal hosting, DI, Auth, Swagger)
    │
    └── postman/
        ├── NeuroSync.postman_collection.json
        └── NeuroSync.postman_environment.json
```

---

## 🧱 Arquitetura & Decisões

### 1) Domínio & Arquitetura

- Entidades alinhadas 1:1 com o DDL Oracle fornecido
- Regras de negócio:
  - Métodos em entidades (`Usuario.RegistrarTentativaFalha`, `ReservaEstacao.AtualizarStatus`, etc.)
  - Regras adicionais encapsuladas em Services de Aplicação (ex.: validação de intervalo de datas da reserva)
- Uso de `BaseEntity` para padronizar `Id`, `DataCadastro`, `Ativo` (mapeado ou ignorado conforme existência no banco)
- Repositórios no domínio (`IUsuarioRepository`, `IEstacaoTrabalhoRepository`, `IReservaEstacaoRepository`) + implementações em Infrastructure

### 2) Aplicação

- Serviços de aplicação bem definidos:
  - `AuthService` (login + geração de JWT)
  - `UsuarioService`, `EstacaoService`, `ReservaService`, `PreferenciaSensorialService`
- DTOs específicos para entrada/saída:
  - Nunca expõe entidades diretamente via controllers
- Tratamento de erros:
  - `BusinessException` mapeada para ProblemDetails via `ExceptionHandlingMiddleware`
  - `[ApiController] + Data Annotations` → `ValidationProblemDetails` automático
- Respostas paginadas (`PagedResult<T>`) e HATEOAS (`LinkDto`) nos recursos principais

### 3) Infra & Dados

- EF Core + Oracle:
  - `NeuroSyncDbContext` com `ToTable`, `HasColumnName`, `HasMaxLength`, `HasColumnType`, etc.
  - Conversão de `bool` ⇔ `'S'/'N'` via `ValueConverter`
  - Relacionamentos `HasOne/HasMany/WithMany` conforme DDL
- Repositórios concretos:
  - `EfRepository<T>` para CRUD genérico
  - Repositórios específicos para queries adicionais e chamadas de functions
- Migrations:
  - Migration inicial `20251125000100_InitialCreate` gerando o schema base
  - Projeto configurado para `dotnet ef database update`

### 4) Camada Web

- Controllers com boas práticas:
  - Injeção de dependência de serviços
  - Retornos tipados (`ActionResult<T>`) com códigos HTTP corretos
- Endpoints `/search`:
  - `/api/usuarios/search`
  - `/api/estacoes/search`
  - `/api/reservas/search`
  - Suporte a paginação, ordenação e filtros por querystring
- HATEOAS:
  - Estações e Reservas retornam links de navegação (`self`, `reservas`, `avaliacao`, métricas)
- Autenticação/Autorização:
  - `AuthController` público
  - Demais controllers `[Authorize]` com esquema Bearer

### 5) Documentação (README + Swagger)

- Este README documenta:
  - Visão geral, arquitetura, como rodar, variáveis, endpoints principais, exemplos curl
- Swagger:
  - Habilitado em `Program.cs` (ambiente Development)
  - Com definição de segurança JWT para teste de endpoints autenticados

---

## 🚀 Como Rodar o Projeto

### ✅ Pré-requisitos

- .NET SDK 7 instalado
- Oracle Database acessível com o schema/tabelas do DDL fornecido
- Connection string válida no `appsettings.Development.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "User Id=USUARIO;Password=SENHA;Data Source=servidor:1521/SERVICE"
},
"Jwt": {
  "SecretKey": "chave-super-secreta-32+chars",
  "Issuer": "neuro-sync-api",
  "Audience": "neuro-sync-clients",
  "ExpirationMinutes": 120
}
```

### 🔧 Passo a Passo (CLI)

1. **Clonar o repositório**

```bash
git clone <url-do-repo>
cd neuro-sync-csharp/neuro-sync
```

2. **Restaurar e compilar**

```bash
dotnet restore
dotnet build
```

3. **Aplicar Migrations no Oracle**

> Certifique-se de que a connection string aponta para o schema correto e o usuário tem permissão de criação de objetos.

```bash
dotnet ef database update \
  --project src/NeuroSync.Infrastructure/NeuroSync.Infrastructure.csproj \
  --startup-project src/NeuroSync.Api/NeuroSync.Api.csproj
```

4. **Rodar a API**

```bash
dotnet run --project src/NeuroSync.Api/NeuroSync.Api.csproj
```

Por padrão (launchSettings), a API expõe HTTP como `http://localhost:5170`.


## 🌐 Rotas / Endpoints Principais

### Auth

- `POST /auth/login`  
  ```bash
  curl --location 'http://localhost:5170/auth/login' \
    --header 'Content-Type: application/json' \
    --data-raw '{
      "email": "henrique.souza@neurosync.com",
      "senha": "HASH_HENRIQUE"
    }'
  ```
  **Resposta (200):** `{ token, expiraEm, usuarioId, nome, email, perfis[] }`

### Usuários

- `GET /api/usuarios/{id}`
- `GET /api/usuarios/search?pageNumber=1&pageSize=20&sortBy=NomeCompleto&sortDirection=asc`
- `POST /api/usuarios`
- `PUT /api/usuarios/{id}`
- `DELETE /api/usuarios/{id}`

Exemplo search:

```bash
curl 'http://localhost:5170/api/usuarios/search?pageNumber=1&pageSize=10' \
  -H "Authorization: Bearer $TOKEN"
```

### Estações de Trabalho

- `GET /api/estacoes/{id}`
- `GET /api/estacoes/search?pageNumber=1&pageSize=20&status=ATIVA&codigo=EST-`
- `POST /api/estacoes`
- `PUT /api/estacoes/{id}`
- `DELETE /api/estacoes/{id}`
- `GET /api/estacoes/{id}/indice-conforto?inicio=2025-01-01T08:00:00Z&fim=2025-01-01T18:00:00Z`
- `GET /api/estacoes/{id}/taxa-ocupacao?inicio=...&fim=...`

### Reservas

- `GET /api/reservas/{id}`
- `GET /api/reservas/search?pageNumber=1&pageSize=10&usuarioId=1`
- `POST /api/reservas`
- `PATCH /api/reservas/{id}/status?statusId=2&motivo=cancelado`
- `GET /api/reservas/{id}/compatibilidade`
- `POST /api/reservas/{id}/avaliacao`
- `DELETE /api/reservas/{id}`

### Preferências Sensoriais

- `POST /api/preferencias`
- `GET /api/preferencias/usuario/{usuarioId}`

---

## 🧪 Como Testar (Swagger & Postman)

### Via Swagger

1. Rodar a API (`dotnet run` ou F5 no Visual Studio).
2. Acessar: `http://localhost:5170/swagger`
3. Primeiro chame `POST /auth/login` com um usuário válido.
4. Copie o token JWT da resposta.
5. No Swagger, botão **Authorize** → cole `Bearer <seu-token>`.
6. Teste endpoints protegidos (`/api/*`).

### Via Postman

- Coleção: `postman/NeuroSync.postman_collection.json`
- Environment: `postman/NeuroSync.postman_environment.json`

Passos:

1. Importe o environment **Neuro-Sync Local**.
2. Importe a coleção **Neuro-Sync API**.
3. Selecione o environment.
4. Rode `Auth → POST /auth/login` e copie o token para a variável `jwt` do environment.
5. Use os grupos **Usuarios**, **Estacoes**, **Reservas**, **Preferencias Sensoriais** para testar o fluxo completo.


## 👥 Equipe

- **Gabriel Cruz** – RM 559613  
- **Kauã Ferreira** – RM 560992  
- **Vinicius Bitú** – RM 560227  

Neuro-Sync © 2025 – Global Solution FIAP
