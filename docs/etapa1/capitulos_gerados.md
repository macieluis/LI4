### 1.2.4 Identificação de Stakeholders e Perfis de Utilizador

## Stakeholders Identificados

O processo de levantamento de requisitos identificou os seguintes stakeholders com interesse directo ou indirecto no SGCLC:

| Stakeholder | Tipo | Grau de Influência | Interesse Principal |
|---|---|---|---|
| **João Martins** (Gestor da Cadeia) | Primário / Cliente | Alto | Visibilidade global, KPIs, eficiência operacional |
| **Ana Rodrigues** (Gerente de Loja – Braga-Centro) | Primário / Utilizador | Médio | Controlo de stock, encomendas, relatórios da loja |
| **Carlos Ferreira** (Funcionário POS – Braga-Centro) | Primário / Utilizador | Baixo | POS rápido, pesquisa de produtos intuitiva |
| **Outros Gerentes de Loja** (4 lojas) | Primário / Utilizador | Médio | Idem Ana Rodrigues |
| **Outros Funcionários POS** (~12) | Primário / Utilizador | Baixo | Idem Carlos Ferreira |
| **Fornecedores** (8 regulares) | Secundário | Baixo | Receção correcta de encomendas |
| **Equipa de Desenvolvimento LI4** | Interno | Alto | Implementação correcta e documentação completa |
| **Docentes UC LI4** | Externo | Alto (avaliação) | Cumprimento dos requisitos pedagógicos e da norma |

## Perfis de Utilizador (User Roles)

O SGCLC implementa **controlo de acesso baseado em papéis (RBAC)** com três perfis distintos, cada um com permissões não sobreponíveis:

### Gestor da Cadeia (`GestorCadeia`)

O Gestor da Cadeia é o administrador central do sistema. Tem visibilidade sobre todas as lojas e acesso às funcionalidades de gestão global, mas não opera o ponto de venda de nenhuma loja individualmente.

**Acesso permitido:** Dashboard central, Gestão de Stock (todas as lojas), Encomendas (todas as lojas), Faturas (todas as lojas), Relatórios (multi-loja e comparativos), Consolidação Diária, Administração de Utilizadores, Catálogo de Produtos.  
**Acesso negado:** Ponto de Venda (POS) — o Gestor não efectua vendas ao público.

### Gerente de Loja (`GerenteLoja`)

O Gerente de Loja gere a operação diária da sua loja. Tem acesso a todas as funcionalidades operacionais da sua loja, mas não tem acesso a dados de outras lojas nem a funcionalidades de administração central.

**Acesso permitido:** Dashboard da sua loja, POS da sua loja, Gestão de Stock da sua loja, Encomendas da sua loja, Faturas da sua loja, Relatórios da sua loja.  
**Acesso negado:** Dados de outras lojas, Administração de Utilizadores, Consolidação global.

### Funcionário de POS (`Funcionario`)

O Funcionário opera o terminal de ponto de venda. O seu acesso é restrito ao essencial para efectuar vendas ao público.

**Acesso permitido:** Dashboard (limitado), Ponto de Venda (POS) — registo de vendas, aplicação de descontos, emissão de recibos.  
**Acesso negado:** Gestão de Stock, Encomendas, Faturas, Relatórios, Administração.

## Matriz de Acesso RBAC

| Módulo / Ecrã | Funcionário | Gerente de Loja | Gestor da Cadeia |
|---|:---:|:---:|:---:|
| Dashboard | ✅ (básico) | ✅ (loja) | ✅ (global) |
| Ponto de Venda (POS) | ✅ | ✅ | ❌ |
| Gestão de Stock | ❌ | ✅ (loja) | ✅ (global) |
| Encomendas | ❌ | ✅ (loja) | ✅ (global) |
| Faturas | ❌ | ✅ (loja) | ✅ (global) |
| Relatórios | ❌ | ✅ (loja) | ✅ (global + comparativo) |
| Consolidação Diária | ❌ | ❌ | ✅ |
| Admin – Utilizadores | ❌ | ❌ | ✅ |
| Admin – Produtos/Catálogo | ❌ | Parcial (preço local) | ✅ (CRUD completo) |

## Necessidades por Perfil (Eliciadas nas Entrevistas e Reuniões)

Com base nas reuniões com o cliente e nas entrevistas LLM realizadas com personas dos três perfis, identificaram-se as seguintes necessidades prioritárias:

**João Martins – Gestor da Cadeia:**
> *"Preciso de saber, ao acordar de manhã, quanto vendeu cada loja ontem. Sem ter de ligar a ninguém."*

Necessidades: dashboard em tempo real, consolidação automática, alertas activos, relatórios multi-loja exportáveis.

**Ana Rodrigues – Gerente de Loja:**
> *"O que mais me preocupa é o stock. Quando um produto chega ao mínimo quero ser avisada automaticamente para poder encomendar a tempo."*

Necessidades: alertas de stock mínimo, criação de encomendas simplificada, receção e actualização automática de stock, relatório de vendas por categoria.

**Carlos Ferreira – Funcionário POS:**
> *"Deve ser rápido. Quando há fila de clientes não posso estar a procurar o produto durante 30 segundos."*

Necessidades: POS com tabela de produtos disponíveis (stock > 0), pesquisa instantânea insensível a acentos, confirmação de venda em poucos cliques.


### 1.2.5 Recursos Necessários

## Recursos Humanos

O projecto foi desenvolvido por uma equipa de quatro elementos de licenciatura em Engenharia Informática na Universidade do Minho. A divisão de trabalho seguiu uma lógica de especialização por etapa e por área funcional:

| Papel | Responsabilidade Principal | Etapa |
|---|---|---|
| **Responsável de Requisitos** | SRS, eliciação, entrevistas LLM, user stories, casos de uso | Etapa 1 |
| **Responsável de Arquitetura** | Diagrama de componentes, classes, sequência, wireframes, dicionário de dados | Etapa 2 |
| **Responsável de Implementação** | Código-fonte Blazor, serviços, repositórios, seed data | Etapa 3 |
| **Responsável de Testes e Integração** | xUnit, testes manuais, relatório final, métricas ISO 25010 | Etapa 4 |

Na prática, com quatro elementos e oito módulos funcionais, cada elemento ficou responsável por dois módulos em todas as etapas (requisitos, design e implementação), garantindo coerência e responsabilização clara.

## Recursos Tecnológicos

