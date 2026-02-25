# Arquitetura e Design de Software
## Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência

**Versão:** 1.0 | **Data:** 24 de Fevereiro de 2026 | **Projeto:** LI4 2025/2026

---

## 1. Visão Geral da Arquitetura

O SGCLC adota uma **arquitetura em 3 camadas** (Three-Tier Architecture) com separação clara de responsabilidades:

```
┌─────────────────────────────────────┐
│    CAMADA DE APRESENTAÇÃO (UI)      │  ← Blazor Server (Razor Components)
│    Blazor Components + Layouts      │
├─────────────────────────────────────┤
│    CAMADA DE LÓGICA DE NEGÓCIO      │  ← Services, Domain Logic
│    Application Services + Domain    │
├─────────────────────────────────────┤
│    CAMADA DE DADOS                  │  ← EF Core + SQLite/SQL Server
│    Repositories + DbContext         │
└─────────────────────────────────────┘
```

O sistema segue o padrão **Repository + Service Layer**, onde:
- **Repositories** encapsulam o acesso à base de dados
- **Services** implementam a lógica de negócio
- **Blazor Components** consomem os serviços e gerem o estado da UI

---

## 2. Diagrama de Componentes

```mermaid
graph TB
    subgraph Cliente["🌐 Browser (Cliente)"]
        UI["Blazor Components\n(Razor UI)"]
    end

    subgraph Servidor["🖥️ Servidor ASP.NET Core"]
        BL["Blazor Server Hub\n(SignalR)"]
        
        subgraph AppLayer["Application Layer"]
            AuthSvc["AuthService"]
            ProdSvc["ProductService"]
            StockSvc["StockService"]
            SalesSvc["SalesService"]
            OrderSvc["OrderService"]
            InvSvc["InvoiceService"]
            ConsSvc["ConsolidationService"]
            RepSvc["ReportService"]
        end

        subgraph DataLayer["Data Layer (EF Core)"]
            UoW["UnitOfWork"]
            ProdRepo["ProductRepository"]
            StockRepo["StockRepository"]
            SalesRepo["SalesRepository"]
            OrderRepo["OrderRepository"]
            ConRepo["ConsolidationRepository"]
        end

        Jobs["Background Jobs\n(Hosted Service)"]
    end

    subgraph BD["🗄️ Base de Dados"]
        DB[("SQLite / SQL Server\nSGCLC_DB")]
    end

    UI <-->|SignalR| BL
    BL --> AppLayer
    AppLayer --> UoW
    UoW --> ProdRepo
    UoW --> StockRepo
    UoW --> SalesRepo
    UoW --> OrderRepo
    UoW --> ConRepo
    DataLayer -->|EF Core| DB
    Jobs -->|trigger| ConsSvc
```

---

## 3. Diagrama de Classes (Domínio)

```mermaid
classDiagram
    class Loja {
        +int Id
        +string Nome
        +string Morada
        +string Telefone
        +string Email
        +bool Ativa
        +List~Stock~ Stocks
        +List~Venda~ Vendas
        +List~Encomenda~ Encomendas
        +List~Consolidacao~ Consolidacoes
        +List~Utilizador~ Utilizadores
    }

    class Produto {
        +int Id
        +string Codigo
        +string Nome
        +string Descricao
        +decimal PrecoCusto
        +decimal PrecoBaseVenda
        +string UnidadeMedida
        +string Foto
        +bool Ativo
        +int CategoriaId
        +Categoria Categoria
    }

    class Categoria {
        +int Id
        +string Nome
        +int? CategoriaPaiId
        +Categoria CategoriaPai
        +List~Categoria~ SubCategorias
        +List~Produto~ Produtos
    }

    class Stock {
        +int Id
        +int LojaId
        +int ProdutoId
        +decimal Quantidade
        +decimal StockMinimo
        +decimal? PrecoVendaLocal
        +Loja Loja
        +Produto Produto
        +bool EmAlerta()
    }

    class Venda {
        +int Id
        +int LojaId
        +int FuncionarioId
        +DateTime DataHora
        +decimal SubTotal
        +decimal TotalDesconto
        +decimal Total
        +EstadoVenda Estado
        +List~LinhaVenda~ Linhas
        +Loja Loja
        +Utilizador Funcionario
    }

    class LinhaVenda {
        +int Id
        +int VendaId
        +int ProdutoId
        +decimal Quantidade
        +decimal PrecoUnitario
        +decimal Desconto
        +decimal SubTotal()
        +Produto Produto
    }

    class Fornecedor {
        +int Id
        +string Nome
        +string NIF
        +string Morada
        +string Telefone
        +string Email
        +bool Ativo
        +List~Encomenda~ Encomendas
    }

    class Encomenda {
        +int Id
        +int LojaId
        +int FornecedorId
        +DateTime DataCriacao
        +DateTime? DataRececao
        +EstadoEncomenda Estado
        +string Observacoes
        +List~LinhaEncomenda~ Linhas
        +Loja Loja
        +Fornecedor Fornecedor
    }

    class LinhaEncomenda {
        +int Id
        +int EncomendaId
        +int ProdutoId
        +decimal QuantidadePedida
        +decimal? QuantidadeRecebida
        +Produto Produto
    }

    class Fatura {
        +int Id
        +string Numero
        +int LojaId
        +int? VendaId
        +string NomeCliente
        +string NIFCliente
        +DateTime DataEmissao
        +decimal Total
        +EstadoFatura Estado
        +List~LinhaFatura~ Linhas
    }

    class Consolidacao {
        +int Id
        +int LojaId
        +DateTime DataConsolidacao
        +DateTime DataHoraExecucao
        +decimal TotalVendas
        +int NumeroTransacoes
        +decimal TotalDescontos
        +ResultadoConsolidacao Resultado
        +string? ErroDetalhes
    }

    class Utilizador {
        +string Id
        +string Nome
        +string Email
        +PapelUtilizador Papel
        +int? LojaId
        +bool Ativo
        +Loja? Loja
    }

    class AjusteStock {
        +int Id
        +int LojaId
        +int ProdutoId
        +string UtilizadorId
        +decimal Variacao
        +string Motivo
        +DateTime DataHora
    }

    Loja "1" --> "*" Stock
    Loja "1" --> "*" Venda
    Loja "1" --> "*" Encomenda
    Loja "1" --> "*" Consolidacao
    Loja "1" --> "*" Utilizador
    Produto "1" --> "*" Stock
    Produto "1" --> "*" LinhaVenda
    Produto "1" --> "*" LinhaEncomenda
    Categoria "1" --> "*" Produto
    Categoria "0..1" --> "*" Categoria : subcategorias
    Venda "1" --> "*" LinhaVenda
    Encomenda "1" --> "*" LinhaEncomenda
    Fornecedor "1" --> "*" Encomenda
    Venda "0..1" --> "0..1" Fatura
```

