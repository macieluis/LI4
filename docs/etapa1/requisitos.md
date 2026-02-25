# Levantamento e Análise de Requisitos
## Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência

**Versão:** 2.0 | **Data:** 24 de Fevereiro de 2026 | **Projeto:** LI4 2025/2026  
**Norma:** IEEE 830 / ISO/IEC/IEEE 29148

---

## 1. Metodologia de Levantamento de Requisitos

O levantamento de requisitos foi realizado com recurso a várias técnicas complementares:

- **Reuniões com o cliente** – sessões estruturadas com o responsável pela cadeia de lojas (ver atas em Anexo A)
- **Questionários** – enviados a gerentes de loja e funcionários (ver Anexo B)
- **Análise de documentação** – análise de processos existentes (folhas de cálculo de stock, faturas manuais)
- **Observação direta** – acompanhamento do processo de venda e reposição numa loja

### Fontes Identificadas

| ID Fonte | Descrição |
|---|---|
| **R1** | Reunião inicial com o Gestor da Cadeia (05/02/2026) |
| **R2** | Reunião com Gerentes de Loja (10/02/2026) |
| **R3** | Questionário a Funcionários de POS |
| **R4** | Análise de Processos Existentes |
| **R5** | Benchmarking de sistemas similares |

---

## 2. Stakeholders

| Stakeholder | Papel no Sistema | Interesse Principal |
|---|---|---|
| **Gestor da Cadeia** | Utilizador primário (nível central) | Visão consolidada, relatórios, decisão estratégica |
| **Gerente de Loja** | Utilizador primário (nível loja) | Gestão de stock, encomendas, relatórios da loja |
| **Funcionário de POS** | Utilizador operacional | Rapidez e simplicidade no registo de vendas |
| **Fornecedor** | Stakeholder externo | Receber encomendas de forma clara |
| **Departamento de TI** | Responsável técnico | Manutenção, disponibilidade, segurança |

---

## 3. Requisitos Funcionais

**Legenda:**
- **Fonte:** origem do requisito (R1–R5)
- **Relevância:** Essencial / Importante / Desejável
- **Prioridade:** Alta / Média / Baixa
- **Estado:** Aprovado / Em revisão / Pendente

---

### 3.1 Autenticação e Gestão de Utilizadores

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF01** | O sistema deve permitir que utilizadores se autentiquem com email e password | R1 | Essencial | Alta | Aprovado |
| **RF02** | O sistema deve restringir funcionalidades com base no papel do utilizador (Gestor da Cadeia, Gerente de Loja, Funcionário) | R1 | Essencial | Alta | Aprovado |
| **RF03** | O Gestor da Cadeia deve poder criar, editar, desativar e eliminar contas de utilizador e atribuir papéis e lojas | R1 | Essencial | Alta | Aprovado |
| **RF04** | O sistema deve suportar recuperação de password por email com link temporário (válido 1 hora) | R3 | Importante | Média | Aprovado |
| **RF05** | Após 5 tentativas de login falhadas, a conta deve ser bloqueada automaticamente por 15 minutos | R1, R4 | Importante | Média | Aprovado |

---

### 3.2 Gestão de Produtos e Catálogo

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF06** | O sistema deve permitir criar, consultar, editar e desativar (logicamente) produtos com os seguintes atributos: código único, nome, descrição, categoria, preço de custo, preço de venda, unidade de medida e foto (opcional) | R1, R4 | Essencial | Alta | Aprovado |
| **RF07** | O sistema deve suportar organização de produtos em categorias e subcategorias hierárquicas | R1 | Essencial | Alta | Aprovado |
| **RF08** | O sistema deve permitir que o preço de venda de um produto seja diferente por loja (sobrepondo o preço base central) | R2 | Importante | Média | Aprovado |
| **RF09** | O sistema deve disponibilizar um catálogo de produtos pesquisável por nome, código ou categoria | R2, R3 | Essencial | Alta | Aprovado |
| **RF10** | O sistema deve alert ar o utilizador em caso de código de produto duplicado ao criar um novo produto | R4 | Importante | Média | Aprovado |

---

### 3.3 Gestão de Stock

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF11** | O sistema deve apresentar o stock atual de cada produto em cada loja com indicação visual de alerta | R2 | Essencial | Alta | Aprovado |
| **RF12** | O stock deve ser atualizado automaticamente após cada venda ou receção de encomenda | R1, R4 | Essencial | Alta | Aprovado |
| **RF13** | O Gerente de Loja deve poder definir o stock mínimo por produto. O sistema gera alerta quando o stock atinge ou desce abaixo desse valor | R2 | Essencial | Alta | Aprovado |
| **RF14** | O Gestor da Cadeia deve visualizar alertas de stock agregados de todas as lojas no dashboard central | R1 | Importante | Alta | Aprovado |
| **RF15** | O Gerente de Loja deve poder registar ajustes manuais de stock (quebras, devoluções, inventário) com motivo obrigatório, registados em auditoria | R2, R4 | Essencial | Alta | Aprovado |
| **RF16** | O sistema deve permitir a exportação da listagem de stock em formato CSV | R2 | Desejável | Baixa | Aprovado |