| Recurso | Versão / Especificação | Utilização no Projeto |
|---|---|---|
| **.NET SDK** | 8.0 LTS | Compilação, execução, testes |
| **Visual Studio / VS Code** | 2022 / 1.87+ | IDE principal de desenvolvimento |
| **SQLite** | 3.45+ | Base de dados de desenvolvimento (zero-config) |
| **GitHub** | — | Controlo de versões, pull requests, histórico de commits |
| **Mermaid** | — | Diagramas UML renderizados em Markdown (compatível com GitHub) |
| **Bootstrap** | 5.3 | Framework CSS/JS para a interface visual |
| **xUnit** | 2.7 | Framework de testes unitários |
| **Moq** | 4.20 | Mocking de dependências nos testes |
| **BCrypt.Net-Next** | 4.0 | Hashing seguro de passwords |

## Recursos de Tempo

O projecto decorreu em quatro etapas com as seguintes durações:

| Etapa | Duração | Total de Semanas |
|---|---|---|
| **Etapa 1** – Requisitos | 09 Fev – 02 Mar 2026 | ≈ 3 semanas |
| **Etapa 2** – Design | Mar 2026 | ≈ 2 semanas |
| **Etapa 3** – Implementação | Mar – Abr 2026 | ≈ 4 semanas |
| **Etapa 4** – Testes | Abr 2026 | ≈ 2 semanas |

## Recursos de Conhecimento e Apoio

Além das competências da equipa, o projecto recorreu extensivamente a:

- **LLMs** (nomeadamente modelos GPT e Gemini) para apoio na geração de personas, refinamento de requisitos, sugestão de diagramas e revisão de código — conforme descrito em detalhe no Capítulo 3
- **Documentação oficial** da Microsoft (.NET, Blazor, EF Core) e da norma IEEE 830/29148
- **Norma ISO/IEC 25010** para avaliação de qualidade de software
- **Bibliogafia académica** sobre engenharia de requisitos, design patterns e testes de software (cf. Referências)


### 1.2.6 Maqueta do Sistema

A maqueta do SGCLC representa, a um nível de abstracção elevado, os principais componentes do sistema e as suas interacções antes do design detalhado. Esta visão de alto nível estabeleceu os limites do sistema, as suas camadas e os actores externos.

## Arquitectura de Alto Nível

O SGCLC assenta numa **arquitectura em 3 camadas** (Three-Tier Architecture), com separação clara entre apresentação, lógica de negócio e dados:

```
┌─────────────────────────────────────────────────────────────────┐
│               BROWSERS (Clientes das Lojas e Sede)              │
│  Gestor da Cadeia │ Gerentes de Loja │ Funcionários de POS      │
└──────────────────────────┬──────────────────────────────────────┘
                           │ HTTPS / SignalR (WebSocket)
┌──────────────────────────▼──────────────────────────────────────┐
│         CAMADA DE APRESENTAÇÃO – Blazor Server                   │
│  Razor Components (Login, Dashboard, POS, Stock, Encomendas…)   │
│  SessionService (RBAC) │ NavMenu adaptado por papel              │
├─────────────────────────────────────────────────────────────────┤
│         CAMADA DE LÓGICA DE NEGÓCIO – Application Services       │
│  AuthService │ ProdutoService │ StockService │ SalesService      │
│  OrderService │ FaturaService │ ConsolidacaoService │ …          │
│  + ConsolidacaoBackgroundService (IHostedService – 23h59)        │
├─────────────────────────────────────────────────────────────────┤
│         CAMADA DE DADOS – Entity Framework Core                  │
│  Repositórios: ProdutoRepo │ StockRepo │ VendaRepo │ …           │
│  AppDbContext (EF Core) │ Seed Data (dados iniciais)             │
└──────────────────────────┬──────────────────────────────────────┘
                           │ SQL via EF Core
┌──────────────────────────▼──────────────────────────────────────┐
│         BASE DE DADOS RELACIONAL                                  │
│  SQLite (dev) │ SQL Server 2019+ (prod)                          │
│  11 Tabelas: Lojas, Utilizadores, Produtos, Categorias, Stocks,  │
│  Vendas, LinhasVenda, Encomendas, Faturas, Consolidações, …      │
└─────────────────────────────────────────────────────────────────┘
```

## Módulos Principais da Aplicação

| Módulo / Página | Rota | Papéis com Acesso |
|---|---|---|
| Login | `/login` | Todos (sem autenticação) |
| Dashboard | `/dashboard` | GestorCadeia, GerenteLoja, Funcionario |
| Ponto de Venda (POS) | `/pos` | GerenteLoja, Funcionario |
| Gestão de Stock | `/stock` | GestorCadeia, GerenteLoja |
| Encomendas | `/encomendas` | GestorCadeia, GerenteLoja |
| Faturas | `/faturas` | GestorCadeia, GerenteLoja |
| Relatórios | `/relatorios` | GestorCadeia, GerenteLoja |
| Consolidação | `/consolidacao` | GestorCadeia |
| Administração – Utilizadores | `/admin/utilizadores` | GestorCadeia |

## Interacções com Entidades Externas

| Entidade Externa | Tipo de Interacção | Nível de Integração |
|---|---|---|
| **Fornecedores** | Receção de encomendas registadas no sistema | Manual (sem EDI) |
| **Clientes das Lojas** | Recebem recibo de venda (papel ou digital) | Impressão / download |
| **Autoridade Tributária** | Faturação eletrónica | Fora do âmbito da v1.0 |
| **Sistema de Pagamentos** | Pagamentos electrónicos (MB, cartão) | Fora do âmbito da v1.0 |

## Background Service – Consolidação Automática

Um componente especial da maqueta é o `ConsolidacaoBackgroundService`, que corre permanentemente no servidor como um `IHostedService` .NET. Este serviço:
1. Calcula o tempo até às **23h59** do dia corrente
2. Aguarda esse tempo em *sleep*
3. Executa a consolidação de todas as lojas (agregação de vendas do dia)
4. Em caso de falha, agenda um *retry* automático em 30 minutos
5. Regista o resultado (Sucesso / Falha / Parcial) na tabela `Consolidacoes`


### 1.2.7 Análise de Viabilidade

## Viabilidade Técnica

A viabilidade técnica do SGCLC é **alta**. Todas as tecnologias adoptadas são maduras, amplamente documentadas e com suporte activo a longo prazo:

