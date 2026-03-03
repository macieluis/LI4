# User Stories
## Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência

**Versão:** 1.0 | **Data:** 24 de Fevereiro de 2026 | **Projeto:** LI4 2025/2026

---

## Atores
- **GC** – Gestor da Cadeia
- **GL** – Gerente de Loja
- **FN** – Funcionário

---

## Épico 1 – Autenticação e Controlo de Acesso

### US-01 | Login no Sistema
> **Como** qualquer utilizador registado,  
> **quero** autenticar-me com email e password,  
> **para** aceder às funcionalidades que correspondem ao meu papel.

**Critérios de Aceitação:**
- [ ] Login correto redireciona para o dashboard do papel do utilizador
- [ ] Password incorreta mostra mensagem de erro sem revelar qual campo está errado
- [ ] Após 5 tentativas falhadas, a conta é bloqueada por 15 minutos
- [ ] Sessão expira automaticamente após 8 horas de inatividade

### US-02 | Gestão de Utilizadores
> **Como** Gestor da Cadeia,  
> **quero** criar, editar e desativar contas de utilizador,  
> **para** controlar quem acede ao sistema e com que permissões.

**Critérios de Aceitação:**
- [ ] Posso criar utilizadores com nome, email, password temporária e papel
- [ ] Posso atribuir um utilizador a uma ou mais lojas
- [ ] Posso desativar uma conta sem a eliminar (preserva histórico)
- [ ] Utilizador desativado não consegue fazer login

### US-03 | Recuperação de Password
> **Como** qualquer utilizador,  
> **quero** recuperar a minha password por email,  
> **para** aceder ao sistema caso a tenha esquecido.

**Critérios de Aceitação:**
- [ ] Ao submeter o email, recebo um link de redefinição válido por 1 hora
- [ ] O link expira após uso ou após 1 hora
- [ ] A nova password deve ter no mínimo 8 caracteres

---

## Épico 2 – Gestão de Produtos e Catálogo

### US-04 | Criar Produto
> **Como** Gestor da Cadeia,  
> **quero** criar novos produtos no catálogo central,  
> **para** que estejam disponíveis em todas as lojas.

**Critérios de Aceitação:**
- [ ] Campos obrigatórios: código único, nome, categoria, preço base de venda, unidade de medida
- [ ] Código deve ser único; sistema alerta duplicados
- [ ] Posso associar uma foto ao produto
- [ ] Produto criado fica disponível para todas as lojas de imediato

### US-05 | Pesquisar Produtos
> **Como** qualquer utilizador,  
> **quero** pesquisar produtos por nome, código ou categoria,  
> **para** encontrar rapidamente o produto que procuro.

**Critérios de Aceitação:**
- [x] Pesquisa retorna resultados em tempo real (máx. 1 segundo)
- [x] Insensível a maiúsculas/minúsculas ("AGUA" encontra "Água")
- [x] Insensível a acentos ("cafe" encontra "Café", "accao" encontra "Ação")
- [x] Pesquisa por nome, código de produto e categoria
- [ ] Posso filtrar por categoria

### US-06 | Definir Preço por Loja
> **Como** Gerente de Loja,  
> **quero** definir um preço de venda diferente do preço base para produtos na minha loja,  
> **para** adaptar preços ao mercado local.

**Critérios de Aceitação:**
- [ ] Posso sobrepor o preço base de qualquer produto com um preço específico da minha loja
- [ ] O preço da loja tem prioridade sobre o preço base no POS
- [ ] O Gestor da Cadeia consegue ver todos os preços por loja

---

## Épico 3 – Gestão de Stock

### US-07 | Visualizar Stock da Minha Loja
> **Como** Gerente de Loja,  
> **quero** ver o stock atual de todos os produtos da minha loja,  
> **para** saber o que está disponível e o que precisa de ser reposto.

**Critérios de Aceitação:**
- [ ] Vejo uma lista de produtos com quantidade disponível
- [ ] Produtos abaixo do stock mínimo aparecem destacados (vermelho/alerta)
- [ ] Posso filtrar por categoria e pesquisar por produto
- [ ] Posso exportar a listagem em CSV

