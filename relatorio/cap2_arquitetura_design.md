# Capítulo 2 — Arquitetura e Design do Software utilizando LLM

## 2.1. Arquitetura Global do Sistema

O SGCLC foi concebido segundo uma arquitectura em três camadas (*Three-Tier Architecture*), complementada pelo padrão Repository + Service Layer, que assegura a separação clara entre a interface do utilizador, a lógica de negócio e o acesso a dados. Esta abordagem foi adoptada para responder directamente aos problemas diagnosticados na QuickMart (P1–P8, secção 1.2): a centralização de dados num repositório único, a necessidade de informação em tempo real para tomada de decisão, e a automatização de processos operacionais como a consolidação diária de vendas.

A solução foi implementada em .NET 8 com Blazor Server como framework de apresentação, tirando partido da comunicação bidirecional em tempo real proporcionada pelo SignalR. Esta escolha permite manter as interfaces sincronizadas com o estado do servidor sem necessidade de desenvolver uma API REST separada, uma vez que se trata de uma aplicação de gestão interna em que todos os utilizadores acedem através de um browser.

A camada de apresentação, materializada no projecto ConvenienceChain.Web, é responsável pela interface gráfica através de componentes Blazor Server (.razor). Estes componentes estão organizados por funcionalidade — Auth, Dashboard, POS, Stock, Encomendas, Faturas, Relatórios, Consolidação e Admin — e comunicam exclusivamente com a camada de lógica de negócio através de interfaces, nunca acedendo directamente ao contexto de base de dados. Esta camada inclui ainda o SessionService, que gere o estado da sessão RBAC, e o ConsolidacaoBackgroundService, um IHostedService responsável pela consolidação diária automática às 23:59 (RF33–RF36).

A camada de lógica de negócio, implementada no projecto ConvenienceChain.Core, constitui o núcleo do domínio e não possui qualquer dependência de infraestrutura. Nela residem as catorze entidades de domínio, os DTOs definidos como records C# imutáveis, as cinco enumerações de estado (EstadoVenda, EstadoEncomenda, EstadoFatura, PapelUtilizador, ResultadoConsolidacao), os contratos de serviço através de dez interfaces de serviço e onze interfaces de repositório, bem como as implementações dos dez serviços de aplicação. Todas as regras de negócio, validações e fluxos transaccionais estão encapsulados nesta camada.

A camada de acesso a dados, concretizada no projecto ConvenienceChain.Data, implementa os onze repositórios definidos pelas interfaces da camada Core. Utiliza o Entity Framework Core 8 com SQLite como motor de base de dados em desenvolvimento, incluindo o AppDbContext com treze DbSets e a configuração completa do modelo relacional via Fluent API.

O fluxo de dependências segue estritamente o princípio de inversão de dependências (DIP), traduzido na relação Web → Core ← Data. A camada Core define as abstracções (interfaces) que são implementadas pela camada Data e consumidas pela camada Web. Nenhuma camada superior depende de uma implementação concreta de uma camada inferior — apenas de contratos. A injecção de dependências é configurada centralmente no Program.cs, onde todos os repositórios e serviços são registados com ciclo de vida Scoped (um por request/circuito SignalR), enquanto o SessionService é registado como Singleton para manter o estado partilhado entre circuitos.

A Tabela 2.1 resume a organização dos três assemblies do sistema, as suas responsabilidades e dependências.

| Assembly | Responsabilidade | Dependências |
|---|---|---|
| ConvenienceChain.Core | Domínio, contratos de serviço (interfaces), DTOs | Nenhuma dependência de infraestrutura |
| ConvenienceChain.Data | Acesso à base de dados, contexto EF Core, repositórios | Depende de Core |
| ConvenienceChain.Web | Interface Blazor, injecção de dependências, configuração | Depende de Core e Data |

*Tabela 2.1 — Organização dos assemblies do SGCLC.*

[IMAGEM: Diagrama de arquitectura global do sistema — diagrama de camadas com as 3 camadas, fluxos de dependência e componentes transversais (SessionService, BackgroundService)]

No que diz respeito à stack tecnológica, as escolhas foram orientadas pela adequação ao contexto de uma aplicação de gestão interna e pela coerência do ecossistema .NET. O ASP.NET Core 8.0 com Blazor Server foi escolhido pela capacidade de renderização server-side com SignalR, eliminando a necessidade de uma API REST separada. O Entity Framework Core 8.0 proporciona o mapeamento objecto-relacional tipado com suporte a múltiplos motores de base de dados. O SQLite foi adoptado em desenvolvimento pela sua simplicidade de configuração e portabilidade, estando prevista a migração para SQL Server 2019+ em produção (RNF15). A linguagem C# 12 oferece tipagem estática e suporte nativo a async/await, enquanto o Bootstrap 5.3 com Bootstrap Icons assegura a responsividade e consistência visual da interface. Para segurança, o BCrypt.Net-Next 4.0.3 é utilizado na hash de passwords com salt automático. A geração de documentos PDF é realizada através do QuestPDF 2024.3.4, uma biblioteca open-source com API fluente e sem dependência de Office. Os testes unitários utilizam xUnit 2.7 com Moq 4.20 para mocking e FluentAssertions 6.12 para asserções.


## 2.2. Modelação Estrutural (Diagramas de Classes e Componentes)

### 2.2.1. Diagrama de Componentes

O sistema é composto por quatro projectos na solução .NET (ConvenienceChain.sln), com dependências unidireccionais que reforçam a separação de responsabilidades descrita na secção anterior.

O projecto ConvenienceChain.Web constitui o ponto de entrada da aplicação e contém dez componentes Razor organizados por funcionalidade: Auth/Login, Dashboard/Index, POS/Index, Stock/Index, Encomendas/Index, Faturas/Index, Relatorios/Index, Consolidacao/Index, Admin/Utilizadores e a página raiz Index. Dispõe de dois layouts — MainLayout.razor, que implementa a sidebar adaptada ao papel do utilizador (RBAC-aware), e EmptyLayout.razor, utilizado na página de Login. Inclui três serviços específicos da camada Web: o SessionService (singleton, gestão de estado de sessão), o ConsolidacaoBackgroundService (IHostedService, consolidação diária às 23:59) e o SeedData (inicialização de dados de demonstração). A configuração de arranque reside no Program.cs (injecção de dependências, middleware e seed), no appsettings.json (connection string e hora de consolidação) e no launchSettings.json (porta 5050).