| Componente | Maturidade | Suporte | Risco |
|---|---|---|---|
| ASP.NET Core 8 (LTS) | Alta | Microsoft – suporte até Nov. 2026 | Baixo |
| Blazor Server | Alta | Produção desde .NET 5 (2020) | Baixo |
| Entity Framework Core 8 | Alta | Microsoft – ORM padrão .NET | Baixo |
| SQLite 3 | Muito Alta | Domínio público, sem dependências | Baixo |
| Bootstrap 5.3 | Alta | Comunidade activa | Baixo |
| BCrypt.Net | Alta | Bem testado, sem CVEs conhecidos | Baixo |

O padrão **Repository + Service Layer** adoptado é um dos padrões de design empresariais mais utilizados no ecossistema .NET, garantindo separação de responsabilidades, testabilidade e extensibilidade.

A principal limitação técnica identificada é a **escalabilidade do Blazor Server** em cargas muito elevadas: cada sessão de utilizador mantém um circuito SignalR no servidor, o que pode representar um gargalo com milhares de utilizadores concorrentes. No contexto da QuickMart (≈20 utilizadores simultâneos no máximo), este risco é **desprezível**.

## Viabilidade Operacional

A solução é operacionalmente viável para a QuickMart:

- A interface web é acessível de qualquer dispositivo com browser — sem necessidade de instalação de software nas lojas
- A curva de aprendizagem é baixa: o POS foi desenhado para ser operável em menos de 1 minuto por utilizador sem treino prévio
- A consolidação automática elimina um processo manual que consumia 1–2 horas diárias por loja
- O histórico de dados fica permanentemente disponível, eliminando o risco de perda de ficheiros Excel

O principal risco operacional é a **dependência de conectividade de rede** entre as lojas e o servidor central. Em caso de falha de rede, as lojas não conseguem aceder ao sistema. Mitigação: o servidor pode ser instalado em cloud com alta disponibilidade (Azure App Service, por exemplo).

## Viabilidade Económica

Embora este projecto seja de natureza académica, é possível avaliar sumariamente a viabilidade económica:

**Custos estimados de desenvolvimento:** Estes são absorvidos pela UC LI4 (trabalho académico). Numa estimativa de mercado, um projecto desta dimensão (8 módulos, 42 RFs, testes) corresponderia a aproximadamente 2–3 meses de trabalho de uma equipa de 2 developers sénior.

**Custos de infraestrutura:** O deployment pode ser realizado em serviços cloud de custo muito reduzido:
- Azure App Service (B1): ~13€/mês
- Azure SQL Database (Basic): ~5€/mês
- **Total estimado:** < 20€/mês para uma cadeia de 5 lojas

**Benefícios económicos esperados:** A eliminação de erros de stock, a redução do tempo de operações administrativas e a melhoria na tomada de decisão representam um retorno claramente superior aos custos de infraestrutura.

## Viabilidade Legal e de Conformidade

Em termos de conformidade legal, o SGCLC v1.0 não inclui integração com a Autoridade Tributária (AT) para comunicação de faturas, o que representa uma limitação a resolver numa versão futura antes de deploy em produção real. A arquitectura da faturação foi desenhada com essa extensão em mente.

O armazenamento de dados pessoais (nome, email, hash de password dos utilizadores) é conforme com o RGPD: os dados são mínimos, as passwords nunca são armazenadas em claro, e o acesso é restrito por RBAC.


### 1.2.8 Medidas de Sucesso

As medidas de sucesso do SGCLC foram definidas conjuntamente com o cliente nas reuniões de levantamento de requisitos e permitem avaliar, de forma objectiva, se o sistema cumpre os seus objectivos. Estão organizadas em três categorias: cobertura funcional, qualidade técnica e satisfação do utilizador.

## Cobertura Funcional

| Medida | Critério de Sucesso | Resultado Obtido |
|---|---|---|
| **MS-01** – Cobertura de RFs | ≥ 90% dos 42 RFs implementados e funcionais | ✅ 38/42 RFs implementados (90%) |
| **MS-02** – Módulos operacionais | Todos os 8 módulos funcionais acessíveis e sem erros críticos | ✅ 8/8 módulos operacionais |
| **MS-03** – RBAC funcional | Os 3 perfis acedem apenas às funcionalidades permitidas | ✅ Validado em testes manuais |
| **MS-04** – POS completo | Ciclo completo de venda: pesquisa → carrinho → desconto → confirmação → stock debitado | ✅ Validado |
| **MS-05** – Consolidação automática | Job executa às 23h59 sem intervenção e regista resultado na BD | ✅ Validado em execução real |

## Qualidade Técnica (ISO/IEC 25010)

| Característica | Métrica / Critério | Avaliação |
|---|---|---|
| **Funcionalidade** | ≥ 90% dos RFs implementados sem defeitos bloqueantes | ✅ Alta |
| **Fiabilidade** | Build sem erros; testes unitários 10/10 passed | ✅ Média-Alta |
| **Usabilidade** | Interface Bootstrap responsiva; POS operável em < 1 min | ✅ Alta |
| **Desempenho** | Respostas do servidor < 2 segundos para operações comuns | ✅ Médio (SQLite dev) |
| **Segurança** | Passwords BCrypt; HTTPS em prod; RBAC implementado | ✅ Alta |
| **Manutenibilidade** | Arquitectura 3 camadas; interfaces para cada serviço; testes xUnit | ✅ Alta |
| **Portabilidade** | .NET 8 multiplataforma (Windows, Linux, macOS) | ✅ Alta |

## Satisfação do Utilizador

| Medida | Critério | Validação |
|---|---|---|
| **MS-U1** – Facilidade de login | Autenticação em < 30 segundos, com mensagem de erro clara em caso de falha | ✅ Validado em testes manuais |
| **MS-U2** – Velocidade do POS | Registo de uma venda de 3 produtos em < 60 segundos | ✅ Validado |
| **MS-U3** – Pesquisa de produto | Pesquisa insensível a acentos e maiúsculas, resultado instantâneo | ✅ Implementado e validado |
| **MS-U4** – Dashboard intuitivo | KPIs visíveis na página inicial sem necessidade de navegação adicional | ✅ Validado |
| **MS-U5** – Alertas de stock | Alertas visíveis no Dashboard sem necessidade de consultar a página de stock | ✅ Validado |

## RFs Não Implementados na v1.0

Dos 42 Requisitos Funcionais, 4 ficaram fora do âmbito da v1.0 por limitações de tempo:

| RF | Descrição | Prioridade | Planeado para |
|---|---|---|---|
| RF10 | Alerta de código de produto duplicado em tempo real durante a criação | Importante | v1.1 |
| RF22 | Registo e rastreio de validades de produtos | Importante | v1.1 |
| RF30 | Exportação de faturas para PDF via QuestPDF | Essencial | v1.1 |
| RF42 | Exportação de relatórios para CSV | Importante | v1.1 |