---

## 4. Diagrama de Sequência – Registo de Venda

```mermaid
sequenceDiagram
    actor FN as Funcionário
    participant UI as Blazor POS
    participant SS as SalesService
    participant SK as StockService
    participant DB as Base de Dados

    FN->>UI: Pesquisa produto (código/nome)
    UI->>SS: SearchProductsAsync(query)
    SS->>DB: SELECT produtos WHERE search
    DB-->>SS: Lista de produtos
    SS-->>UI: Resultado
    UI-->>FN: Mostra produtos encontrados

    FN->>UI: Adiciona produto ao carrinho (qty)
    UI->>SK: CheckStockAsync(lojaId, produtoId, qty)
    SK->>DB: SELECT Stock WHERE LojaId, ProdutoId
    DB-->>SK: Stock atual
    alt Stock suficiente
        SK-->>UI: OK
        UI-->>FN: Produto adicionado; total atualizado
    else Stock insuficiente
        SK-->>UI: Erro: stock insuficiente
        UI-->>FN: Alerta de stock
    end

    FN->>UI: Confirma venda
    UI->>SS: CreateSaleAsync(lojaId, userId, carrinho)
    SS->>DB: BEGIN TRANSACTION
    SS->>DB: INSERT Venda + LinhaVenda
    SS->>SK: DeductStockAsync(lojaId, linhas)
    SK->>DB: UPDATE Stock SET Quantidade -= qty (por linha)
    DB-->>SK: OK
    SS->>DB: COMMIT TRANSACTION
    DB-->>SS: Venda criada
    SS-->>UI: VendaDTO (id, total, linhas)
    UI-->>FN: Recibo gerado / Venda concluída
```

---

## 5. Diagrama de Sequência – Consolidação Diária

```mermaid
sequenceDiagram
    participant JOB as Background Job
    participant CS as ConsolidationService
    participant LS as LojaService
    participant SS as SalesService
    participant DB as Base de Dados

    JOB->>CS: TriggerConsolidationAsync(data)
    CS->>LS: GetActiveStoresAsync()
    LS->>DB: SELECT Lojas WHERE Ativa = true
    DB-->>CS: Lista de lojas

    loop Para cada loja
        CS->>SS: GetDailySalesAsync(lojaId, data)
        SS->>DB: SELECT Vendas WHERE LojaId AND Data
        DB-->>CS: Vendas do dia
        CS->>CS: Calcula: TotalVendas, NrTransacoes, TotalDescontos
        CS->>DB: INSERT Consolidacao(LojaId, Data, Totais, Sucesso)
        DB-->>CS: OK
    end

    CS->>DB: UPDATE Dashboard KPIs
    CS-->>JOB: Relatório de consolidação
```

---

## 6. Estrutura do Projeto

