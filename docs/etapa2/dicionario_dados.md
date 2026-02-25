# Dicionário de Dados
## Sistema de Gestão Integrada para uma Cadeia de Lojas de Conveniência

**Versão:** 1.0 | **Data:** 24 de Fevereiro de 2026 | **Projeto:** LI4 2025/2026

---

## Convenções

| Símbolo | Significado |
|---|---|
| PK | Chave Primária |
| FK | Chave Estrangeira |
| UK | Valor Único |
| NN | Not Null (obrigatório) |
| DEFAULT | Valor por omissão |

---

## Tabela: `Lojas`

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| `Id` | INT | PK, NN, AUTO_INCREMENT | Identificador único da loja | `1` |
| `Nome` | NVARCHAR(100) | NN | Designação comercial da loja | `"QuickMart Braga-Centro"` |
| `Morada` | NVARCHAR(300) | NN | Endereço completo da loja | `"Rua do Souto, 45, Braga"` |
| `Telefone` | NVARCHAR(20) | — | Contacto telefónico | `"253123456"` |
| `Email` | NVARCHAR(200) | — | Email de contacto da loja | `"braga@quickmart.pt"` |
| `Ativa` | BIT | NN, DEFAULT 1 | Indica se a loja está operacional | `1` |

---

## Tabela: `Categorias`

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| `Id` | INT | PK, NN, AUTO_INCREMENT | Identificador único da categoria | `3` |
| `Nome` | NVARCHAR(100) | NN | Designação da categoria | `"Bebidas"` |
| `CategoriaPaiId` | INT | FK(Categorias.Id), NULL | ID da categoria pai (null = categoria raiz) | `NULL` |

---

## Tabela: `Produtos`

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| `Id` | INT | PK, NN, AUTO_INCREMENT | Identificador único do produto | `42` |
| `Codigo` | NVARCHAR(50) | NN, UK | Código único do produto (código de barras ou interno) | `"5601227012345"` |
| `Nome` | NVARCHAR(200) | NN | Designação comercial do produto | `"Água Mineral 500ml"` |
| `Descricao` | NVARCHAR(1000) | — | Descrição detalhada do produto | `"Água mineral natural sem gás"` |
| `PrecoCusto` | DECIMAL(18,2) | NN | Preço de custo (sem IVA) em euros | `0.25` |
| `PrecoBaseVenda` | DECIMAL(18,2) | NN | Preço de venda base (com IVA) em euros | `0.60` |
| `UnidadeMedida` | NVARCHAR(20) | NN, DEFAULT 'unidade' | Unidade de medida do produto | `"unidade"`, `"kg"`, `"litro"` |
| `Foto` | NVARCHAR(500) | NULL | Caminho relativo da imagem do produto | `"/img/produtos/agua500ml.jpg"` |
| `Ativo` | BIT | NN, DEFAULT 1 | Indica se o produto está disponível | `1` |
| `CategoriaId` | INT | FK(Categorias.Id), NN | Categoria à qual o produto pertence | `3` |

---

## Tabela: `Stocks`

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| `Id` | INT | PK, NN, AUTO_INCREMENT | Identificador único do registo de stock | `101` |
| `LojaId` | INT | FK(Lojas.Id), NN | Loja onde o stock existe | `1` |
| `ProdutoId` | INT | FK(Produtos.Id), NN | Produto ao qual o stock pertence | `42` |
| `Quantidade` | DECIMAL(18,3) | NN, DEFAULT 0 | Quantidade disponível em stock | `150.000` |
| `StockMinimo` | DECIMAL(18,3) | NN, DEFAULT 0 | Quantidade mínima; abaixo gera alerta | `20.000` |
| `PrecoVendaLocal` | DECIMAL(18,2) | NULL | Preço local da loja (sobrepõe PrecoBaseVenda) | `0.55` |

> **Restrição de unicidade:** `(LojaId, ProdutoId)` – cada produto aparece no máximo uma vez por loja.

---