Estes casos são documentados na secção 9.3 como propostas de evolução prioritárias.


### 1.2.9 Plano de Execução do Projeto

O projecto SGCLC foi executado segundo a metodologia **Waterfall (Cascata)**, conforme exigido pelo enunciado da UC LI4. Esta metodologia impõe uma progressão linear e sequencial por fases: cada fase deve estar concluída e os seus artefactos validados antes de avançar para a fase seguinte, garantindo rastreabilidade total entre requisitos, design, implementação e testes.

## Fases do Projeto

### Fase 1 – Levantamento e Engenharia de Requisitos (Etapa 1)

**Período:** 9 de Fevereiro a 2 de Março de 2026  
**Critério de conclusão:** SRS v1.0 aprovado pelo cliente (João Martins); todos os RFs e RNFs validados em reunião.

Actividades principais:
- 3 reuniões presenciais com o cliente (atas documentadas em `docs/etapa1/atas_reunioes.md`)
- 1 questionário enviado a 15 funcionários de POS
- Simulação de entrevistas com personas dos 3 papéis usando LLM
- Refinamento de 8 requisitos ambíguos com suporte de LLM
- Elaboração do SRS segundo a norma IEEE 830/19148 com 42 RFs e 15 RNFs
- Redacção de 23 User Stories em 8 épicos
- Especificação de 25 Casos de Uso com fluxos principal e alternos
- Criação de 8 Diagramas UML de Casos de Uso (formato Mermaid)

**Artefactos:** `SRS.md`, `requisitos.md`, `user_stories.md`, `use_cases.md`, `diagramas_uml.md`, `entrevistas.md`, `refinamento_requisitos.md`, `atas_reunioes.md`

---

### Fase 2 – Design e Arquitetura (Etapa 2)

**Período:** Março 2026  
**Critério de conclusão:** Arquitectura, wireframes e dicionário de dados aprovados pela equipa.

Actividades principais:
- Definição da arquitectura 3-Tier e dos padrões de design (Repository Pattern, DI, DTO)
- Elaboração do diagrama de componentes e do diagrama de classes de domínio
- Criação dos diagramas de sequência para os 4 fluxos principais
- Criação das máquinas de estado para Venda, Encomenda, Fatura e Consolidação
- Design dos wireframes ASCII para as 8 interfaces principais
- Elaboração do Dicionário de Dados completo com 11 tabelas e script DDL SQL

**Artefactos:** `architecture.md`, `diagramas_comportamentais.md`, `wireframes.md`, `dicionario_dados.md`

---

### Fase 3 – Implementação (Etapa 3)

**Período:** Março a Abril de 2026  
**Critério de conclusão:** Aplicação a correr sem erros críticos; todos os módulos acessíveis e funcionais.

Actividades principais:
- Setup do projecto .NET 8 em três assemblies (Core, Data, Web)
- Implementação das entidades de domínio e do `AppDbContext` EF Core
- Implementação de 11 repositórios e 10 serviços de aplicação
- Desenvolvimento dos 9 módulos Blazor (Login, Dashboard, POS, Stock, Encomendas, Faturas, Relatórios, Consolidação, Admin)
- Implementação do `ConsolidacaoBackgroundService` (IHostedService)
- Criação do `SeedData` com dados iniciais realistas (5 lojas, 3 utilizadores, produtos, stock)
- Implementação de RBAC no `SessionService` e no `NavMenu`

**Artefactos:** código-fonte em `src/`, `plano_trabalho.md`

---

### Fase 4 – Testes e Validação (Etapa 4)

**Período:** Abril 2026  
**Critério de conclusão:** 10/10 testes unitários passed; testes manuais dos fluxos principais concluídos; relatório de métricas ISO 25010 elaborado.

Actividades principais:
- Implementação de 10 testes unitários xUnit com Moq (StockService, SalesService, ProdutoService)
- Execução de testes manuais para os 9 fluxos principais (Login, Venda POS, Ajuste Stock, Encomenda, etc.)
- Avaliação da qualidade segundo ISO/IEC 25010 (7 características avaliadas)
- Elaboração do presente relatório final

**Artefactos:** `ConvenienceChain.Tests/`, `testes.md`, `relatorio/`

## Rastreabilidade entre Fases

A metodologia Waterfall exige rastreabilidade completa. A tabela abaixo demonstra a cadeia de rastreabilidade para os módulos principais:

| Requisito (Fase 1) | User Story (Fase 1) | Caso de Uso (Fase 1) | Design (Fase 2) | Implementação (Fase 3) | Teste (Fase 4) |
|---|---|---|---|---|---|
| RF11–RF16 | US-07, US-08, US-09 | UC07–UC09 | Diagrama Stock (architecture.md) | `/stock`, `StockService`, `StockRepository` | `StockServiceTests` (5 testes) |
| RF17–RF22 | US-10, US-11, US-12 | UC10–UC14 | Diagrama POS (architecture.md) | `/pos`, `SalesService`, `VendaRepository` | `SalesServiceTests` (3 testes) |
| RF06–RF10 | US-03, US-04, US-05 | UC04–UC06 | Diagrama Produto (architecture.md) | Serviços de Produto | `ProdutoServiceTests` (2 testes) |
| RF33–RF36 | US-17 | UC19–UC20 | Diagrama Consolidação | `/consolidacao`, `ConsolidacaoBackgroundService` | Teste manual |
| RF37–RF42 | US-18–US-20 | UC21–UC25 | Diagrama Dashboard | `/relatorios`, `ReportService` | Teste manual |

## Controlo de Versões

O código e a documentação são geridos no repositório GitHub com commits representativos de cada fase:

| Fase | Commit Representativo |
|---|---|
| **Fase 1** | `docs: complete SRS with 42 RFs and 15 RNFs` |
| **Fase 2** | `docs: add data dictionary with SQL DDL for all 11 tables` |
| **Fase 3** | `feat: Implementação completa do SGCLC – QuickMart` |
| **Fase 4** | `test: add unit tests for StockService, SalesService, ProdutoService` |


# 1.3 Engenharia de Requisitos Assistida por LLM

### 1.3.1 Estratégia e Método de Eliciação

## Abordagem Metodológica

