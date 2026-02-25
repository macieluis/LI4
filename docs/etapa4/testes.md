# Etapa 4 – Testes e Avaliação

## 1. Estratégia de Testes

Seguiu-se a pirâmide de testes:
- **Testes Unitários (xUnit):** lógica de negócio isolada (Moq + FluentAssertions)
- **Testes de Integração:** (planeados – EF Core InMemory)
- **Testes Manuais:** validação de fluxos UI

## 2. Testes Unitários Implementados

**Projeto:** `src/ConvenienceChain.Tests/ConvenienceChain.Tests.csproj`

| Nº | Classe | Método | Cenário |
|---|---|---|---|
| 1 | `StockServiceTests` | `AjustarStock_DeveAtualizarQuantidade` | Ajuste positivo de stock |
| 2 | `StockServiceTests` | `AjustarStock_NegativoDeveLancarExcecao` | Stock não pode ficar negativo |
| 3 | `StockServiceTests` | `CheckStock_RetornaTrueQuandoHaStock` | Verificação de disponibilidade |
| 4 | `StockServiceTests` | `CheckStock_RetornaFalseQuandoNaoHaStock` | Sem stock disponível |
| 5 | `StockServiceTests` | `GetAlertas_RetornaApenasProdutosAbaixoMinimo` | Alertas de stock mínimo |
| 6 | `SalesServiceTests` | `CreateSale_DeveDebitarStock` | Venda cria débito de stock |
| 7 | `SalesServiceTests` | `CreateSale_StockInsuficienteDeveLancarExcecao` | Venda bloqueada sem stock |
| 8 | `SalesServiceTests` | `CancelSale_DeveReporStock` | Cancelamento repõe stock |
| 9 | `ProdutoServiceTests` | `CreateProduto_CodigoDuplicadoDeveLancarExcecao` | Código único obrigatório |
| 10 | `ProdutoServiceTests` | `GetAll_RetornaApenasAtivos` | Filtro por produtos ativos |

## 3. Cobertura Estimada

| Serviço | Cobertura estimada |
|---|---|
| StockService | ~80% |
| SalesService | ~75% |
| ProdutoService | ~60% |
| AuthService | ~50% (manual) |
| ConsolidacaoService | Testes integração (planeados) |

## 4. Métricas ISO/IEC 25010

| Característica | Avaliação | Notas |
|---|---|---|
| **Funcionalidade** | ✅ Alta | Todos os RFs implementados |
| **Usabilidade** | ✅ Alta | UI Bootstrap responsiva |
| **Fiabilidade** | ✅ Média-Alta | Testes unitários + tratamento de erros |
| **Desempenho** | ✅ Médio | SQLite dev; SQL Server em prod |
| **Segurança** | ✅ Alta | BCrypt + RBAC + HTTPS |
| **Manutenibilidade** | ✅ Alta | Arquitetura 3 camadas + repositórios |
| **Portabilidade** | ✅ Alta | .NET 8 multiplataforma |

## 5. Resultados dos Testes

```bash
# Executar:
cd /caminho/para/projeto
dotnet test src/ConvenienceChain.Tests/

# Resultado esperado:
# Passed:  10
# Failed:   0
# Skipped:  0
# Total:   10
```

## 6. Testes Manuais

| Fluxo | Resultado | Observações |
|---|---|---|
| Login com credenciais válidas | ✅ Passa | Redireciona para Dashboard |
| Login com credenciais inválidas | ✅ Passa | Mensagem de erro visível |
| Venda POS completa | ✅ Passa | Stock deduzido corretamente |
| Ajuste de stock com motivo | ✅ Passa | Auditoria registada |
| Criação de encomenda | ✅ Passa | Estado Pendente |
| Receção de encomenda | ✅ Passa | Stock reposto |
| Relatório por período | ✅ Passa | KPIs corretos |
| Consolidação manual | ✅ Passa | Histórico atualizado |
| RBAC: Funcionário sem acesso a Relatórios | ✅ Passa | Redirect para Dashboard |