### US-08 | Definir Stock Mínimo
> **Como** Gerente de Loja,  
> **quero** definir o stock mínimo para cada produto na minha loja,  
> **para** ser alertado quando precisar de encomendar mais.

**Critérios de Aceitação:**
- [ ] Posso definir um valor de stock mínimo por produto por loja
- [ ] Quando o stock atinge ou desce abaixo desse valor, recebo uma notificação/alerta no dashboard
- [ ] O alerta é visível também no dashboard do Gestor da Cadeia

### US-09 | Ajuste Manual de Stock
> **Como** Gerente de Loja,  
> **quero** registar ajustes manuais ao stock (quebras, perdas, inventário),  
> **para** manter o stock do sistema alinhado com a realidade física.

**Critérios de Aceitação:**
- [ ] Posso selecionar um produto e indicar a variação (positiva ou negativa)
- [ ] Campo de motivo é obrigatório (ex: "Inventário", "Quebra", "Roubo")
- [ ] O ajuste é registado no log de auditoria com utilizador, data/hora e motivo
- [ ] O stock é atualizado imediatamente após o registo

---

## Épico 4 – Ponto de Venda (POS)

### US-10 | Registar uma Venda
> **Como** Funcionário,  
> **quero** registar vendas adicionando produtos ao carrinho,  
> **para** processar compras dos clientes.

**Critérios de Aceitação:**
- [ ] Posso adicionar produtos por código ou pesquisa por nome
- [ ] O total é calculado automaticamente em tempo real
- [ ] Posso ajustar quantidade de cada linha
- [ ] Posso remover linhas do carrinho antes de finalizar
- [ ] Ao finalizar, o stock é deduzido automaticamente

### US-11 | Aplicar Desconto
> **Como** Funcionário,  
> **quero** aplicar descontos a uma venda,  
> **para** refletir promoções ou descontos autorizados.

**Critérios de Aceitação:**
- [ ] Posso aplicar desconto percentual ou de valor fixo por linha
- [ ] Posso aplicar desconto global à venda
- [ ] O desconto aplicado é visível no recibo

### US-12 | Emitir Recibo
> **Como** Funcionário,  
> **quero** que o sistema gere um recibo após cada venda,  
> **para** entregar comprovativo ao cliente.

**Critérios de Aceitação:**
- [ ] Recibo inclui: nome e morada da loja, data/hora, lista de produtos, quantidades, preços unitários, descontos e total
- [ ] Recibo pode ser impresso ou visualizado em ecrã
- [ ] Recibo é gerado em menos de 2 segundos

### US-13 | Cancelar/Devolver Venda
> **Como** Gerente de Loja,  
> **quero** cancelar uma venda ou processar devoluções,  
> **para** corrigir erros ou processar devoluções de clientes.

**Critérios de Aceitação:**
- [ ] Posso cancelar uma venda (total) ou devolver linhas individuais
- [ ] Motivo de cancelamento/devolução é obrigatório
- [ ] O stock é reposto automaticamente após cancelamento/devolução
- [ ] A operação fica registada no log de auditoria

---

## Épico 5 – Gestão de Fornecedores e Encomendas

### US-14 | Gerir Fornecedores
> **Como** Gestor da Cadeia,  
> **quero** manter um registo centralizado de fornecedores,  
> **para** poder associá-los a produtos e encomendas.

**Critérios de Aceitação:**
- [ ] Posso criar, editar e desativar fornecedores
- [ ] Cada fornecedor tem: nome, NIF, morada e contactos
- [ ] Posso ver os produtos associados a cada fornecedor

### US-15 | Criar Encomenda a Fornecedor
> **Como** Gerente de Loja,  
> **quero** criar uma encomenda de reposição a um fornecedor,  
> **para** reabastecer o stock da minha loja.

**Critérios de Aceitação:**
- [ ] Posso selecionar o fornecedor e adicionar produtos com quantidades
- [ ] A encomenda é criada no estado "Pendente"
- [ ] O Gestor da Cadeia é notificado de novas encomendas
- [ ] Posso ver o histórico de encomendas da minha loja