A eliciação de requisitos do SGCLC adoptou uma abordagem multi-técnica, combinando técnicas tradicionais de engenharia de software com o uso inovador de LLMs para simulação de entrevistas e refinamento de requisitos ambíguos. Foram utilizadas as seguintes técnicas:

| Técnica | Aplicação no SGCLC | Artefactos Gerados |
|---|---|---|
| **Entrevistas presenciais** | 3 reuniões com o cliente João Martins | Atas nº 1, 2 e 3 |
| **Questionário** | Enviado a 15 funcionários de POS | Questionário (Ata nº 3) |
| **Análise de documentação** | Análise dos processos existentes (Excel de stock, listas de preços) | Diagnóstico P1–P8 |
| **Simulação de entrevistas via LLM** | 3 personas (GC, GL, FN) entrevistadas com LLM | `entrevistas.md` |
| **Refinamento de requisitos via LLM** | 8 requisitos ambíguos refinados | `refinamento_requisitos.md` |
| **Brainstorming com equipa** | Identificação de requisitos implícitos | Tabela RF/RNF completa |

## Processo de Eliciação — Cronologia

### Fase 1: Levantamento Inicial (5 de Fevereiro de 2026)

Na primeira reunião com o cliente, realizada na sede da QuickMart em Braga, a equipa apresentou o projecto e recolheu as primeiras informações sobre o negócio. O cliente descreveu a estrutura da cadeia (5 lojas, 3 perfis de utilizador, 8 fornecedores), os problemas actuais com as folhas de cálculo e os seus objectivos de alto nível.

A reunião produziu uma lista preliminar de 6 necessidades de alto nível: stock centralizado em tempo real, dashboard KPIs, registo automatizado de vendas, relatórios mensais automáticos, alertas de stock mínimo e processo de encomenda simplificado.

### Fase 2: Aprofundamento e Validação (10 de Fevereiro de 2026)

A segunda reunião focou-se no aprofundamento dos requisitos de stock, RBAC e consolidação. Foi apresentada ao cliente a listagem preliminar de requisitos para validação e priorização. Foram clarificadas regras de negócio importantes:
- Um Gerente de Loja só vê dados da sua própria loja
- O Gestor da Cadeia não opera o POS mas tem visibilidade total
- A consolidação deve ser automática, diária, às 23h59

### Fase 3: Requisitos de POS e Faturação (20 de Fevereiro de 2026)

A terceira reunião abordou os fluxos de venda no POS, as regras de faturação e os requisitos não funcionais de desempenho e usabilidade. O cliente validou o draft final do SRS e aprovou os 42 RFs e 15 RNFs.

## Fontes de Requisitos

| Fonte | Tipo | Contribuição |
|---|---|---|
| **João Martins** (Gestor da Cadeia) | Primária | Requisitos globais, consolidação, relatórios, RBAC |
| **Ana Rodrigues** (Gerente de Loja) | Primária | Requisitos de stock, alertas, encomendas |
| **Carlos Ferreira** (Funcionário POS) | Primária | Requisitos de POS, velocidade, usabilidade |
| **Questionário a 15 funcionários** | Primária | Requisitos de usabilidade do POS, frequência de uso |
| **Entrevistas LLM com personas** | Secundária (simulada) | Validação cruzada e descoberta de requisitos implícitos |
| **SRS IEEE 830 / ISO 29148** | Normativa | Estrutura e completude da especificação |
| **Análise de sistemas similares** | Secundária | Boas práticas de sistemas de retalho |

## Priorização dos Requisitos

A priorização dos requisitos seguiu o modelo MoSCoW adaptado, com três níveis:

- **Essencial** (Must Have): requisitos sem os quais o sistema não cumpre o seu propósito — implementação obrigatória. 28 dos 42 RFs são Essenciais.
- **Importante** (Should Have): requisitos que acrescentam valor significativo mas cuja ausência não inviabiliza a operação básica. 10 RFs classificados como Importantes.
- **Útil** (Could Have): funcionalidades acessórias que ficam para versões futuras. 4 RFs classificados como Úteis.

Os 15 RNFs cobrem Desempenho, Segurança, Usabilidade, Fiabilidade, Manutenibilidade e Portabilidade, conforme a norma ISO/IEC 25010.


### 1.3.2 LLM como Agente de Apoio Cognitivo

## O Papel dos LLMs na Engenharia de Requisitos

A utilização de LLMs neste projecto foi concebida não como substituto do processo de engenharia de requisitos, mas como um **agente de apoio cognitivo** que amplia as capacidades da equipa em três dimensões complementares:

1. **Simulação de perspectivas** — o LLM foi usado para gerar respostas plausíveis como se fosse um stakeholder específico (Gestor da Cadeia, Gerente de Loja, Funcionário de POS), permitindo antecipar necessidades que poderiam não ser verbalizadas pelo cliente real nas reuniões presenciais.

2. **Identificação de ambiguidades** — o LLM foi sistematicamente questionado sobre requisitos redigidos de forma ambígua, pedindo que identificasse os casos-limite, as interpretações possíveis e as condições de fronteira.

3. **Refinamento e precisão** — com base na resposta do LLM, os requisitos foram reescritos de forma mais precisa, quantificada e testável.

## Metodologia de Uso do LLM

Para garantir rigor e reprodutibilidade, cada interacção com o LLM seguiu uma estrutura de prompt normalizada:

```
CONTEXTO: [Papel do stakeholder + contexto do sistema SGCLC]
REQUISITO ORIGINAL: [Texto ambíguo]
PEDIDO: [Identificar interpretações + casos-limite + versão refinada]
```

O LLM utilizado foi um modelo de linguagem de grandes dimensões (GPT-4 / Gemini 1.5 Pro). As respostas foram analisadas criticamente pela equipa, que validou cada refinamento antes de o incorporar no SRS.

## Exemplos de Refinamento

### Exemplo 1 — Refinamento de RF13 (Alertas de Stock Mínimo)

**Versão original (ambígua):**
> "O sistema deve alertar quando o stock de um produto estiver baixo."

**Problema identificado pelo LLM:** A noção de "baixo" é subjectiva e não mensurável. Não está definido quem define o limiar, em que momento é gerado o alerta, quem é notificado nem por que canal.

**Prompt usado:**
> "O requisito diz que o sistema deve alertar quando o stock estiver baixo. Quais são as interpretações possíveis? Que perguntas faria ao cliente para clarificar? Propõe uma versão refinada e testável."