## Tabela: `Utilizadores`

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| `Id` | NVARCHAR(36) | PK, NN | GUID do utilizador | `"a1b2c3d4-..."` |
| `Nome` | NVARCHAR(200) | NN | Nome completo do utilizador | `"Ana Rodrigues"` |
| `Email` | NVARCHAR(300) | NN, UK | Endereço de email (usado no login) | `"ana@quickmart.pt"` |
| `PasswordHash` | NVARCHAR(500) | NN | Hash BCrypt da password | `"$2a$12$..."` |
| `Papel` | NVARCHAR(20) | NN | Papel no sistema: `GestorCadeia`, `GerenteLoja`, `Funcionario` | `"GerenteLoja"` |
| `LojaId` | INT | FK(Lojas.Id), NULL | Loja associada (null = Gestor da Cadeia) | `1` |
| `Ativo` | BIT | NN, DEFAULT 1 | Indica se a conta está ativa | `1` |
| `CriadoEm` | DATETIME | NN, DEFAULT GETUTCDATE() | Data e hora de criação da conta | `2026-02-10 09:00:00` |

---

## Tabela: `Vendas`

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| `Id` | INT | PK, NN, AUTO_INCREMENT | Identificador único da venda | `5001` |
| `LojaId` | INT | FK(Lojas.Id), NN | Loja onde a venda foi realizada | `2` |
| `FuncionarioId` | NVARCHAR(36) | FK(Utilizadores.Id), NN | Funcionário que registou a venda | `"b2c3d4e5-..."` |
| `DataHora` | DATETIME | NN, DEFAULT GETUTCDATE() | Data e hora do registo da venda | `2026-02-24 14:35:00` |
| `SubTotal` | DECIMAL(18,2) | NN | Soma dos subtotais das linhas (antes de descontos globais) | `12.50` |
| `TotalDesconto` | DECIMAL(18,2) | NN, DEFAULT 0 | Total de descontos aplicados | `1.00` |
| `Total` | DECIMAL(18,2) | NN | Valor final pago pelo cliente | `11.50` |
| `Estado` | NVARCHAR(20) | NN, DEFAULT 'Concluida' | Estado da venda: `Concluida`, `Cancelada`, `Devolvida` | `"Concluida"` |
| `MotivoAnulacao` | NVARCHAR(500) | NULL | Motivo do cancelamento/devolução | `"Produto com defeito"` |

---

## Tabela: `LinhasVenda`

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| `Id` | INT | PK, NN, AUTO_INCREMENT | Identificador único da linha | `9001` |
| `VendaId` | INT | FK(Vendas.Id), NN | Venda à qual a linha pertence | `5001` |
| `ProdutoId` | INT | FK(Produtos.Id), NN | Produto vendido | `42` |
| `Quantidade` | DECIMAL(18,3) | NN, > 0 | Quantidade vendida | `3.000` |
| `PrecoUnitario` | DECIMAL(18,2) | NN, > 0 | Preço unitário aplicado no momento da venda | `0.60` |
| `Desconto` | DECIMAL(18,2) | NN, DEFAULT 0, >= 0 | Desconto em valor absoluto aplicado a esta linha | `0.10` |

---

## Tabela: `Fornecedores`

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| `Id` | INT | PK, NN, AUTO_INCREMENT | Identificador único do fornecedor | `7` |
| `Nome` | NVARCHAR(200) | NN | Nome ou designação social do fornecedor | `"Distribuidora Norte Lda."` |
| `NIF` | NVARCHAR(20) | NN | Número de identificação fiscal | `"509876543"` |
| `Morada` | NVARCHAR(300) | — | Morada do fornecedor | `"Rua Industrial, 10, Guimarães"` |
| `Telefone` | NVARCHAR(20) | — | Contacto telefónico | `"253456789"` |
| `Email` | NVARCHAR(200) | — | Email de encomendas | `"encomendas@distnorte.pt"` |
| `Ativo` | BIT | NN, DEFAULT 1 | Indica se o fornecedor está ativo | `1` |

---

