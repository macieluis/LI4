# Relatório de Etapa 1 – Conceção e Engenharia de Requisitos
## Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência (SGCLC)

---

**Unidade Curricular:** Laboratórios de Informática IV (LI4)  
**Instituição:** Universidade do Minho – Escola de Engenharia  
**Curso:** Licenciatura em Engenharia Informática  
**Ano Letivo:** 2025/2026  
**Tema:** 1 – Sistema de Gestão Integrada para uma Cadeia de Pequenas Lojas de Conveniência  
**Data:** 24 de Fevereiro de 2026  

---

## Resumo

O presente relatório descreve o trabalho realizado na **Etapa 1** do projeto LI4 2025/2026, correspondente à fase de **Conceção e Engenharia de Requisitos** do sistema **SGCLC – Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência**.

O projeto enquadra-se no Tema 1 do enunciado e tem como objetivo desenvolver um sistema web centralizado para apoiar a gestão da cadeia de lojas de conveniência **QuickMart**, com 5 lojas distribuídas pelo distrito de Braga. O sistema substituirá os processos manuais existentes (folhas de cálculo, registos em papel) por uma plataforma integrada de gestão de vendas, stock, encomendas, faturação e relatórios.

Nesta etapa, foi realizado o levantamento e análise de requisitos através de **três reuniões formais com o cliente**, um **questionário a 15 funcionários de POS** e **análise documental** dos processos existentes. Resultaram deste processo **42 Requisitos Funcionais** e **15 Requisitos Não Funcionais**, organizados segundo a norma IEEE 830. Foram ainda criadas **23 User Stories** em 8 épicos e **25 Casos de Uso** com fluxos normais e alternativos, garantindo rastreabilidade completa entre todos os artefactos.

O estado final desta etapa é **completo** – todos os artefactos de requisitos foram produzidos, validados com o cliente e aprovados pela equipa, estando prontos para servir de base à Etapa 2 (Design e Arquitetura).

---

## Índice