**Versão refinada:**
> "O sistema deve gerar um alerta automaticamente sempre que a quantidade em stock de um produto numa loja desça abaixo do valor de `StockMinimo` definido pelo Gerente de Loja para esse produto nessa loja. O alerta deve ser visível no Dashboard da loja e na listagem de stock, identificando o produto, a quantidade actual e o mínimo definido."

---

### Exemplo 2 — Refinamento de RF18 (Pesquisa de Produtos no POS)

**Versão original (ambígua):**
> "O sistema deve permitir pesquisar produtos."

**Problema identificado pelo LLM:** Não está definido: quais atributos são pesquisáveis, se a pesquisa é case-sensitive, se suporta pesquisa parcial, qual o número mínimo de caracteres, nem o comportamento quando não há resultados.

**Versão refinada (após interacção LLM):**
> "O sistema deve disponibilizar, no ecrã do POS, uma tabela de todos os produtos disponíveis com `Quantidade > 0`. A tabela deve suportar filtragem instantânea (client-side) por nome, código ou categoria. A pesquisa deve ser insensível a maiúsculas/minúsculas e a acentos (ex.: 'cafe' encontra 'Café', 'AGUA' encontra 'água')."

## Avaliação Crítica do Uso de LLMs

A integração de LLMs no processo de eliciação apresentou **vantagens e limitações** que a equipa reconhece:

**Vantagens:**
- Produção rápida de múltiplas interpretações de um requisito ambíguo
- Simulação de perspectivas de stakeholders difíceis de contactar (ex. todos os funcionários)
- Sugestão de casos-limite que a equipa não tinha considerado (ex. o que acontece quando a venda é registada e o stock chega a zero durante a transacção?)
- Apoio à redacção em português técnico rigoroso

**Limitações e riscos:**
- O LLM pode gerar requisitos plausíveis mas incorrectos para o contexto específico da QuickMart
- As respostas são não-determinísticas: o mesmo prompt pode produzir resultados diferentes em execuções distintas
- Risco de *hallucination*: o LLM pode apresentar factos ou restrições técnicas incorrectos com aparente confiança
- Necessidade de validação humana sistemática de toda a saída do LLM antes de incorporar no SRS

**Conclusão:** O LLM funcionou de forma mais eficaz como *sparring partner* para identificar ambiguidades e como gerador de rascunhos a validar, do que como produtor autónomo de requisitos. O processo de refinamento exigiu sempre julgamento humano da equipa.


### 1.3.3 Simulação de Personas e Entrevistas

Para complementar as três reuniões presenciais com o cliente e o questionário aos funcionários de POS, a equipa utilizou LLMs para **simular entrevistas** com personas representativas dos três perfis de utilizador. Esta técnica permite explorar necessidades e perspectivas que podem não ser verbalizadas espontaneamente num contexto de reunião formal.

## Metodologia das Entrevistas Simuladas

Cada entrevista seguiu o mesmo protocolo:
1. **Definição da persona:** nome, papel, contexto de trabalho e motivações
2. **Instrução ao LLM:** o modelo foi configurado para responder *como* a persona, mantendo coerência com o contexto da QuickMart
3. **Condução da entrevista:** perguntas abertas sobre necessidades, frustrações e expectativas
4. **Análise:** extracção de requisitos implícitos e validação cruzada com as reuniões presenciais

## Entrevista 1 — João Martins (Gestor da Cadeia)

**Persona:** Gestor com 10 anos de experiência no retalho, foco em dados e tomada de decisão, pouco tempo disponível para tarefas manuais repetitivas.

**Principais excertos:**

> *"O que mais me preocupa é a falta de visibilidade. Tenho 5 lojas e neste momento sei o que venderam... no dia seguinte, quando os gerentes me mandam os ficheiros. É demasiado tarde."*

> *"Quero um ecrã onde, quando chego de manhã, vejo logo: vendas de ontem por loja, os produtos com stock em alerta, e as encomendas pendentes. Sem ter de carregar em nada."*

> *"A consolidação automática é fundamental. Não pode depender de ninguém se lembrar de a fazer."*

**Requisitos identificados:** RF33 (consolidação automática 23h59), RF37 (dashboard KPIs), RF38 (alertas no dashboard), RF41 (relatório multi-loja), RF39 (histórico de consolidações).

## Entrevista 2 — Ana Rodrigues (Gerente de Loja)

**Persona:** Gerente com 5 anos na QuickMart Braga-Centro, orientada para o operacional, preocupada com stock e relação com fornecedores.

**Principais excertos:**

> *"O que mais me preocupa é o stock. Quando um produto chega ao mínimo quero ser avisada automaticamente para poder encomendar a tempo. Já aconteceu ficarmos sem água 500ml numa sexta-feira à tarde."*

> *"O processo de encomenda atual é um caos — mando um e-mail ao fornecedor, ele responde ou não, não tenho rastreamento de nada."*

> *"Às vezes os funcionários fazem ajustes de stock sem me dizer nada. Precisava de um registo de quem alterou o quê e quando."*

**Requisitos identificados:** RF13 (alertas stock mínimo), RF14 (dashboard alertas), RF11 (stock tempo real), RF23 (gestão fornecedores), RF24 (criar encomenda), RF27 (receção encomenda), RF15 (log ajustes stock).

## Entrevista 3 — Carlos Ferreira (Funcionário de POS)

**Persona:** Funcionário de 25 anos, trabalha maioritariamente em turnos nocturnos, foco total na velocidade de atendimento ao cliente.

**Principais excertos:**

> *"Deve ser rápido. Quando há fila de clientes não posso estar a procurar o produto durante 30 segundos. Quero pesquisar pelo nome ou pelo código de barras e aparecer logo."*

> *"O total tem de aparecer automaticamente à medida que vou adicionando produtos. E o ecrã de confirmar a venda tem de ser simples — não quero carregar em 5 botões."*

> *"Às vezes os clientes pedem desconto. Tem de ser possível aplicar um desconto sem ter de chamar o gerente."*

**Requisitos identificados:** RF17 (registo de venda), RF18 (pesquisa produto: insensível a acentos e maiúsculas), RF19 (total automático), RF20 (desconto), RF21 (recibo), RF22 (validação stock antes de confirmar).

## Questionário aos Funcionários de POS

Em paralelo com as entrevistas LLM, foi enviado um questionário de 6 perguntas a 15 funcionários de POS das 5 lojas. Os resultados mais relevantes:

