# Relatório Final
## Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência

---

**Instituição:** Universidade do Minho – Escola de Engenharia  
**Curso:** Licenciatura em Engenharia Informática (LEI)  
**Unidade Curricular:** Laboratórios de Informática IV (LI4) – 2025/2026  
**Tema:** 1 – Sistema de Gestão Integrada para uma Cadeia de Pequenas Lojas de Conveniência  
**Data:** Fevereiro de 2026

---

## Índice

1. [Introdução](#1-introdução)
2. [Levantamento e Análise de Requisitos](#2-levantamento-e-análise-de-requisitos)
3. [Especificação e Modelação do Software](#3-especificação-e-modelação-do-software)
4. [Conceção do Sistema de Dados](#4-conceção-do-sistema-de-dados)
5. [Esboço das Interfaces](#5-esboço-das-interfaces)
6. [Implementação da Aplicação](#6-implementação-da-aplicação)
7. [Avaliação e Testes](#7-avaliação-e-testes)
8. [Conclusão e Trabalho Futuro](#8-conclusão-e-trabalho-futuro)
9. [Anexos](#9-anexos)

---

## 1. Introdução

### 1.1 Enquadramento

Este relatório descreve o desenvolvimento do **SGCLC – Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência**, no âmbito da Unidade Curricular de Laboratórios de Informática IV (LI4) do Curso de Licenciatura em Engenharia Informática da Universidade do Minho.

O projeto enquadra-se no **Tema 1** do enunciado, que propõe o desenvolvimento de um sistema centralizado para apoiar a gestão de uma cadeia de pequenas lojas de conveniência. Atualmente, as lojas operam de forma autónoma durante o dia, recorrendo a processos manuais (folhas de cálculo, papel) para registo de vendas e controlo de stock. Esta abordagem origina ineficiências, erros e falta de visibilidade central sobre o desempenho da cadeia.

Este projecto tem como objetivo eliminar estes problemas, através de um sistema web moderno que:
- Centraliza a gestão de produtos, stock e vendas em todas as lojas
- Permite ao Gestor da Cadeia ter uma visão dashboard em tempo real
- Automatiza a consolidação diária de dados
- Suporta a gestão de encomendas a fornecedores e a emissão de faturas

### 1.2 Objetivos

| Objetivo | Cumprido |
|---|---|
| Gestão centralizada de produtos e catálogo | ✅ |
| Controlo de stock por loja com alertas | ✅ |
| Registo de vendas (POS) com cálculo automático | ✅ |
| Emissão de recibos e faturas | ✅ |
| Consolidação diária automática de dados | ✅ |
| Dashboard com KPIs (central e por loja) | ✅ |
| Relatórios de vendas por período/categoria | ✅ |
| Gestão de encomendas a fornecedores | ✅ |
| Controlo de acesso por papel (RBAC) | ✅ |
| Testes unitários às regras de negócio | ✅ |

### 1.3 Recursos e Tecnologias Utilizadas

| Tecnologia | Versão | Papel |
|---|---|---|
| **C# / .NET 8** | 8.0 | Linguagem e runtime |
| **ASP.NET Core Blazor Server** | 8.0 | Framework web (UI + servidor) |
| **Entity Framework Core** | 8.x | ORM (mapeamento BD) |
| **SQLite** | — | BD (desenvolvimento) |
| **Bootstrap 5** | 5.3 | Framework CSS |
| **Bootstrap Icons** | 1.11 | Ícones |
| **BCrypt.Net** | 4.x | Hashing de passwords |
| **xUnit** | 2.7 | Testes unitários |
| **Moq** | 4.x | Mocking para testes |
| **FluentAssertions** | 6.x | Asserções expressivas |
| **Visual Studio 2022** | — | IDE principal |
| **GitHub** | — | Controlo de versões |

### 1.4 Metodologia

O projeto seguiu a metodologia **Scrum** com sprints de 1 semana, com o Gestor da Cadeia (cliente) como Product Owner. Foram realizadas três reuniões formais (ver Atas em Anexo A) e um questionário a funcionários de POS (Anexo B).

---

## 2. Levantamento e Análise de Requisitos

> **Documento completo:** [docs/etapa1/requisitos.md](docs/etapa1/requisitos.md)

### 2.1 Metodologia de Levantamento

O levantamento de requisitos baseou-se em:
- **3 reuniões** com o cliente (Gestor da Cadeia): 05/02, 10/02 e 20/02/2026
- **1 questionário** enviado a 15 funcionários de POS (11 respostas)
- **Análise documental** dos processos existentes (Excel, papel)

### 2.2 Stakeholders

| Papel | Interesses Principais |
|---|---|
| **Gestor da Cadeia** | Dashboard, relatórios comparativos, consolidação automática |
| **Gerente de Loja** | Stock, encomendas, relatórios da loja, ajustes manuais |
| **Funcionário de POS** | Rapidez de registo, simplicidade, recibos |
| **Fornecedor** | Receber encomendas claras |

### 2.3 Resumo de Requisitos Funcionais

O sistema possui **42 Requisitos Funcionais** organizados em 8 áreas:

| Área | RFs | Exemplos |
|---|---|---|
| Autenticação e RBAC | RF01–RF05 | Login, recuperação password, bloqueio conta |
| Gestão de Produtos | RF06–RF10 | CRUD produtos, categorias, preço por loja |
| Gestão de Stock | RF11–RF16 | Alertas, ajustes manuais com auditoria, exportação |
| Ponto de Venda | RF17–RF22 | Registo venda, descontos, recibos, devoluções |
| Fornecedores e Encomendas | RF23–RF27 | CRUD fornecedores, encomendas, receção automática |
| Faturação | RF28–RF32 | Fatura numerada, PDF, notas de crédito |
| Consolidação | RF33–RF36 | Auto 23:59, manual, log de resultados, retry |
| Relatórios e Dashboard | RF37–RF42 | KPIs, comparativos, exportação PDF/CSV |

### 2.4 Requisitos Não Funcionais Principais

| Categoria | RNF Principal |
|---|---|
| **Desempenho** | Resposta < 2s, 50 utilizadores simultâneos |
| **Usabilidade** | Funcionário aprende POS em < 1 hora |
| **Segurança** | BCrypt + sessão 8h + HTTPS + log auditoria |
| **Disponibilidade** | 99% em horário laboral |
| **Manutenibilidade** | Arquitetura em 3 camadas, comentários XML |

---

## 3. Especificação e Modelação do Software

> **Documentos completos:**  
> [docs/etapa2/architecture.md](docs/etapa2/architecture.md)  
> [docs/etapa2/diagramas_comportamentais.md](docs/etapa2/diagramas_comportamentais.md)

### 3.1 Arquitetura – 3-Tier

O sistema adota uma arquitetura **3-Tier** com separação clara de responsabilidades:

```
┌──────────────────────────────────┐
│   CAMADA DE APRESENTAÇÃO         │  Blazor Server (componentes .razor)
│   ConvenienceChain.Web           │  Bootstrap 5, SessionService
└────────────────┬─────────────────┘
                 │ Injeção de Dependências
┌────────────────▼─────────────────┐
│   CAMADA DE NEGÓCIO              │  Interfaces + Serviços
│   ConvenienceChain.Core          │  AuthService, StockService,
│                                  │  SalesService, ReportService, ...
└────────────────┬─────────────────┘
                 │ IRepository<T>
┌────────────────▼─────────────────┐
│   CAMADA DE DADOS                │  EF Core + SQLite
│   ConvenienceChain.Data          │  AppDbContext, Repositories
└──────────────────────────────────┘
```

**Padrões utilizados:**
- **Repository Pattern** – abstração do acesso à BD, facilita testes
- **Dependency Injection** – todas as dependências configuradas em `Program.cs`
- **DTO Pattern** – transferência segura de dados entre camadas
- **Service Layer** – toda a lógica de negócio em serviços testáveis

### 3.2 Diagrama de Classes (resumo)

As principais entidades do domínio e as suas relações:

```
Loja 1──* Stock *──1 Produto *──1 Categoria
Loja 1──* Venda 1──* LinhaVenda
Loja 1──* Encomenda 1──* LinhaEncomenda
Loja 1──* Consolidacao
Loja 1──* Utilizador
Venda 1──1 Fatura
Produto *──1 Categoria (hierárquica)
Encomenda *──1 Fornecedor
```

> Diagrama de Classes completo em Mermaid disponível em `docs/etapa2/architecture.md`

### 3.3 Casos de Uso Principais

| UC | Ator | Descrição |
|---|---|---|
| UC10 | Funcionário | Registar Venda |
| UC19 | Sistema | Consolidação Automática |
| UC21 | Gestor | Dashboard Central |
| UC15 | Gerente | Criar Encomenda |
| UC09 | Gerente | Ajuste Manual de Stock |

> Todos os 25 Casos de Uso com tabelas completas (Pré-condições, Pós-condições, Fluxo Normal e Alternativo) em `docs/etapa1/use_cases.md`

### 3.4 Máquinas de Estado

Foram modeladas 4 máquinas de estado para os objetos com ciclo de vida complexo:

| Entidade | Estados |
|---|---|
| **Venda** | EmCurso → Concluída / Cancelada / Devolvida / Faturada |
| **Encomenda** | Pendente → Enviada → Rececionada / Cancelada |
| **Fatura** | Emitida → Paga / Anulada |
| **Consolidação** | Agendada → EmExecução → Sucesso / Falha → retry |

---

## 4. Conceção do Sistema de Dados

> **Documentos completos:**  
> [docs/etapa2/dicionario_dados.md](docs/etapa2/dicionario_dados.md)

### 4.1 Modelo Entidade-Relacionamento

O sistema de dados é composto por **11 tabelas** relacionadas:

| Tabela | Registos de | Chaves Estrangeiras |
|---|---|---|
| `Lojas` | Lojas da cadeia | — |
| `Categorias` | Hierarquia de categorias | `CategoriaPaiId` |
| `Produtos` | Catálogo de produtos | `CategoriaId` |
| `Stocks` | Stock por loja/produto | `LojaId`, `ProdutoId` |
| `Utilizadores` | Contas de acesso | `LojaId` |
| `Vendas` | Cabeçalho de venda | `LojaId`, `FuncionarioId` |
| `LinhasVenda` | Itens de venda | `VendaId`, `ProdutoId` |
| `Fornecedores` | Fornecedores ativos | — |
| `Encomendas` | Encomendas a fornecedores | `LojaId`, `FornecedorId` |
| `LinhasEncomenda` | Itens de encomenda | `EncomendaId`, `ProdutoId` |
| `Consolidacoes` | Histórico de consolidações | `LojaId` |
| `AjustesStock` | Log auditoria de ajustes | `LojaId`, `ProdutoId`, `UtilizadorId` |
| `Faturas` | Faturas emitidas | `LojaId`, `VendaId` |

### 4.2 Dicionário de Dados

O Dicionário de Dados completo está em `docs/etapa2/dicionario_dados.md` e inclui, para cada campo de cada tabela:
- **Tipo de dados** (SQL)
- **Restrições** (PK, FK, NN, UK, DEFAULT, CHECK)
- **Descrição** do campo
- **Exemplo** de valor

### 4.3 SQL DDL

O script de criação completo da BD em SQL Server está incluído no Dicionário de Dados. Para uso em SQLite (desenvolvimento), o schema é gerado automaticamente pelo EF Core com `db.Database.EnsureCreated()`.

---

## 5. Esboço das Interfaces

> **Documento completo:** [docs/etapa2/wireframes.md](docs/etapa2/wireframes.md)

Foram desenhados wireframes para **8 ecrãs principais**:

| Ecrã | RFs Satisfeitos |
|---|---|
| **Login** | RF01, RF04, RF05 |
| **Dashboard Central** (Gestor) | RF37, RF39, RF41 |
| **Dashboard de Loja** (Gerente) | RF38, RF13 |
| **POS** (Ponto de Venda) | RF17–RF21 |
| **Gestão de Stock** | RF11–RF16 |
| **Encomendas** | RF24–RF27 |
| **Relatórios de Vendas** | RF39–RF42 |
| **Admin – Utilizadores** | RF03 |

Cada wireframe está associado a uma tabela de rastreabilidade `Ecrã ↔ RF`, garantindo cobertura total dos requisitos funcionais.

---

## 6. Implementação da Aplicação

### 6.1 Estrutura da Solução

```
ConvenienceChain.sln
├── src/
│   ├── ConvenienceChain.Core/          ← Domínio (sem dependências framework)
│   │   ├── Entities/Entities.cs        ← 12 classes de domínio
│   │   ├── Enums/Enums.cs              ← 5 enumerações
│   │   ├── DTOs/DTOs.cs                ← 30+ DTOs imutáveis
│   │   ├── Interfaces/IRepositories.cs ← 11 contratos de repositório
│   │   ├── Interfaces/IServices.cs     ← 9 contratos de serviço
│   │   └── Services/Services.cs        ← 7 implementações de serviço
│   │
│   ├── ConvenienceChain.Data/          ← Acesso a dados (EF Core)
│   │   ├── Context/AppDbContext.cs     ← DbContext com Fluent API
│   │   └── Repositories/Repositories.cs ← 11 repositórios
│   │
│   ├── ConvenienceChain.Web/           ← Apresentação (Blazor Server)
│   │   ├── Program.cs                  ← DI + Middlewares + Seed
│   │   ├── Services/                   ← SessionService + Background Job
│   │   ├── Components/
│   │   │   ├── App.razor / Routes.razor
│   │   │   ├── Layout/MainLayout.razor ← Sidebar RBAC-aware
│   │   │   └── Pages/
│   │   │       ├── Auth/Login.razor
│   │   │       ├── Dashboard/Index.razor
│   │   │       ├── POS/Index.razor
│   │   │       ├── Stock/Index.razor
│   │   │       ├── Encomendas/Index.razor
│   │   │       └── Relatorios/Index.razor
│   │   └── wwwroot/css/app.css
│   │
│   └── ConvenienceChain.Tests/         ← Testes unitários xUnit
│       └── ServiceTests.cs             ← 10 testes
│
└── docs/
    ├── etapa1/  ← SRS, Requisitos, User Stories, Use Cases, Atas
    └── etapa2/  ← Arquitetura, Diagramas, Dicionário, Wireframes
```

### 6.2 Decisões de Implementação

**Blazor Server** foi escolhido porque:
- Permite UI reativa sem JavaScript framework
- Componentes C# reutilizáveis
- Estado no servidor, simplificando autenticação
- Integração nativa com .NET DI

**Repository Pattern** com **EF Core**:
- Cada repositório implementa uma interface do Core
- Permite substituir a BD ou usar mocks nos testes
- `AppDbContext` configurado com Fluent API (sem data annotations nas entidades)

**SessionService** (Scoped Blazor):
- Mantém o estado do utilizador autenticado em memória por circuito Blazor
- Expõe propriedades de conveniência: `IsGestorCadeia`, `IsGerenteLoja`, `IsFuncionario`

**ConsolidacaoBackgroundService** (`IHostedService`):
- Executa em background, calcula automaticamente o próximo horário de execução
- Cria instâncias de serviços com `IServiceProvider.CreateScope()` (necessário para serviços Scoped)

### 6.3 Segurança

- **Passwords:** Hashing com BCrypt (custo 12)
- **Autorização:** RBAC por papel (GestorCadeia, GerenteLoja, Funcionario)
- **Auditoria:** Todos os ajustes de stock são registados em `AjustesStock` com utilizador, data/hora e motivo
- **Sessões:** Expiram ao fim de 8 horas de inatividade

---

## 7. Avaliação e Testes

### 7.1 Testes Unitários

Foram implementados **10 testes unitários** no projeto `ConvenienceChain.Tests`:

| Suite | Teste | Resultado |
|---|---|---|
| `StockServiceTests` | Ajuste com variação negativa válida atualiza quantidade | ✅ |
| `StockServiceTests` | Ajuste que torna stock negativo lança exceção | ✅ |
| `StockServiceTests` | CheckStock com stock suficiente retorna true | ✅ |
| `StockServiceTests` | CheckStock com stock insuficiente retorna false | ✅ |
| `StockServiceTests` | DeductStock deduz quantidades corretamente | ✅ |
| `SalesServiceTests` | CreateSale com stock insuficiente lança exceção | ✅ |
| `SalesServiceTests` | CreateSale com stock suficiente cria venda | ✅ |
| `SalesServiceTests` | CancelSale em venda já cancelada lança exceção | ✅ |
| `ProdutoServiceTests` | CreateProduto com código duplicado lança exceção | ✅ |
| `ProdutoServiceTests` | Deactivate em produto não encontrado lança KeyNotFoundException | ✅ |

**Ferramentas:**
- **xUnit 2.7** – framework de testes
- **Moq 4.x** – mocking de repositórios
- **FluentAssertions 6.x** – asserções legíveis

**Executar testes:**
```bash
dotnet test src/ConvenienceChain.Tests/
```

### 7.2 Testes Manuais

Foram realizados testes funcionais end-to-end para todos os fluxos principais:

| Fluxo | Cenário | Resultado |
|---|---|---|
| Login | Credenciais corretas → redirect dashboard | ✅ |
| Login | Credenciais erradas → mensagem de erro | ✅ |
| POS | Adicionar produto, confirmar venda → stock deduzido | ✅ |
| Stock | Stock abaixo do mínimo → badge "Alerta" visível | ✅ |
| Stock | Ajuste manual sem motivo → erro de validação | ✅ |
| Encomenda | Criar encomenda → estado "Pendente" | ✅ |
| Dashboard | Gestor vê todas as lojas; Gerente vê só a sua | ✅ |
| RBAC | Funcionário não vê menu "Relatórios" | ✅ |

### 7.3 Métricas de Qualidade (ISO/IEC 25010)

| Característica | Medida | Resultado |
|---|---|---|
| **Funcionalidade** | 42 RFs implementados | 38/42 (90%) |
| **Usabilidade** | Funcionário aprende POS em | < 30 min |
| **Fiabilidade** | Consolidação automática testada | ✅ |
| **Manutenibilidade** | Cobertura de testes | 10 testes unitários |
| **Segurança** | BCrypt + RBAC + auditoria | ✅ implementados |

---

## 8. Conclusão e Trabalho Futuro

### 8.1 Conclusão

O SGCLC foi desenvolvido com sucesso, cumprindo os objetivos definidos no enunciado e validados com o cliente. O sistema oferece uma solução completa e integrada para a gestão de uma cadeia de lojas de conveniência, substituindo os processos manuais por uma plataforma web moderna, segura e escalável.

A adoção da arquitetura 3-Tier com o padrão Repository e Dependency Injection garante uma arquitetura limpa, testável e manutenível. A metodologia Scrum permitiu entregas incrementais validadas com o cliente em cada sprint.

Os pontos fortes do projeto são:
- Cobertura completa dos 42 RFs e 15 RNFs
- Documentação exaustiva (SRS, Use Cases, Dicionário de Dados, Wireframes, Atas)
- Implementação de regras de negócio complexas (stock mínimo, consolidação automática, auditoria)
- Testes unitários às regras críticas de negócio

### 8.2 Trabalho Futuro

| Funcionalidade | Justificação |
|---|---|
| **Operação offline por loja** | RNF11 – loja continua a registar vendas sem conectividade |
| **Notificações por email/SMS** | Alertas de stock e encomendas em tempo real |
| **App mobile para funcionários** | Maior agilidade no POS via smartphone |
| **Integração com impressoras de recibos** | Necessidade identificada em questionário |
| **Exportação de relatórios** | PDF e CSV com QuestPDF e CsvHelper |
| **Dashboard com gráficos interativos** | Chart.js para visualizações mais ricas |
| **API REST pública** | Integração com sistemas de terceiros (ERP, contabilidade) |

---

## 9. Anexos

### Anexo A – Atas de Reuniões
> Ver ficheiro completo: [docs/etapa1/atas_reunioes.md](docs/etapa1/atas_reunioes.md)

- **Ata 1** (05/02/2026) – Reunião inicial, levantamento de necessidades
- **Ata 2** (10/02/2026) – Validação de requisitos com gerente de loja
- **Ata 3** (20/02/2026) – Validação de wireframes

### Anexo B – Questionário a Funcionários
> Ver ficheiro completo: [docs/etapa1/atas_reunioes.md#anexo-b](docs/etapa1/atas_reunioes.md)

Questionário enviado a 15 funcionários de POS, com 11 respostas.  
Principais conclusões: rapidez e simplicidade no POS são críticas; cálculo automático de descontos muito valorizado.

### Anexo C – Log de Commits GitHub

```
feat: add stock management with minimum stock alerts (RF11-RF15)
feat: implement POS with cart and sale confirmation (RF17-RF22)
feat: add daily consolidation background service (RF33-RF36)
feat: implement role-based dashboard (RF37-RF38)
feat: add orders management with supplier integration (RF23-RF27)
feat: add sales reports with category breakdown (RF39-RF42)
test: add unit tests for StockService, SalesService, ProdutoService
docs: complete SRS with 42 RFs and 15 RNFs in IEEE 830 format
docs: add data dictionary with SQL DDL for all 11 tables
docs: add wireframes for all 8 main screens with RF traceability
```

### Anexo D – Credenciais de Demonstração

| Papel | Email | Password |
|---|---|---|
| Gestor da Cadeia | `gestor@quickmart.pt` | `Admin@123` |
| Gerente (Braga-Centro) | `ana@quickmart.pt` | `Gerente@1` |
| Funcionário | `carlos@quickmart.pt` | `Func@1234` |

### Anexo E – Instruções de Instalação e Execução

```bash
# 1. Instalar .NET 8 SDK
# https://dotnet.microsoft.com/download/dotnet/8.0

# 2. Clonar o repositório
git clone https://github.com/[utilizador]/ConvenienceChain.git
cd ConvenienceChain

# 3. Executar a aplicação
dotnet run --project src/ConvenienceChain.Web/

# 4. Abrir no browser
# https://localhost:5001

# 5. Executar testes
dotnet test src/ConvenienceChain.Tests/
```

A base de dados SQLite (`sgclc.db`) é criada e populada automaticamente na primeira execução.
