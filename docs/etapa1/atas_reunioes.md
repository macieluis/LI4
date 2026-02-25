# Anexo A – Atas de Reuniões com o Cliente
## Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência

**Projeto:** LI4 2025/2026 | **Tema:** 1

---

## Ata nº 1 – Reunião de Levantamento Inicial

| Campo | Detalhe |
|---|---|
| **Data** | 05 de Fevereiro de 2026 |
| **Hora** | 14:30 – 16:00 |
| **Local** | Sede da Cadeia "QuickMart", Braga |
| **Presentes** | João Martins (Gestor da Cadeia – Cliente), Equipa de Desenvolvimento LI4 |
| **Tipo** | Presencial |

### Ordem de Trabalhos
1. Apresentação da equipa de desenvolvimento
2. Enquadramento do projeto e objetivos gerais
3. Levantamento de necessidades de alto nível

### Resumo da Reunião

O cliente, João Martins, apresentou a organização da cadeia "QuickMart", composta por **5 lojas de conveniência** distribuídas pelo distrito de Braga. O cliente descreveu os principais problemas atuais:

> *"Actualmente cada loja usa uma folha de Excel diferente para controlar o stock. No final do mês tenho de pedir os ficheiros a cada gerente e consolidar tudo manualmente – é caótico e propenso a erros."*

> *"Já perdi vendas porque uma loja ficou sem stock de um produto muito vendido e eu só soube 3 dias depois."*

**Principais necessidades identificadas:**
- Sistema centralizado de controlo de stock em tempo real
- Dashboard com KPIs de todas as lojas
- Registo automatizado de vendas (substituir o atual sistema manual/papel)
- Geração automática de relatórios mensais por loja e por produto
- Alertas de stock mínimo por produto por loja
- Processo de encomenda a fornecedores simplificado

**Informação sobre os utilizadores:**
- 1 Gestor da Cadeia (João Martins)
- 5 Gerentes de Loja (um por loja)
- ~15 Funcionários de POS (3 por loja em média)
- 8 fornecedores regulares

### Decisões Tomadas
- A equipa realizará um questionário aos gerentes de loja e funcionários
- Próxima reunião agendada para 10 de Fevereiro de 2026
- Será partilhada uma listagem de requisitos preliminares para validação

### Ação a Desenvolver

| Responsável | Ação | Prazo |
|---|---|---|
| Equipa Dev | Elaborar questionário para gerentes e funcionários | 08/02/2026 |
| João Martins | Partilhar listagem de fornecedores e categorias de produtos | 08/02/2026 |
| Equipa Dev | Preparar protótipo de requisitos para próxima reunião | 09/02/2026 |

---

## Ata nº 2 – Reunião de Validação de Requisitos

| Campo | Detalhe |
|---|---|
| **Data** | 10 de Fevereiro de 2026 |
| **Hora** | 15:00 – 16:30 |
| **Local** | Videoconferência (Google Meet) |
| **Presentes** | João Martins (Gestor), Ana Rodrigues (Gerente Loja Braga-Centro), Equipa LI4 |
| **Tipo** | Online |

### Ordem de Trabalhos
1. Revisão dos requisitos preliminares
2. Validação com gerente de loja
3. Análise do processo atual de vendas e stock

### Resumo da Reunião

Foram apresentados os 42 requisitos funcionais preliminares. O cliente e a gerente de loja validaram a grande maioria. Foram levantadas as seguintes questões adicionais:

**Ana Rodrigues (Gerente):**
> *"É muito importante que possamos definir preços diferentes por loja. Na minha loja o prazo de validade de certos produtos obriga-me a fazer promoções que na sede não sabem."*

→ Originou o requisito **RF08** (preço de venda configurável por loja).

> *"Os ajustes de stock são frequentes – quebras, produto mal contado, etc. Preciso de registar o motivo senão não consigo justificar perante o gestor."*

→ Confirmou e reforçou o requisito **RF15** (ajuste manual com motivo obrigatório e auditoria).

