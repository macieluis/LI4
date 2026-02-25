# SGCLC – Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência

> **LI4 2024/2025 · Universidade do Minho**  
> Tema 1 – Sistema de Gestão para Cadeia de Conveniência (QuickMart)

---

## 📋 Sobre o Projeto

O **SGCLC** é um sistema web desenvolvido para substituir os processos manuais de gestão da cadeia de lojas de conveniência **QuickMart**, centralizando operações de ponto de venda, stock, encomendas, faturação e relatórios numa única plataforma.

### Funcionalidades Implementadas

| Módulo | Descrição | Rota |
|---|---|---|
| 🔐 **Autenticação** | Login com RBAC (3 papéis) | `/login` |
| 📊 **Dashboard** | KPIs em tempo real por loja/cadeia | `/dashboard` |
| 🛒 **Ponto de Venda (POS)** | Registo de vendas, descontos, recibos | `/pos` |
| 📦 **Stock** | Alertas, ajuste manual com auditoria | `/stock` |
| 🚚 **Encomendas** | Criar, receber e cancelar encomendas | `/encomendas` |
| 🧾 **Faturas** | Listagem de faturas emitidas | `/faturas` |
| 📈 **Relatórios** | Vendas por período, categoria, top produtos | `/relatorios` |
| 🔄 **Consolidação** | Encerramento diário automático/manual | `/consolidacao` |
| 👥 **Utilizadores** | Gestão de contas (Gestor apenas) | `/admin/utilizadores` |

---

## 🚀 Como Executar — Passo a Passo

### 1. Instalar o .NET 8 SDK

> Se já tens o .NET 8 instalado (verifica com `dotnet --version`), salta para o passo 2.

**macOS (via Homebrew):**
```bash
brew install --cask dotnet-sdk
```
ou descarrega o instalador em: https://dotnet.microsoft.com/download/dotnet/8.0

**Windows:**
Descarrega e executa o instalador `.exe` em: https://dotnet.microsoft.com/download/dotnet/8.0

**Linux (Ubuntu/Debian):**
```bash
sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0
```

Verificar instalação:
```bash
dotnet --version   # deve mostrar 8.0.x
```

---

### 2. Obter o código

```bash
# Opção A — clonar via Git (se tiverem acesso ao repositório)
git clone <URL_DO_REPO>
cd LI4

# Opção B — abrir a pasta onde está o projeto
cd /caminho/para/pasta/LI4
```

---

### 3. Iniciar a aplicação

```bash
dotnet run --project src/ConvenienceChain.Web/
```

Aguarda até aparecer a linha:
```
Now listening on: http://localhost:5000
```

Depois abre o browser em: **http://localhost:5000**

> **Nota macOS com Homebrew:** se o `dotnet` não for encontrado, usa:
> ```bash
> export PATH="$PATH:/usr/local/share/dotnet"
> dotnet run --project src/ConvenienceChain.Web/
> ```

---

### 4. Fazer login

| Papel | Email | Password |
|---|---|---|
| **Gestor da Cadeia** | `gestor@quickmart.pt` | `Admin@123` |
| **Gerente de Loja** | `ana@quickmart.pt` | `Gerente@1` |
| **Funcionário** | `carlos@quickmart.pt` | `Func@1234` |

> A base de dados (`sgclc.db`) é criada e populada automaticamente na **primeira** execução com 5 lojas, 15 produtos, 5 utilizadores e stocks de demonstração. **Não é necessária qualquer configuração adicional.**

---

### 5. Parar o servidor

Prima **Ctrl+C** no terminal onde o servidor está a correr.

---

### 6. Executar os Testes Unitários

```bash
dotnet test src/ConvenienceChain.Tests/
# Resultado esperado: Passed: 10, Failed: 0
```

---

### 🔧 Resolução de Problemas

**"Login do Gestor não funciona"**  
A base de dados pode estar corrompida ou antiga. Apaga-a e reinicia o servidor:
```bash
# macOS/Linux
rm src/ConvenienceChain.Web/sgclc.db
dotnet run --project src/ConvenienceChain.Web/

# Windows (PowerShell)
Remove-Item src\ConvenienceChain.Web\sgclc.db
dotnet run --project src\ConvenienceChain.Web\
```

**"dotnet: command not found" (macOS)**
```bash
export PATH="$PATH:/usr/local/share/dotnet"
```
Para tornar permanente, adiciona esta linha ao teu `~/.zshrc` ou `~/.bashrc`.

