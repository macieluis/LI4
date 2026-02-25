# Diagramas Complementares – Modelação Comportamental
## Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência

**Versão:** 1.0 | **Data:** 24 de Fevereiro de 2026 | **Projeto:** LI4 2025/2026

---

## 1. Máquinas de Estado

### 1.1 Máquina de Estados – Venda

```mermaid
stateDiagram-v2
    [*] --> EmCurso : Funcionário inicia venda no POS

    EmCurso --> EmCurso : Adiciona produto ao carrinho
    EmCurso --> EmCurso : Remove produto do carrinho
    EmCurso --> EmCurso : Aplica desconto

    EmCurso --> Concluida : Funcionário confirma venda\n[stock suficiente para todos os produtos]
    EmCurso --> Abandonada : Funcionário cancela antes de confirmar

    Concluida --> Cancelada : Gerente cancela venda\n[motivo obrigatório]
    Concluida --> Devolvida : Gerente processa devolução\n[parcial ou total]
    Concluida --> Faturada : Fatura emitida associada

    Cancelada --> [*]
    Devolvida --> [*]
    Faturada --> [*]
    Abandonada --> [*]
```

---

### 1.2 Máquina de Estados – Encomenda

```mermaid
stateDiagram-v2
    [*] --> Pendente : Gerente de Loja cria encomenda

    Pendente --> Enviada : Encomenda comunicada ao fornecedor
    Pendente --> Cancelada : Gerente cancela encomenda\n[motivo obrigatório]

    Enviada --> Rececionada : Gerente regista receção\n[quantidades podem diferir do pedido]
    Enviada --> Cancelada : Cancelamento após envio\n[só Gestor da Cadeia]

    Rececionada --> [*] : Stock atualizado automaticamente
    Cancelada --> [*]
```

---

### 1.3 Máquina de Estados – Fatura

```mermaid
stateDiagram-v2
    [*] --> Emitida : Fatura gerada pelo sistema

    Emitida --> Paga : Pagamento registado pelo gerente
    Emitida --> Anulada : Gerente anula fatura\n[emissão de nota de crédito]

    Paga --> [*]
    Anulada --> [*]
```

---

### 1.4 Máquina de Estados – Consolidação

```mermaid
stateDiagram-v2
    [*] --> Agendada : Hora configurada atingida\nou trigger manual

    Agendada --> EmExecucao : Job inicia processamento

    EmExecucao --> Sucesso : Todos os dados agregados\nsem erros
    EmExecucao --> Falha : Erro de comunicação\nou dados inválidos
    EmExecucao --> Parcial : Algumas lojas falharam

    Sucesso --> [*] : Dashboard central atualizado
    Falha --> Agendada : Retry automático\nem 30 minutos
    Parcial --> [*] : Dashboard parcialmente atualizado\nGestor notificado
```

---

## 2. Diagramas de Sequência Adicionais

### 2.1 Sequência – Login

```mermaid
sequenceDiagram
    actor U as Utilizador
    participant UI as Blazor Login Page
    participant AS as AuthService
    participant UR as UtilizadorRepository
    participant DB as Base de Dados
    participant SS as SessionStore

    U->>UI: Acede à aplicação
    UI-->>U: Apresenta formulário de login

    U->>UI: Submete email + password
    UI->>AS: LoginAsync(email, password)
    AS->>UR: GetByEmailAsync(email)
    UR->>DB: SELECT Utilizadores WHERE Email = @email
    DB-->>UR: Utilizador (ou null)
    UR-->>AS: Utilizador?

    alt Utilizador não encontrado ou inativo
        AS-->>UI: null (falha)
        UI-->>U: "Credenciais inválidas"
    else Utilizador encontrado
        AS->>AS: BCrypt.Verify(password, hash)
        alt Password incorreta
            AS-->>UI: null (falha)
            UI-->>U: "Credenciais inválidas"
        else Password correta
            AS->>SS: CreateSession(userId, papel, lojaId)
            SS-->>AS: SessionToken
            AS-->>UI: LoginResultDto(userId, nome, papel, lojaId)
            UI-->>U: Redirect → Dashboard (conforme papel)
        end
    end
```

---

### 2.2 Sequência – Criação de Encomenda