## Tabela: `Encomendas`

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| `Id` | INT | PK, NN, AUTO_INCREMENT | Identificador único da encomenda | `301` |
| `LojaId` | INT | FK(Lojas.Id), NN | Loja que efetuou a encomenda | `3` |
| `FornecedorId` | INT | FK(Fornecedores.Id), NN | Fornecedor destinatário | `7` |
| `DataCriacao` | DATETIME | NN, DEFAULT GETUTCDATE() | Data/hora de criação da encomenda | `2026-02-20 09:00:00` |
| `DataRececao` | DATETIME | NULL | Data/hora de receção da encomenda | `2026-02-22 14:00:00` |
| `Estado` | NVARCHAR(20) | NN, DEFAULT 'Pendente' | Estado: `Pendente`, `Enviada`, `Rececionada`, `Cancelada` | `"Rececionada"` |
| `Observacoes` | NVARCHAR(1000) | — | Notas adicionais sobre a encomenda | `"Urgente – stock mínimo atingido"` |

---

## Tabela: `LinhasEncomenda`

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| `Id` | INT | PK, NN, AUTO_INCREMENT | Identificador único da linha | `1201` |
| `EncomendaId` | INT | FK(Encomendas.Id), NN | Encomenda à qual a linha pertence | `301` |
| `ProdutoId` | INT | FK(Produtos.Id), NN | Produto encomendado | `42` |
| `QuantidadePedida` | DECIMAL(18,3) | NN, > 0 | Quantidade solicitada ao fornecedor | `200.000` |
| `QuantidadeRecebida` | DECIMAL(18,3) | NULL, >= 0 | Quantidade efetivamente recebida (preenchido na receção) | `195.000` |

---

## Tabela: `Faturas`

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| `Id` | INT | PK, NN, AUTO_INCREMENT | Identificador único da fatura | `2001` |
| `Numero` | NVARCHAR(50) | NN, UK | Número da fatura (formato: FAT-{Loja}-{Ano}-{Seq}) | `"FAT-001-2026-00001"` |
| `LojaId` | INT | FK(Lojas.Id), NN | Loja que emitiu a fatura | `1` |
| `VendaId` | INT | FK(Vendas.Id), NULL | Venda associada (null = fatura manual) | `5001` |
| `NomeCliente` | NVARCHAR(200) | NN | Nome ou designação do cliente | `"Empresa XYZ Lda."` |
| `NIFCliente` | NVARCHAR(20) | NN | NIF do cliente | `"501234567"` |
| `MoradaCliente` | NVARCHAR(300) | — | Morada do cliente | `"Av. Central, 1, Porto"` |
| `DataEmissao` | DATETIME | NN, DEFAULT GETUTCDATE() | Data e hora de emissão | `2026-02-24 15:00:00` |
| `Total` | DECIMAL(18,2) | NN | Valor total da fatura | `11.50` |
| `Estado` | NVARCHAR(20) | NN, DEFAULT 'Emitida' | Estado: `Emitida`, `Paga`, `Anulada` | `"Emitida"` |

---

## Tabela: `Consolidacoes`

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| `Id` | INT | PK, NN, AUTO_INCREMENT | Identificador único do registo | `501` |
| `LojaId` | INT | FK(Lojas.Id), NN | Loja a que respeita a consolidação | `2` |
| `DataConsolidacao` | DATE | NN | Data do dia consolidado | `2026-02-23` |
| `DataHoraExecucao` | DATETIME | NN | Data/hora em que a consolidação foi executada | `2026-02-23 23:59:05` |
| `TotalVendas` | DECIMAL(18,2) | NN, DEFAULT 0 | Soma total de vendas do dia | `1250.75` |
| `NumeroTransacoes` | INT | NN, DEFAULT 0 | Número de transações de venda do dia | `87` |
| `TotalDescontos` | DECIMAL(18,2) | NN, DEFAULT 0 | Soma de descontos concedidos no dia | `45.20` |
| `Resultado` | NVARCHAR(20) | NN | Resultado: `Sucesso`, `Falha`, `Parcial` | `"Sucesso"` |
| `ErroDetalhes` | NVARCHAR(2000) | NULL | Mensagem de erro em caso de falha | `NULL` |

> **Restrição de unicidade:** `(LojaId, DataConsolidacao)` – uma consolidação por loja por dia.

---

## Tabela: `AjustesStock`

