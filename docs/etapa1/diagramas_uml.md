# Diagramas UML – Etapa 1
## Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência (SGCLC)

**Versão:** 1.0 | **Data:** 24 de Fevereiro de 2026 | **Projeto:** LI4 2025/2026  
**Norma:** UML 2.5 (Use Case Diagrams)

> **Nota:** Esta etapa (Etapa 1) foca os **Diagramas de Casos de Uso** (Use Case Diagrams), que representam as interações entre atores e o sistema ao nível funcional. Os diagramas estruturais e comportamentais mais detalhados (Classes, Sequência, Componentes) são produzidos na Etapa 2.

---

## Índice

1. [Atores do Sistema](#1-atores-do-sistema)
2. [Diagrama Global de Casos de Uso](#2-diagrama-global-de-casos-de-uso)
3. [Diagrama 1 – Autenticação e Gestão de Utilizadores](#3-diagrama-1--autenticação-e-gestão-de-utilizadores)
4. [Diagrama 2 – Gestão de Produtos e Catálogo](#4-diagrama-2--gestão-de-produtos-e-catálogo)
5. [Diagrama 3 – Gestão de Stock](#5-diagrama-3--gestão-de-stock)
6. [Diagrama 4 – Ponto de Venda (POS)](#6-diagrama-4--ponto-de-venda-pos)
7. [Diagrama 5 – Gestão de Fornecedores e Encomendas](#7-diagrama-5--gestão-de-fornecedores-e-encomendas)
8. [Diagrama 6 – Faturação](#8-diagrama-6--faturação)
9. [Diagrama 7 – Consolidação Diária](#9-diagrama-7--consolidação-diária)
10. [Diagrama 8 – Relatórios e Dashboard](#10-diagrama-8--relatórios-e-dashboard)

---

## 1. Atores do Sistema

| Ator | Tipo | Descrição |
|---|---|---|
| **Gestor da Cadeia (GC)** | Primário | Administrador central com acesso total ao sistema |
| **Gerente de Loja (GL)** | Primário | Gere uma loja específica |
| **Funcionário (FN)** | Primário | Opera o POS da loja |
| **Sistema (SYS)** | Secundário | O próprio sistema SGCLC (para processos automáticos) |

---

## 2. Diagrama Global de Casos de Uso

Visão geral de todas as áreas funcionais e os atores que as utilizam:

```mermaid
graph LR
    GC["👤 Gestor\nda Cadeia"]
    GL["👤 Gerente\nde Loja"]
    FN["👤 Funcionário\n(POS)"]
    SYS(["⚙️ Sistema\n(automático)"])

    subgraph SGCLC["SGCLC – Sistema de Gestão Integrada"]
        Auth["🔐 Autenticação\ne Utilizadores"]
        Prod["📦 Gestão de\nProdutos"]
        Stock["📊 Gestão\nde Stock"]
        POS["🛒 Ponto de\nVenda (POS)"]
        Enc["🚚 Fornecedores\ne Encomendas"]
        Fat["🧾 Faturação"]
        Cons["🔄 Consolidação\nDiária"]
        Rel["📈 Relatórios\ne Dashboard"]
    end

    GC --- Auth
    GC --- Prod
    GC --- Stock
    GC --- POS
    GC --- Enc
    GC --- Fat
    GC --- Cons
    GC --- Rel

    GL --- Auth
    GL --- Stock
    GL --- POS
    GL --- Enc
    GL --- Fat
    GL --- Rel

    FN --- Auth
    FN --- POS
    FN --- Fat

    SYS --- Cons
```

---

## 3. Diagrama 1 – Autenticação e Gestão de Utilizadores

```mermaid
flowchart LR
    GC(["👤 Gestor\nda Cadeia"])
    GL(["👤 Gerente\nde Loja"])
    FN(["👤 Funcionário"])

    subgraph AUTH["Autenticação e Gestão de Utilizadores"]
        UC01(["UC01\nLogin no Sistema"])
        UC02(["UC02\nRecuperar Password"])
        UC03(["UC03\nGerir Utilizadores"])
        UC03a(["UC03a\nCriar Utilizador"])
        UC03b(["UC03b\nEditar Utilizador"])
        UC03c(["UC03c\nDesativar Utilizador"])
        UC03d(["UC03d\nAtribuir Papel/Loja"])
    end

    GC --- UC01
    GC --- UC02
    GC --- UC03
    GL --- UC01
    GL --- UC02
    FN --- UC01
    FN --- UC02

    UC03 -->|inclui| UC03a
    UC03 -->|inclui| UC03b
    UC03 -->|inclui| UC03c
    UC03 -->|inclui| UC03d
```

**RFs cobertos:** RF01, RF02, RF03, RF04, RF05

---

## 4. Diagrama 2 – Gestão de Produtos e Catálogo

```mermaid
flowchart LR
    GC(["👤 Gestor\nda Cadeia"])
    GL(["👤 Gerente\nde Loja"])
    FN(["👤 Funcionário"])

    subgraph PROD["Gestão de Produtos e Catálogo"]
        UC04(["UC04\nGerir Produtos"])
        UC04a(["UC04a\nCriar Produto"])
        UC04b(["UC04b\nEditar Produto"])
        UC04c(["UC04c\nDesativar Produto"])
        UC05(["UC05\nGerir Categorias"])
        UC06(["UC06\nDefinir Preço\npor Loja"])
        UC_CAT(["UC\nPesquisar Produtos"])
    end

    GC --- UC04
    GC --- UC05
    GL --- UC06
    GL --- UC_CAT
    FN --- UC_CAT

    UC04 -->|inclui| UC04a
    UC04 -->|inclui| UC04b
    UC04 -->|inclui| UC04c
    UC04a -->|estende| UC_CAT
```

**RFs cobertos:** RF06, RF07, RF08, RF09, RF10

---

## 5. Diagrama 3 – Gestão de Stock

```mermaid
flowchart LR
    GC(["👤 Gestor\nda Cadeia"])
    GL(["👤 Gerente\nde Loja"])
    SYS(["⚙️ Sistema"])

    subgraph STOCK["Gestão de Stock"]
        UC07(["UC07\nVisualizar Stock"])
        UC08(["UC08\nDefinir Stock\nMínimo"])
        UC09(["UC09\nAjuste Manual\nde Stock"])
        UC09a(["UC09a\nRegistar Motivo\nde Ajuste"])
        UC_ALERTA(["UC\nGerar Alerta\nde Stock Baixo"])
        UC_EXP(["UC\nExportar Stock\nem CSV"])
    end

    GC --- UC07
    GL --- UC07
    GL --- UC08
    GL --- UC09
    SYS --- UC_ALERTA

    UC08 -->|estende| UC_ALERTA
    UC09 -->|inclui| UC09a
    UC07 -->|estende| UC_EXP
```

**RFs cobertos:** RF11, RF12, RF13, RF14, RF15, RF16

---

## 6. Diagrama 4 – Ponto de Venda (POS)

```mermaid
flowchart LR
    FN(["👤 Funcionário"])
    GL(["👤 Gerente\nde Loja"])
    SYS(["⚙️ Sistema"])

    subgraph POS["Ponto de Venda (POS)"]
        UC10(["UC10\nRegistar Venda"])
        UC10a(["UC10a\nAdicionar Produto\nao Carrinho"])
        UC10b(["UC10b\nValidar Stock\nDisponível"])
        UC11(["UC11\nAplicar Desconto"])
        UC12(["UC12\nEmitir Recibo"])
        UC13(["UC13\nCancelar / Devolver\nVenda"])
        UC13a(["UC13a\nRegistar Motivo"])
        UC13b(["UC13b\nRepor Stock"])
        UC14(["UC14\nHistórico de\nVendas"])
    end

    FN --- UC10
    FN --- UC11
    FN --- UC12
    GL --- UC13
    GL --- UC14
    GC(["👤 Gestor\nda Cadeia"]) --- UC14

    UC10 -->|inclui| UC10a
    UC10 -->|inclui| UC10b
    UC10 -->|inclui| UC12
    UC11 -->|estende| UC10
    UC13 -->|inclui| UC13a
    UC13 -->|inclui| UC13b
    SYS -->|despoleta| UC10b
```

**RFs cobertos:** RF17, RF18, RF19, RF20, RF21, RF22

---

## 7. Diagrama 5 – Gestão de Fornecedores e Encomendas

```mermaid
flowchart LR
    GC(["👤 Gestor\nda Cadeia"])
    GL(["👤 Gerente\nde Loja"])
    SYS(["⚙️ Sistema"])

    subgraph ENC["Fornecedores e Encomendas"]
        UC_FORN(["UC14\nGerir Fornecedores"])
        UC_ENC(["UC15\nCriar Encomenda"])
        UC_EST(["UC\nAcompanhar Estado\nde Encomenda"])
        UC_REC(["UC16\nRecepcionar\nEncomenda"])
        UC_REC_STOCK(["UC\nAtualizar Stock\nApós Receção"])
        UC_NOTIF(["UC\nNotificar Gestor\nde Nova Encomenda"])
    end

    GC --- UC_FORN
    GL --- UC_ENC
    GL --- UC_EST
    GL --- UC_REC
    SYS --- UC_NOTIF

    UC_ENC -->|estende| UC_NOTIF
    UC_REC -->|inclui| UC_REC_STOCK
    UC_ENC -->|estende| UC_EST
```

**RFs cobertos:** RF23, RF24, RF25, RF26, RF27

---

## 8. Diagrama 6 – Faturação

```mermaid
flowchart LR
    GC(["👤 Gestor\nda Cadeia"])
    GL(["👤 Gerente\nde Loja"])
    FN(["👤 Funcionário"])

    subgraph FAT["Faturação"]
        UC17(["UC17\nEmitir Fatura"])
        UC17a(["UC17a\nAssociar a Venda"])
        UC17b(["UC17b\nExportar PDF"])
        UC18(["UC18\nConsultar Faturas"])
        UC18a(["UC18a\nFiltrar por Data\n/ Estado"])
        UC_NC(["UC\nEmitir Nota\nde Crédito"])
    end

    FN --- UC17
    GL --- UC17
    GL --- UC18
    GC --- UC18

    UC17 -->|inclui| UC17a
    UC17 -->|inclui| UC17b
    UC18 -->|inclui| UC18a
    UC18 -->|estende| UC_NC
```

**RFs cobertos:** RF28, RF29, RF30, RF31, RF32

---

## 9. Diagrama 7 – Consolidação Diária

```mermaid
flowchart LR
    GC(["👤 Gestor\nda Cadeia"])
    SYS(["⚙️ Sistema\n(Job Automático)"])

    subgraph CONS["Consolidação Diária"]
        UC19(["UC19\nConsolidação\nAutomática"])
        UC20(["UC20\nConsolidação\nManual"])
        UC_LOG(["UC\nRegistar Log de\nConsolidação"])
        UC_RETRY(["UC\nRetry Automático\nem Caso de Falha"])
        UC_NOTIF(["UC\nNotificar Gestor\nde Resultado"])
    end

    GC --- UC20
    SYS --- UC19

    UC19 -->|inclui| UC_LOG
    UC20 -->|inclui| UC_LOG
    UC19 -->|estende| UC_RETRY
    UC19 -->|inclui| UC_NOTIF
    UC20 -->|inclui| UC_NOTIF
```

**RFs cobertos:** RF33, RF34, RF35, RF36

---

## 10. Diagrama 8 – Relatórios e Dashboard

```mermaid
flowchart LR
    GC(["👤 Gestor\nda Cadeia"])
    GL(["👤 Gerente\nde Loja"])

    subgraph REL["Relatórios e Dashboard"]
        UC21(["UC21\nDashboard Central\n(KPIs Globais)"])
        UC22(["UC22\nDashboard de Loja\n(KPIs Loja)"])
        UC23(["UC23\nRelatório de Vendas"])
        UC23a(["UC23a\nFiltrar por Período\n/ Categoria"])
        UC24(["UC24\nExportar Relatório\n(PDF / CSV)"])
        UC25(["UC25\nRelatório Comparativo\nentre Lojas"])
    end

    GC --- UC21
    GC --- UC23
    GC --- UC25
    GL --- UC22
    GL --- UC23

    UC23 -->|inclui| UC23a
    UC23 -->|estende| UC24
    UC25 -->|inclui| UC23a
    UC25 -->|estende| UC24
```

**RFs cobertos:** RF37, RF38, RF39, RF40, RF41, RF42

---

## Resumo – Cobertura dos Diagramas

| Diagrama | Área Funcional | UCs | RFs |
|---|---|---|---|
| Diagrama 1 | Autenticação e Utilizadores | UC01–UC03 | RF01–RF05 |
| Diagrama 2 | Produtos e Catálogo | UC04–UC06 | RF06–RF10 |
| Diagrama 3 | Gestão de Stock | UC07–UC09 | RF11–RF16 |
| Diagrama 4 | Ponto de Venda (POS) | UC10–UC14 | RF17–RF22 |
| Diagrama 5 | Fornecedores e Encomendas | UC14–UC16 | RF23–RF27 |
| Diagrama 6 | Faturação | UC17–UC18 | RF28–RF32 |
| Diagrama 7 | Consolidação Diária | UC19–UC20 | RF33–RF36 |
| Diagrama 8 | Relatórios e Dashboard | UC21–UC25 | RF37–RF42 |
| **Total** | **8 áreas** | **25 UCs** | **42 RFs** |
