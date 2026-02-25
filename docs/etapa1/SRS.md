# Software Requirements Specification (SRS)
## Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência

---

**Versão:** 1.0  
**Data:** 24 de Fevereiro de 2026  
**Curso:** LI4 – Laboratórios de Informática IV, UMinho  
**Norma:** IEEE 830 / ISO/IEC/IEEE 29148  

---

## Índice
1. [Introdução](#1-introdução)
2. [Descrição Geral](#2-descrição-geral)
3. [Requisitos Funcionais](#3-requisitos-funcionais)
4. [Requisitos Não Funcionais](#4-requisitos-não-funcionais)
5. [Restrições e Pressupostos](#5-restrições-e-pressupostos)
6. [Interfaces Externas](#6-interfaces-externas)
7. [Glossário](#7-glossário)

---

## 1. Introdução

### 1.1 Propósito
Este documento especifica os requisitos do **Sistema de Gestão Integrada para uma Cadeia de Pequenas Lojas de Conveniência** (doravante designado **SGCLC**). Destina-se a orientar o desenvolvimento do sistema ao longo das etapas do projeto LI4 2025/2026.

### 1.2 Âmbito do Sistema
O SGCLC é uma aplicação web que apoia a gestão diária de uma cadeia de lojas de conveniência. O sistema permite que cada loja opere de forma **autónoma** durante o dia (registo de vendas, controlo de stock local), enquanto um **servidor central** agrega e consolida os dados de todas as lojas ao fim de cada dia, disponibilizando relatórios de gestão e apoio à tomada de decisão.

**Em âmbito:**
- Gestão de produtos, categorias e preços
- Controlo de stocks por loja
- Registo de vendas e ponto de venda (POS)
- Gestão de fornecedores e encomendas
- Consolidação diária de dados entre lojas e servidor central
- Faturação (emissão e consulta de faturas)
- Geração de relatórios (por loja, período, categoria)
- Gestão de utilizadores e permissões

**Fora de âmbito:**
- Integração com sistemas de pagamento externos
- Contabilidade fiscal avançada
- Gestão de recursos humanos

### 1.3 Definições, Acrónimos e Abreviaturas
| Termo | Definição |
|---|---|
| SGCLC | Sistema de Gestão Integrada para Cadeia de Lojas de Conveniência |
| POS | Point of Sale – terminal de registo de vendas |
| Stock | Quantidade de um produto disponível numa loja |
| Consolidação | Processo de sincronização e agregação de dados das lojas no servidor central |
| SRS | Software Requirements Specification |
| RF | Requisito Funcional |
| RNF | Requisito Não Funcional |
| LLM | Large Language Model |
| EF Core | Entity Framework Core (ORM para .NET) |

### 1.4 Referências
- IEEE Std 830-1998 – IEEE Recommended Practice for Software Requirements Specifications
- ISO/IEC/IEEE 29148:2018 – Systems and software engineering – Requirements engineering
- ISO/IEC 25010:2011 – Systems and software Quality Requirements and Evaluation (SQuaRE)

### 1.5 Visão Geral do Documento
A Secção 2 descreve o sistema em termos gerais (contexto, utilizadores, funções principais). A Secção 3 especifica os requisitos funcionais. A Secção 4 especifica os requisitos não funcionais. As secções restantes abordam restrições, interfaces e glossário.

---

## 2. Descrição Geral

### 2.1 Perspetiva do Produto
O SGCLC é um sistema de informação web multi-loja com uma arquitetura cliente-servidor em três camadas. Cada loja dispõe de um terminal de acesso à aplicação (browser), e existe um servidor central que agrega os dados de todas as lojas.

```
[Loja A] ──┐
[Loja B] ──┼──► [Servidor Central SGCLC] ──► [Gestor da Cadeia]
[Loja C] ──┘
```

### 2.2 Funções Principais do Sistema
1. Gestão centralizada de produtos, preços e categorias
2. Controlo de stock por loja, com alertas de stock mínimo
3. Registo de vendas (POS) em cada loja
4. Gestão de fornecedores e encomendas de reposição
5. Consolidação diária automática de dados das lojas
6. Emissão e consulta de faturas
7. Geração de relatórios analíticos de gestão
8. Gestão de utilizadores com controlo de acesso baseado em papéis (RBAC)

### 2.3 Utilizadores e Stakeholders
| Papel | Descrição | Permissões-Chave |
|---|---|---|
| **Gestor da Cadeia** | Administrador central, gere toda a cadeia | Acesso total: todos os relatórios, todas as lojas, configuração global |
| **Gerente de Loja** | Gere uma loja específica | Gestão de stock e fornecedores da sua loja, relatórios da sua loja |
| **Funcionário** | Opera o POS da loja | Registo de vendas, consulta de preços e stock |
| **Fornecedor** | (externo, sem acesso direto ao sistema) | Recebe encomendas por via convencional |

### 2.4 Restrições Gerais
- O sistema deve ser acessível via browser, sem necessidade de instalação em clientes
- Cada loja deve conseguir registar vendas mesmo em modo de conectividade reduzida (operação autónoma)
- A consolidação diária ocorre automaticamente num horário configurável (ex: 23:59)
- O sistema deve suportar múltiplas lojas simultaneamente

### 2.5 Pressupostos e Dependências
- Existe conexão de rede entre cada loja e o servidor central (mínimo para consolidação)
- O servidor central dispõe de capacidade suficiente para armazenar dados históricos de todas as lojas
- Os utilizadores têm acesso a um dispositivo com browser moderno

---

## 3. Requisitos Funcionais

### 3.1 Autenticação e Gestão de Utilizadores

**RF01 – Login de Utilizador**
> O sistema deve permitir que utilizadores se autentiquem com email e password.

**RF02 – Controlo de Acesso por Papel**
> O sistema deve restringir funcionalidades com base no papel do utilizador: Gestor da Cadeia, Gerente de Loja, Funcionário.

**RF03 – Gestão de Utilizadores (Admin)**
> O Gestor da Cadeia deve poder criar, editar, desativar e eliminar contas de utilizador e atribuir papéis.

**RF04 – Recuperação de Password**
> O sistema deve suportar recuperação de password por email.

---

### 3.2 Gestão de Produtos e Categorias

**RF05 – CRUD de Produtos**
> O sistema deve permitir criar, consultar, editar e eliminar (logicamente) produtos. Cada produto tem: código, nome, descrição, categoria, preço de custo, preço de venda, unidade de medida e foto (opcional).

**RF06 – Gestão de Categorias**
> O sistema deve permitir organizar produtos em categorias e subcategorias hierárquicas.

**RF07 – Definição de Preços por Loja**
> O sistema deve permitir que o preço de venda de um produto possa ser sobreposto ao nível de loja (preço base definido centralmente).

**RF08 – Catálogo de Produtos**
> O sistema deve disponibilizar um catálogo de produtos pesquisável por nome, código ou categoria.

---

### 3.3 Gestão de Stock

**RF09 – Visualização de Stock por Loja**
> O sistema deve apresentar o stock atual de cada produto em cada loja.

**RF10 – Atualização de Stock**
> O stock deve ser atualizado automaticamente após cada venda registada ou receção de encomenda.

**RF11 – Definição de Stock Mínimo**
> O Gerente de Loja deve poder definir o stock mínimo por produto. Quando atingido, o sistema gera um alerta.

**RF12 – Alertas de Stock Baixo**
> O sistema deve notificar o Gerente de Loja quando o stock de um produto atingir ou descer abaixo do mínimo definido. O Gestor da Cadeia deve visualizar alertas agregados de todas as lojas.

**RF13 – Ajuste Manual de Stock**
> O Gerente de Loja deve poder registar ajustes manuais de stock (quebras, devoluções) com motivo obrigatório.

---

### 3.4 Registo de Vendas (POS)

**RF14 – Registo de Venda**
> O sistema deve permitir ao Funcionário registar uma venda, adicionando produtos (por código ou pesquisa), quantidades e calculando o total automaticamente.

**RF15 – Aplicação de Descontos**
> O sistema deve suportar a aplicação de descontos percentuais ou de valor fixo por linha de venda ou sobre o total.

**RF16 – Emissão de Ticket/Recibo**
> Após concluída, o sistema deve gerar um recibo/ticket da venda com data, hora, loja, produtos, quantidades, preços e total.

**RF17 – Cancelamento/Devolução de Venda**
> O sistema deve permitir o cancelamento de uma venda ou devolução de itens, com motivo obrigatório e reversão do stock.

**RF18 – Histórico de Vendas por Loja**
> O sistema deve permitir consultar o histórico de vendas de uma loja com filtros por data, produto e funcionário.

---

### 3.5 Gestão de Fornecedores e Encomendas

**RF19 – CRUD de Fornecedores**
> O sistema deve permitir gerir fornecedores: nome, NIF, morada, contactos, produtos fornecidos.

**RF20 – Criação de Encomenda**
> O Gerente de Loja deve poder criar encomendas a fornecedores, especificando produtos e quantidades.

**RF21 – Acompanhamento de Encomendas**
> O sistema deve registar o estado das encomendas: Pendente, Enviada, Rececionada, Cancelada.

**RF22 – Receção de Encomenda**
> Ao registar a receção de uma encomenda, o stock dos produtos envolvidos deve ser atualizado automaticamente.

---

### 3.6 Faturação

**RF23 – Geração de Fatura**
> O sistema deve gerar faturas para clientes (empresas) associadas a vendas, com dados da loja, cliente, data, produtos e totais.

**RF24 – Listagem de Faturas**
> O sistema deve permitir consultar e filtrar faturas por loja, data, estado (emitida, paga, anulada).

**RF25 – Emissão de Nota de Crédito**
> O sistema deve suportar a emissão de notas de crédito em caso de devolução faturada.

---

### 3.7 Consolidação Diária

**RF26 – Consolidação Automática**
> O sistema deve executar automaticamente a consolidação dos dados de cada loja no final de cada dia (hora configurável), sincronizando vendas, movimentos de stock e encomendas com o servidor central.

**RF27 – Consolidação Manual**
> O Gestor da Cadeia deve poder desencadear manualmente a consolidação de uma ou de todas as lojas.

**RF28 – Registo de Consolidação**
> O sistema deve manter um log de cada consolidação efetuada (data/hora, loja, resultado, totais consolidados).

---

### 3.8 Relatórios e Dashboard

**RF29 – Dashboard Central**
> O Gestor da Cadeia deve visualizar um dashboard com KPIs: total de vendas do dia (por loja e agregado), alertas de stock, encomendas pendentes, produtos mais vendidos.

**RF30 – Dashboard de Loja**
> O Gerente de Loja deve visualizar um dashboard com os KPIs da sua loja: vendas do dia, stock em alerta, encomendas pendentes.

**RF31 – Relatório de Vendas por Loja**
> O sistema deve gerar relatórios de vendas de uma loja filtráveis por período (dia, semana, mês, intervalo personalizado).

**RF32 – Relatório de Vendas por Categoria**
> O sistema deve gerar relatórios de vendas agregadas por categoria de produto.

**RF33 – Relatório por Produto**
> O sistema deve apresentar a evolução de vendas de um produto ao longo do tempo.

**RF34 – Relatório Comparativo entre Lojas**
> O Gestor da Cadeia deve poder comparar o desempenho de todas as lojas num período selecionado.

**RF35 – Exportação de Relatórios**
> Os relatórios devem ser exportáveis em formato PDF e CSV.

---

## 4. Requisitos Não Funcionais

### 4.1 Desempenho
**RNF01 – Tempo de Resposta**
> As páginas e operações de consulta devem responder em menos de 2 segundos em condições normais de carga.

**RNF02 – Carga Concorrente**
> O sistema deve suportar pelo menos 50 utilizadores simultâneos sem degradação significativa de desempenho.

### 4.2 Usabilidade
**RNF03 – Interface Intuitiva**
> A interface deve ser intuitiva, permitindo que um funcionário realize uma venda sem formação prévia superior a 1 hora.

**RNF04 – Responsividade**
> A interface deve ser utilizável em ecrãs de desktop e tablet (mínimo 768px de largura).

**RNF05 – Acessibilidade**
> A interface deve seguir as diretrizes WCAG 2.1 nível AA nas funcionalidades críticas (registo de vendas, dashboard).

### 4.3 Segurança
**RNF06 – Autenticação Segura**
> As passwords devem ser armazenadas com hashing (BCrypt ou PBKDF2). As sessões devem expirar ao fim de 8 horas de inatividade.

**RNF07 – Autorização**
> O sistema deve impedir que um utilizador aceda a dados de uma loja à qual não tem permissão.

**RNF08 – Auditoria**
> O sistema deve registar em log todas as operações críticas (vendas, ajustes de stock, alterações de preço, consolidações) com utilizador, data/hora e dados alterados.

**RNF09 – HTTPS**
> Toda a comunicação deve ser feita sobre HTTPS.

### 4.4 Fiabilidade e Disponibilidade
**RNF10 – Disponibilidade**
> O sistema deverá ter disponibilidade mínima de 99% em horário laboral (08:00–22:00).

**RNF11 – Tolerância a Falhas de Rede**
> Cada loja deve conseguir continuar a registar vendas localmente mesmo sem conectividade com o servidor central, sincronizando quando a ligação for restabelecida.

### 4.5 Manutenibilidade
**RNF12 – Modularidade**
> O sistema deve ser desenvolvido em camadas bem definidas (Apresentação, Lógica de Negócio, Dados) para facilitar manutenção e evolução.

**RNF13 – Documentação**
> O código deve conter comentários XML nos métodos públicos e interfaces. Deve existir documentação de API.

### 4.6 Portabilidade
**RNF14 – Independência de SO**
> O servidor deve poder ser executado em Windows ou Linux.

**RNF15 – Browser**
> A aplicação deve ser compatível com Chrome, Firefox, Edge e Safari (versões iguais ou superiores às dos últimos 2 anos).

---

## 5. Restrições e Pressupostos

- **Linguagem:** C# / .NET 8
- **Framework UI:** Blazor Server
- **ORM:** Entity Framework Core 8
- **Base de Dados:** SQLite (desenvolvimento), SQL Server ou PostgreSQL (produção)
- **Autenticação:** ASP.NET Core Identity com JWT ou Cookies
- **Relatórios:** biblioteca como QuestPDF ou ClosedXML para exportação
- A equipa é composta por estudantes de Engenharia Informática, com conhecimentos em C# e .NET

---

## 6. Interfaces Externas

### 6.1 Interface de Utilizador
- Aplicação web responsiva acessível por browser
- Linguagem: Português (Portugal)
- Tema visual: claro, com suporte futuro para tema escuro

### 6.2 Interface de Hardware
- Nenhuma interface de hardware dedicada (leitor de código de barras pode ser usado via input de teclado)

### 6.3 Interface de Software
- **Base de Dados:** SQLite/SQL Server via EF Core
- **Email (opcional):** SMTP para notificações e recuperação de password

### 6.4 Interface de Comunicação
- HTTP/HTTPS via browser
- Sem API pública na fase inicial (todo o acesso via UI web)

---

## 7. Glossário

| Termo | Definição |
|---|---|
| Cadeia de Lojas | Conjunto de lojas de conveniência geridas centralmente |
| Consolidação | Processo de agregação e sincronização de dados das lojas no servidor central |
| POS | Terminal de ponto de venda para registo de vendas ao público |
| Stock | Inventário físico disponível de um produto numa loja |
| Stock Mínimo | Threshold abaixo do qual um alerta de reposição é emitido |
| Encomenda | Pedido de reposição de produtos a um fornecedor |
| KPI | Key Performance Indicator – indicador-chave de desempenho |
| RBAC | Role-Based Access Control – controlo de acesso por papel |
| Fatura | Documento fiscal emitido após uma venda |
| Nota de Crédito | Documento emitido em caso de devolução de produtos faturados |