| Campo | Tipo | Restrições | Descrição | Exemplo |
|---|---|---|---|---|
| `Id` | INT | PK, NN, AUTO_INCREMENT | Identificador único do ajuste | `801` |
| `LojaId` | INT | FK(Lojas.Id), NN | Loja onde o ajuste foi efetuado | `1` |
| `ProdutoId` | INT | FK(Produtos.Id), NN | Produto ajustado | `42` |
| `UtilizadorId` | NVARCHAR(36) | FK(Utilizadores.Id), NN | Utilizador que efetuou o ajuste | `"c3d4e5f6-..."` |
| `Variacao` | DECIMAL(18,3) | NN | Variação aplicada (+entrada / -saída) | `-5.000` |
| `Motivo` | NVARCHAR(500) | NN | Motivo do ajuste (obrigatório) | `"Quebra – produto caído"` |
| `DataHora` | DATETIME | NN, DEFAULT GETUTCDATE() | Data e hora do ajuste | `2026-02-24 10:15:00` |

---

## SQL DDL – Script de Criação da Base de Dados

```sql
-- Base de Dados: SGCLC
-- SQL Server 2019+

CREATE DATABASE SGCLC;
GO
USE SGCLC;
GO

CREATE TABLE Lojas (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    Nome       NVARCHAR(100)     NOT NULL,
    Morada     NVARCHAR(300)     NOT NULL,
    Telefone   NVARCHAR(20),
    Email      NVARCHAR(200),
    Ativa      BIT               NOT NULL DEFAULT 1
);

CREATE TABLE Categorias (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    Nome           NVARCHAR(100)     NOT NULL,
    CategoriaPaiId INT               REFERENCES Categorias(Id)
);

CREATE TABLE Produtos (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    Codigo          NVARCHAR(50)      NOT NULL UNIQUE,
    Nome            NVARCHAR(200)     NOT NULL,
    Descricao       NVARCHAR(1000),
    PrecoCusto      DECIMAL(18,2)     NOT NULL,
    PrecoBaseVenda  DECIMAL(18,2)     NOT NULL,
    UnidadeMedida   NVARCHAR(20)      NOT NULL DEFAULT 'unidade',
    Foto            NVARCHAR(500),
    Ativo           BIT               NOT NULL DEFAULT 1,
    CategoriaId     INT               NOT NULL REFERENCES Categorias(Id)
);

CREATE TABLE Utilizadores (
    Id           NVARCHAR(36)  PRIMARY KEY,
    Nome         NVARCHAR(200) NOT NULL,
    Email        NVARCHAR(300) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    Papel        NVARCHAR(20)  NOT NULL,
    LojaId       INT           REFERENCES Lojas(Id),
    Ativo        BIT           NOT NULL DEFAULT 1,
    CriadoEm    DATETIME      NOT NULL DEFAULT GETUTCDATE()
);

CREATE TABLE Stocks (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    LojaId          INT           NOT NULL REFERENCES Lojas(Id),
    ProdutoId       INT           NOT NULL REFERENCES Produtos(Id),
    Quantidade      DECIMAL(18,3) NOT NULL DEFAULT 0,
    StockMinimo     DECIMAL(18,3) NOT NULL DEFAULT 0,
    PrecoVendaLocal DECIMAL(18,2),
    CONSTRAINT UQ_Stock_Loja_Produto UNIQUE (LojaId, ProdutoId)
);

CREATE TABLE Vendas (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    LojaId         INT           NOT NULL REFERENCES Lojas(Id),
    FuncionarioId  NVARCHAR(36)  NOT NULL REFERENCES Utilizadores(Id),
    DataHora       DATETIME      NOT NULL DEFAULT GETUTCDATE(),
    SubTotal       DECIMAL(18,2) NOT NULL,
    TotalDesconto  DECIMAL(18,2) NOT NULL DEFAULT 0,
    Total          DECIMAL(18,2) NOT NULL,
    Estado         NVARCHAR(20)  NOT NULL DEFAULT 'Concluida',
    MotivoAnulacao NVARCHAR(500)
);

CREATE TABLE LinhasVenda (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    VendaId       INT           NOT NULL REFERENCES Vendas(Id),
    ProdutoId     INT           NOT NULL REFERENCES Produtos(Id),
    Quantidade    DECIMAL(18,3) NOT NULL CHECK (Quantidade > 0),
    PrecoUnitario DECIMAL(18,2) NOT NULL CHECK (PrecoUnitario > 0),
    Desconto      DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (Desconto >= 0)
);

CREATE TABLE Fornecedores (
    Id       INT IDENTITY(1,1) PRIMARY KEY,
    Nome     NVARCHAR(200) NOT NULL,
    NIF      NVARCHAR(20)  NOT NULL,
    Morada   NVARCHAR(300),
    Telefone NVARCHAR(20),
    Email    NVARCHAR(200),
    Ativo    BIT           NOT NULL DEFAULT 1
);

CREATE TABLE Encomendas (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    LojaId       INT          NOT NULL REFERENCES Lojas(Id),
    FornecedorId INT          NOT NULL REFERENCES Fornecedores(Id),
    DataCriacao  DATETIME     NOT NULL DEFAULT GETUTCDATE(),
    DataRececao  DATETIME,
    Estado       NVARCHAR(20) NOT NULL DEFAULT 'Pendente',
    Observacoes  NVARCHAR(1000)
);

CREATE TABLE LinhasEncomenda (
    Id                  INT IDENTITY(1,1) PRIMARY KEY,
    EncomendaId         INT           NOT NULL REFERENCES Encomendas(Id),
    ProdutoId           INT           NOT NULL REFERENCES Produtos(Id),
    QuantidadePedida    DECIMAL(18,3) NOT NULL CHECK (QuantidadePedida > 0),
    QuantidadeRecebida  DECIMAL(18,3)
);

CREATE TABLE Faturas (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    Numero         NVARCHAR(50)  NOT NULL UNIQUE,
    LojaId         INT           NOT NULL REFERENCES Lojas(Id),
    VendaId        INT           REFERENCES Vendas(Id),
    NomeCliente    NVARCHAR(200) NOT NULL,
    NIFCliente     NVARCHAR(20)  NOT NULL,
    MoradaCliente  NVARCHAR(300),
    DataEmissao    DATETIME      NOT NULL DEFAULT GETUTCDATE(),
    Total          DECIMAL(18,2) NOT NULL,
    Estado         NVARCHAR(20)  NOT NULL DEFAULT 'Emitida'
);

CREATE TABLE LinhasFatura (
    Id               INT IDENTITY(1,1) PRIMARY KEY,
    FaturaId         INT           NOT NULL REFERENCES Faturas(Id),
    DescricaoProduto NVARCHAR(200) NOT NULL,
    Quantidade       DECIMAL(18,3) NOT NULL,
    PrecoUnitario    DECIMAL(18,2) NOT NULL,
    Desconto         DECIMAL(18,2) NOT NULL DEFAULT 0
);

CREATE TABLE Consolidacoes (
    Id                  INT IDENTITY(1,1) PRIMARY KEY,
    LojaId              INT           NOT NULL REFERENCES Lojas(Id),
    DataConsolidacao    DATE          NOT NULL,
    DataHoraExecucao    DATETIME      NOT NULL DEFAULT GETUTCDATE(),
    TotalVendas         DECIMAL(18,2) NOT NULL DEFAULT 0,
    NumeroTransacoes    INT           NOT NULL DEFAULT 0,
    TotalDescontos      DECIMAL(18,2) NOT NULL DEFAULT 0,
    Resultado           NVARCHAR(20)  NOT NULL,
    ErroDetalhes        NVARCHAR(2000),
    CONSTRAINT UQ_Consolidacao_Loja_Data UNIQUE (LojaId, DataConsolidacao)
);

CREATE TABLE AjustesStock (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    LojaId     INT           NOT NULL REFERENCES Lojas(Id),
    ProdutoId  INT           NOT NULL REFERENCES Produtos(Id),
    UtilizadorId NVARCHAR(36) NOT NULL REFERENCES Utilizadores(Id),
    Variacao   DECIMAL(18,3) NOT NULL,
    Motivo     NVARCHAR(500) NOT NULL,
    DataHora   DATETIME      NOT NULL DEFAULT GETUTCDATE()
);
```