1. [Introdução](#1-introdução)  
   1.1. [Contextualização](#11-contextualização)  
   1.2. [Apresentação do Caso de Estudo](#12-apresentação-do-caso-de-estudo)  
   1.3. [Motivação e Objetivos](#13-motivação-e-objetivos)  
   1.4. [Estrutura do Relatório](#14-estrutura-do-relatório)  
2. [Metodologia de Levantamento de Requisitos](#2-metodologia-de-levantamento-de-requisitos)  
3. [Stakeholders e Utilizadores do Sistema](#3-stakeholders-e-utilizadores-do-sistema)  
4. [Requisitos Funcionais](#4-requisitos-funcionais)  
5. [Requisitos Não Funcionais](#5-requisitos-não-funcionais)  
6. [User Stories](#6-user-stories)  
7. [Casos de Uso](#7-casos-de-uso)  
8. [Rastreabilidade](#8-rastreabilidade)  
9. [Conclusões e Trabalho Futuro](#9-conclusões-e-trabalho-futuro)  
10. [Referências Bibliográficas](#10-referências-bibliográficas)  

**Anexos:**  
- [Anexo A – Atas de Reuniões com o Cliente](#anexo-a--atas-de-reuniões-com-o-cliente)  
- [Anexo B – Questionário a Funcionários de POS](#anexo-b--questionário-a-funcionários-de-pos)  
- [Anexo C – Glossário](#anexo-c--glossário)  

---

## 1. Introdução

### 1.1. Contextualização

A cadeia de lojas de conveniência **QuickMart** é composta por 5 unidades distribuídas pelo distrito de Braga. Atualmente, cada loja opera de forma completamente autónoma e desconectada, recorrendo a processos manuais — folhas de cálculo Excel, registo de vendas em papel e comunicação por telefone — para a gestão diária.

Esta dispersão operacional origina múltiplos problemas identificados em reuniões com a gestão:

- **Falta de visibilidade em tempo real:** o Gestor da Cadeia só tem acesso aos dados de cada loja quando os gerentes enviam manualmente os ficheiros, processo que pode demorar vários dias;
- **Ineficiência na gestão de stock:** ruturas de stock não são detetadas atempadamente, resultando em perda de vendas;
- **Erros de consolidação manual:** a agregação mensal dos dados de 5 lojas em folhas de cálculo diferentes é propensa a inconsistências e erros;
- **Processos de encomenda não standardizados:** cada gerente gere encomendas a fornecedores de forma ad hoc, sem registo centralizado.

O projeto **SGCLC** surge como resposta a estes problemas, propondo a digitalização e centralização dos processos de gestão da cadeia.

### 1.2. Apresentação do Caso de Estudo

O caso de estudo corresponde ao **Tema 1** do enunciado de LI4 2025/2026: *"Sistema de Gestão Integrada para uma Cadeia de Pequenas Lojas de Conveniência"*.

A organização cliente — **QuickMart** — apresenta as seguintes características:

| Característica | Detalhe |
|---|---|
| **Número de lojas** | 5 (distrito de Braga) |
| **Utilizadores do sistema** | 1 Gestor da Cadeia + 5 Gerentes de Loja + ~15 Funcionários de POS |
| **Fornecedores regulares** | 8 |
| **Processos atuais** | Manual (Excel, papel, telefone) |
| **Objetivo** | Substituir processos manuais por sistema web integrado |

A solução a desenvolver é uma **aplicação web centralizada** que permite:

1. A cada loja registar vendas e controlar o seu stock em tempo real;
2. Ao Gestor da Cadeia ter visibilidade global sobre toda a cadeia;
3. A consolidação automática diária dos dados de todas as lojas num servidor central;
4. A geração de relatórios de gestão e apoio à tomada de decisão.

```
[Loja A – Braga Centro]  ──┐
[Loja B – Palmeira]       ──┤──► [Servidor Central SGCLC] ──► [Gestor da Cadeia]
[Loja C – Maximinos]      ──┤
[Loja D – Barcelos]       ──┤
[Loja E – Guimarães]      ──┘
```
*Figura 1 – Arquitetura de alto nível do SGCLC*

### 1.3. Motivação e Objetivos

**Motivação:** Os processos manuais em vigor na QuickMart geram ineficiências operacionais, erros de gestão e perda de oportunidades de negócio. A ausência de informação atualizada obriga o Gestor da Cadeia a tomar decisões estratégicas com dados incompletos ou desatualizados.

**Objetivos do sistema:**

| Objetivo | Descrição |
|---|---|
| **Centralização** | Unificar a gestão de produtos, stock e vendas de todas as lojas numa plataforma única |
| **Tempo real** | Disponibilizar informação atualizada sobre stock e vendas a qualquer momento |
| **Automação** | Substituir processos manuais por processos automatizados (consolidação, alertas, relatórios) |
| **Rastreabilidade** | Manter auditoria completa de todas as operações críticas |
| **Escalabilidade** | Suportar o crescimento futuro da cadeia (novas lojas, novos produtos) |

**Objetivos desta etapa (Etapa 1):**

1. Realizar o levantamento completo de requisitos junto dos stakeholders
2. Documentar os requisitos numa SRS conforme a norma IEEE 830
3. Criar User Stories e Casos de Uso com critérios de aceitação verificáveis
4. Validar os requisitos com o cliente antes de avançar para a fase de design

### 1.4. Estrutura do Relatório

O presente relatório está organizado da seguinte forma:

- **Capítulo 2** descreve a metodologia de levantamento de requisitos utilizada;
- **Capítulo 3** caracteriza os stakeholders e utilizadores do sistema;
- **Capítulo 4** apresenta os 42 Requisitos Funcionais organizados por área funcional;
- **Capítulo 5** apresenta os 15 Requisitos Não Funcionais por categoria;
- **Capítulo 6** expõe as 23 User Stories em formato ágil com critérios de aceitação;
- **Capítulo 7** resume os principais Casos de Uso;
- **Capítulo 8** apresenta a matriz de rastreabilidade RF ↔ UC;
- **Capítulo 9** apresenta as conclusões e próximos passos;
- Os **Anexos** incluem as atas de reuniões, o questionário e o glossário.

---

## 2. Metodologia de Levantamento de Requisitos

O levantamento de requisitos foi conduzido com recurso a quatro técnicas complementares:

| Técnica | Descrição | Referência |
|---|---|---|
| **Reuniões com o cliente** | 3 sessões estruturadas com o Gestor da Cadeia e gerentes de loja | Ver Anexo A |
| **Questionário** | Enviado a 15 funcionários de POS; 11 respostas recebidas | Ver Anexo B |
| **Análise documental** | Análise dos processos existentes (Excel, faturas manuais, processos de encomenda) | — |
| **Observação direta** | Acompanhamento do processo de venda e reposição numa loja | — |

### Fontes de Requisitos Identificadas

| ID | Fonte | Descrição |
|---|---|---|
| **R1** | Reunião inicial com o Gestor da Cadeia (05/02/2026) | Necessidades estratégicas, requisitos de alto nível |
| **R2** | Reunião com Gerentes de Loja (10/02/2026) | Requisitos operacionais, stock, encomendas |
| **R3** | Questionário a Funcionários de POS | Usabilidade POS, rapidez, descontos |
| **R4** | Análise de Processos Existentes | Restrições técnicas, regras de negócio implícitas |
| **R5** | Benchmarking de sistemas similares | Boas práticas, RNFs de desempenho e segurança |

---

## 3. Stakeholders e Utilizadores do Sistema

| Stakeholder | Papel no Sistema | Interesse Principal |
|---|---|---|
| **Gestor da Cadeia** | Utilizador primário (nível central) | Visão consolidada, relatórios, decisão estratégica |
| **Gerente de Loja** | Utilizador primário (nível loja) | Gestão de stock, encomendas, relatórios da loja |
| **Funcionário de POS** | Utilizador operacional | Rapidez e simplicidade no registo de vendas |
| **Fornecedor** | Stakeholder externo | Receber encomendas de forma clara e estruturada |
| **Departamento de TI** | Responsável técnico | Manutenção, disponibilidade, segurança do sistema |

### Controlo de Acesso por Papel (RBAC)

| Funcionalidade | Gestor da Cadeia | Gerente de Loja | Funcionário |
|---|---|---|---|
| Dashboard global | ✅ | ❌ | ❌ |
| Dashboard de loja | ✅ (todas) | ✅ (só a sua) | ❌ |
| Ponto de Venda (POS) | ✅ | ✅ | ✅ |
| Gestão de Stock | ✅ | ✅ (só a sua) | 👁 (só leitura) |
| Encomendas | ✅ | ✅ (só a sua) | ❌ |
| Faturação | ✅ | ✅ (só a sua) | ❌ |
| Relatórios | ✅ (global) | ✅ (só a sua) | ❌ |
| Consolidação | ✅ | ❌ | ❌ |
| Gestão de Utilizadores | ✅ | ❌ | ❌ |

---

## 4. Requisitos Funcionais

O sistema possui **42 Requisitos Funcionais** organizados em 8 áreas funcionais. Para cada requisito são indicados: ID, descrição, fonte, relevância, prioridade e estado.

**Legenda:**  
- **Fonte:** R1–R5 (ver Capítulo 2)  
- **Relevância:** Essencial / Importante / Desejável  
- **Prioridade:** Alta / Média / Baixa  
- **Estado:** Aprovado / Em revisão / Pendente

### 4.1 Autenticação e Gestão de Utilizadores

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF01** | O sistema deve permitir que utilizadores se autentiquem com email e password | R1 | Essencial | Alta | Aprovado |
| **RF02** | O sistema deve restringir funcionalidades com base no papel do utilizador (Gestor da Cadeia, Gerente de Loja, Funcionário) | R1 | Essencial | Alta | Aprovado |
| **RF03** | O Gestor da Cadeia deve poder criar, editar, desativar e eliminar contas de utilizador e atribuir papéis e lojas | R1 | Essencial | Alta | Aprovado |
| **RF04** | O sistema deve suportar recuperação de password por email com link temporário (válido 1 hora) | R3 | Importante | Média | Aprovado |
| **RF05** | Após 5 tentativas de login falhadas, a conta deve ser bloqueada automaticamente por 15 minutos | R1, R4 | Importante | Média | Aprovado |

### 4.2 Gestão de Produtos e Catálogo

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF06** | O sistema deve permitir criar, consultar, editar e desativar produtos com os atributos: código único, nome, descrição, categoria, preço de custo, preço de venda, unidade de medida e foto (opcional) | R1, R4 | Essencial | Alta | Aprovado |
| **RF07** | O sistema deve suportar organização de produtos em categorias e subcategorias hierárquicas | R1 | Essencial | Alta | Aprovado |
| **RF08** | O sistema deve permitir que o preço de venda de um produto seja diferente por loja (sobrepondo o preço base central) | R2 | Importante | Média | Aprovado |
| **RF09** | O sistema deve disponibilizar um catálogo de produtos pesquisável por nome, código ou categoria | R2, R3 | Essencial | Alta | Aprovado |
| **RF10** | O sistema deve alertar o utilizador em caso de código de produto duplicado ao criar um novo produto | R4 | Importante | Média | Aprovado |

### 4.3 Gestão de Stock

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF11** | O sistema deve apresentar o stock atual de cada produto em cada loja com indicação visual de alerta | R2 | Essencial | Alta | Aprovado |
| **RF12** | O stock deve ser atualizado automaticamente após cada venda ou receção de encomenda | R1, R4 | Essencial | Alta | Aprovado |
| **RF13** | O Gerente de Loja deve poder definir o stock mínimo por produto; o sistema gera alerta quando o stock atinge ou desce abaixo desse valor | R2 | Essencial | Alta | Aprovado |
| **RF14** | O Gestor da Cadeia deve visualizar alertas de stock agregados de todas as lojas no dashboard central | R1 | Importante | Alta | Aprovado |
| **RF15** | O Gerente de Loja deve poder registar ajustes manuais de stock (quebras, devoluções, inventário) com motivo obrigatório, registados em auditoria | R2, R4 | Essencial | Alta | Aprovado |
| **RF16** | O sistema deve permitir a exportação da listagem de stock em formato CSV | R2 | Desejável | Baixa | Aprovado |

### 4.4 Ponto de Venda (POS)

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF17** | O sistema deve permitir ao Funcionário registar uma venda adicionando produtos por código ou pesquisa, calculando o total automaticamente | R3, R1 | Essencial | Alta | Aprovado |
| **RF18** | O sistema deve suportar a aplicação de descontos percentuais ou de valor fixo por linha de venda ou sobre o total da venda | R2, R3 | Importante | Média | Aprovado |
| **RF19** | Após a conclusão da venda, o sistema deve gerar automaticamente um recibo com: data/hora, loja, produtos, quantidades, preços unitários, descontos e total | R3, R1 | Essencial | Alta | Aprovado |
| **RF20** | O sistema deve impedir a venda de uma quantidade superior ao stock disponível, com mensagem de alerta ao utilizador | R4 | Essencial | Alta | Aprovado |
| **RF21** | O sistema deve permitir ao Gerente de Loja cancelar uma venda ou processar devoluções de itens, com motivo obrigatório e reversão automática do stock | R2 | Essencial | Alta | Aprovado |
| **RF22** | O sistema deve permitir consultar o histórico de vendas de uma loja com filtros por data, produto e funcionário | R1, R2 | Importante | Média | Aprovado |

### 4.5 Gestão de Fornecedores e Encomendas

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF23** | O sistema deve permitir gerir fornecedores: criar, editar e desativar, com os dados: nome, NIF, morada, contactos | R1 | Essencial | Alta | Aprovado |
| **RF24** | O Gerente de Loja deve poder criar encomendas de reposição a fornecedores, especificando produtos e quantidades | R2 | Essencial | Alta | Aprovado |
| **RF25** | O sistema deve registar e apresentar o estado das encomendas: Pendente, Enviada, Rececionada, Cancelada | R2 | Essencial | Alta | Aprovado |
| **RF26** | Ao registar a receção de uma encomenda, o stock dos produtos envolvidos deve ser atualizado automaticamente com as quantidades efetivamente recebidas | R2, R4 | Essencial | Alta | Aprovado |
| **RF27** | O Gestor da Cadeia deve ser notificado de novas encomendas criadas pelas lojas | R1 | Importante | Média | Aprovado |

### 4.6 Faturação

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF28** | O sistema deve permitir a emissão de faturas associadas a vendas, com dados completos da loja, cliente (nome e NIF) e linhas de produto | R1, R4 | Essencial | Alta | Aprovado |
| **RF29** | As faturas devem ter numeração sequencial única por loja e ano | R4 | Essencial | Alta | Aprovado |
| **RF30** | O sistema deve permitir consultar e filtrar faturas por loja, data e estado (Emitida, Paga, Anulada) | R1, R2 | Importante | Média | Aprovado |
| **RF31** | O sistema deve suportar a exportação de faturas em formato PDF | R1 | Importante | Média | Aprovado |
| **RF32** | O sistema deve suportar a emissão de notas de crédito em caso de devolução faturada | R1, R4 | Desejável | Baixa | Aprovado |

### 4.7 Consolidação Diária

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF33** | O sistema deve executar automaticamente a consolidação dos dados de cada loja no servidor central ao fim de cada dia, numa hora configurável (default: 23:59) | R1 | Essencial | Alta | Aprovado |
| **RF34** | O Gestor da Cadeia deve poder desencadear manualmente a consolidação de uma ou de todas as lojas a qualquer momento | R1 | Essencial | Alta | Aprovado |
| **RF35** | O sistema deve manter um registo (log) de cada consolidação efetuada, com: data/hora, loja, resultado (sucesso/falha) e totais consolidados | R1, R4 | Essencial | Alta | Aprovado |
| **RF36** | Em caso de falha de consolidação, o sistema deve tentar novamente de 30 em 30 minutos e notificar o Gestor da Cadeia | R1 | Importante | Alta | Aprovado |

### 4.8 Relatórios e Dashboard

| ID | Descrição | Fonte | Relevância | Prioridade | Estado |
|---|---|---|---|---|---|
| **RF37** | O Gestor da Cadeia deve visualizar um dashboard central com KPIs globais: total de vendas do dia (por loja e agregado), alertas de stock, encomendas pendentes e produtos mais vendidos | R1 | Essencial | Alta | Aprovado |
| **RF38** | O Gerente de Loja deve visualizar um dashboard com os KPIs da sua loja: vendas do dia, stock em alerta e encomendas pendentes | R2 | Essencial | Alta | Aprovado |
| **RF39** | O sistema deve gerar relatórios de vendas de uma ou todas as lojas filtráveis por período (dia, semana, mês, intervalo personalizado) | R1 | Essencial | Alta | Aprovado |
| **RF40** | O sistema deve gerar relatórios de vendas agregadas por categoria de produto | R1 | Importante | Média | Aprovado |
| **RF41** | O Gestor da Cadeia deve poder comparar o desempenho de todas as lojas num período selecionado, com gráfico comparativo | R1 | Importante | Média | Aprovado |
| **RF42** | Os relatórios devem ser exportáveis em formato PDF e CSV | R1 | Importante | Média | Aprovado |

---

## 5. Requisitos Não Funcionais

O sistema possui **15 Requisitos Não Funcionais** organizados por categoria de qualidade segundo a norma ISO/IEC 25010.

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

## 6. User Stories

As User Stories seguem o formato padrão: *"Como [ator], quero [ação], para [benefício]"*, com Critérios de Aceitação verificáveis. Estão organizadas em 8 épicos funcionais.

### Épico 1 – Autenticação e Controlo de Acesso

| ID | User Story | Ator | RFs Relacionados |
|---|---|---|---|
| **US-01** | Login no Sistema | GC / GL / FN | RF01, RF02, RF05 |
| **US-02** | Gestão de Utilizadores | GC | RF03 |
| **US-03** | Recuperação de Password | GC / GL / FN | RF04 |

**US-01 – Login no Sistema**
> Como qualquer utilizador registado, quero autenticar-me com email e password, para aceder às funcionalidades que correspondem ao meu papel.

*Critérios de Aceitação:*
- Login correto redireciona para o dashboard do papel do utilizador
- Password incorreta mostra mensagem de erro sem revelar qual campo está errado
- Após 5 tentativas falhadas, a conta é bloqueada por 15 minutos
- Sessão expira automaticamente após 8 horas de inatividade

---

### Épico 2 – Gestão de Produtos e Catálogo

| ID | User Story | Ator | RFs Relacionados |
|---|---|---|---|
| **US-04** | Criar Produto | GC | RF06, RF10 |
| **US-05** | Pesquisar Produtos | GC / GL / FN | RF09 |
| **US-06** | Definir Preço por Loja | GL | RF08 |

**US-04 – Criar Produto**
> Como Gestor da Cadeia, quero criar novos produtos no catálogo central, para que estejam disponíveis em todas as lojas.

*Critérios de Aceitação:*
- Campos obrigatórios: código único, nome, categoria, preço base de venda, unidade de medida
- Código deve ser único; sistema alerta duplicados
- Posso associar uma foto ao produto
- Produto criado fica disponível para todas as lojas de imediato

---

### Épico 3 – Gestão de Stock

| ID | User Story | Ator | RFs Relacionados |
|---|---|---|---|
| **US-07** | Visualizar Stock da Minha Loja | GL | RF11, RF16 |
| **US-08** | Definir Stock Mínimo | GL | RF13, RF14 |
| **US-09** | Ajuste Manual de Stock | GL | RF15 |

**US-07 – Visualizar Stock da Minha Loja**
> Como Gerente de Loja, quero ver o stock atual de todos os produtos da minha loja, para saber o que está disponível e o que precisa de ser reposto.

*Critérios de Aceitação:*
- Lista de produtos com quantidade disponível
- Produtos abaixo do stock mínimo aparecem destacados (vermelho/alerta)
- Posso filtrar por categoria e pesquisar por produto
- Posso exportar a listagem em CSV

---

### Épico 4 – Ponto de Venda (POS)

| ID | User Story | Ator | RFs Relacionados |
|---|---|---|---|
| **US-10** | Registar uma Venda | FN | RF17, RF20 |
| **US-11** | Aplicar Desconto | FN | RF18 |
| **US-12** | Emitir Recibo | FN | RF19 |
| **US-13** | Cancelar/Devolver Venda | GL | RF21, RF22 |

**US-10 – Registar uma Venda**
> Como Funcionário, quero registar vendas adicionando produtos ao carrinho, para processar compras dos clientes.

*Critérios de Aceitação:*
- Posso adicionar produtos por código ou pesquisa por nome
- O total é calculado automaticamente em tempo real
- Sistema impede venda de quantidade superior ao stock disponível
- Ao finalizar, o stock é deduzido automaticamente

---

### Épico 5 – Gestão de Fornecedores e Encomendas

| ID | User Story | Ator | RFs Relacionados |
|---|---|---|---|
| **US-14** | Gerir Fornecedores | GC | RF23 |
| **US-15** | Criar Encomenda a Fornecedor | GL | RF24, RF25, RF27 |
| **US-16** | Registar Receção de Encomenda | GL | RF26 |

### Épico 6 – Faturação

| ID | User Story | Ator | RFs Relacionados |
|---|---|---|---|
| **US-17** | Emitir Fatura | FN / GL | RF28, RF29, RF31 |
| **US-18** | Consultar Faturas | GL | RF30, RF32 |

### Épico 7 – Consolidação Diária

| ID | User Story | Ator | RFs Relacionados |
|---|---|---|---|
| **US-19** | Consolidação Automática Diária | Sistema | RF33, RF35, RF36 |
| **US-20** | Consolidação Manual | GC | RF34 |

**US-19 – Consolidação Automática Diária**
> Como Gestor da Cadeia, quero que os dados de todas as lojas sejam consolidados automaticamente no servidor central ao fim de cada dia, para ter uma visão global e atualizada da cadeia.

*Critérios de Aceitação:*
- A consolidação corre automaticamente à hora configurada (default: 23:59)
- Após consolidação, o dashboard central mostra os dados do dia consolidado
- O sistema regista um log por cada consolidação (loja, data/hora, resultado, totais)
- Em caso de erro, o sistema tenta novamente e notifica o administrador

### Épico 8 – Relatórios e Dashboard

| ID | User Story | Ator | RFs Relacionados |
|---|---|---|---|
| **US-21** | Dashboard Central | GC | RF37 |
| **US-22** | Dashboard de Loja | GL | RF38 |
| **US-23** | Relatório de Vendas | GC / GL | RF39, RF40, RF42 |
| **US-24** | Relatório Comparativo entre Lojas | GC | RF41 |

---

## 7. Casos de Uso

Foram identificados **25 Casos de Uso** que cobrem todos os fluxos funcionais do sistema. A tabela seguinte apresenta cada UC com o ator principal e os requisitos associados.

| ID UC | Nome | Ator Principal | RFs |
|---|---|---|---|
| **UC01** | Login no Sistema | Todos | RF01, RF02, RF05 |
| **UC02** | Recuperar Password | Todos | RF04 |
| **UC03** | Gerir Utilizadores | Gestor Cadeia | RF03 |
| **UC04** | Gerir Produtos | Gestor Cadeia | RF06, RF10 |
| **UC05** | Gerir Categorias | Gestor Cadeia | RF07 |
| **UC06** | Definir Preço por Loja | Gerente Loja | RF08 |
| **UC07** | Visualizar Stock | Gerente / Gestor | RF11, RF12, RF16 |
| **UC08** | Definir Stock Mínimo | Gerente Loja | RF13 |
| **UC09** | Ajuste Manual de Stock | Gerente Loja | RF14, RF15 |
| **UC10** | Registar Venda | Funcionário | RF17, RF20 |
| **UC11** | Aplicar Desconto | Funcionário | RF18 |
| **UC12** | Emitir Recibo | Funcionário | RF19 |
| **UC13** | Cancelar/Devolver Venda | Gerente Loja | RF21, RF22 |
| **UC14** | Gerir Fornecedores | Gestor Cadeia | RF23 |
| **UC15** | Criar Encomenda | Gerente Loja | RF24, RF25, RF27 |
| **UC16** | Recepcionar Encomenda | Gerente Loja | RF26 |
| **UC17** | Emitir Fatura | Funcionário / Gerente | RF28, RF29, RF31 |
| **UC18** | Consultar Faturas | Gerente / Gestor | RF30, RF32 |
| **UC19** | Consolidação Automática | Sistema (Job) | RF33, RF35, RF36 |
| **UC20** | Consolidação Manual | Gestor Cadeia | RF34 |
| **UC21** | Dashboard Central | Gestor Cadeia | RF37 |
| **UC22** | Dashboard de Loja | Gerente Loja | RF38 |
| **UC23** | Relatório de Vendas | Gestor / Gerente | RF39, RF40, RF42 |
| **UC24** | Exportar Relatório | Gestor / Gerente | RF42 |
| **UC25** | Relatório Comparativo | Gestor Cadeia | RF41 |

> O documento completo com fluxos normais, alternativos e pré/pós-condições de cada UC está disponível em `docs/etapa1/use_cases.md`.

---

## 8. Rastreabilidade

### Matriz de Rastreabilidade RF ↔ UC

| Requisito | Caso(s) de Uso |
|---|---|
| RF01, RF02, RF04, RF05 | UC01 – Login; UC02 – Recuperar Password |
| RF03 | UC03 – Gerir Utilizadores |
| RF06, RF09, RF10 | UC04 – Gerir Produtos |
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
| RF41 | UC25 – Relatório Comparativo |

### Cobertura por Área Funcional

| Área Funcional | Nº RFs | Nº UCs associados | User Stories |
|---|---|---|---|
| Autenticação e RBAC | 5 | 3 | US-01, US-02, US-03 |
| Gestão de Produtos | 5 | 3 | US-04, US-05, US-06 |
| Gestão de Stock | 6 | 3 | US-07, US-08, US-09 |
| Ponto de Venda | 6 | 4 | US-10, US-11, US-12, US-13 |
| Fornecedores e Encomendas | 5 | 3 | US-14, US-15, US-16 |
| Faturação | 5 | 2 | US-17, US-18 |
| Consolidação | 4 | 2 | US-19, US-20 |
| Relatórios e Dashboard | 6 | 4 | US-21, US-22, US-23, US-24 |
| **Total** | **42** | **24** | **23** |

---

## 9. Conclusões e Trabalho Futuro

### 9.1. Conclusões

A Etapa 1 do projeto SGCLC foi concluída com sucesso. O processo de levantamento de requisitos, conduzido através de reuniões, questionários e análise documental, permitiu identificar com clareza as necessidades dos stakeholders e traduzi-las num conjunto coerente e rastreável de requisitos.

Os principais artefactos produzidos nesta etapa são:

| Artefacto | Ficheiro | Conteúdo |
|---|---|---|
| SRS (IEEE 830) | `docs/etapa1/SRS.md` | Especificação completa de requisitos |
| Tabelas de Requisitos | `docs/etapa1/requisitos.md` | 42 RFs + 15 RNFs com rastreabilidade |
| User Stories | `docs/etapa1/user_stories.md` | 23 User Stories em 8 épicos |
| Casos de Uso | `docs/etapa1/use_cases.md` | 25 UCs com fluxos completos |
| Atas de Reuniões | `docs/etapa1/atas_reunioes.md` | 3 atas + questionário a funcionários |

Os requisitos foram validados e aprovados pelo cliente (João Martins – Gestor da Cadeia QuickMart) na reunião de 10 de Fevereiro de 2026, com registo em ata.

**Pontos fortes desta etapa:**
- Cobertura completa de todos os módulos funcionais identificados no enunciado
- Rastreabilidade bidirecional entre RF ↔ UC ↔ User Story
- Requisitos classificados por relevância, prioridade e estado
- Participação ativa do cliente na validação dos requisitos

### 9.2. Trabalho Futuro

A próxima etapa do projeto (Etapa 2 – Design e Arquitetura) utilizará os requisitos produzidos nesta etapa como base para:

1. **Definir a arquitetura do sistema** – escolha de padrões (3-Tier, Repository, etc.);
2. **Elaborar os diagramas UML** – Classes, Sequência, Componentes;
3. **Criar o modelo de dados** – Entidade-Relacionamento e SQL DDL;
4. **Desenhar os wireframes** das interfaces principais (associados a cada UC/RF).

---

## 10. Referências Bibliográficas

[1] IEEE Std 830-1998 – *IEEE Recommended Practice for Software Requirements Specifications*. IEEE, 1998.

[2] ISO/IEC/IEEE 29148:2018 – *Systems and software engineering – Life cycle processes – Requirements engineering*. ISO/IEC/IEEE, 2018.

[3] ISO/IEC 25010:2011 – *Systems and software Quality Requirements and Evaluation (SQuaRE) – System and software quality models*. ISO/IEC, 2011.

[4] Sommerville, I. (2015). *Software Engineering* (10th ed.). Pearson.

[5] Robertson, S., & Robertson, J. (2012). *Mastering the Requirements Process: Getting Requirements Right* (3rd ed.). Addison-Wesley.

---

## Anexo A – Atas de Reuniões com o Cliente

### Ata nº 1 – Reunião de Levantamento Inicial

| Campo | Detalhe |
|---|---|
| **Data** | 05 de Fevereiro de 2026 |
| **Hora** | 14:30 – 16:00 |
| **Local** | Sede da Cadeia "QuickMart", Braga |
| **Presentes** | João Martins (Gestor da Cadeia – Cliente), Equipa de Desenvolvimento LI4 |
| **Tipo** | Presencial |

**Principais necessidades identificadas:**
- Sistema centralizado de controlo de stock em tempo real
- Dashboard com KPIs de todas as lojas
- Registo automatizado de vendas (substituir o atual sistema manual/papel)
- Geração automática de relatórios mensais por loja e por produto
- Alertas de stock mínimo por produto por loja
- Processo de encomenda a fornecedores simplificado

**Declarações do cliente:**

> *"Actualmente cada loja usa uma folha de Excel diferente para controlar o stock. No final do mês tenho de pedir os ficheiros a cada gerente e consolidar tudo manualmente – é caótico e propenso a erros."*

> *"Já perdi vendas porque uma loja ficou sem stock de um produto muito vendido e eu só soube 3 dias depois."*

**Ações a desenvolver:**

| Responsável | Ação | Prazo |
|---|---|---|
| Equipa Dev | Elaborar questionário para gerentes e funcionários | 08/02/2026 |
| João Martins | Partilhar listagem de fornecedores e categorias de produtos | 08/02/2026 |
| Equipa Dev | Preparar protótipo de requisitos para próxima reunião | 09/02/2026 |

---

### Ata nº 2 – Reunião de Validação de Requisitos

| Campo | Detalhe |
|---|---|
| **Data** | 10 de Fevereiro de 2026 |
| **Hora** | 15:00 – 16:30 |
| **Local** | Videoconferência (Google Meet) |
| **Presentes** | João Martins (Gestor), Ana Rodrigues (Gerente Loja Braga-Centro), Equipa LI4 |
| **Tipo** | Online |

Foram apresentados os 42 requisitos funcionais preliminares. Declarações relevantes:

> *"É muito importante que possamos definir preços diferentes por loja. Na minha loja o prazo de validade de certos produtos obriga-me a fazer promoções que na sede não sabem."* — Ana Rodrigues

→ Originou o requisito **RF08** (preço de venda configurável por loja).

> *"Os ajustes de stock são frequentes – quebras, produto mal contado, etc. Preciso de registar o motivo senão não consigo justificar perante o gestor."* — Ana Rodrigues

→ Confirmou e reforçou o requisito **RF15** (ajuste manual com motivo obrigatório e auditoria).

> *"Quero ter um ecrã principal onde, de manhã, logo ao acordar, veja como correu o dia anterior em todas as lojas."* — João Martins

→ Confirmou e detalhou o requisito **RF37** (Dashboard Central).

> *"A consolidação diária tem de ser automática. Não posso depender de ninguém para que os dados apareçam no servidor."* — João Martins

→ Tornou **RF33** Essencial/Alta prioridade.

**Requisitos alterados nesta reunião:**

| RF | Alteração |
|---|---|
| RF08 | **Adicionado** – preço de venda configurável por loja |
| RF15 | **Elevada prioridade** – Importante → Essencial |
| RF33 | **Confirmado como Essencial** |

---

### Ata nº 3 – Reunião de Validação de Wireframes

| Campo | Detalhe |
|---|---|
| **Data** | 20 de Fevereiro de 2026 |
| **Hora** | 14:00 – 15:00 |
| **Local** | Videoconferência |
| **Presentes** | João Martins (Gestor), Equipa LI4 |

Foram apresentados os wireframes das principais interfaces. Decisões tomadas:
- Wireframes aprovados ✅
- Equipa avança para a fase de implementação
- Reunião final de aceitação agendada para Maio de 2026

---

## Anexo B – Questionário a Funcionários de POS

**Data de envio:** 08 de Fevereiro de 2026  
**Respostas recebidas:** 11 de 15 funcionários (73%)

**1. Como é atualmente efetuado o registo de uma venda?**
- 73% – registo manual em papel / caderno
- 18% – sistema de caixa antigo sem ligação à gestão central
- 9% – folha de Excel no computador da loja

**2. O que é mais importante numa nova aplicação de POS?**
- 91% – Rapidez de registo (pesquisa de produto ágil)
- 82% – Cálculo automático de descontos e totais
- 73% – Impressão/visualização de recibo
- 55% – Simplicidade da interface

**Conclusões:**
- A simplicidade e velocidade do POS são críticas → reforça **RNF03** e **RF17**
- A capacidade de aplicar descontos é muito valorizada → confirma **RF18**
- A necessidade de feedback visual de stock em tempo real → reforça **RF20**

---

## Anexo C – Glossário

| Termo | Definição |
|---|---|
| **SGCLC** | Sistema de Gestão Integrada para Cadeia de Lojas de Conveniência |
| **POS** | Point of Sale – terminal de registo de vendas ao público |
| **Stock** | Inventário físico disponível de um produto numa loja |
| **Stock Mínimo** | Threshold abaixo do qual um alerta de reposição é emitido |
| **Consolidação** | Processo de agregação e sincronização de dados das lojas no servidor central |
| **Encomenda** | Pedido de reposição de produtos a um fornecedor |
| **KPI** | Key Performance Indicator – indicador-chave de desempenho |
| **RBAC** | Role-Based Access Control – controlo de acesso baseado em papéis |
| **Fatura** | Documento fiscal emitido após uma venda |
| **Nota de Crédito** | Documento emitido em caso de devolução de produtos faturados |
| **SRS** | Software Requirements Specification – Especificação de Requisitos de Software |
| **RF** | Requisito Funcional |
| **RNF** | Requisito Não Funcional |
| **UC** | Use Case – Caso de Uso |
| **US** | User Story |
| **IEEE 830** | Norma para especificação de requisitos de software |
| **EF Core** | Entity Framework Core – ORM para .NET |
| **LLM** | Large Language Model – modelo de linguagem de grande escala |