---

### 3.4 Ponto de Venda (POS)

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF17** | O sistema deve permitir ao Funcionário registar uma venda adicionando produtos por código ou pesquisa, calculando o total automaticamente | R3, R1 | Essencial | Alta | Aprovado |
| **RF18** | O sistema deve suportar a aplicação de descontos percentuais ou de valor fixo por linha de venda ou sobre o total da venda | R2, R3 | Importante | Média | Aprovado |
| **RF19** | Após a conclusão da venda, o sistema deve gerar automaticamente um recibo com: data/hora, loja, produtos, quantidades, preços unitários, descontos e total | R3, R1 | Essencial | Alta | Aprovado |
| **RF20** | O sistema deve impedir a venda de uma quantidade superior ao stock disponível, com mensagem de alerta ao utilizador | R4 | Essencial | Alta | Aprovado |
| **RF21** | O sistema deve permitir ao Gerente de Loja cancelar uma venda ou processar devoluções de itens, com motivo obrigatório e reversão automática do stock | R2 | Essencial | Alta | Aprovado |
| **RF22** | O sistema deve permitir consultar o histórico de vendas de uma loja com filtros por data, produto e funcionário | R1, R2 | Importante | Média | Aprovado |

---

### 3.5 Gestão de Fornecedores e Encomendas

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF23** | O sistema deve permitir gerir fornecedores: criar, editar e desativar, com os dados: nome, NIF, morada, contactos | R1 | Essencial | Alta | Aprovado |
| **RF24** | O Gerente de Loja deve poder criar encomendas de reposição a fornecedores, especificando produtos e quantidades | R2 | Essencial | Alta | Aprovado |
| **RF25** | O sistema deve registar e apresentar o estado das encomendas: Pendente, Enviada, Rececionada, Cancelada | R2 | Essencial | Alta | Aprovado |
| **RF26** | Ao registar a receção de uma encomenda, o stock dos produtos envolvidos deve ser atualizado automaticamente com as quantidades efetivamente recebidas | R2, R4 | Essencial | Alta | Aprovado |
| **RF27** | O Gestor da Cadeia deve ser notificado de novas encomendas criadas pelas lojas | R1 | Importante | Média | Aprovado |

---

### 3.6 Faturação

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF28** | O sistema deve permitir a emissão de faturas associadas a vendas, com dados completos da loja, cliente (nome e NIF) e linhas de produto | R1, R4 | Essencial | Alta | Aprovado |
| **RF29** | As faturas devem ter numeração sequencial única por loja e ano | R4 | Essencial | Alta | Aprovado |
| **RF30** | O sistema deve permitir consultar e filtrar faturas por loja, data e estado (Emitida, Paga, Anulada) | R1, R2 | Importante | Média | Aprovado |
| **RF31** | O sistema deve suportar a exportação de faturas em formato PDF | R1 | Importante | Média | Aprovado |
| **RF32** | O sistema deve suportar a emissão de notas de crédito em caso de devolução faturada | R1, R4 | Desejável | Baixa | Aprovado |

---

### 3.7 Consolidação Diária

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF33** | O sistema deve executar automaticamente a consolidação dos dados de cada loja no servidor central ao fim de cada dia, numa hora configurável (default: 23:59) | R1 | Essencial | Alta | Aprovado |
| **RF34** | O Gestor da Cadeia deve poder desencadear manualmente a consolidação de uma ou de todas as lojas a qualquer momento | R1 | Essencial | Alta | Aprovado |
| **RF35** | O sistema deve manter um registo (log) de cada consolidação efetuada, com: data/hora, loja, resultado (sucesso/falha) e totais consolidados | R1, R4 | Essencial | Alta | Aprovado |
| **RF36** | Em caso de falha de consolidação, o sistema deve tentar novamente de 30 em 30 minutos e notificar o Gestor da Cadeia | R1 | Importante | Alta | Aprovado |

---

### 3.8 Relatórios e Dashboard

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF37** | O Gestor da Cadeia deve visualizar um dashboard central com KPIs globais: total de vendas do dia (por loja e agregado), alertas de stock, encomendas pendentes e produtos mais vendidos | R1 | Essencial | Alta | Aprovado |
| **RF38** | O Gerente de Loja deve visualizar um dashboard com os KPIs da sua loja: vendas do dia, stock em alerta e encomendas pendentes | R2 | Essencial | Alta | Aprovado |
| **RF39** | O sistema deve gerar relatórios de vendas de uma ou todas as lojas filtráveis por período (dia, semana, mês, intervalo personalizado) | R1 | Essencial | Alta | Aprovado |
| **RF40** | O sistema deve gerar relatórios de vendas agregadas por categoria de produto | R1 | Importante | Média | Aprovado |
| **RF41** | O Gestor da Cadeia deve poder comparar o desempenho de todas as lojas num período selecionado, com gráfico comparativo | R1 | Importante | Média | Aprovado |
| **RF42** | Os relatórios devem ser exportáveis em formato PDF e CSV | R1 | Importante | Média | Aprovado |