**Porta 5000 em uso**  
Usa uma porta diferente:
```bash
dotnet run --project src/ConvenienceChain.Web/ --urls "http://localhost:5050"
```

**Build falha com erros de pacotes**
```bash
dotnet restore
dotnet build src/ConvenienceChain.Web/
```

---

## 🔑 Credenciais de Demonstração

| Papel | Email | Password | Acesso |
|---|---|---|---|
| **Gestor da Cadeia** | `gestor@quickmart.pt` | `Admin@123` | Tudo (dashboard global, relatórios, consolidação, utilizadores) |
| **Gerente de Loja** | `ana@quickmart.pt` | `Gerente@1` | POS, stock, encomendas, faturas da loja |
| **Funcionário** | `carlos@quickmart.pt` | `Func@1234` | POS e stock básico |

---

## 🏗️ Arquitetura

```
┌─────────────────────────────┐
│   Apresentação (Blazor)     │  ← Componentes Razor + Bootstrap 5
├─────────────────────────────┤
│   Negócio (Core)            │  ← Serviços, Entities, DTOs, Interfaces
├─────────────────────────────┤
│   Dados (Data)              │  ← EF Core + SQLite + Repositórios
└─────────────────────────────┘
```

### Estrutura do Repositório

```
LI4/
├── README.md
├── docs/
│   ├── etapa1/              ← Requisitos (SRS, RFs/RNFs, User Stories, UCs, Atas)
│   ├── etapa2/              ← Arquitetura, wireframes, diagramas, dicionário de dados
│   ├── etapa3/              ← Gestão Scrum (backlog, sprints)
│   ├── etapa4/              ← Testes e métricas de qualidade
│   └── relatorio_final.md   ← Relatório final completo
│
└── src/
    ├── ConvenienceChain.Core/    ← Domínio: entidades, serviços, interfaces, DTOs
    ├── ConvenienceChain.Data/    ← EF Core: DbContext, migrações, repositórios
    ├── ConvenienceChain.Web/     ← Blazor Server: páginas, layout, serviços web
    │   └── sgclc.db             ← Base de dados SQLite (gerada automaticamente)
    └── ConvenienceChain.Tests/   ← Testes xUnit (10 testes unitários)
```

---

## 🛠️ Stack Tecnológica

| Tecnologia | Versão | Uso |
|---|---|---|
| C# / .NET | 8.0 | Linguagem e runtime |
| ASP.NET Core Blazor Server | 8.0 | Framework UI reativa |
| Entity Framework Core | 8.0 | ORM e migrações |
| SQLite | — | Base de dados (desenvolvimento) |
| Bootstrap 5 + Icons | 5.3 | CSS e ícones |
| BCrypt.Net-Next | 4.0 | Hashing de passwords |
| xUnit + Moq + FluentAssertions | — | Testes unitários |

---

## 👥 Controlo de Acesso (RBAC)

```
GestorCadeia  → Tudo + relatórios globais + consolidação + gestão utilizadores
GerenteLoja   → POS + stock + encomendas + faturas (só da sua loja)
Funcionario   → POS + stock (leitura)
```

---

## 📚 Documentação

Toda a documentação está em `docs/`:

| Documento | Conteúdo |
|---|---|
| `etapa1/SRS.md` | Especificação de Requisitos de Software (IEEE 830) |
| `etapa1/requisitos.md` | 42 RFs + 15 RNFs com prioridade, estado e rastreabilidade |
| `etapa1/user_stories.md` | 23 User Stories (8 épicos) |
| `etapa1/use_cases.md` | 25 Casos de Uso com fluxos alternativos |
| `etapa1/atas_reunioes.md` | Atas das reuniões com o cliente + questionário |
| `etapa2/architecture.md` | Diagrama de arquitetura 3 camadas + diagramas UML |
| `etapa2/wireframes.md` | 8 wireframes com rastreabilidade RF ↔ Ecrã |
| `etapa2/dicionario_dados.md` | Dicionário de dados + SQL DDL completo |
| `etapa2/diagramas_comportamentais.md` | Máquinas de estado + diagramas de sequência |
| `etapa3/scrum.md` | Product Backlog (23 US, 104 SP) + 4 Sprints |
| `etapa4/testes.md` | Resultados dos testes + métricas ISO/IEC 25010 |
| `relatorio_final.md` | **Relatório Final** (todas as secções) |

---

## ⚙️ Configuração (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=sgclc.db"
  },
  "Consolidacao": {
    "HoraExecucao": "23:59"
  }
}
```

Para usar SQL Server em produção, alterar a connection string e o provider no `Program.cs`.