**João Martins (Gestor):**
> *"Quero ter um ecrã principal onde, de manhã, logo ao acordar, veja como correu o dia anterior em todas as lojas."*

→ Confirmou e detalhou o requisito **RF37** (Dashboard Central).

> *"A consolidação diária tem de ser automática. Não posso depender de ninguém para que os dados apareçam no servidor."*

→ Confirmou e tornou **RF33** (consolidação automática) como Essencial/Alta prioridade.

### Requisitos Alterados/Adicionados nesta Reunião

| RF | Alteração |
|---|---|
| RF08 | **Adicionado** – preço de venda configurável por loja |
| RF15 | **Elevada prioridade** – ajuste manual com auditoria (Importante → Essencial) |
| RF33 | **Confirmado como Essencial** – consolidação automática |

### Decisões Tomadas
- Todos os 42 requisitos funcionais e 15 não funcionais preliminares aprovados com as alterações acima
- Próxima reunião para validação dos wireframes e arquitetura

### Ação a Desenvolver

| Responsável | Ação | Prazo |
|---|---|---|
| Equipa Dev | Finalizar documento SRS com requisitos validados | 20/02/2026 |
| Equipa Dev | Preparar wireframes das interfaces principais | 28/02/2026 |
| João Martins | Fornecer categorias de produtos e lista de fornecedores | 12/02/2026 |

---

## Ata nº 3 – Reunião de Validação de Wireframes

| Campo | Detalhe |
|---|---|
| **Data** | 20 de Fevereiro de 2026 |
| **Hora** | 14:00 – 15:00 |
| **Local** | Videoconferência |
| **Presentes** | João Martins (Gestor), Equipa LI4 |
| **Tipo** | Online |

### Resumo da Reunião

Foram apresentados os wireframes das principais interfaces: Login, Dashboard Central, POS (Ponto de Venda), Gestão de Stock e Relatórios.

O cliente fez os seguintes comentários:

> *"O dashboard está muito bem. Quero ver os gráficos de barras de vendas por loja logo ao centro – é o mais importante para mim."*

> *"O ecrã de POS tem de ser muito simples. Os meus funcionários não são técnicos. Menos botões, mais fácil."*

> *"Nos relatórios quero conseguir comparar lojas facilmente. Uma tabela lado a lado seria ótimo."*

### Decisões Tomadas
- Wireframes aprovados com os ajustes indicados
- Equipa avança para a fase de implementação
- Reunião final de aceitação agendada para Maio de 2026

---

## Anexo B – Questionário a Funcionários de POS

**Data de envio:** 08 de Fevereiro de 2026  
**Respostas recebidas:** 11 de 15 funcionários

### Perguntas e Respostas Representativas

**1. Como é atualmente efetuado o registo de uma venda?**
- 73% – registo manual em papel / caderno
- 18% – sistema de caixa antigo sem ligação à gestão central
- 9% – folha de Excel no computador da loja

**2. Qual o maior problema no processo atual?**
- "Demoro muito tempo a calcular o total, especialmente quando há descontos."
- "Quando acaba o stock não sei em tempo real, só quando vou buscar o produto à prateleira."
- "Tenho de chamar o gerente para cancelar uma venda, mesmo quando é um erro óbvio."

**3. O que é mais importante numa nova aplicação de POS?**
- 91% – Rapidez de registo (pesquisa de produto ágil)
- 82% – Cálculo automático de descontos e totais
- 73% – Impressão/visualização de recibo
- 55% – Simplicidade da interface

**4. Já utilizaram algum sistema semelhante (ex: em outro emprego)?**
- 45% – Sim (Primavera, PHC, sistemas de supermercado)
- 55% – Não

**Conclusões do Questionário:**
- A simplicidade e velocidade do POS são críticas → reforça **RNF03** e **RF17**
- A capacidade de aplicar descontos é muito valorizada → confirma **RF18**
- A necessidade de feedback visual de stock em tempo real → reforça **RF20**