```
/ConvenienceChain
├── src/
│   ├── ConvenienceChain.Web/                # Blazor Server App
│   │   ├── Components/
│   │   │   ├── Pages/                       # Páginas Blazor (.razor)
│   │   │   │   ├── Dashboard/
│   │   │   │   ├── POS/
│   │   │   │   ├── Products/
│   │   │   │   ├── Stock/
│   │   │   │   ├── Orders/
│   │   │   │   ├── Invoices/
│   │   │   │   ├── Reports/
│   │   │   │   └── Admin/
│   │   │   └── Shared/                      # Layout, NavMenu, etc.
│   │   ├── wwwroot/                         # CSS, JS, imagens estáticas
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── ConvenienceChain.Core/               # Domínio e Lógica de Negócio
│   │   ├── Entities/                        # Entidades de domínio
│   │   ├── Interfaces/                      # IRepository, IService
│   │   ├── Services/                        # Application Services
│   │   ├── DTOs/                            # Data Transfer Objects
│   │   └── Enums/
│   │
│   └── ConvenienceChain.Data/               # Infraestrutura de Dados
│       ├── Context/                         # AppDbContext (EF Core)
│       ├── Repositories/                    # Implementações de Repository
│       ├── Migrations/
│       └── Seed/                            # Dados iniciais (seed data)
│
├── tests/
│   ├── ConvenienceChain.Tests.Unit/
│   └── ConvenienceChain.Tests.Integration/
│
└── docs/
    ├── etapa1/
    ├── etapa2/
    ├── etapa3/
    └── etapa4/
```

---

## 7. Modelo de Base de Dados (Esquema ER Simplificado)

```mermaid
erDiagram
    LOJA {
        int Id PK
        string Nome
        string Morada
        string Telefone
        bool Ativa
    }
    UTILIZADOR {
        string Id PK
        string Nome
        string Email
        string Papel
        int LojaId FK
    }
    CATEGORIA {
        int Id PK
        string Nome
        int CategoriaPaiId FK
    }
    PRODUTO {
        int Id PK
        string Codigo UK
        string Nome
        decimal PrecoCusto
        decimal PrecoBaseVenda
        int CategoriaId FK
        bool Ativo
    }
    STOCK {
        int Id PK
        int LojaId FK
        int ProdutoId FK
        decimal Quantidade
        decimal StockMinimo
        decimal PrecoVendaLocal
    }
    VENDA {
        int Id PK
        int LojaId FK
        string FuncionarioId FK
        datetime DataHora
        decimal Total
        string Estado
    }
    LINHA_VENDA {
        int Id PK
        int VendaId FK
        int ProdutoId FK
        decimal Quantidade
        decimal PrecoUnit
        decimal Desconto
    }
    FORNECEDOR {
        int Id PK
        string Nome
        string NIF
        string Email
        bool Ativo
    }
    ENCOMENDA {
        int Id PK
        int LojaId FK
        int FornecedorId FK
        datetime DataCriacao
        string Estado
    }
    LINHA_ENCOMENDA {
        int Id PK
        int EncomendaId FK
        int ProdutoId FK
        decimal QtyPedida
        decimal QtyRecebida
    }
    CONSOLIDACAO {
        int Id PK
        int LojaId FK
        date DataConsolidacao
        decimal TotalVendas
        int NrTransacoes
        string Resultado
    }
    FATURA {
        int Id PK
        string Numero UK
        int LojaId FK
        int VendaId FK
        string NomeCliente
        string NIFCliente
        decimal Total
    }

    LOJA ||--o{ UTILIZADOR : "tem"
    LOJA ||--o{ STOCK : "tem"
    LOJA ||--o{ VENDA : "regista"
    LOJA ||--o{ ENCOMENDA : "faz"
    LOJA ||--o{ CONSOLIDACAO : "tem"
    PRODUTO ||--o{ STOCK : "está em"
    PRODUTO ||--o{ LINHA_VENDA : "incluído em"
    PRODUTO ||--o{ LINHA_ENCOMENDA : "incluído em"
    CATEGORIA ||--o{ PRODUTO : "classifica"
    CATEGORIA ||--o{ CATEGORIA : "subcategoria de"
    VENDA ||--o{ LINHA_VENDA : "tem"
    VENDA ||--o| FATURA : "pode gerar"
    ENCOMENDA ||--o{ LINHA_ENCOMENDA : "tem"
    FORNECEDOR ||--o{ ENCOMENDA : "recebe"
```

---

## 8. Decisões de Design

| Decisão | Escolha | Justificação |
|---|---|---|
| Framework | Blazor Server (.NET 8) | Produtividade, estado no servidor, compatível com .NET ecosystem |
| ORM | Entity Framework Core 8 | Code-first, migrations, LINQ queries, suporte a SQLite e SQL Server |
| BD Desenvolvimento | SQLite | Zero configuração, ficheiro local, ideal para desenvolvimento |
| BD Produção | SQL Server / PostgreSQL | Robustez, escalabilidade, suporte a transações concorrentes |
| Autenticação | ASP.NET Core Identity | Integrado, cookie-based, suporte a RBAC nativo |
| Jobs Agendados | .NET Hosted Services (IHostedService) | Integrado no ecossistema, sem dependências externas |
| Padrão de Acesso a Dados | Repository + Unit of Work | Testabilidade, separação de concerns, facilidade de mock em testes |
| Relatórios PDF | QuestPDF | Open-source, fluent API, sem dependência de Office |
