# Esboço de Interfaces (Wireframes)
## Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência

**Versão:** 1.0 | **Data:** 24 de Fevereiro de 2026 | **Projeto:** LI4 2025/2026

> Os wireframes seguintes representam o layout e estrutura das interfaces principais do SGCLC.
> Cada ecrã está associado aos Requisitos Funcionais que satisfaz.
> As interfaces serão implementadas em **Blazor Server** com **Bootstrap 5**, mantendo fidelidade a estes esboços.

---

## 1. Layout Geral da Aplicação

```
┌─────────────────────────────────────────────────────────────────┐
│  🏪 SGCLC – QuickMart       [Loja: Braga-Centro]  👤 Ana R. [↩]│  ← NavBar
├────────────────┬────────────────────────────────────────────────┤
│                │                                                 │
│  📊 Dashboard  │                                                 │
│  🛒 POS        │          ÁREA DE CONTEÚDO PRINCIPAL            │
│  📦 Stock      │                                                 │
│  📋 Encomendas │                                                 │
│  🧾 Faturas    │                                                 │
│  📈 Relatórios │                                                 │
│  ⚙️  Admin     │                                                 │
│                │                                                 │
└────────────────┴────────────────────────────────────────────────┘
```

**RFs satisfeitos:** RF02 (RBAC – menus adaptados ao papel do utilizador)

---

## 2. Ecrã de Login (RF01, RF04, RF05)