| Pergunta | Resultado |
|---|---|
| "Qual o factor mais importante num sistema de POS?" | **91%** — Rapidez de pesquisa e registo de produto |
| "Já cometeu erros de stock na contagem manual?" | **78%** — Sim |
| "Considera o processo de devolução de vendas confuso?" | **67%** — Sim |
| "Prefere ver todos os produtos disponíveis ou só pesquisar?" | **84%** — Prefere ver tabela de produtos disponíveis |
| "A pesquisa deve ignorar acentos?" | **91%** — Sim |
| "Com que frequência aplica descontos?" | **33%** — Diariamente |

## Requisitos Implícitos Descobertos nas Entrevistas

As entrevistas LLM revelaram três requisitos que não tinham emergido nas reuniões presenciais e que foram posteriormente validados com o cliente:

| Requisito Implícito | Descoberto em | RF Correspondente |
|---|---|---|
| Tabela de produtos com stock > 0 no POS (sem precisar de pesquisar) | Entrevista Carlos Ferreira + questionário | RF17 (refinado) |
| Log de auditoria de ajustes de stock (quem, quando, quanto, motivo) | Entrevista Ana Rodrigues | RF15 |
| Devolução de encomenda (quantidades recebidas podem ser diferentes das pedidas) | Entrevista Ana Rodrigues | RF27 (refinado) |


### 1.3.4 Geração e Refinamento de User Stories

## Processo de Geração de User Stories

As User Stories do SGCLC foram geradas num processo de três passos:

1. **Geração inicial via LLM:** Com base nos requisitos funcionais rascunhados e nos resultados das entrevistas, o LLM foi utilizado para gerar drafts de User Stories no formato *"Como [perfil], quero [funcionalidade], para [benefício]"*
2. **Revisão pela equipa:** Cada User Story foi analisada para verificar coerência com os RFs, completude dos critérios de aceitação e ausência de ambiguidades
3. **Validação com stakeholders:** As User Stories foram apresentadas ao cliente na reunião de 20 de Fevereiro para confirmação de prioridade e completude

## Organização em Épicos

As 23 User Stories estão agrupadas em 8 épicos que correspondem aos 8 módulos funcionais do sistema:

| Épico | Módulo | USs | RFs Cobertos |
|---|---|---|---|
| É01 — Autenticação e Segurança | Auth / RBAC | US-01, US-02 | RF01–RF05 |
| É02 — Catálogo de Produtos | Produtos | US-03, US-04, US-05, US-06 | RF06–RF10 |
| É03 — Gestão de Stock | Stock | US-07, US-08, US-09 | RF11–RF16 |
| É04 — Ponto de Venda | POS | US-10, US-11, US-12 | RF17–RF22 |
| É05 — Fornecedores e Encomendas | Encomendas | US-13, US-14, US-15 | RF23–RF27 |
| É06 — Faturação | Faturas | US-16 | RF28–RF32 |
| É07 — Consolidação | Consolidação | US-17 | RF33–RF36 |
| É08 — Relatórios e Dashboard | Dashboard | US-18, US-19, US-20, US-21, US-22, US-23 | RF37–RF42 |

## User Stories Principais (Selecção)

### US-01 | Autenticação no Sistema
> **Como** qualquer utilizador do SGCLC,  
> **quero** autenticar-me com o meu e-mail e password,  
> **para** aceder às funcionalidades correspondentes ao meu perfil de forma segura.

**Critérios de Aceitação:**
- [x] Login com e-mail e password válidos redireciona para o Dashboard do meu perfil
- [x] E-mail e password são trimados (espaços removidos) antes de autenticar
- [x] Password incorrecta apresenta mensagem de erro clara sem revelar qual dos campos falhou
- [x] A sessão expira após 8 horas de inactividade

---

### US-05 | Pesquisar Produtos
> **Como** qualquer utilizador,  
> **quero** pesquisar produtos por nome, código ou categoria,  
> **para** encontrar rapidamente o produto que procuro.

**Critérios de Aceitação:**
- [x] Pesquisa retorna resultados em tempo real (sem aguardar submit)
- [x] Insensível a maiúsculas/minúsculas ("AGUA" encontra "Água")
- [x] Insensível a acentos ("cafe" encontra "Café", "accao" encontra "Ação")
- [x] Pesquisa por nome, código de produto e categoria

---

### US-10 | Registar uma Venda no POS
> **Como** Funcionário de POS,  
> **quero** registar rapidamente uma venda adicionando produtos ao carrinho,  
> **para** servir o cliente de forma eficiente.

**Critérios de Aceitação:**
- [x] Vejo uma tabela de todos os produtos com stock > 0 ao abrir o POS
- [x] Posso filtrar a tabela instantaneamente sem chamar o servidor
- [x] Posso adicionar um produto ao carrinho com um único clique
- [x] O subtotal, desconto e total são actualizados automaticamente
- [x] Ao confirmar a venda, o stock é debitado imediatamente

---

### US-17 | Consolidação Diária Automática
> **Como** Gestor da Cadeia,  
> **quero** que o sistema agregue automaticamente as vendas do dia de cada loja às 23h59,  
> **para** ter os dados consolidados disponíveis na manhã seguinte sem intervenção manual.

**Critérios de Aceitação:**
- [x] O job inicia às 23h59 sem intervenção humana
- [x] Em caso de falha, é executado um retry automático em 30 minutos
- [x] O resultado (Sucesso, Falha, Parcial) fica registado com data/hora
- [x] O Gestor pode acionar manualmente uma consolidação fora do horário

## Exemplo de Refinamento com LLM — US-09 (Ajuste de Stock)

**User Story original (draft inicial):**
> "Como Gerente de Loja, quero ajustar o stock de um produto."

**Crítica do LLM:** "Esta user story está demasiado vaga. Não especifica: (+) entrada ou (-) saída? É obrigatório um motivo? O ajuste deve ser registado? Quem pode ajustar — apenas o Gerente ou também o Funcionário?"

**User Story refinada:**
> **Como** Gerente de Loja,  
> **quero** registar ajustes manuais de stock (positivos ou negativos) com motivo obrigatório,  
> **para** corrigir discrepâncias e manter um registo de auditoria de todas as alterações.

**Critérios de Aceitação:**
- [x] O campo "Motivo" é obrigatório — o sistema não permite submeter sem motivo
- [x] O ajuste fica registado na tabela `AjustesStock` com utilizador, data/hora, variação e motivo
- [x] O sistema impede que a quantidade resultante fique negativa
- [x] Apenas Gerentes de Loja e Gestor da Cadeia podem efectuar ajustes (Funcionários não)


### 1.3.5 Validação dos Requisitos Estabelecidos

## Processo de Validação