```mermaid
sequenceDiagram
    actor GL as Gerente de Loja
    participant UI as Blazor Encomendas
    participant OS as OrderService
    participant ER as EncomendaRepository
    participant DB as Base de Dados
    participant NS as NotificationService

    GL->>UI: Acede a "Nova Encomenda"
    UI->>OS: GetFornecedoresAsync()
    OS-->>UI: Lista de fornecedores
    UI-->>GL: Selecionar fornecedor e produtos

    GL->>UI: Preenche produtos, qtds e submit
    UI->>OS: CreateAsync(CreateEncomendaDto)
    OS->>OS: Valida dados (fornecedor ativo, produtos existentes)

    alt Dados inválidos
        OS-->>UI: Exception(mensagem)
        UI-->>GL: Apresenta erro de validação
    else Dados válidos
        OS->>ER: AddAsync(Encomenda)
        ER->>DB: INSERT Encomendas + LinhasEncomenda
        DB-->>ER: Encomenda criada (Id gerado)
        ER-->>OS: Encomenda

        OS->>NS: NotifyGestorCadeiaAsync("Nova encomenda da loja X")
        NS-->>OS: OK

        OS-->>UI: EncomendaDto
        UI-->>GL: "Encomenda criada com sucesso (ID: #301)"
    end
```

---

### 2.3 Sequência – Geração de Relatório de Vendas

```mermaid
sequenceDiagram
    actor GC as Gestor da Cadeia
    participant UI as Blazor Relatórios
    participant RS as ReportService
    participant VR as VendaRepository
    participant DB as Base de Dados

    GC->>UI: Acede a "Relatórios" → "Vendas"
    UI-->>GC: Formulário: loja, data início, data fim, categoria

    GC->>UI: Define filtros e clica "Gerar"
    UI->>RS: GetRelatorioVendasAsync(lojaId?, de, ate, categoriaId?)
    RS->>VR: GetByLojaAsync(lojaId, de, ate)
    VR->>DB: SELECT Vendas + LinhasVenda WHERE filtros
    DB-->>VR: Lista de vendas
    VR-->>RS: Vendas

    RS->>RS: Calcula KPIs:
    Note over RS: Total Vendas = SUM(Vendas.Total)
    Note over RS: Nr Transações = COUNT(Vendas)
    Note over RS: Ticket Médio = Total / Nr
    Note over RS: Agrupa por Categoria e por Dia

    RS-->>UI: RelatorioVendasDto (KPIs + gráficos + top produtos)
    UI-->>GC: Apresenta relatório com gráficos e tabelas

    opt Exportar PDF
        GC->>UI: Clica "Exportar PDF"
        UI->>RS: ExportRelatorioVendasPdfAsync(filtros)
        RS->>RS: Gera PDF com QuestPDF
        RS-->>UI: byte[] PDF
        UI-->>GC: Download automático do ficheiro
    end
```

---

### 2.4 Sequência – Ajuste Manual de Stock

```mermaid
sequenceDiagram
    actor GL as Gerente de Loja
    participant UI as Blazor Stock
    participant SK as StockService
    participant SR as StockRepository
    participant AR as AjusteStockRepository
    participant DB as Base de Dados

    GL->>UI: Seleciona produto → "Ajuste Manual"
    UI-->>GL: Formulário: variação (+/-) e motivo

    GL->>UI: Preenche variação (-5) e motivo "Quebra – produto caído"
    UI->>SK: AjustarStockAsync(lojaId, produtoId, -5, motivo, userId)
    SK->>SR: GetAsync(lojaId, produtoId)
    SR->>DB: SELECT Stock WHERE LojaId AND ProdutoId
    DB-->>SR: Stock atual (Quantidade = 150)
    SR-->>SK: Stock

    SK->>SK: Nova Quantidade = 150 + (-5) = 145
    alt Nova Quantidade < 0
        SK-->>UI: Exception("Stock não pode ficar negativo")
        UI-->>GL: "Erro: variação inválida"
    else Nova Quantidade >= 0
        SK->>SR: UpdateAsync(stock com Quantidade = 145)
        SR->>DB: UPDATE Stocks SET Quantidade = 145
        DB-->>SR: OK
        SK->>AR: AddAsync(AjusteStock)
        AR->>DB: INSERT AjustesStock (log de auditoria)
        DB-->>AR: OK
        SK-->>UI: OK
        UI-->>GL: "Stock atualizado: 145 unidades"
    end
```