```
┌─────────────────────────────────────────────────────────────────┐
│                                                                 │
│                    🏪  SGCLC – QuickMart                        │
│              Sistema de Gestão de Cadeia de Lojas               │
│                                                                 │
│           ┌─────────────────────────────────────┐              │
│           │           INICIAR SESSÃO             │              │
│           │                                     │              │
│           │  Email: [________________________]  │              │
│           │                                     │              │
│           │  Password: [____________________]  │              │
│           │                                     │              │
│           │         [  ENTRAR  ]                │              │
│           │                                     │              │
│           │     Esqueceu a password? Clique aqui│              │
│           └─────────────────────────────────────┘              │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## 3. Dashboard Central – Gestor da Cadeia (RF37, RF39, RF41)

```
┌─────────────────────────────────────────────────────────────────┐
│  Dashboard Central                          📅 24 Feb 2026      │
│                                        [🔄 Consolidar Agora]    │
├──────────────┬──────────────┬──────────────┬────────────────────┤
│ 💰 Vendas    │ 🔔 Alertas   │ 📦 Encomendas│ 🏪 Lojas Ativas   │
│   Hoje       │   Stock      │  Pendentes   │                    │
│   €8.250,30  │     12       │      5       │     5 / 5          │
│  ▲ +5% ontem │  [Ver todas] │  [Ver todas] │                    │
├──────────────┴──────────────┴──────────────┴────────────────────┤
│  VENDAS POR LOJA – HOJE                                         │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │ Braga-Centro ████████████████████████████ €3.120        │   │
│  │ Palmeira     ████████████████████     €2.100            │   │
│  │ Maximinos    ████████████████         €1.580            │   │
│  │ Barcelos     ███████████              €1.050            │   │
│  │ Guimarães    ██████                     €400            │   │
│  └─────────────────────────────────────────────────────────┘   │
├─────────────────────────────────────────────────────────────────┤
│  VENDAS ÚLTIMOS 7 DIAS                    TOP PRODUTOS HOJE     │
│  ┌──────────────────────────────┐  ┌──────────────────────────┐ │
│  │  €  ▲                        │  │ 1. Água 500ml   142 un.  │ │
│  │8000 │       ╭──╮             │  │ 2. Café Expres   98 un.  │ │
│  │6000 │   ╭───╯  ╰──╮         │  │ 3. Red Bull     87 un.   │ │
│  │4000 │───╯          ╰──       │  │ 4. Pão de Leite  76 un. │ │
│  │     18 19 20 21 22 23 24 Feb │  │ 5. Sumo Laranja  65 un. │ │
│  └──────────────────────────────┘  └──────────────────────────┘ │
└─────────────────────────────────────────────────────────────────┘
```

---

## 4. Dashboard de Loja – Gerente (RF38)

```
┌─────────────────────────────────────────────────────────────────┐
│  Dashboard – Loja Braga-Centro              📅 24 Feb 2026      │
├──────────────┬──────────────┬──────────────┬────────────────────┤
│ 💰 Vendas    │ 📦 Trans.    │ 🔔 Alertas   │ 📋 Encomendas     │
│   Hoje       │   Hoje       │   Stock      │   Pendentes        │
│   €3.120,40  │    87        │      4       │        2           │
├──────────────┴──────────────┴──────────────┴────────────────────┤
│  ALERTAS DE STOCK (4)                                           │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │ ⚠️ Água 500ml     Stock: 8    Mínimo: 20  [Encomendar]  │   │
│  │ ⚠️ Red Bull 250ml Stock: 5    Mínimo: 15  [Encomendar]  │   │
│  │ ⚠️ Sumo Laranja   Stock: 12   Mínimo: 20  [Encomendar]  │   │
│  │ ⚠️ Café Solúvel   Stock: 3    Mínimo: 10  [Encomendar]  │   │
│  └──────────────────────────────────────────────────────────┘   │
├─────────────────────────────────────────────────────────────────┤
│  ÚLTIMAS VENDAS                                                 │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │ #5234  14:32  Ana R.  €12,50  Concluída  [Ver]            │ │
│  │ #5233  14:15  Carlos M. €3,20 Concluída  [Ver]            │ │
│  │ #5232  14:02  Ana R.  €28,90  Concluída  [Ver]            │ │
│  └────────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────┘
```

---

## 5. Ponto de Venda – POS (RF17, RF18, RF19, RF20)

```
┌──────────────────────────────┬───────────────────────────────────┐
│  🛒 NOVA VENDA               │  CARRINHO                         │
│  Loja: Braga-Centro          │                                   │
│  Funcionário: Ana R.         │ Produto         Qtd  P.Unit Total │
│                              │ ─────────────── ──── ───── ─────  │
│  Pesquisar produto:          │ Água 500ml       3   €0,60 €1,80  │
│  [________________] [🔍]    │ Red Bull 250ml   1   €1,80 €1,80  │
│                              │ Pão de Leite     2   €0,35 €0,70  │
│  Resultados:                 │ ─────────────── ──── ───── ─────  │
│  ┌────────────────────────┐  │ Subtotal:              €4,30      │
│  │ 5601.. Água 500ml €0,60│  │ Desconto: [___%] [___€]  -€0,00  │
│  │ 5600.. Água 1,5L  €1,20│  │ ─────────────────────────────    │
│  │ 5699.. Coca-Cola  €1,50│  │ TOTAL:                 €4,30      │
│  └────────────────────────┘  │                                   │
│                              │  [🗑️ Limpar]  [✅ CONFIRMAR]     │
└──────────────────────────────┴───────────────────────────────────┘
```

---

## 6. Gestão de Stock (RF11, RF12, RF13, RF15, RF16)

```
┌─────────────────────────────────────────────────────────────────┐
│  📦 Stock – Loja Braga-Centro          [+ Ajuste Manual] [📥 CSV]│
├──────────────────────────────────────────────────────────────────┤
│  Pesquisar: [____________]  Categoria: [Todas ▼]  [🔍 Filtrar]  │
├───────────────────────┬──────┬──────────┬───────┬───────────────┤
│ Produto               │ Cód. │ Qtd Atual│ Mínimo│ Ações         │
├───────────────────────┼──────┼──────────┼───────┼───────────────┤
│ ⚠️ Água 500ml        │ 5601 │    8     │  20   │ [✏️] [📋]    │
│ ✅ Coca-Cola 330ml   │ 5602 │   145    │  30   │ [✏️] [📋]    │
│ ⚠️ Red Bull 250ml    │ 5603 │    5     │  15   │ [✏️] [📋]    │
│ ✅ Sumo Laranja       │ 5604 │   230    │  20   │ [✏️] [📋]    │
│ ⚠️ Café Solúvel      │ 5605 │    3     │  10   │ [✏️] [📋]    │
│ ✅ Pão de Leite       │ 5610 │   180    │  50   │ [✏️] [📋]    │
├───────────────────────┴──────┴──────────┴───────┴───────────────┤
│  ⚠️ = Abaixo do stock mínimo    ✅ = Stock OK                   │
└─────────────────────────────────────────────────────────────────┘
```

---

## 7. Lista de Encomendas (RF24, RF25, RF27)

```
┌─────────────────────────────────────────────────────────────────┐
│  📋 Encomendas – Loja Braga-Centro           [+ Nova Encomenda] │
├─────────────────────────────────────────────────────────────────┤
│  Filtrar: Estado [Todos ▼]    [🔍]                              │
├────┬─────────────┬─────────────────┬──────────────┬────────────┤
│ #  │ Data        │ Fornecedor      │ Estado       │ Ações      │
├────┼─────────────┼─────────────────┼──────────────┼────────────┤
│301 │ 20/02/2026  │ Distribuidora   │ 🟡 Pendente  │ [Ver][❌] │
│300 │ 15/02/2026  │ Norte Lda.      │ ✅ Rececionada│ [Ver]     │
│299 │ 10/02/2026  │ AguasPortugal   │ ✅ Rececionada│ [Ver]     │
│298 │ 05/02/2026  │ Distribuidora   │ ❌ Cancelada  │ [Ver]     │
└────┴─────────────┴─────────────────┴──────────────┴────────────┘
```

---

## 8. Relatórios de Vendas (RF39, RF40, RF41, RF42)

```
┌─────────────────────────────────────────────────────────────────┐
│  📈 Relatório de Vendas                                         │
├─────────────────────────────────────────────────────────────────┤
│  Loja: [Todas ▼]  De: [01/02/2026]  Até: [24/02/2026]          │
│  Categoria: [Todas ▼]             [📊 Gerar Relatório]         │
│                                   [📄 PDF]  [📊 CSV]           │
├──────────────┬──────────────┬──────────────┬────────────────────┤
│ Total Vendas │ Nº Transações│ Ticket Médio │ Desconto Total     │
│  €185.420    │    2.341     │    €79,21    │      €8.230        │
├─────────────────────────────────────────────────────────────────┤
│  VENDAS POR CATEGORIA          │  VENDAS POR DIA                │
│  ┌───────────────────────────┐ │  ┌──────────────────────────┐  │
│  │ Bebidas       ████ 42%   │ │  │ €  ▲                     │  │
│  │ Snacks        ███  28%   │ │  │    │  ╭──╮  ╭──╮         │  │
│  │ Lacticínios   ██   15%   │ │  │    │──╯  ╰──╯  ╰──       │  │
│  │ Higiene       █    10%   │ │  │    Jan          Fev       │  │
│  │ Outros        █    5%    │ │  └──────────────────────────┘  │
│  └───────────────────────────┘ │                                │
└─────────────────────────────────────────────────────────────────┘
```

---

## 9. Rastreabilidade Wireframes ↔ Requisitos

| Ecrã | RFs Satisfeitos | Papel de Utilizador |
|---|---|---|
| Login | RF01, RF04, RF05 | Todos |
| Dashboard Central | RF37, RF39, RF41, RF33 | Gestor da Cadeia |
| Dashboard de Loja | RF38, RF13, RF14 | Gerente de Loja |
| POS | RF17, RF18, RF19, RF20, RF21 | Funcionário |
| Gestão de Stock | RF11, RF12, RF13, RF15, RF16 | Gerente de Loja |
| Encomendas | RF24, RF25, RF26, RF27 | Gerente de Loja |
| Relatórios | RF39, RF40, RF41, RF42 | Gestor, Gerente |
| Admin – Utilizadores | RF03 | Gestor da Cadeia |
| Admin – Produtos | RF06, RF07, RF08, RF09, RF10 | Gestor da Cadeia |
| Faturas | RF28, RF29, RF30, RF31 | Gerente, Funcionário |
