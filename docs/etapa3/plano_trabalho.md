# Gestão de Projeto – Metodologia Waterfall

**Projeto:** SGCLC – Sistema de Gestão Integrada para Cadeia de Conveniência  
**UC:** LI4 2025/2026 – Universidade do Minho  

---

## 1. Metodologia Adotada

O projeto seguiu a metodologia **Waterfall (Cascata)**, caracterizada por fases sequenciais e bem delimitadas. Cada fase deve estar concluída e validada antes de avançar para a seguinte, garantindo rastreabilidade total entre requisitos, design e implementação.

```
┌─────────────────────────────────────────────────────────┐
│  1. Requisitos  →  2. Design  →  3. Implementação  →  4. Testes  │
└─────────────────────────────────────────────────────────┘
```

| Fase | Etapa | Período | Entregáveis |
|---|---|---|---|
| **1. Requisitos** | Etapa 1 | Fev 2026 | SRS, User Stories, Use Cases, Atas |
| **2. Design** | Etapa 2 | Fev 2026 | Arquitetura, Wireframes, Dicionário de Dados |
| **3. Implementação** | Etapa 3 | Mar–Abr 2026 | Código fonte completo |
| **4. Testes e Validação** | Etapa 4 | Abr 2026 | Plano de testes, resultados, métricas |

---

## 2. Equipa do Projeto

| Papel | Nome |
|---|---|
| **Responsável de Requisitos** | [Nome do elemento] |
| **Responsável de Arquitetura** | [Nome do elemento] |
| **Responsável de Implementação** | [Nome do elemento] |
| **Responsável de Testes** | [Nome do elemento] |
| **Cliente (Utilizador Final)** | João Martins (Gestor da Cadeia QuickMart) |

---

## 3. Plano de Trabalho

### Fase 1 — Levantamento de Requisitos (Etapa 1)

**Objetivo:** Definir completamente o âmbito e os requisitos do sistema antes de qualquer design ou implementação.

**Atividades:**
- 3 reuniões com o cliente para levantamento de necessidades
- 1 questionário a 15 funcionários de POS
- Análise documental dos processos existentes
- Elaboração do SRS (Software Requirements Specification)
- Definição de 42 Requisitos Funcionais e 15 RNFs
- Criação de 23 User Stories e 25 Casos de Uso

**Critério de conclusão:** SRS aprovado pelo cliente e equipa.

---

### Fase 2 — Design e Arquitetura (Etapa 2)

**Objetivo:** Definir completamente a arquitetura e as interfaces antes de iniciar a implementação.

**Atividades:**
- Definição da arquitetura 3-Tier e padrões de design (Repository, DI, DTO)
- Elaboração dos diagramas UML (Classes, Sequência, Componentes)
- Criação do Dicionário de Dados e SQL DDL
- Design dos wireframes de todas as 8 interfaces principais

**Critério de conclusão:** Arquitetura e wireframes aprovados pela equipa.

---

### Fase 3 — Implementação (Etapa 3)

**Objetivo:** Implementar o sistema de acordo com os requisitos e design definidos nas fases anteriores.

**Funcionalidades implementadas por módulo:**

| Módulo | Funcionalidades | RFs |
|---|---|---|
| Autenticação e RBAC | Login, sessão, autorização por papel | RF01–RF05 |
| Ponto de Venda (POS) | Carrinho, descontos, confirmação | RF17–RF22 |
| Gestão de Stock | Alertas, ajustes, auditoria, validades | RF11–RF16 |
| Encomendas | CRUD, receção, estados | RF23–RF27 |
| Faturação | Emissão, listagem, filtros | RF28–RF32 |
| Consolidação | Automática (23h59) e manual | RF33–RF36 |
| Relatórios e Dashboard | KPIs, por período/categoria | RF37–RF42 |
| Gestão de Utilizadores | CRUD contas (Gestor apenas) | RF03 |

**Critério de conclusão:** Todos os módulos implementados e a aplicação a executar sem erros.

---

### Fase 4 — Testes e Validação (Etapa 4)

**Objetivo:** Verificar que o sistema implementado cumpre todos os requisitos definidos na Fase 1.

**Atividades:**
- 10 testes unitários xUnit (StockService, SalesService, ProdutoService)
- Testes funcionais end-to-end para todos os fluxos principais
- Análise de métricas ISO/IEC 25010
- Validação de rastreabilidade: RF → UC → Implementação → Teste

**Resultado:** Passed 10/10 testes; 38/42 RFs implementados (90%).

---

## 4. Rastreabilidade

A metodologia Waterfall exige rastreabilidade completa entre todas as fases. A tabela abaixo demonstra a rastreabilidade para os módulos principais:

| Requisito | User Story | Caso de Uso | Módulo/Página | Teste |
|---|---|---|---|---|
| RF11–RF16 | US05 | UC08, UC09 | `/stock` | `StockServiceTests` |
| RF17–RF22 | US09, US10 | UC10, UC11 | `/pos` | `SalesServiceTests` |
| RF06–RF10 | US03 | UC05, UC06 | Serv. Produto | `ProdutoServiceTests` |
| RF33–RF36 | US17 | UC19 | `/consolidacao` | ConsolidacaoService |
| RF37–RF42 | US18–US20 | UC21–UC25 | `/relatorios` | — |

---

## 5. Controlo de Versões

O código e a documentação são geridos no repositório GitHub:  
**https://github.com/macieluis/LI4**

Principais commits representativos de cada fase:

| Fase | Commit |
|---|---|
| Requisitos | `docs: complete SRS with 42 RFs and 15 RNFs` |
| Design | `docs: add data dictionary with SQL DDL for all 11 tables` |
| Implementação | `feat: Implementação completa do SGCLC – QuickMart` |
| Testes | `test: add unit tests for StockService, SalesService, ProdutoService` |