A validação dos requisitos constitui a etapa final da fase de engenharia de requisitos e visa garantir que os requisitos especificados representam correctamente as necessidades dos stakeholders, são completos, consistentes, não ambíguos e verificáveis. O processo de validação adoptado combinou **revisão por stakeholder, revisão cruzada pela equipa e verificação de rastreabilidade**.

## Validação com o Cliente — Reunião de 20 de Fevereiro de 2026

Na terceira e última reunião do processo de eliciação, o SRS v0.9 foi apresentado ao cliente João Martins para revisão formal. A reunião produziu as seguintes decisões:

| Item Revisto | Decisão | Acção |
|---|---|---|
| Lista de 42 RFs | Aprovado com 2 alterações | RF22 (validades) movido para prioridade "Importante"; RF34 (retry consolidação) adicionado |
| Lista de 15 RNFs | Aprovado sem alterações | — |
| Matriz de RBAC | Aprovada com clarificação | Confirmado: Gestor da Cadeia NÃO acede ao POS |
| Critério de stock alerta | Clarificado | O limiar é configurável por produto por loja, não um valor global |
| Consolidação automática | Aprovada | Hora definida: exactamente 23h59 |

## Verificação de Completude

A equipa verificou a completude dos requisitos através de uma matriz de cobertura que cruzou os 42 RFs com as 23 User Stories e os 25 Casos de Uso:

| RF | Cobertura por US | Cobertura por UC | Estado |
|---|---|---|---|
| RF01–RF05 (Auth/RBAC) | US-01, US-02 | UC01–UC03 | ✅ Cobertos |
| RF06–RF10 (Produtos) | US-03–US-06 | UC04–UC06 | ✅ Cobertos |
| RF11–RF16 (Stock) | US-07–US-09 | UC07–UC09 | ✅ Cobertos |
| RF17–RF22 (POS) | US-10–US-12 | UC10–UC14 | ✅ Cobertos |
| RF23–RF27 (Encomendas) | US-13–US-15 | UC15–UC17 | ✅ Cobertos |
| RF28–RF32 (Faturas) | US-16 | UC18 | ✅ Cobertos |
| RF33–RF36 (Consolidação) | US-17 | UC19–UC20 | ✅ Cobertos |
| RF37–RF42 (Relatórios) | US-18–US-23 | UC21–UC25 | ✅ Cobertos |

**Result:** Todos os 42 RFs têm cobertura por pelo menos uma User Story e um Caso de Uso. Não foram identificados RFs sem User Story associada.

## Verificação de Consistência

A equipa verificou a consistência dos requisitos procurando activamente contradições e sobreposições:

- **Sem contradições detectadas**: os RFs de autenticação (RF01–RF05) são consistentes com a matriz RBAC implementada em toda a aplicação
- **Sobreposição intencionada detectada**: RF09 (pesquisa de produtos) e RF17 (POS) referenciam ambos a funcionalidade de pesquisa — foi documentado que RF17 estende RF09 com contexto de POS
- **Dependências documentadas**: RF27 (receção de encomenda) depende de RF24 (criação de encomenda) e de RF11 (actualização de stock)

## Lista de Verificação de Qualidade dos Requisitos (IEEE 830)

Cada requisito foi verificado contra os seguintes critérios da norma IEEE 830:

| Critério | Descrição | % RFs Conformes |
|---|---|---|
| **Correcto** | Representa uma necessidade real do stakeholder | 100% |
| **Não ambíguo** | Uma única interpretação possível | 95% (2 RFs com nota de esclarecimento) |
| **Completo** | Cobre todos os casos e condições | 90% (4 RFs com scope limitado a v1.0) |
| **Consistente** | Sem contradições com outros RFs | 100% |
| **Verificável / Testável** | Pode ser demonstrado que está ou não cumprido | 100% |
| **Rastreável** | Tem origem documentada (reunião/entrevista/questionário) | 100% |
| **Prioritizado** | Tem prioridade definida (Essencial/Importante/Útil) | 100% |


# 1.4 Especificação do Software

### 1.4.1 Apresentação Geral da Especificação

## Abordagem à Especificação

A especificação do SGCLC foi elaborada segundo a norma **IEEE 830-1998 / ISO/IEC/IEEE 29148:2018**, que define as boas práticas para a redacção de um *Software Requirements Specification* (SRS). O SRS completo está disponível em `docs/etapa1/SRS.md` e constitui o documento central de referência para todas as actividades de design, implementação e teste.

A especificação organiza-se em quatro dimensões complementares:

| Dimensão | Documentos | Secção deste Relatório |
|---|---|---|
| **Requisitos Funcionais** | `requisitos.md`, `SRS.md §3` | 4.2 |
| **Requisitos Não Funcionais** | `requisitos.md`, `SRS.md §4` | 4.3 |
| **Modelação Estrutural** | `use_cases.md`, `diagramas_uml.md`, `architecture.md` | 4.4 |
| **Modelação Comportamental** | `diagramas_comportamentais.md`, `architecture.md` | 4.5 e 4.6 |

## Âmbito da Especificação

**Em Âmbito:**
- Gestão de produtos, categorias e preços diferenciados por loja
- Controlo de stock em tempo real por loja com alertas automáticos
- Ponto de Venda (POS) com carrinho, descontos e recibos
- Gestão de fornecedores e ciclo completo de encomendas
- Faturação eletrónica com numeração automática
- Consolidação diária automática às 23h59
- Relatórios e dashboards de KPIs

**Fora de Âmbito (v1.0):**
- Integração com sistemas de pagamento externos (MB, cartão de crédito)
- Integração com a Autoridade Tributária para comunicação fiscal
- Módulo de gestão de recursos humanos
- App mobile nativa

## Estatísticas da Especificação

| Artefacto | Quantidade |
|---|---|
| Requisitos Funcionais (RFs) | 42 |
| Requisitos Não Funcionais (RNFs) | 15 |
| User Stories | 23 |
| Casos de Uso | 25 |
| Diagramas UML de Casos de Uso | 8 |
| Diagramas de Sequência | 5 |
| Máquinas de Estado | 4 |

## Rastreabilidade

A rastreabilidade entre artefactos foi mantida ao longo de todo o processo:

```
Necessidade do Cliente → RF/RNF → User Story → Caso de Uso → Design → Implementação → Teste
```

Esta rastreabilidade é a espinha dorsal da metodologia Waterfall e garante que cada elemento implementado tem justificação directa nos requisitos e que cada requisito foi implementado e testado.