### US-16 | Registar Receção de Encomenda
> **Como** Gerente de Loja,  
> **quero** registar a receção de uma encomenda,  
> **para** que o stock seja atualizado automaticamente.

**Critérios de Aceitação:**
- [ ] Ao registar receção, posso indicar quantidades efetivamente recebidas (podem diferir do pedido)
- [ ] O stock é atualizado automaticamente com as quantidades recebidas
- [ ] A encomenda muda para o estado "Rececionada"

---

## Épico 6 – Faturação

### US-17 | Emitir Fatura
> **Como** Funcionário / Gerente de Loja,  
> **quero** emitir uma fatura associada a uma venda,  
> **para** formalizar a transação com clientes empresariais.

**Critérios de Aceitação:**
- [ ] Posso selecionar uma venda e emitir uma fatura com os dados do cliente (nome, NIF)
- [ ] A fatura inclui: número único, data, dados da loja e do cliente, linhas de produto e totais
- [ ] A fatura pode ser visualizada e exportada em PDF

### US-18 | Consultar Faturas
> **Como** Gerente de Loja,  
> **quero** consultar as faturas emitidas pela minha loja,  
> **para** gerir o histórico de faturação.

**Critérios de Aceitação:**
- [ ] Posso filtrar faturas por data, cliente e estado
- [ ] Posso exportar a lista em CSV
- [ ] Posso abrir e reimprimir qualquer fatura

---

## Épico 7 – Consolidação Diária

### US-19 | Consolidação Automática Diária
> **Como** Gestor da Cadeia,  
> **quero** que os dados de todas as lojas sejam consolidados automaticamente no servidor central ao fim de cada dia,  
> **para** ter uma visão global e atualizada da cadeia.

**Critérios de Aceitação:**
- [ ] A consolidação corre automaticamente à hora configurada (default: 23:59)
- [ ] Após consolidação, o dashboard central mostra os dados do dia consolidado
- [ ] O sistema regista um log por cada consolidação (loja, data/hora, resultado, totais)
- [ ] Em caso de erro, o sistema tenta novamente e notifica o administrador

### US-20 | Consolidação Manual
> **Como** Gestor da Cadeia,  
> **quero** poder desencadear a consolidação manualmente,  
> **para** obter dados atualizados a qualquer momento.

**Critérios de Aceitação:**
- [ ] Botão "Consolidar Agora" disponível no dashboard central
- [ ] Posso consolidar uma loja específica ou todas simultaneamente
- [ ] Feedback visual do progresso da consolidação

---

## Épico 8 – Relatórios e Dashboard

### US-21 | Dashboard Central
> **Como** Gestor da Cadeia,  
> **quero** ver um dashboard com KPIs globais,  
> **para** ter uma visão geral e rápida do desempenho da cadeia.

**Critérios de Aceitação:**
- [ ] Vejo: total de vendas do dia (global e por loja), número de alertas de stock, encomendas pendentes
- [ ] Vejo gráfico de vendas dos últimos 7 dias
- [ ] Dashboard atualiza automaticamente (ou tem botão de refresh)

### US-22 | Relatório de Vendas
> **Como** Gestor da Cadeia / Gerente de Loja,  
> **quero** gerar relatórios de vendas com filtros por loja, período e categoria,  
> **para** analisar o desempenho e tomar decisões informadas.

**Critérios de Aceitação:**
- [ ] Posso filtrar por loja, intervalo de datas e categoria de produto
- [ ] O relatório mostra: total de vendas, quantidade de transações, produtos mais vendidos
- [ ] Poso exportar o relatório em PDF e CSV

### US-23 | Relatório Comparativo entre Lojas
> **Como** Gestor da Cadeia,  
> **quero** comparar o desempenho de todas as lojas num período,  
> **para** identificar lojas com melhor e pior desempenho.

**Critérios de Aceitação:**
- [ ] Tabela e gráfico comparativo de vendas por loja no período selecionado
- [ ] Posso selecionar métricas a comparar: total de vendas, nº de transações, ticket médio
- [ ] Exportável em PDF e CSV
