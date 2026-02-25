# Gestão de Projeto – Metodologia Scrum
## Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência

**Versão:** 1.0 | **Data:** 24 de Fevereiro de 2026 | **Projeto:** LI4 2025/2026

---

## 1. Equipa e Papéis Scrum

| Papel | Nome |
|---|---|
| **Product Owner** | João Martins (Cliente – Gestor da Cadeia QuickMart) |
| **Scrum Master** | [Nome do elemento responsável] |
| **Developer Team** | [Membros da equipa LI4] |

---

## 2. Product Backlog

O Product Backlog é ordenado por valor de negócio e prioridade. Cada item corresponde a uma User Story (ver `docs/etapa1/user_stories.md`).

| # | User Story | Prioridade | Esforço (SP) | Sprint |
|---|---|---|---|---|
| 1 | US-01 – Login no sistema | Alta | 3 | 1 |
| 2 | US-02 – Gestão de Utilizadores | Alta | 5 | 1 |
| 3 | US-04 – Criar Produto | Alta | 5 | 1 |
| 4 | US-05 – Pesquisar Produtos | Alta | 3 | 1 |
| 5 | US-07 – Visualizar Stock da Loja | Alta | 3 | 1 |
| 6 | US-10 – Registar Venda (POS) | Alta | 8 | 2 |
| 7 | US-12 – Emitir Recibo | Alta | 3 | 2 |
| 8 | US-08 – Definir Stock Mínimo | Alta | 2 | 2 |
| 9 | US-09 – Ajuste Manual de Stock | Alta | 3 | 2 |
| 10 | US-13 – Cancelar/Devolver Venda | Alta | 5 | 2 |
| 11 | US-14 – Gerir Fornecedores | Alta | 5 | 3 |
| 12 | US-15 – Criar Encomenda | Alta | 5 | 3 |
| 13 | US-16 – Recepcionar Encomenda | Alta | 5 | 3 |
| 14 | US-21 – Dashboard Central | Alta | 8 | 3 |
| 15 | US-22 – Relatório de Vendas | Média | 8 | 4 |
| 16 | US-19 – Consolidação Automática | Alta | 8 | 4 |
| 17 | US-20 – Consolidação Manual | Média | 3 | 4 |
| 18 | US-17 – Emitir Fatura | Média | 5 | 4 |
| 19 | US-23 – Relatório Comparativo | Média | 5 | 4 |
| 20 | US-11 – Aplicar Desconto | Média | 3 | 2 |
| 21 | US-06 – Definir Preço por Loja | Média | 3 | 3 |
| 22 | US-03 – Recuperar Password | Baixa | 3 | 4 |
| 23 | US-18 – Consultar Faturas | Baixa | 3 | 4 |

**Total Story Points:** 104 SP

---

## 3. Sprints

### Sprint 1 – Fundações (23/03 a 30/03/2026)
**Objetivo:** Configurar o ambiente, autenticação e gestão básica de produtos

| US | Tarefa | Responsável | Estado |
|---|---|---|---|
| US-01 | Implementar Login (Blazor + AuthService + BCrypt) | — | ⬜ |
| US-02 | Criar gestão de utilizadores (CRUD + RBAC) | — | ⬜ |
| US-04 | Criar CRUD de Produtos + validação de código único | — | ⬜ |
| US-05 | Implementar pesquisa de produtos em tempo real | — | ⬜ |
| US-07 | Criar listagem de stock por loja com alertas visuais | — | ⬜ |
| — | Configurar BD (SQL Server + EF Core + migrations) | — | ⬜ |
| — | Layout Blazor: NavMenu + sidebar conforme papel | — | ⬜ |
| — | Seed data: 5 lojas, 3 users, 20 produtos, categorias | — | ⬜ |

**Velocidade planeada:** 22 SP | **Critério de Done:** Login funcional, CRUD de produtos operacional, stock visível

---

### Sprint 2 – Ponto de Venda (30/03 a 06/04/2026)
**Objetivo:** Funcionalidade core de registo de vendas

| US | Tarefa | Responsável | Estado |
|---|---|---|---|
| US-10 | POS: adicionar produtos, calcular total, confirmar venda | — | ⬜ |
| US-11 | Aplicar descontos por linha e total | — | ⬜ |
| US-12 | Gerar e mostrar recibo após venda | — | ⬜ |
| US-08 | Definir stock mínimo por produto | — | ⬜ |
| US-09 | Ajuste manual de stock com auditoria | — | ⬜ |
| US-13 | Cancelamento e devolução de venda | — | ⬜ |

**Velocidade planeada:** 24 SP | **Critério de Done:** POS end-to-end funcional, stock deduzido automaticamente, recibo gerado

---

### Sprint 3 – Fornecedores, Encomendas e Dashboard (06/04 a 13/04/2026)
**Objetivo:** Gestão de fornecedores, encomendas e dashboards

| US | Tarefa | Responsável | Estado |
|---|---|---|---|
| US-14 | CRUD de fornecedores | — | ⬜ |
| US-15 | Criar encomenda a fornecedor | — | ⬜ |
| US-16 | Recepcionar encomenda + atualizar stock | — | ⬜ |
| US-21 | Dashboard Central com KPIs e gráficos | — | ⬜ |
| US-06 | Definir preço de venda por loja | — | ⬜ |

**Velocidade planeada:** 26 SP | **Critério de Done:** Ciclo completo de encomenda funcional, dashboard com KPIs corretos

---

### Sprint 4 – Relatórios, Consolidação e Faturação (13/04 a 27/04/2026)
**Objetivo:** Relatórios, consolidação automática e faturação

| US | Tarefa | Responsável | Estado |
|---|---|---|---|
| US-22 | Relatório de vendas com filtros e gráficos | — | ⬜ |
| US-19 | Consolidação automática (job agendado) | — | ⬜ |
| US-20 | Consolidação manual (botão no dashboard) | — | ⬜ |
| US-17 | Emissão de fatura em PDF | — | ⬜ |
| US-23 | Relatório comparativo entre lojas | — | ⬜ |
| US-03 | Recuperação de password por email | — | ⬜ |
| US-18 | Consulta e filtragem de faturas | — | ⬜ |

**Velocidade planeada:** 32 SP | **Critério de Done:** Consolidação automática funcional, relatório PDF exportável

---

## 4. Definition of Done (DoD)

Uma User Story está **Done** quando:
- [ ] O código está implementado e funcional
- [ ] Os comentários XML estão presentes nos métodos públicos
- [ ] Existe pelo menos 1 teste unitário para a lógica de negócio
- [ ] A funcionalidade foi testada manualmente no browser
- [ ] O código passou por code review de pelo menos 1 elemento
- [ ] O commit foi feito no repositório GitHub com mensagem descritiva

---

## 5. Burndown Chart (Planeado)

| Sprint | SP Planeados | SP Restantes (início) | SP Restantes (fim ideal) |
|---|---|---|---|
| Sprint 1 | 22 | 104 | 82 |
| Sprint 2 | 24 | 82 | 58 |
| Sprint 3 | 26 | 58 | 32 |
| Sprint 4 | 32 | 32 | 0 |

---

## 6. Repositório e Gestão de Código

- **Plataforma:** GitHub
- **Estratégia de branches:** `main` (produção) + `develop` + `feature/US-XX-descricao`
- **Pull Requests:** obrigatórios para merge em `develop`, com aprovação de 1 elemento
- **Commits:** seguir a convenção Conventional Commits: `feat:`, `fix:`, `docs:`, `test:`, `refactor:`