O projecto ConvenienceChain.Core constitui o núcleo do domínio, contendo catorze entidades de domínio definidas em Entities/Entities.cs, dez interfaces de serviço e onze interfaces de repositório em Interfaces/, dez implementações de serviço em Services/Services.cs, mais de trinta DTOs imutáveis (records C#) em DTOs/DTOs.cs e cinco enumerações em Enums/Enums.cs.

O projecto ConvenienceChain.Data encapsula o acesso a dados, contendo o AppDbContext com treze DbSets e configuração completa via Fluent API, bem como as onze implementações de repositório definidas em Repositories/Repositories.cs.

Por último, o projecto ConvenienceChain.Tests contém dez testes unitários implementados com xUnit, utilizando Moq para mocking e FluentAssertions para asserções. Os testes recorrem ao provider InMemory do EF Core para isolar a execução da base de dados real.

[IMAGEM: Diagrama de componentes UML — mostrar os 4 projectos, os sub-componentes internos e as dependências entre eles. Incluir o fluxo Browser ↔ SignalR ↔ Blazor Server ↔ Services ↔ Repositories ↔ SQLite]

### 2.2.2. Diagrama de Classes do Domínio

O modelo de domínio é composto por catorze entidades organizadas em oito agregados funcionais. As relações entre entidades reflectem directamente os requisitos funcionais (RF06–RF42) e os módulos do sistema definidos na secção 1.2.

A entidade Loja (Id, Nome, Morada, Telefone, Email, Ativa) funciona como raiz do agregado central, agregando Stocks, Vendas, Encomendas, Consolidações, AjustesStock e Utilizadores. Cada loja da cadeia QuickMart corresponde a um registo nesta entidade, com a flag Ativa a suportar a desactivação lógica.

No agregado de catálogo, a entidade Categoria (Id, Nome, CategoriaPaiId) suporta hierarquias de categorias através de uma auto-referência com DeleteBehavior.Restrict, satisfazendo o RF07. A entidade Produto (Id, Codigo como chave única, Nome, Descricao, PrecoCusto, PrecoBaseVenda, UnidadeMedida, Foto, Ativo, CategoriaId, DataValidade) modela o catálogo central com suporte a preço diferenciado por loja (RF08) e prazo de validade (RF22).

O agregado de inventário é composto pela entidade Stock (Id, LojaId, ProdutoId, Quantidade, StockMinimo, PrecoVendaLocal) com um índice composto único sobre (LojaId, ProdutoId). Esta entidade inclui duas propriedades calculadas não persistidas: EmAlerta, que retorna verdadeiro quando a Quantidade é inferior ou igual ao StockMinimo (RF13), e PrecoVendaEfetivo, que devolve o PrecoVendaLocal se definido, ou o PrecoBaseVenda do produto caso contrário (RF08). A entidade AjusteStock (Id, LojaId, ProdutoId, UtilizadorId, Variacao, Motivo, DataHora) regista a auditoria de todos os ajustes manuais de stock (RF15).

O agregado de vendas compreende a entidade Venda (Id, LojaId, FuncionarioId, DataHora, SubTotal, TotalDesconto, Total, Estado, MotivoAnulacao) com a sua colecção de LinhaVenda (Id, VendaId, ProdutoId, Quantidade, PrecoUnitario, Desconto, e a propriedade calculada SubTotal). A relação entre Venda e Funcionário utiliza DeleteBehavior.Restrict para proteger o histórico de vendas. Os estados possíveis — Concluida, Cancelada e Devolvida — implementam os requisitos RF17 a RF21.

No agregado de encomendas encontram-se a entidade Fornecedor (Id, Nome, NIF, Morada, Telefone, Email, Ativo) e a entidade Encomenda (Id, LojaId, FornecedorId, DataCriacao, DataRececao, Estado, Observacoes) com a sua colecção de LinhaEncomenda (Id, EncomendaId, ProdutoId, QuantidadePedida, QuantidadeRecebida). O ciclo de estados segue a sequência Pendente → Enviada → Rececionada ou Cancelada (RF23–RF27).

O agregado de faturação engloba a entidade Fatura (Id, Numero como chave única, LojaId, VendaId opcional, NomeCliente, NIFCliente, MoradaCliente, DataEmissao, Total, Estado) com a colecção de LinhaFatura (Id, FaturaId, DescricaoProduto, Quantidade, PrecoUnitario, Desconto, e a propriedade calculada SubTotal). A associação opcional entre Fatura e Venda (1:0..1) satisfaz o RF29. O Numero da fatura é gerado automaticamente no formato FAT-{LojaId:D3}-{Ano}-{Seq:D5} (RF28).

O agregado de consolidação é composto pela entidade Consolidacao (Id, LojaId, DataConsolidacao, DataHoraExecucao, TotalVendas, NumeroTransacoes, TotalDescontos, Resultado, ErroDetalhes) com um índice composto único sobre (LojaId, DataConsolidacao). Os resultados possíveis — Sucesso, Falha e Parcial — cobrem os cenários previstos nos requisitos RF33 a RF36.

Finalmente, o agregado de utilizadores é representado pela entidade Utilizador (Id como GUID, Nome, Email como chave única, PasswordHash, Papel, LojaId opcional, Ativo, Telefone, Notas, CriadoEm). O Gestor da Cadeia não tem loja associada (LojaId nulo), enquanto os três papéis disponíveis — GestorCadeia, GerenteLoja e Funcionario — implementam o controlo de acesso baseado em papéis definido no RF02.

[IMAGEM: Diagrama de classes UML completo — todas as 14 entidades com atributos, tipos, multiplicidades e relações. Incluir as enumerações (EstadoVenda, EstadoEncomenda, EstadoFatura, PapelUtilizador, ResultadoConsolidacao)]


## 2.3. Modelação Comportamental (Diagramas de Sequência e Atividades)

### 2.3.1. Máquinas de Estado

O sistema possui quatro máquinas de estado que governam o ciclo de vida das entidades transaccionais, assegurando que as transições entre estados são válidas e que os efeitos colaterais (como a reposição de stock) são executados de forma consistente.

A máquina de estados da Venda (RF17–RF21) reflecte o fluxo operacional do ponto de venda. Uma venda inicia no estado EmCurso quando o funcionário adiciona o primeiro produto ao carrinho no POS. Durante este estado, é possível adicionar ou remover produtos e aplicar descontos. Ao confirmar a venda, e se o stock for suficiente para todos os produtos, a venda transita para o estado Concluida — neste momento o stock é debitado automaticamente e o recibo é emitido. A partir do estado Concluida, a venda pode ser Cancelada, o que exige a indicação de um motivo obrigatório e desencadeia a reposição automática do stock, ou pode transitar para o estado Devolvida no caso de devolução parcial ou total, com reposição proporcional do stock. Se o funcionário abandona o carrinho antes de confirmar, a venda é simplesmente descartada sem registo na base de dados.

[IMAGEM: Diagrama de máquina de estados — Venda (EmCurso → Concluida → Cancelada | Devolvida)]

A máquina de estados da Encomenda (RF24–RF27) modela o ciclo de reposição de inventário. Uma encomenda é criada com o estado Pendente pelo Gerente de Loja e pode transitar para o estado Enviada, que representa a comunicação ao fornecedor, ou ser directamente Cancelada. Uma encomenda no estado Enviada pode ser Rececionada, o que implica o registo das quantidades efectivamente recebidas e a actualização automática do stock da loja, ou pode ser Cancelada pelo Gestor da Cadeia. O sistema suporta a receção de quantidades inferiores às pedidas, sendo a QuantidadeRecebida independente da QuantidadePedida.

[IMAGEM: Diagrama de máquina de estados — Encomenda (Pendente → Enviada → Rececionada | Cancelada)]

A máquina de estados da Fatura (RF28–RF32) é mais simples, reflectindo o ciclo contabilístico. Uma fatura nasce com o estado Emitida e pode transitar para Paga quando o pagamento é registado, ou para Anulada, o que implica a emissão de uma nota de crédito conforme o RF31.

[IMAGEM: Diagrama de máquina de estados — Fatura (Emitida → Paga | Anulada)]

A máquina de estados da Consolidação (RF33–RF36) governa o processo de agregação diária de dados. O processo inicia como Agendada quando a hora configurada é atingida ou quando o Gestor acciona manualmente a consolidação (RF36). Transita para EmExecucao quando o background job inicia o processamento e pode terminar num de três estados: Sucesso, quando todos os dados são agregados correctamente; Falha, quando ocorre um erro e é agendado um retry automático após trinta minutos (RF34); ou Parcial, quando algumas lojas falham mas outras concluem com sucesso.

[IMAGEM: Diagrama de máquina de estados — Consolidação (Agendada → EmExecucao → Sucesso | Falha | Parcial)]

### 2.3.2. Diagramas de Sequência

Os diagramas de sequência seguintes descrevem os fluxos mais relevantes do sistema, evidenciando a interacção entre as camadas e os serviços envolvidos em cada operação.

**Processo de Autenticação (RF01, RF04, RF05)**

O fluxo de autenticação inicia quando o utilizador acede à página Auth/Login.razor e submete as suas credenciais (email e password). O componente Blazor invoca o método LoginAsync do IAuthService, que por sua vez consulta o IUtilizadorRepository através do método GetByEmailAsync para obter o registo do utilizador. Se o utilizador não existe ou está inactivo (Ativo = false), o serviço retorna null e a interface apresenta uma mensagem genérica "Credenciais inválidas" sem revelar qual dos campos está incorrecto, prevenindo assim a enumeração de contas (RF04). Se o utilizador existe, a password submetida é verificada contra o hash armazenado utilizando BCrypt.Verify, um algoritmo resistente a timing attacks. Em caso de sucesso, é criado um LoginResultDto com o identificador, nome, email, papel e loja do utilizador, e o SessionService armazena a sessão activa. A sessão HTTP é configurada com um timeout de oito horas de inactividade (RF05), utilizando cookies HttpOnly e IsEssential. Por fim, o router Blazor redireciona o utilizador para o Dashboard adequado ao seu papel.

[IMAGEM: Diagrama de sequência — Login (Utilizador → Login.razor → AuthService → UtilizadorRepository → BD → SessionService)]

**Registo de Venda no POS (RF17–RF21)**

O processo de registo de uma venda constitui o fluxo mais crítico do sistema. O Funcionário acede ao componente POS/Index.razor, que carrega automaticamente todos os produtos com stock positivo na loja do utilizador através do IStockService.GetStockLojaAsync. Os produtos são apresentados numa tabela com filtro instantâneo client-side, insensível a acentos e maiúsculas, que utiliza normalização Unicode (NormalizationForm.FormD) para permitir pesquisas como "cafe" para encontrar "Café" (RF09, RNF08). O funcionário adiciona produtos ao carrinho clicando no respectivo botão, define as quantidades desejadas e, opcionalmente, aplica descontos em valor absoluto em euros (RF18, RF20). O sistema calcula automaticamente o SubTotal (somatório de Quantidade × PrecoUnitario), o TotalDesconto e o Total = SubTotal − Desconto (RF19).

Ao confirmar a venda, o componente invoca o método CreateSaleAsync do ISalesService com o CreateVendaDto contendo a loja, o funcionário e as linhas de venda. O SalesService valida a disponibilidade de stock para todos os produtos do carrinho de forma atómica — se qualquer produto não tiver stock suficiente, a venda inteira é rejeitada com uma InvalidOperationException. Caso o stock seja suficiente, o serviço cria a entidade Venda com estado Concluida e as respectivas LinhaVenda, invoca o IStockService.DeductStockAsync para debitar o stock numa transacção EF Core (RNF10), e emite automaticamente uma fatura através do IFaturaService.EmitirAsync associada à venda (RF21). A falha na emissão da fatura não bloqueia a conclusão da venda. O componente apresenta então uma confirmação com o identificador da venda e o respectivo timestamp.

No caso de cancelamento posterior, o ISalesService.CancelSaleAsync valida que a venda se encontra no estado Concluida (não é possível cancelar uma venda já cancelada), repõe o stock via IStockService.ReporStockAsync, regista o motivo de anulação e altera o estado para Cancelada.

[IMAGEM: Diagrama de sequência — Registo de Venda (Funcionário → POS.razor → SalesService → StockService → VendaRepository → FaturaService → BD)]

**Criação e Receção de Encomenda (RF24–RF27)**

O processo de encomenda de reposição inicia quando o Gerente de Loja acede ao componente Encomendas/Index.razor e solicita uma nova encomenda. O componente carrega a lista de fornecedores activos através do IFornecedorService.GetAllAsync e o catálogo de produtos via IProdutoService.GetAllAsync. O Gerente seleciona o fornecedor pretendido, adiciona linhas de produtos com as respectivas quantidades e opcionalmente insere observações. O componente invoca então o IOrderService.CreateAsync com o CreateEncomendaDto, e o OrderService cria a entidade Encomenda com estado Pendente e as respectivas LinhaEncomenda, com o campo QuantidadeRecebida a null.

Quando os produtos chegam à loja, o Gerente seleciona a encomenda pendente e acciona a receção. O modal de receção pré-preenche as quantidades recebidas com os valores pedidos, sendo estes editáveis para permitir a receção parcial. O componente invoca o IOrderService.RecepcionarAsync, e o OrderService executa três acções em sequência: define o estado como Rececionada e a DataRececao como a hora actual, regista a QuantidadeRecebida para cada linha, e actualiza automaticamente o stock da loja incrementando a Stock.Quantidade com os valores recebidos (RF27). A interface confirma a operação com a mensagem "Encomenda #X rececionada. Stock atualizado automaticamente."

[IMAGEM: Diagrama de sequência — Encomenda e Receção (Gerente → Encomendas.razor → OrderService → EncomendaRepository → StockService → BD)]

**Consolidação Diária Automática (RF33–RF36)**

O processo de consolidação é executado automaticamente pelo ConsolidacaoBackgroundService, um IHostedService que calcula o tempo de espera até à hora alvo configurada no appsettings.json (por defeito, 23:59) e aguarda via Task.Delay. Na hora alvo, o serviço invoca o IConsolidacaoService.ConsolidarTodasAsync para a data do dia. O ConsolidacaoService obtém todas as lojas activas e, para cada uma, consulta as vendas com estado Concluida do dia através do IVendaRepository, calcula os indicadores agregados — TotalVendas (somatório dos totais), NumeroTransacoes (contagem) e TotalDescontos (somatório dos descontos) — e persiste o resultado. Se já existe uma consolidação para a combinação loja-data, esta é actualizada; caso contrário, é criada uma nova. O resultado é marcado como Sucesso ou, em caso de erro, como Falha com os detalhes registados em ErroDetalhes. Em caso de falha, o BackgroundService executa um retry automático após trinta minutos (RF34). O Gestor da Cadeia pode igualmente accionar manualmente uma consolidação a qualquer momento através do botão "Consolidar Agora" disponível na página de Consolidação (RF36).

[IMAGEM: Diagrama de actividades — Consolidação Diária (trigger → obter lojas → loop: agregar vendas por loja → gravar resultado → retry se falha)]

**Ajuste Manual de Stock (RF15, RF16)**

O fluxo de ajuste manual de stock inicia quando o Gerente acede ao componente Stock/Index.razor, seleciona um produto e solicita um ajuste manual. O modal apresenta dois campos: a variação (positiva para entradas, negativa para saídas) e o motivo (obrigatório, RF15). O componente invoca o IStockService.AjustarStockAsync, e o StockService calcula a nova quantidade como stockActual + variacao. Se o resultado for negativo, é lançada uma InvalidOperationException, pois o stock não pode ficar abaixo de zero (RF16). Caso a operação seja válida, o serviço actualiza a Stock.Quantidade e cria um registo de AjusteStock como log de auditoria, preservando a identificação do utilizador, a variação aplicada, o motivo e o timestamp.

[IMAGEM: Diagrama de sequência — Ajuste de Stock (Gerente → Stock.razor → StockService → StockRepository → AjusteStockRepository → BD)]

**Geração de Relatório de Vendas (RF39–RF42)**

O fluxo de geração de relatórios inicia quando o utilizador acede ao componente Relatorios/Index.razor e define os filtros pretendidos: loja (dropdown disponível apenas para o Gestor), data início, data fim e, opcionalmente, categoria. Estão disponíveis atalhos rápidos para os últimos sete dias, trinta dias ou três meses. O componente invoca o IReportService.GetRelatorioVendasAsync, e o ReportService filtra exclusivamente as vendas com estado Concluida no período seleccionado. O serviço calcula então os indicadores: TotalVendas (somatório dos totais), NrTransacoes (contagem), TicketMedio (TotalVendas / NrTransacoes), vendas agrupadas por categoria, os dez produtos mais vendidos por quantidade e por valor, e a evolução temporal das vendas por dia. O resultado é devolvido num RelatorioVendasDto e apresentado na interface com KPI cards, gráficos de barras por categoria e uma tabela de ranking de produtos. O utilizador pode opcionalmente exportar o relatório em formato PDF, gerado com QuestPDF, ou em formato CSV.

[IMAGEM: Diagrama de sequência — Relatório de Vendas (Utilizador → Relatorios.razor → ReportService → VendaRepository → BD → QuestPDF)]


## 2.4. Conceção do Sistema de Dados

### 2.4.1. Modelo Entidade-Relacionamento

O modelo de dados do SGCLC é composto por treze tabelas persistidas em SQLite através do Entity Framework Core 8, configuradas via Fluent API no AppDbContext. Estas treze tabelas correspondem às catorze entidades de domínio descritas na secção 2.2.2, tendo a entidade LinhaFatura sido adicionada durante a implementação para suportar o detalhe das faturas. O modelo foi desenvolvido em paralelo com a especificação de requisitos (Etapa 1) e refinado durante a fase de design (Etapa 2), cobrindo os oito módulos funcionais do sistema: Autenticação, Catálogo, Stock, POS, Encomendas, Faturação, Consolidação e Relatórios.

O esquema segue os princípios da modelação relacional com ênfase na normalização até à terceira forma normal, na integridade referencial enforçada por chaves estrangeiras, e nas restrições de unicidade que reflectem directamente as regras de negócio. A Figura 2.4.1 apresenta o diagrama entidade-relacionamento completo do sistema.

[IMAGEM: Diagrama Entidade-Relacionamento completo gerado no Visual Paradigm — todas as 13 tabelas com atributos, tipos, chaves (PK, FK, UK) e cardinalidades]

### 2.4.2. Descrição dos Elementos de Dados e Relacionamentos

Apresenta-se em seguida a descrição detalhada de cada tabela do modelo, incluindo os campos, tipos de dados, restrições e relações, em alinhamento com o dicionário de dados e a implementação no AppDbContext.

A tabela Lojas representa as lojas da cadeia QuickMart, com cinco registos iniciais no seed de dados (Braga-Centro, Palmeira, Maximinos, Barcelos e Guimarães). Cada registo contém o identificador auto-incrementado, o nome da loja (até 100 caracteres), a morada completa, o telefone e email de contacto, e uma flag Ativa que suporta a desactivação lógica (soft delete). A tabela estabelece relações de um-para-muitos com Stocks, Vendas, Encomendas, Consolidacoes, AjustesStock, Utilizadores e Faturas.

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| Id | INT | PK, NN, AUTO | Identificador único da loja | 1 |
| Nome | NVARCHAR(100) | NN | Designação comercial | "QuickMart Braga-Centro" |
| Morada | NVARCHAR(300) | NN | Endereço completo | "Rua do Souto, 45, Braga" |
| Telefone | NVARCHAR(20) | — | Contacto telefónico | "253123456" |
| Email | NVARCHAR(200) | — | Email de contacto | "braga@quickmart.pt" |
| Ativa | BIT | NN, DEFAULT 1 | Loja operacional (soft delete) | 1 |

*Tabela 2.2 — Estrutura da tabela Lojas.*

A tabela Categorias armazena as categorias hierárquicas de produto, com seis categorias iniciais: Bebidas, Bebidas Alcoólicas, Snacks, Lacticínios, Higiene e Congelados. A auto-referência através do campo CategoriaPaiId permite definir subcategorias, utilizando DeleteBehavior.Restrict para proteger a integridade da hierarquia.

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| Id | INT | PK, NN, AUTO | Identificador único | 3 |
| Nome | NVARCHAR(100) | NN | Designação da categoria | "Bebidas" |
| CategoriaPaiId | INT | FK(Categorias.Id), NULL | Categoria pai (null = raiz) | NULL |

*Tabela 2.3 — Estrutura da tabela Categorias.*

A tabela Produtos constitui o catálogo central de produtos, com quinze registos no seed. Cada produto possui um código único (Codigo), o nome comercial, a descrição, os preços de custo e de venda base, a unidade de medida, uma imagem opcional, a flag de activação e a referência à categoria. O campo DataValidade, opcional, suporta o controlo de prazos de validade (RF22).

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| Id | INT | PK, NN, AUTO | Identificador único | 42 |
| Codigo | NVARCHAR(50) | NN, UK | Código único (barras/interno) | "BEB001" |
| Nome | NVARCHAR(200) | NN | Designação comercial | "Água Mineral 500ml" |
| Descricao | NVARCHAR(1000) | — | Descrição detalhada | "Água mineral natural sem gás" |
| PrecoCusto | DECIMAL(18,2) | NN | Preço de custo (€) | 0.25 |
| PrecoBaseVenda | DECIMAL(18,2) | NN | Preço de venda base (€) | 0.60 |
| UnidadeMedida | NVARCHAR(20) | NN, DEFAULT 'unidade' | Unidade de medida | "unidade" |
| Foto | NVARCHAR(500) | NULL | Caminho da imagem | NULL |
| Ativo | BIT | NN, DEFAULT 1 | Disponível (soft delete) | 1 |
| CategoriaId | INT | FK(Categorias.Id), NN | Categoria do produto | 3 |
| DataValidade | DATE | NULL | Prazo de validade | 2026-04-15 |

*Tabela 2.4 — Estrutura da tabela Produtos.*

A tabela Stocks materializa o inventário de cada produto por loja. A restrição de unicidade composta (LojaId, ProdutoId) assegura que cada produto aparece no máximo uma vez por loja. O campo PrecoVendaLocal, quando definido, sobrepõe o PrecoBaseVenda do produto, implementando o requisito RF08 de diferenciação de preços por loja. As propriedades calculadas EmAlerta (Quantidade ≤ StockMinimo) e PrecoVendaEfetivo (PrecoVendaLocal ?? PrecoBaseVenda) não são persistidas na base de dados, sendo calculadas em runtime.

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| Id | INT | PK, NN, AUTO | Identificador único | 101 |
| LojaId | INT | FK(Lojas.Id), NN | Loja | 1 |
| ProdutoId | INT | FK(Produtos.Id), NN | Produto | 42 |
| Quantidade | DECIMAL(18,3) | NN, DEFAULT 0 | Quantidade disponível | 150.000 |
| StockMinimo | DECIMAL(18,3) | NN, DEFAULT 0 | Mínimo para alerta (RF12) | 20.000 |
| PrecoVendaLocal | DECIMAL(18,2) | NULL | Preço local (sobrepõe base, RF08) | 0.55 |

*Tabela 2.5 — Estrutura da tabela Stocks.*

A tabela Utilizadores armazena as contas de utilizador do sistema, utilizando um GUID como chave primária para evitar colisões em cenários de migração. O email funciona como chave única e credencial de login. A password é armazenada exclusivamente sob a forma de hash BCrypt com custo 12 e salt automático (RNF04). O campo Papel, armazenado como string, determina o nível de acesso RBAC (RF02), enquanto o LojaId associa o utilizador a uma loja específica — sendo nulo para o Gestor da Cadeia.

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| Id | NVARCHAR(36) | PK, NN | GUID do utilizador | "a1b2c3d4-..." |
| Nome | NVARCHAR(200) | NN | Nome completo | "Ana Rodrigues" |
| Email | NVARCHAR(300) | NN, UK | Email (login) | "ana@quickmart.pt" |
| PasswordHash | NVARCHAR(500) | NN | Hash BCrypt (cost=12) | "$2a$12$..." |
| Papel | NVARCHAR(20) | NN | Papel RBAC (RF02) | "GerenteLoja" |
| LojaId | INT | FK(Lojas.Id), NULL | Loja associada | 1 |
| Ativo | BIT | NN, DEFAULT 1 | Conta activa | 1 |
| Telefone | NVARCHAR(20) | NULL | Contacto | "912345678" |
| Notas | NVARCHAR(500) | NULL | Observações | NULL |
| CriadoEm | DATETIME | NN, DEFAULT UTC | Data de criação | 2026-02-10 09:00:00 |

*Tabela 2.6 — Estrutura da tabela Utilizadores.*

A tabela Vendas regista o cabeçalho de cada venda efectuada no ponto de venda (RF17–RF21). A relação com o Funcionário utiliza DeleteBehavior.Restrict para preservar o histórico, e a relação opcional com Faturas (1:0..1) permite associar uma fatura a uma venda.

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| Id | INT | PK, NN, AUTO | Identificador único | 5001 |
| LojaId | INT | FK(Lojas.Id), NN | Loja da venda | 2 |
| FuncionarioId | NVARCHAR(36) | FK(Utilizadores.Id), NN | Funcionário | "b2c3..." |
| DataHora | DATETIME | NN, DEFAULT UTC | Timestamp da venda | 2026-02-24 14:35:00 |
| SubTotal | DECIMAL(18,2) | NN | Soma das linhas | 12.50 |
| TotalDesconto | DECIMAL(18,2) | NN, DEFAULT 0 | Descontos aplicados | 1.00 |
| Total | DECIMAL(18,2) | NN | Valor final | 11.50 |
| Estado | NVARCHAR(20) | NN, DEFAULT 'Concluida' | Estado da venda | "Concluida" |
| MotivoAnulacao | NVARCHAR(500) | NULL | Motivo de cancelamento | NULL |

*Tabela 2.7 — Estrutura da tabela Vendas.*

A tabela LinhasVenda contém as linhas de produto de cada venda, com a propriedade calculada SubTotal = (Quantidade × PrecoUnitario) − Desconto.

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| Id | INT | PK, NN, AUTO | Identificador único | 9001 |
| VendaId | INT | FK(Vendas.Id), NN | Venda associada | 5001 |
| ProdutoId | INT | FK(Produtos.Id), NN | Produto vendido | 42 |
| Quantidade | DECIMAL(18,3) | NN, > 0 | Quantidade vendida | 3.000 |
| PrecoUnitario | DECIMAL(18,2) | NN, > 0 | Preço no momento | 0.60 |
| Desconto | DECIMAL(18,2) | NN, DEFAULT 0, ≥ 0 | Desconto absoluto (€) | 0.10 |

*Tabela 2.8 — Estrutura da tabela LinhasVenda.*

A tabela Fornecedores regista os fornecedores da cadeia, com três registos iniciais no seed: Distribuidora Norte, Águas de Portugal e SnackWorld Iberia.

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| Id | INT | PK, NN, AUTO | Identificador único | 7 |
| Nome | NVARCHAR(200) | NN | Designação social | "Distribuidora Norte Lda." |
| NIF | NVARCHAR(20) | NN | NIF | "509876543" |
| Morada | NVARCHAR(300) | — | Morada | "Rua Industrial, 10" |
| Telefone | NVARCHAR(20) | — | Contacto | "253456789" |
| Email | NVARCHAR(200) | — | Email | "encomendas@distnorte.pt" |
| Ativo | BIT | NN, DEFAULT 1 | Fornecedor activo | 1 |

*Tabela 2.9 — Estrutura da tabela Fornecedores.*

A tabela Encomendas regista as encomendas de reposição (RF23–RF27), com relações N:1 para Lojas e Fornecedores (este último com DeleteBehavior.Restrict) e 1:N para LinhasEncomenda.

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| Id | INT | PK, NN, AUTO | Identificador único | 301 |
| LojaId | INT | FK(Lojas.Id), NN | Loja que encomenda | 3 |
| FornecedorId | INT | FK(Fornecedores.Id), NN | Fornecedor | 7 |
| DataCriacao | DATETIME | NN, DEFAULT UTC | Data de criação | 2026-02-20 09:00:00 |
| DataRececao | DATETIME | NULL | Data de receção | 2026-02-22 14:00:00 |
| Estado | NVARCHAR(20) | NN, DEFAULT 'Pendente' | Estado da encomenda | "Rececionada" |
| Observacoes | NVARCHAR(1000) | — | Notas adicionais | "Urgente" |

*Tabela 2.10 — Estrutura da tabela Encomendas.*

A tabela LinhasEncomenda contém as linhas de produto de cada encomenda, distinguindo a quantidade pedida da quantidade efectivamente recebida.

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| Id | INT | PK, NN, AUTO | Identificador único | 1201 |
| EncomendaId | INT | FK(Encomendas.Id), NN | Encomenda | 301 |
| ProdutoId | INT | FK(Produtos.Id), NN | Produto | 42 |
| QuantidadePedida | DECIMAL(18,3) | NN, > 0 | Quantidade solicitada | 200.000 |
| QuantidadeRecebida | DECIMAL(18,3) | NULL, ≥ 0 | Quantidade recebida | 195.000 |

*Tabela 2.11 — Estrutura da tabela LinhasEncomenda.*

A tabela Faturas regista as faturas emitidas (RF28–RF32). O campo Numero é uma chave única gerada automaticamente no formato FAT-{LojaId:D3}-{Ano}-{Seq:D5}. A associação opcional com a tabela Vendas (VendaId nullable) permite faturas independentes de vendas, enquanto a relação Fatura↔Venda utiliza DeleteBehavior.Restrict para impedir eliminações inconsistentes. Os dados do cliente (NomeCliente, NIFCliente, MoradaCliente) são intencionalmente desnormalizados para preservar a informação no momento da emissão.

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| Id | INT | PK, NN, AUTO | Identificador único | 2001 |
| Numero | NVARCHAR(50) | NN, UK | Número sequencial (RF28) | "FAT-001-2026-00001" |
| LojaId | INT | FK(Lojas.Id), NN | Loja emissora | 1 |
| VendaId | INT | FK(Vendas.Id), NULL | Venda associada (RF29) | 5001 |
| NomeCliente | NVARCHAR(200) | NN | Nome do cliente | "Consumidor Final" |
| NIFCliente | NVARCHAR(20) | NN | NIF do cliente | "999999990" |
| MoradaCliente | NVARCHAR(300) | — | Morada do cliente | "Av. Central, 1" |
| DataEmissao | DATETIME | NN, DEFAULT UTC | Data de emissão | 2026-02-24 15:00:00 |
| Total | DECIMAL(18,2) | NN | Valor total | 11.50 |
| Estado | NVARCHAR(20) | NN, DEFAULT 'Emitida' | Estado da fatura | "Emitida" |

*Tabela 2.12 — Estrutura da tabela Faturas.*

A tabela LinhasFatura contém as linhas de cada fatura, com o campo DescricaoProduto desnormalizado para preservar a descrição do produto no momento da emissão.

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| Id | INT | PK, NN, AUTO | Identificador único | 3001 |
| FaturaId | INT | FK(Faturas.Id), NN | Fatura | 2001 |
| DescricaoProduto | NVARCHAR(200) | NN | Descrição do produto | "Água Mineral 500ml" |
| Quantidade | DECIMAL(18,3) | NN | Quantidade | 3.000 |
| PrecoUnitario | DECIMAL(18,2) | NN | Preço unitário | 0.60 |
| Desconto | DECIMAL(18,2) | NN, DEFAULT 0 | Desconto absoluto | 0.00 |

*Tabela 2.13 — Estrutura da tabela LinhasFatura.*

A tabela Consolidacoes armazena os resultados das consolidações diárias (RF33–RF36), com a restrição de unicidade composta (LojaId, DataConsolidacao) a garantir que existe no máximo uma consolidação por loja por dia.

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| Id | INT | PK, NN, AUTO | Identificador único | 501 |
| LojaId | INT | FK(Lojas.Id), NN | Loja consolidada | 2 |
| DataConsolidacao | DATE | NN | Dia consolidado | 2026-02-23 |
| DataHoraExecucao | DATETIME | NN | Quando foi executada | 2026-02-23 23:59:05 |
| TotalVendas | DECIMAL(18,2) | NN, DEFAULT 0 | Soma de vendas | 1250.75 |
| NumeroTransacoes | INT | NN, DEFAULT 0 | Nº de transacções | 87 |
| TotalDescontos | DECIMAL(18,2) | NN, DEFAULT 0 | Soma de descontos | 45.20 |
| Resultado | NVARCHAR(20) | NN | Resultado | "Sucesso" |
| ErroDetalhes | NVARCHAR(2000) | NULL | Detalhes de erro | NULL |

*Tabela 2.14 — Estrutura da tabela Consolidacoes.*

Por último, a tabela AjustesStock regista a auditoria de todos os ajustes manuais de stock (RF15), funcionando como um log imutável que preserva a rastreabilidade de cada operação.

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| Id | INT | PK, NN, AUTO | Identificador único | 801 |
| LojaId | INT | FK(Lojas.Id), NN | Loja | 1 |
| ProdutoId | INT | FK(Produtos.Id), NN | Produto ajustado | 42 |
| UtilizadorId | NVARCHAR(36) | FK(Utilizadores.Id), NN | Quem ajustou | "c3d4..." |
| Variacao | DECIMAL(18,3) | NN | +entrada / −saída | -5.000 |
| Motivo | NVARCHAR(500) | NN | Motivo obrigatório | "Quebra – produto caído" |
| DataHora | DATETIME | NN, DEFAULT UTC | Quando | 2026-02-24 10:15:00 |

*Tabela 2.15 — Estrutura da tabela AjustesStock.*

### 2.4.3. Normalização do Modelo de Dados

O modelo de dados encontra-se na Terceira Forma Normal (3FN), conforme verificado durante a fase de design.

Relativamente à Primeira Forma Normal (1FN), todos os atributos são atómicos e não existem grupos repetitivos. As colecções de linhas (LinhaVenda, LinhaEncomenda, LinhaFatura) são modeladas como entidades separadas com chaves estrangeiras, em vez de atributos multivalorados, garantindo que cada célula contém um único valor indivisível.

No que respeita à Segunda Forma Normal (2FN), todos os atributos não-chave dependem totalmente da chave primária. A entidade Stock utiliza um índice composto único (LojaId, ProdutoId) para garantir a unicidade de cada combinação loja-produto, mantendo contudo uma chave primária surrogada Id conforme convenção do Entity Framework Core. Todos os atributos — Quantidade, StockMinimo e PrecoVendaLocal — dependem funcionalmente da combinação completa (LojaId, ProdutoId) e não de apenas uma das suas partes.

Quanto à Terceira Forma Normal (3FN), não existem dependências transitivas entre atributos não-chave. Algumas decisões de design merecem destaque: o PrecoVendaLocal está na entidade Stock e não no Produto, pois é específico de cada combinação loja-produto — um mesmo produto pode ter preços diferentes em lojas diferentes (RF08). O PrecoBaseVenda permanece no Produto como preço de referência do catálogo central, sendo o preço efectivo de venda determinado em runtime pela propriedade calculada PrecoVendaEfetivo. Os dados do cliente (NomeCliente, NIFCliente, MoradaCliente) estão intencionalmente desnormalizados na entidade Fatura para preservar a informação tal como existia no momento da emissão — esta é uma prática corrente em sistemas de faturação, onde o documento fiscal deve reflectir os dados vigentes na data de emissão, independentemente de alterações posteriores. De forma análoga, a DescricaoProduto na LinhaFatura é desnormalizada para preservar a designação histórica do produto. Os campos derivados presentes nos DTOs, como NomeLoja e NomeFornecedor, são calculados em runtime através de joins e não persistidos, evitando redundância.

### 2.4.4. Requisitos de Armazenamento e Integridade

No domínio da integridade referencial, todas as chaves estrangeiras são configuradas via Fluent API do Entity Framework Core e enforçadas pelo SQLite ao nível da base de dados. As relações com Fornecedor (Encomenda.FornecedorId), Utilizador (Venda.FuncionarioId, Utilizador.LojaId) e Fatura (Fatura.VendaId) utilizam DeleteBehavior.Restrict para impedir a eliminação em cascata de registos referenciados, protegendo assim a integridade do histórico de vendas, encomendas e faturas. A auto-referência na entidade Categoria utiliza igualmente DeleteBehavior.Restrict para evitar remoções que invalidariam a hierarquia de categorias.

As restrições de unicidade reflectem directamente as regras de negócio do sistema, conforme resumido na Tabela 2.16.

| Tabela | Restrição UK | Significado de Negócio |
|---|---|---|
| Utilizadores | (Email) | Um endereço de email corresponde a uma única conta |
| Produtos | (Codigo) | Cada produto tem um código de barras ou código interno único |
| Stocks | (LojaId, ProdutoId) | Um produto aparece no máximo uma vez por loja |
| Faturas | (Numero) | A numeração de faturas é globalmente única |
| Consolidacoes | (LojaId, DataConsolidacao) | Existe no máximo uma consolidação por loja por dia |

*Tabela 2.16 — Restrições de unicidade do modelo de dados.*

No que respeita às restrições de domínio, os campos obrigatórios incluem os nomes das entidades principais (Loja, Produto, Fornecedor, Utilizador), o código e email como identificadores de negócio, e o motivo do AjusteStock. Os limites de comprimento variam entre 20 caracteres para campos como NIF e Telefone e 2000 caracteres para ErroDetalhes da Consolidação. A precisão decimal é diferenciada: valores monetários utilizam precisão (18,2) enquanto quantidades de stock utilizam precisão (18,3) para suportar unidades fraccionárias como quilogramas e litros. As enumerações são armazenadas como strings através de HasConversion<string>(), o que melhora a legibilidade dos dados armazenados e permite a consulta directa na base de dados sem necessidade de tabelas de lookup.

Quanto à segurança dos dados, as passwords dos utilizadores são armazenadas exclusivamente com hash BCrypt (custo 12, salt automático) via BCrypt.Net-Next, conforme exigido pelo RNF04. Nenhuma password é guardada em texto claro. A sessão HTTP utiliza cookies com HttpOnly e IsEssential, com timeout de oito horas de inactividade (RF05, RNF05). As mensagens de erro de autenticação são deliberadamente genéricas, sem indicação de qual campo falhou (RF04), prevenindo a enumeração de contas.

O motor de base de dados em desenvolvimento é o SQLite, que armazena os dados num único ficheiro (sgclc.db) com configuração zero e portabilidade entre macOS e Windows. Em produção, está prevista a utilização de SQL Server 2019+ ou PostgreSQL, sendo a migração possível por simples alteração da connection string no appsettings.json (RNF15). A inicialização da base de dados é feita através de Database.EnsureCreated() no arranque da aplicação, com seed automático via SeedData.InitializeAsync() que cria cinco lojas, quinze produtos, seis categorias, cinco utilizadores, três fornecedores e catorze dias de dados de vendas de demonstração.

### 2.4.5. Validação do Modelo de Dados

A validação do modelo de dados é realizada a quatro níveis complementares, assegurando que os dados que chegam à base de dados cumprem todas as restrições definidas.

Ao nível da base de dados (Nível 1), as restrições de integridade — chaves primárias, chaves estrangeiras, índices únicos, campos obrigatórios e CHECK constraints — são configuradas via Fluent API e aplicadas directamente pelo SQLite. Qualquer violação resulta numa excepção antes da persistência dos dados.

Ao nível da camada de serviços (Nível 2), a lógica de negócio valida as regras antes de invocar o repositório. O StockService.AjustarStockAsync verifica que o ajuste não resulta em stock negativo (RF16). O StockService.CheckStockAsync pré-valida a disponibilidade de stock antes das vendas. O SalesService.CreateSaleAsync valida o stock para todos os produtos do carrinho antes de iniciar a transacção, rejeitando a venda inteira se qualquer produto não tiver stock suficiente. O SalesService.CancelSaleAsync confirma que a venda se encontra no estado Concluida. O ProdutoService.CreateAsync valida a unicidade do Codigo. O AuthService.LoginAsync valida as credenciais e o estado activo do utilizador.

Ao nível dos DTOs (Nível 3), os contratos de entrada são definidos como records C# imutáveis, que actuam como barreira entre os dados recebidos e as entidades de domínio. Esta separação impede que dados inválidos ou não autorizados — como propriedades de navegação ou identificadores internos — cheguem às entidades. Os DTOs de criação (CreateVendaDto, CreateEncomendaDto, EmitirFaturaDto, CreateUtilizadorDto) definem explicitamente os campos aceites em cada operação.

Ao nível dos testes automatizados (Nível 4), dez testes unitários com xUnit e EF Core InMemory validam o comportamento dos serviços críticos. Cinco testes verificam o StockService (ajuste negativo válido, ajuste que resulta em stock negativo, verificação de stock suficiente e insuficiente, dedução de stock). Três testes verificam o SalesService (venda com stock insuficiente, venda com stock suficiente incluindo débito, cancelamento de venda já cancelada). Dois testes verificam o ProdutoService (criação com código duplicado, desactivação de produto inexistente).


## 2.5. Design de Interfaces do Sistema

### 2.5.1. Estrutura Geral e Arquitectura de Informação

O design de interfaces do SGCLC foi orientado por dois princípios definidos durante a Etapa 2 de design. O primeiro, designado "role-first navigation", estabelece que o menu de navegação e as funcionalidades disponíveis são inteiramente determinados pelo papel do utilizador autenticado. Assim, um Funcionário de POS nunca visualiza o menu de Relatórios, e um Gestor da Cadeia nunca acede ao botão de Confirmar Venda no POS. O segundo princípio, "density by role", define que a complexidade da interface escala com o nível hierárquico do papel: o Funcionário dispõe de um POS simples e focado na operação de venda, o Gerente tem acesso a dashboards com KPIs e múltiplos módulos de gestão, e o Gestor tem a visão mais completa com dados agregados de todas as lojas.

A aplicação utiliza um layout base (MainLayout.razor) organizado em três zonas: a barra de navegação superior, que apresenta o nome da aplicação, a loja activa, o nome do utilizador e o botão de logout; a barra lateral (sidebar), que contém o menu de navegação adaptado ao papel RBAC do utilizador; e a área de conteúdo central, renderizada pelo router Blazor com o componente correspondente à rota activa. A página de Login recorre a um layout separado (EmptyLayout.razor) sem sidebar, proporcionando um ecrã limpo e centrado.

A Tabela 2.17 apresenta a matriz de acesso RBAC, que define quais os módulos acessíveis a cada papel, reflectindo o princípio de privilégio mínimo e os requisitos de controlo de acesso (RF02).

| Módulo / Ecrã | Funcionário | Gerente de Loja | Gestor da Cadeia |
|---|:---:|:---:|:---:|
| Dashboard | Básico | Loja | Global |
| Ponto de Venda (POS) | Sim | Sim | Não |
| Gestão de Stock | Não | Loja | Global |
| Encomendas | Não | Loja | Global |
| Faturas | Não | Loja | Global |
| Relatórios | Não | Loja | Global + comparativo |
| Consolidação Diária | Não | Não | Sim |
| Admin – Utilizadores | Não | Parcial (Funcionários) | Completo |

*Tabela 2.17 — Matriz de acesso RBAC por módulo e papel.*

### 2.5.2. Caracterização das Interfaces

Cada interface do sistema é seguidamente descrita quanto às suas funcionalidades, requisitos satisfeitos e particularidades de implementação. Os wireframes completos estão documentados em docs/etapa2/wireframes.md.

O ecrã de Login (RF01, RF04, RF05) apresenta um formulário centrado com campos de email e password. A submissão invoca o IAuthService.LoginAsync e, em caso de credenciais inválidas, apresenta uma mensagem genérica "Credenciais inválidas" sem indicação de qual campo falhou, em conformidade com o RF04. A página inclui uma caixa de dicas com credenciais de demonstração para facilitar os testes. Após autenticação bem-sucedida, o utilizador é redirecionado para o Dashboard adequado ao seu papel, com a sessão configurada para expirar após oito horas de inactividade (RF05).

[IMAGEM: Captura de ecrã — Página de Login]

O Dashboard Central, acessível ao Gestor da Cadeia (RF37, RF41), apresenta quatro KPI cards no topo com o total de vendas do dia (agregado de todas as lojas), o número de alertas de stock, o total de encomendas pendentes e o número de lojas activas. Dispõe de um botão "Consolidar Agora" para trigger manual da consolidação (RF36), um gráfico de vendas por loja com barras de progresso e uma tabela resumo por loja com vendas e transacções. Os dados são carregados através do IReportService.GetDashboardCentralAsync().

[IMAGEM: Captura de ecrã — Dashboard Central (Gestor)]

O Dashboard de Loja, destinado ao Gerente (RF38, RF13, RF14), apresenta quatro KPI cards específicos da loja do gerente: vendas de hoje, transacções de hoje, alertas de stock e encomendas pendentes. Inclui uma lista de alertas de stock activos com ligação directa à gestão de stock e uma tabela com as vendas dos últimos sete dias, detalhando data, total e número de transacções. Os dados provêm do IReportService.GetDashboardLojaAsync(lojaId).

[IMAGEM: Captura de ecrã — Dashboard de Loja (Gerente)]

O Ponto de Venda (RF17–RF21) é a interface mais crítica do sistema e foi desenhada com uma organização em duas colunas optimizada para rapidez operacional (RNF07 — operável sem treino formal em menos de 60 segundos por venda). A coluna esquerda apresenta a tabela de todos os produtos com stock positivo na loja, com filtro instantâneo client-side insensível a acentos e maiúsculas através de normalização Unicode FormD (RF09, RNF08). Cada linha mostra o nome, código, categoria, indicador de stock (badge amarelo em alerta, verde se OK) e preço efectivo (local se definido, base caso contrário). O botão de adição ao carrinho está directamente acessível em cada linha. A coluna direita contém o carrinho com as linhas de venda (produto, quantidade editável, preço unitário, total por linha e botão de remoção), um campo de desconto em valor absoluto em euros (RF20) e o cálculo automático de subtotal, desconto e total (RF19). Os botões "Limpar Carrinho" e "Confirmar Venda" completam a interface. Ao confirmar, o sistema valida o stock, debita as quantidades e emite a fatura automaticamente (RF21). O acesso é restrito a Funcionários e Gerentes; o Gestor da Cadeia é bloqueado.

[IMAGEM: Captura de ecrã — Ponto de Venda (POS)]

A interface de Gestão de Stock (RF11–RF16) apresenta uma tabela de stock com pesquisa por nome, código ou categoria e filtros de estado: Todos, Em alerta, Stock OK, Expirados e A expirar (com prazo inferior a 30 dias). Os indicadores visuais de validade utilizam badges vermelhas para produtos expirados e badges amarelas com os dias restantes para produtos próximos do fim de validade. Para o Gestor da Cadeia, a página inclui um dropdown de selecção de loja, enquanto para o Gerente o carregamento é automático com a sua loja. O modal de ajuste manual apresenta campos de variação (+/-) e motivo obrigatório (RF15), com validação de stock não-negativo (RF16) e registo de auditoria em AjustesStock.

[IMAGEM: Captura de ecrã — Gestão de Stock]

A interface de Encomendas (RF24–RF27) apresenta uma listagem com filtros por estado (Todas, Pendente, Enviada, Rececionada, Cancelada) e badges coloridos por estado: amarelo para Pendente, azul para Enviada, verde para Rececionada e vermelho para Cancelada. O modal de nova encomenda permite a selecção do fornecedor através de um dropdown, a inserção de observações e a adição de linhas de produto com quantidade. O modal de receção, disponível apenas para encomendas pendentes, pré-preenche as quantidades recebidas com os valores pedidos, sendo estes editáveis para suportar receções parciais. Ao confirmar a receção, o stock da loja é actualizado automaticamente (RF27). O cancelamento está disponível apenas para encomendas com estado Pendente.

[IMAGEM: Captura de ecrã — Encomendas]

A interface de Faturas (RF28–RF32) apresenta uma listagem com filtros de data (De / Até), mostrando para cada fatura o número (formato FAT-XXX-YYYY-ZZZZZ), a data de emissão, o cliente (ou "Consumidor Final" por defeito), o total e o estado. Os estados são distinguidos visualmente por badges: amarelo para Emitida, verde para Paga e vermelho para Anulada.

[IMAGEM: Captura de ecrã — Faturas]

A interface de Relatórios de Vendas (RF39–RF42) constitui o painel de análise do sistema. Dispõe de filtros de loja (dropdown, disponível apenas para o Gestor), data início e fim, com atalhos rápidos para os últimos 7 dias, 30 dias e 3 meses. Apresenta três KPI cards (Total Vendas, Nº Transacções, Ticket Médio), um gráfico de vendas por categoria com barras de progresso, uma tabela dos dez produtos mais vendidos com ranking, e botões de exportação para PDF (gerado com QuestPDF) e CSV. O carregamento inicial utiliza automaticamente os últimos 30 dias como período de análise.

[IMAGEM: Captura de ecrã — Relatórios de Vendas]

A interface de Consolidação Diária (RF33–RF36) apresenta uma tabela de histórico com as colunas Data, Loja, Total Vendas, Nº Transacções, Executado em e Estado. Os estados são distinguidos por badges verdes (Sucesso) e vermelhas (Falha, com detalhes do erro). O botão "Consolidar Hoje" é visível apenas para o Gestor da Cadeia (RF36), permitindo accionar manualmente a consolidação.

[IMAGEM: Captura de ecrã — Consolidação]

A interface de Administração de Utilizadores (RF03) apresenta cards rápidos com o total de utilizadores, activos e inactivos, seguidos de uma tabela com nome, email, telefone, papel, loja, data de criação e estado. Os utilizadores inactivos são apresentados com linhas em cinzento e texto esmaecido para distinção visual imediata. As acções disponíveis por utilizador incluem visualização de perfil, edição, reset de password (com validação de mínimo 6 caracteres), desactivação/reactivação e eliminação definitiva (com modal de aviso de irreversibilidade). O modal de criação de utilizador contém os campos nome, email, password, telefone, papel, loja e notas. Para o Gerente de Loja, a criação está limitada a Funcionários e a loja é pré-seleccionada automaticamente; o Gestor da Cadeia pode criar qualquer papel e seleccionar qualquer loja.

[IMAGEM: Captura de ecrã — Administração de Utilizadores]

### 2.5.3. Usabilidade e Acessibilidade

A interface foi desenvolvida com Bootstrap 5.3, assegurando o cumprimento dos requisitos não funcionais de usabilidade (RNF07–RNF09).

No que respeita à responsividade (RNF09), o layout adapta-se a ecrãs com largura mínima de 1024 pixels através do sistema de grid do Bootstrap, com a sidebar a colapsar em resoluções mais baixas. A pesquisa accent-insensitive (RNF08) é implementada em todos os campos de pesquisa do sistema — POS e gestão de stock — através de normalização Unicode (NormalizationForm.FormD), que permite encontrar "Café" ao pesquisar "cafe" ou "Água" ao pesquisar "agua". Esta pesquisa é case-insensitive e executada client-side para resposta instantânea. A eficiência operacional (RNF07) é particularmente evidente no POS, desenhado para ser operável em menos de 60 segundos por venda sem treino formal: a tabela mostra apenas produtos com stock positivo, o filtro é instantâneo e a confirmação requer um único clique.

A consistência visual é assegurada através de convenções cromáticas uniformes em toda a aplicação: os alertas de stock utilizam laranja/amarelo para alertas e verde para stock normal; os estados de entidades seguem o padrão verde para estados finais positivos (Concluída, Rececionada, Paga, Sucesso), amarelo para estados intermédios (Pendente, Enviada, Emitida) e vermelho para estados negativos (Cancelada, Anulada, Falha); os botões de acção primária utilizam azul e os de perigo utilizam vermelho.

O feedback ao utilizador é proporcionado através de mensagens de sucesso (alertas verdes) e erro (alertas vermelhos) após cada operação. Os botões são desabilitados durante operações assíncronas, apresentando um spinner para prevenir submissões duplicas. A segurança de navegação é garantida pela verificação do papel do utilizador em cada componente Blazor através do SessionService no OnInitializedAsync — se o utilizador não está autenticado ou não tem permissão para aceder ao componente, é redirecionado para a página de login.

### 2.5.4. Validação das Interfaces

As interfaces foram validadas através de três tipos de verificação complementares.

Os testes funcionais por papel garantiram que todos os fluxos de utilização para cada papel (GestorCadeia, GerenteLoja, Funcionario) funcionam correctamente e que as restrições RBAC são respeitadas em todos os ecrãs, assegurando que cada utilizador acede exclusivamente às funcionalidades autorizadas.

Os testes de fluxos críticos cobriram nove cenários de teste manual: login com credenciais válidas e inválidas; venda completa no POS incluindo adição de produtos, desconto, confirmação e verificação do débito de stock; cancelamento de venda com verificação da reposição de stock; criação e receção de encomenda com verificação da actualização automática de stock; ajuste manual de stock com verificação do log de auditoria; emissão de fatura associada a venda; consolidação manual e verificação do resultado; CRUD completo de utilizadores (criar, editar, desactivar, reactivar e eliminar); e geração de relatório com filtros e exportação.

Os testes de casos limite verificaram o comportamento da interface em situações atípicas: listas vazias (sem produtos, sem vendas), valores zero (desconto = 0), stock esgotado (produto desaparece automaticamente do POS), encomendas sem linhas (prevenido pela interface) e utilizadores inactivos (com destaque visual diferenciado).

A rastreabilidade entre as interfaces e os requisitos funcionais está sintetizada na Tabela 2.18, que demonstra a cobertura completa dos quarenta e dois requisitos funcionais pelos nove ecrãs do sistema.

| Ecrã | RFs Satisfeitos | Funcionário | Gerente | Gestor |
|---|---|:---:|:---:|:---:|
| Login | RF01, RF04, RF05 | Sim | Sim | Sim |
| Dashboard | RF37, RF38, RF13, RF14 | Sim | Sim | Sim |
| POS | RF17, RF18, RF19, RF20, RF21 | Sim | Sim | Não |
| Stock | RF11, RF12, RF13, RF15, RF16 | Não | Sim | Sim |
| Encomendas | RF24, RF25, RF26, RF27 | Não | Sim | Sim |
| Faturas | RF28, RF29, RF31, RF32 | Não | Sim | Sim |
| Relatórios | RF39, RF40, RF41 | Não | Sim | Sim |
| Consolidação | RF33, RF34, RF35, RF36 | Não | Não | Sim |
| Admin – Utilizadores | RF03 | Não | Parcial | Sim |

*Tabela 2.18 — Rastreabilidade entre interfaces e requisitos funcionais.*


## 2.6. Especificação de API e Decisões Arquiteturais

### 2.6.1. Camada de Serviços (API Interna)

O SGCLC não expõe uma API REST externa, tratando-se de uma aplicação Blazor Server em que a comunicação entre o browser e o servidor é gerida internamente pelo SignalR. Contudo, a camada de serviços funciona como uma API interna com contratos bem definidos através de interfaces, que poderiam ser facilmente expostas como API REST no futuro sem alterações à lógica de negócio. Cada interface de serviço define um conjunto de operações coerentes, com parâmetros de entrada tipados (DTOs) e retornos explícitos.

O IAuthService gere a autenticação e sessão (RF01–RF05), disponibilizando o LoginAsync para autenticação com BCrypt (retorna LoginResultDto ou null), o ChangePasswordAsync para alteração de password com re-hash, e o LogoutAsync para limpeza de sessão.

O IProdutoService é responsável pela gestão do catálogo de produtos (RF06–RF10). Oferece operações de listagem de produtos activos (GetAllAsync), pesquisa accent-insensitive por nome, código ou categoria (SearchAsync), consulta por identificador (GetByIdAsync), criação com validação de código único (CreateAsync), actualização (UpdateAsync) e desactivação lógica (DeactivateAsync).

O IStockService gere o inventário (RF11–RF16) com operações para consulta de stock por loja (GetStockLojaAsync) e de todas as lojas (GetStockAllLojasAsync), obtenção de alertas (GetAlertasAsync), ajuste manual com auditoria (AjustarStockAsync), definição de stock mínimo (DefinirStockMinimoAsync), definição de preço local (DefinirPrecoLocalAsync), pré-validação de stock para vendas (CheckStockAsync), débito de stock aquando de vendas (DeductStockAsync) e reposição aquando de cancelamentos (ReporStockAsync).

O ISalesService processa as vendas (RF17–RF22). Disponibiliza a consulta de vendas por identificador (GetByIdAsync) e por loja com filtro de datas (GetByLojaAsync), a criação de venda com validação de stock e débito atómico (CreateSaleAsync), o cancelamento com reposição de stock (CancelSaleAsync) e o processamento de devoluções parciais ou totais (ProcessDevolucoesAsync).

O IOrderService gere as encomendas de reposição (RF23–RF27), com operações de listagem por loja (GetByLojaAsync), consulta de pendentes (GetPendentesAsync), consulta por identificador (GetByIdAsync), criação com estado Pendente (CreateAsync), receção com actualização de stock (RecepcionarAsync) e cancelamento (CancelAsync).

O IFornecedorService é responsável pela gestão de fornecedores (RF23), oferecendo operações de listagem de activos (GetAllAsync), consulta (GetByIdAsync), criação (CreateAsync), actualização (UpdateAsync) e desactivação lógica (DeactivateAsync).

O IFaturaService trata da faturação e exportação de documentos (RF28–RF32). Inclui a listagem por loja (GetByLojaAsync) e global (GetAllFaturasAsync) com filtros de data, a consulta por identificador (GetByIdAsync), a emissão com numeração automática (EmitirAsync) e a exportação em PDF via QuestPDF (ExportPdfAsync).

O IConsolidacaoService gere a consolidação diária (RF33–RF36), disponibilizando a consolidação de todas as lojas activas (ConsolidarTodasAsync), a consolidação individual (ConsolidarLojaAsync) e o histórico de consolidações (GetHistoricoAsync).

O IReportService é responsável pelos relatórios e dashboards (RF37–RF42). Oferece o relatório de vendas com KPIs, top produtos e análise por categoria e dia (GetRelatorioVendasAsync), os dashboards central (GetDashboardCentralAsync) e por loja (GetDashboardLojaAsync), e a exportação em PDF (ExportRelatorioVendasPdfAsync) e CSV (ExportRelatorioVendasCsvAsync).

O IUtilizadorService gere a administração de utilizadores (RF03), com operações de listagem global (GetAllAsync) e por loja (GetByLojaAsync), consulta (GetByIdAsync), criação com hash BCrypt (CreateAsync), actualização (UpdateAsync), desactivação e reactivação (DeactivateAsync, ReactivateAsync), eliminação permanente (DeleteAsync) e reset de password (ResetPasswordAsync).

### 2.6.2. Decisões Arquitecturais

As decisões arquitecturais do SGCLC foram tomadas durante a Etapa 2 de design e estão fundamentadas na adequação ao contexto específico de uma aplicação de gestão interna para uma cadeia de lojas de conveniência. A Tabela 2.19 resume as dez decisões principais, as alternativas consideradas e a justificação para cada escolha.

| # | Decisão | Alternativa | Justificação |
|---|---|---|---|
| DA1 | Blazor Server | Blazor WebAssembly, MVC | Server-side rendering via SignalR; sem necessidade de API REST separada; acesso directo aos serviços; adequado para aplicação interna com latência aceitável |
| DA2 | SQLite (dev) | SQL Server Express | Zero-configuration; portabilidade entre macOS e Windows; troca para SQL Server em produção por alteração da connection string (RNF15) |
| DA3 | Padrão Repository | Acesso directo ao DbContext | Abstrai o acesso a dados; permite substituição do ORM sem impacto na lógica de negócio; facilita testes unitários com provider InMemory (RNF12, RNF13) |
| DA4 | DTOs como records | Classes mutáveis, ViewModels | Imutabilidade e igualdade por valor; separam contratos de entrada/saída das entidades de domínio; previnem exposição de dados internos |
| DA5 | Enums como strings | Enums como inteiros | HasConversion<string>() melhora a legibilidade dos dados armazenados; consulta directa na BD sem tabelas de lookup |
| DA6 | BackgroundService | Quartz.NET, Hangfire | Integrado no ecossistema ASP.NET Core (IHostedService); sem dependências externas; hora configurável em appsettings.json sem recompilação |
| DA7 | SessionService Singleton | ASP.NET Identity, JWT | Simplicidade para Blazor Server (circuito SignalR dedicado); sessão em memória adequada para single-server; timeout de 8 horas (RF05) |
| DA8 | Soft Delete | Eliminação física | Entidades Produto, Fornecedor, Loja e Utilizador usam campo Ativo/Ativa; preserva integridade referencial e histórico |
| DA9 | GUID como PK (Utilizadores) | INT AUTO_INCREMENT | Evita colisões em cenários de migração; prática comum em sistemas de autenticação |
| DA10 | Desnormalização na Fatura | Normalização estrita | NomeCliente, NIFCliente, DescricaoProduto são copiados para preservar o contexto histórico — prática contabilística standard |

*Tabela 2.19 — Decisões arquitecturais do SGCLC.*

A escolha do Blazor Server (DA1) é particularmente relevante, pois define a natureza da comunicação entre o browser e o servidor. Ao contrário de uma SPA tradicional com API REST, o Blazor Server mantém um circuito SignalR persistente para cada utilizador, permitindo que os componentes da interface invoquem directamente os serviços da camada Core sem serialização HTTP. Esta abordagem simplifica significativamente o desenvolvimento de uma aplicação de gestão interna onde a latência de rede é mínima e não existe necessidade de suportar clientes heterogéneos.

A adopção do padrão Repository (DA3), embora adicione uma camada de abstracção, revelou-se fundamental para a testabilidade do sistema. Os testes unitários utilizam o provider InMemory do EF Core para simular a base de dados, e as interfaces de repositório permitem a injecção de implementações alternativas (mocks) para isolar os testes da camada de dados. Esta decisão suporta directamente os requisitos de manutenibilidade (RNF12) e testabilidade (RNF13).

A utilização de DTOs como C# records (DA4) proporciona imutabilidade e igualdade por valor, actuando como contratos explícitos entre camadas. Esta separação é essencial para impedir que dados inválidos ou propriedades de navegação do Entity Framework sejam acidentalmente expostos ou manipulados fora do contexto apropriado.

A desnormalização selectiva na Fatura (DA10) representa um desvio consciente da normalização estrita, justificado pela natureza contabilística do documento. Uma fatura deve preservar fielmente os dados vigentes no momento da sua emissão — o nome do cliente, o NIF e a descrição dos produtos — independentemente de alterações posteriores a essas entidades. Esta é uma prática universalmente aceite em sistemas de faturação e contabilidade.