---

## 4. Requisitos Não Funcionais

| ID | Categoria | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|---|
| **RNF01** | Desempenho | As páginas e operações de consulta devem responder em menos de 2 segundos em condições normais de carga (até 50 utilizadores simultâneos) | R1, R5 | Essencial | Alta | Aprovado |
| **RNF02** | Desempenho | O sistema deve suportar pelo menos 50 utilizadores simultâneos sem degradação significativa do desempenho | R1 | Importante | Média | Aprovado |
| **RNF03** | Usabilidade | A interface deve ser intuitiva, permitindo que um funcionário realize uma venda sem formação prévia superior a 1 hora | R3 | Essencial | Alta | Aprovado |
| **RNF04** | Usabilidade | A interface deve ser responsiva e utilizável em ecrãs de desktop e tablet (largura mínima: 768px) | R1 | Importante | Média | Aprovado |
| **RNF05** | Usabilidade | A aplicação deve estar disponível em Português (Portugal) | R1 | Essencial | Alta | Aprovado |
| **RNF06** | Segurança | As palavras-passe devem ser armazenadas com algoritmo de hashing seguro (BCrypt). As sessões devem expirar ao fim de 8 horas de inatividade | R1, R4 | Essencial | Alta | Aprovado |
| **RNF07** | Segurança | O sistema deve impedir que um utilizador aceda a dados de uma loja à qual não tem permissão | R1, R4 | Essencial | Alta | Aprovado |
| **RNF08** | Segurança | O sistema deve registar em log de auditoria todas as operações críticas (vendas, ajustes de stock, alterações de preço, consolidações) com utilizador, data/hora e dados alterados | R1, R4 | Essencial | Alta | Aprovado |
| **RNF09** | Segurança | Toda a comunicação deve ser feita sobre HTTPS | R4 | Essencial | Alta | Aprovado |
| **RNF10** | Disponibilidade | O sistema deve ter uma disponibilidade mínima de 99% em horário laboral (08:00–22:00) | R1 | Importante | Alta | Aprovado |
| **RNF11** | Fiabilidade | Cada loja deve poder registar vendas localmente mesmo sem conectividade com o servidor central | R2, R4 | Importante | Alta | Em revisão |
| **RNF12** | Manutenibilidade | O código deve ser organizado em camadas (Apresentação, Negócio, Dados) com separação clara de responsabilidades | R4, R5 | Essencial | Alta | Aprovado |
| **RNF13** | Manutenibilidade | O código deve incluir comentários XML nos métodos públicos e interfaces, e deve existir documentação de API interna | R4 | Importante | Média | Aprovado |
| **RNF14** | Portabilidade | O servidor deve poder ser executado em Windows Server ou Linux | R4 | Importante | Média | Aprovado |
| **RNF15** | Compatibilidade | A aplicação deve ser compatível com Chrome, Firefox, Edge e Safari (versões dos últimos 2 anos) | R1, R5 | Importante | Média | Aprovado |

---

## 5. Rastreabilidade Requisitos ↔ Casos de Uso

| RF | Caso(s) de Uso |
|---|---|
| RF01, RF02, RF04, RF05 | UC01 – Login; UC02 – Recuperar Password |
| RF03 | UC03 – Gerir Utilizadores |
| RF06, RF09, RF10 | UC04 – Gerir Produtos; UC05 – Gerir Categorias |
| RF07 | UC05 – Gerir Categorias |
| RF08 | UC06 – Definir Preço por Loja |
| RF11, RF12, RF16 | UC07 – Visualizar Stock |
| RF13 | UC08 – Definir Stock Mínimo |
| RF14, RF15 | UC09 – Ajuste Manual de Stock |
| RF17, RF20 | UC10 – Registar Venda |
| RF18 | UC11 – Aplicar Desconto |
| RF19 | UC12 – Emitir Recibo |
| RF21, RF22 | UC13 – Cancelar/Devolver Venda |
| RF23 | UC14 – Gerir Fornecedores |
| RF24, RF25, RF27 | UC15 – Criar Encomenda |
| RF26 | UC16 – Recepcionar Encomenda |
| RF28, RF29, RF31 | UC17 – Emitir Fatura |
| RF30, RF32 | UC18 – Consultar Faturas |
| RF33, RF35, RF36 | UC19 – Consolidação Automática |
| RF34 | UC20 – Consolidação Manual |
| RF37 | UC21 – Dashboard Central |
| RF38 | UC22 – Dashboard de Loja |
| RF39, RF40, RF42 | UC23 – Relatório de Vendas |
| RF41 | UC25 – Relatório Comparativo entre Lojas |
