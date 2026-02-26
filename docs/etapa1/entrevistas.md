# Entrevistas com Stakeholders (Simuladas com LLM)
## Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência

**Projeto:** LI4 2025/2026 – Tema 1  
**Metodologia:** Entrevistas simuladas com recurso a LLM (Gemini/Antigravity) para elicitação de requisitos  
**Data de simulação:** Fevereiro de 2026  

> **Nota metodológica:** Na impossibilidade de realizar entrevistas presenciais com todos os perfis de utilizador, foi utilizado um LLM (Large Language Model) para simular conversas estruturadas com os diferentes stakeholders do sistema. O LLM foi instruído a comportar-se como um stakeholder específico com o seu papel, motivações e preocupações. As respostas foram posteriormente validadas nas reuniões presenciais com o cliente real (ver Atas em `atas_reunioes.md`).

---

## Índice

1. [Entrevista 1 – Gestor da Cadeia](#entrevista-1--gestor-da-cadeia-joão-martins)
2. [Entrevista 2 – Gerente de Loja](#entrevista-2--gerente-de-loja-ana-rodrigues)
3. [Entrevista 3 – Funcionário de POS](#entrevista-3--funcionário-de-pos-carlos-ferreira)
4. [Requisitos Elicitados por Entrevista](#requisitos-elicitados-por-entrevista)

---

## Entrevista 1 – Gestor da Cadeia (João Martins)

**Perfil simulado:** João Martins, 48 anos. Gestor da cadeia QuickMart com 5 lojas no distrito de Braga. Tem experiência em gestão de retalho mas pouca literacia técnica. O seu maior problema é a falta de visibilidade em tempo real sobre as 5 lojas.

**Data:** 04 de Fevereiro de 2026 (simulada)  
**Duração:** ~30 minutos  
**Formato:** Entrevista semi-estruturada

---

**Entrevistador:** Bom dia, João. Pode descrever-me como gere atualmente as suas 5 lojas em termos de stock e vendas?

**João Martins:** Bom dia. Olha, honestamente é uma dor de cabeça. Cada loja tem o seu próprio sistema — a maioria usa uma folha de Excel diferente, uma delas ainda usa um caderno. No final do mês ligo para cada gerente, peço-lhes para me mandarem os ficheiros, e depois passo o fim de semana a tentar consolidar tudo numa só folha. É completamente ineficiente.

**Entrevistador:** E como é feito o controlo de stock hoje?

**João Martins:** Não é, basicamente. Cada gerente sabe o que tem na prateleira porque vai ver fisicamente. Quando se dá conta que está a acabar um produto, liga para o fornecedor. Mas já aconteceu ficarmos sem Coca-Colas numa loja durante três dias porque o gerente estava de férias e o substituto não sabia que precisava de encomendar.

**Entrevistador:** Que funcionalidades considera absolutamente essenciais num novo sistema?

**João Martins:** Número um, ver o stock de todas as lojas em tempo real — não quero continuar a descobrir as ruturas pelo telefone. Número dois, um dashboard que me mostre logo de manhã como correu o dia anterior: vendas totais, qual foi a loja com mais vendas, se há alertas de stock crítico. Número três, que a consolidação seja automática — não quero depender de ninguém para que os dados apareçam no servidor.

**Entrevistador:** Como gostaria de ver os relatórios organizados?

**João Martins:** Quero conseguir comparar lojas facilmente. Tipo: esta semana, qual foi o desempenho de cada loja? Quais os produtos mais vendidos globalmente? E conseguir filtrar por períodos — semana, mês, ano. Ah, e exportar para Excel, porque ainda tenho reuniões com o contabilista onde levo os dados.

**Entrevistador:** E em termos de quem acede ao sistema?

**João Martins:** Eu preciso de ver tudo. Os gerentes devem ver só a sua loja. E os funcionários devem só poder registar as vendas — não quero que andem a mexer nos preços ou a apagar encomendas.

**Entrevistador:** Tem algum requisito de segurança específico?

**João Martins:** Quero que cada utilizador tenha a sua password. Já tive situações em que um funcionário fez uma venda errada e não havia forma de saber quem foi porque usavam todos o mesmo acesso. Agora cada operação tem de ter um responsável identificado.

---

**Requisitos identificados nesta entrevista:**
- RF01 (login individual), RF02 (RBAC), RF11 (stock em tempo real), RF12 (atualização automática), RF33 (consolidação automática), RF37 (dashboard central), RF39 (relatórios filtráveis), RF41 (comparação entre lojas), RF42 (exportação)

---

## Entrevista 2 – Gerente de Loja (Ana Rodrigues)

**Perfil simulado:** Ana Rodrigues, 34 anos. Gerente da loja QuickMart de Braga-Centro há 4 anos. Gere uma equipa de 3 funcionários. Tem boa literacia digital e usa frequentemente o Excel. Os seus maiores problemas são a gestão de stock e as encomendas a fornecedores.

**Data:** 06 de Fevereiro de 2026 (simulada)  
**Duração:** ~25 minutos  
**Formato:** Entrevista semi-estruturada

---

**Entrevistador:** Boa tarde, Ana. Qual é o maior desafio que enfrenta na gestão diária da sua loja?

**Ana Rodrigues:** Sem dúvida é o stock. Eu tenho um Excel mas é sempre o mesmo problema — quando a minha funcionária regista uma venda num papelinho e eu vou atualizar só no final do turno, já pode ter acontecido de outra funcionária vender o último produto e eu não saber. No sistema novo quero que o stock atualize em tempo real quando se faz uma venda.

**Entrevistador:** Como gere atualmente as encomendas a fornecedores?

**Ana Rodrigues:** Por telefone e email. Verifico o stock visualmente, vejo o que está a acabar, e mando um email ou ligo. Não tenho registo histórico de encomendas. Já me chegou uma encomenda diferente do que eu pedi e já não havia forma de provar o que tinha pedido.

**Entrevistador:** O que precisaria de um módulo de encomendas?

**Ana Rodrigues:** Quero poder criar a encomenda no sistema, ver o histórico, e quando recebo a mercadoria registar o que efetivamente chegou — porque às vezes chegam menos unidades do que encomendei. E quando der entrada, que o stock atualize automaticamente.

**Entrevistador:** Tem alguma necessidade específica em relação a preços?

**Ana Rodrigues:** Isso é muito importante. A minha loja fica no centro de Braga, onde a concorrência é maior. Às vezes preciso de baixar o preço de um produto para ser competitivo, ou fazer promoções em produtos perto do prazo de validade. Preciso de poder definir um preço diferente do preço base que o gestor define centralmente.

**Entrevistador:** E em relação aos ajustes de stock?

**Ana Rodrigues:** Ah, os ajustes são frequentes. Quebras, produtos que caem e partem, diferenças de inventário. O problema atual é que quando ajusto o Excel, não fico com registo do porquê. Já tive de justificar perante o João (gestor) uma diferença de stock e não tinha como explicar porque não tinha registado o motivo. Quero que o sistema obrigue a registar motivo em qualquer ajuste.

**Entrevistador:** Que relatórios precisa regularmente?

**Ana Rodrigues:** Quero ver as vendas do dia, da semana, do mês. Por produto — quais os mais vendidos na minha loja. E quero o dashboard logo quando abro o computador de manhã: quantas vendas ontem, quantos produtos em alerta de stock, encomendas pendentes.

**Entrevistador:** Algum requisito sobre a facilidade de uso?

**Ana Rodrigues:** Tenho uma funcionária com 58 anos que ainda usa o telemóvel com dois dedos. O ecrã de POS tem de ser muito simples — poucos botões, letras grandes, lógico. Não pode precisar de formação de dias para usar.

---

**Requisitos identificados nesta entrevista:**
- RF08 (preço por loja), RF10 (atualização automática de stock), RF13 (stock mínimo), RF15 (ajuste com motivo obrigatório), RF24 (criar encomenda), RF25 (estados da encomenda), RF26 (receção com atualização de stock), RF30 (relatórios de loja), RF38 (dashboard de loja), RNF03 (usabilidade)

---

## Entrevista 3 – Funcionário de POS (Carlos Ferreira)

**Perfil simulado:** Carlos Ferreira, 27 anos. Funcionário de POS na loja QuickMart de Palmeira há 2 anos. Trabalha a tempo parcial (tardes e fins de semana). Tem experiência com smartphones mas nunca usou um sistema de gestão de loja. Os seus maiores problemas são a lentidão do processo atual e os erros de cálculo de descontos.

**Data:** 07 de Fevereiro de 2026 (simulada)  
**Duração:** ~20 minutos  
**Formato:** Entrevista semi-estruturada

---

**Entrevistador:** Olá Carlos. Como é feito atualmente o processo de venda?

**Carlos Ferreira:** Então, o cliente traz os produtos ao balcão, eu escrevo num caderno o produto e o preço que está no produto, faço as contas na cabeça ou numa calculadora de bolso, e digo o total ao cliente. No final do turno passo tudo para o Excel.

**Entrevistador:** Qual é o maior problema neste processo?

**Carlos Ferreira:** Os descontos. O gerente faz promoções — tipo "bebidas com 20% de desconto ao fim de semana" — e eu tenho de calcular na cabeça. Já me enganei várias vezes. Uma vez dei mais desconto do que devia e o gerente ficou muito chateado. Se o sistema calculasse automaticamente era muito melhor.

**Entrevistador:** E quando há um erro numa venda?

**Carlos Ferreira:** Tenho de chamar o gerente. Mesmo que seja um erro óbvio meu — tipo adicionei o produto errado — não posso fazer nada sem ele. Isso atrasa a fila de clientes e cria uma situação desconfortável.

**Entrevistador:** O que é mais importante para si num novo sistema de POS?

**Carlos Ferreira:** Deve ser rápido. Quando há fila de clientes não posso estar a procurar o produto durante 30 segundos. Quero pesquisar pelo nome ou pelo código de barras e aparecer logo. E que o total apareça automaticamente à medida que vou adicionando produtos. Ah, e o ecrã de confirmar a venda tem de ser simples — não quero carregar em 5 botões para concluir uma venda.

**Entrevistador:** E em relação ao stock? Precisa de ver alguma informação de stock durante uma venda?

**Carlos Ferreira:** Seria útil. Já aconteceu um cliente pedir um produto, eu digo "temos" porque vi há meia hora, e depois vou buscar e está esgotado. Se o sistema me avisasse antes de confirmar a venda que está Stock 0, evitava a situação embaraçosa com o cliente.

**Entrevistador:** Alguma vez emite recibos para os clientes?

**Carlos Ferreira:** Às vezes os clientes pedem. Agora escrevemos num papel à mão, é ridículo. Que o sistema imprimisse ou mostrasse no ecrã um recibo limpo com os produtos, os preços e o total seria ótimo.

**Entrevistador:** Algum aspeto de segurança que considere importante?

**Carlos Ferreira:** Cada um ter o seu login. Na nossa loja usamos todos o mesmo computador mas às vezes um de nós faz um erro e depois ninguém sabe quem foi. Se cada um entrar com o seu utilizador, é mais justo — cada um é responsável pelo que faz.

---

**Requisitos identificados nesta entrevista:**
- RF01 (login individual), RF17 (registo de venda rápido), RF18 (descontos automáticos), RF19 (recibo automático), RF20 (validação de stock antes de vender), RF09 (pesquisa de produto), RNF03 (usabilidade — velocidade e simplicidade do POS)

---

## Requisitos Elicitados por Entrevista

A tabela seguinte resume os Requisitos Funcionais e Não Funcionais cuja origem pode ser rastreada às entrevistas:

| RF/RNF | Descrição Resumida | Entrevista(s) |
|---|---|---|
| RF01 | Login individual por utilizador | E1 (João), E3 (Carlos) |
| RF02 | Controlo de acesso por papel (RBAC) | E1 (João) |
| RF08 | Preço de venda configurável por loja | E2 (Ana) |
| RF09 | Pesquisa de produto por nome/código | E3 (Carlos) |
| RF10 | Atualização automática de stock após venda | E2 (Ana) |
| RF11 | Visualização de stock em tempo real | E1 (João), E2 (Ana) |
| RF13 | Stock mínimo com alertas | E1 (João), E2 (Ana) |
| RF15 | Ajuste de stock com motivo obrigatório | E2 (Ana) |
| RF17 | Registo de venda rápido por código/nome | E3 (Carlos) |
| RF18 | Descontos automáticos (% ou valor fixo) | E3 (Carlos) |
| RF19 | Emissão automática de recibo | E3 (Carlos) |
| RF20 | Validação de stock antes de confirmar venda | E3 (Carlos) |
| RF24 | Criação de encomenda a fornecedor | E2 (Ana) |
| RF25 | Estados da encomenda (Pendente/Enviada/Rececionada) | E2 (Ana) |
| RF26 | Atualização de stock ao recepcionar encomenda | E2 (Ana) |
| RF33 | Consolidação automática diária | E1 (João) |
| RF37 | Dashboard central com KPIs globais | E1 (João) |
| RF38 | Dashboard de loja com KPIs da loja | E2 (Ana) |
| RF39 | Relatórios filtráveis por período | E1 (João) |
| RF41 | Comparação de desempenho entre lojas | E1 (João) |
| RF42 | Exportação de relatórios | E1 (João) |
| RNF03 | Interface intuitiva sem formação prévia > 1h | E2 (Ana), E3 (Carlos) |
| RNF08 | Log de auditoria com utilizador identificado | E1 (João), E3 (Carlos) |

---

## Análise Transversal

A análise das três entrevistas revela **necessidades convergentes** nos diferentes perfis:

| Necessidade | GC | GL | FN |
|---|---|---|---|
| Autenticação individual | ✅ | — | ✅ |
| Stock em tempo real | ✅ | ✅ | ✅ |
| Dashboard personalizado | ✅ | ✅ | — |
| Processo de venda rápido | — | — | ✅ |
| Descontos automáticos | — | ✅ | ✅ |
| Gestão de encomendas | ✅ (visão) | ✅ (criação) | — |
| Relatórios | ✅ (global) | ✅ (loja) | — |
| Consolidação automática | ✅ | — | — |
| Auditoria de operações | ✅ | ✅ | — |

> *GC = Gestor da Cadeia · GL = Gerente de Loja · FN = Funcionário POS*

As entrevistas confirmam que os três perfis têm necessidades distintas mas complementares, justificando o modelo de controlo de acesso baseado em papéis (RBAC) definido em RF02.
