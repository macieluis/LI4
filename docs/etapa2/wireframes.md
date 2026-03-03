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

> **Nota de implementação:** O POS foi redesenhado para mostrar diretamente uma tabela de todos os produtos com `Stock > 0` (filtragem instantânea no cliente). Não existe chamada ao servidor por cada pesquisa — o filtro é local, imediato e insensível a acentos e maiúsculas.

```
┌───────────────────────────────────────────────────────────────────┐
│  🛒 PONTO DE VENDA   Loja: Braga-Centro | Ana R.                 │
├─────────────────────────────────────┬─────────────────────────────┤
│  PRODUTOS DISPONÍVEIS (stock > 0)   │  CARRINHO  🛒 3 itens       │
│                                     │                             │
│  [🔍 Pesquisar nome, código, cat ✕] │ Produto      Qtd  P.  Tot  │
│  ──────────────────────────────     │ ──────────── ──── ─── ───   │
│  Produto        Stock  Preço  [+]   │ Água 500ml    3  €0,60 €1,8 │
│  ──────────── ─────── ───── ───     │ Red Bull       1  €1,80 €1,8│
│  Água 500ml   🟡 8    €0,60  [+]   │ Pão Leite      2  €0,35 €0,7│
│  Coca-Cola   ✅ 145   €1,50  [+]   │ ─────────────────────────── │
│  Red Bull    🟡 5    €1,80  [+]    │ Subtotal:           €4,30   │
│  Sumo Laranja ✅ 230  €0,90  [+]   │ Desconto (€): [_____]       │
│  Pão de Leite ✅ 180  €0,35  [+]   │ ─────────────────────────── │
│  Café Expres  ✅ 47   €0,80  [+]   │ TOTAL:              €4,30   │
│                                     │                             │
│  🟡 = Stock em alerta  ✅ = OK     │ [🗑️ Limpar] [✅ CONFIRMAR]  │
└─────────────────────────────────────┴─────────────────────────────┘
```

**RFs satisfeitos:** RF17 (registo de venda), RF18 (descontos), RF19 (recibo), RF20 (validação stock)  
**RBAC:** Apenas **Funcionário** e **Gerente de Loja** acedem ao POS. Gestor da Cadeia não tem acesso.

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

| Ecrã | RFs Satisfeitos | Funcionário | Gerente de Loja | Gestor da Cadeia |
|---|---|:---:|:---:|:---:|
| Login | RF01, RF04, RF05 | ✅ | ✅ | ✅ |
| Dashboard | RF37, RF38, RF39, RF41, RF33 | ✅ | ✅ | ✅ |
| POS | RF17, RF18, RF19, RF20, RF21 | ✅ | ✅ | ❌ |
| Gestão de Stock | RF11, RF12, RF13, RF15, RF16 | ❌ | ✅ | ✅ |
| Encomendas | RF24, RF25, RF26, RF27 | ❌ | ✅ | ✅ |
| Faturas | RF28, RF29, RF30, RF31 | ❌ | ✅ | ✅ |
| Relatórios | RF39, RF40, RF41, RF42 | ❌ | ✅ | ✅ |
| Consolidação | RF33, RF34, RF35, RF36 | ❌ | ❌ | ✅ |
| Admin – Utilizadores | RF03 | ❌ | ❌ | ✅ |

> **Legenda:** ✅ = tem acesso | ❌ = sem acesso (bloqueado via NavMenu e guard de página)
