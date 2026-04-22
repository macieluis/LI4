using ConvenienceChain.Core.Enums;

namespace ConvenienceChain.Core.Entities;

/// <summary>Representa uma loja da cadeia.</summary>
public class Loja
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Morada { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Ativa { get; set; } = true;

    // Navegação
    public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    public ICollection<Venda> Vendas { get; set; } = new List<Venda>();
    public ICollection<Encomenda> Encomendas { get; set; } = new List<Encomenda>();
    public ICollection<Consolidacao> Consolidacoes { get; set; } = new List<Consolidacao>();
    public ICollection<Utilizador> Utilizadores { get; set; } = new List<Utilizador>();
}

/// <summary>Categoria hierárquica de produto.</summary>
public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int? CategoriaPaiId { get; set; }

    public Categoria? CategoriaPai { get; set; }
    public ICollection<Categoria> SubCategorias { get; set; } = new List<Categoria>();
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}

/// <summary>Produto do catálogo central.</summary>
public class Produto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty; // Código único
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal PrecoCusto { get; set; }
    public decimal PrecoBaseVenda { get; set; }
    public string UnidadeMedida { get; set; } = "unidade";
    public string? Foto { get; set; }
    public bool Ativo { get; set; } = true;
    public int CategoriaId { get; set; }
    /// <summary>Prazo de validade do produto (null = sem validade).</summary>
    public DateOnly? DataValidade { get; set; }

    public Categoria Categoria { get; set; } = null!;
    public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    public ICollection<LinhaVenda> LinhasVenda { get; set; } = new List<LinhaVenda>();
    public ICollection<LinhaEncomenda> LinhasEncomenda { get; set; } = new List<LinhaEncomenda>();
}

/// <summary>Stock de um produto numa loja específica.</summary>
public class Stock
{
    public int Id { get; set; }
    public int LojaId { get; set; }
    public int ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public decimal StockMinimo { get; set; } = 0;
    public decimal? PrecoVendaLocal { get; set; } // Sobrepõe PrecoBaseVenda se definido

    public Loja Loja { get; set; } = null!;
    public Produto Produto { get; set; } = null!;

    /// <summary>Indica se o stock está em alerta (abaixo do mínimo).</summary>
    public bool EmAlerta => Quantidade <= StockMinimo;

    /// <summary>Preço de venda efetivo (local se definido, base caso contrário).</summary>
    public decimal PrecoVendaEfetivo => PrecoVendaLocal ?? Produto?.PrecoBaseVenda ?? 0;
}

/// <summary>Venda efetuada num ponto de venda de uma loja.</summary>
public class Venda
{
    public int Id { get; set; }
    public int LojaId { get; set; }
    public string FuncionarioId { get; set; } = string.Empty;
    public DateTime DataHora { get; set; } = DateTime.UtcNow;
    public decimal SubTotal { get; set; }
    public decimal TotalDesconto { get; set; }
    public decimal Total { get; set; }
    public EstadoVenda Estado { get; set; } = EstadoVenda.Concluida;
    public string? MotivoAnulacao { get; set; }

    public Loja Loja { get; set; } = null!;
    public Utilizador Funcionario { get; set; } = null!;
    public ICollection<LinhaVenda> Linhas { get; set; } = new List<LinhaVenda>();
    public Fatura? Fatura { get; set; }
}

/// <summary>Linha de produto numa venda.</summary>
public class LinhaVenda
{
    public int Id { get; set; }
    public int VendaId { get; set; }
    public int ProdutoId { get; set; }
    public decimal Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Desconto { get; set; } = 0; // valor absoluto

    public decimal SubTotal => (Quantidade * PrecoUnitario) - Desconto;

    public Venda Venda { get; set; } = null!;
    public Produto Produto { get; set; } = null!;
}

/// <summary>Fornecedor de produtos para a cadeia.</summary>
public class Fornecedor
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string NIF { get; set; } = string.Empty;
    public string Morada { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;

    public ICollection<Encomenda> Encomendas { get; set; } = new List<Encomenda>();
}

/// <summary>Encomenda de reposição de uma loja a um fornecedor.</summary>
public class Encomenda
{
    public int Id { get; set; }
    public int LojaId { get; set; }
    public int FornecedorId { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataRececao { get; set; }
    public EstadoEncomenda Estado { get; set; } = EstadoEncomenda.Pendente;
    public string Observacoes { get; set; } = string.Empty;

    public Loja Loja { get; set; } = null!;
    public Fornecedor Fornecedor { get; set; } = null!;
    public ICollection<LinhaEncomenda> Linhas { get; set; } = new List<LinhaEncomenda>();
}

/// <summary>Linha de produto numa encomenda.</summary>
public class LinhaEncomenda
{
    public int Id { get; set; }
    public int EncomendaId { get; set; }
    public int ProdutoId { get; set; }
    public decimal QuantidadePedida { get; set; }
    public decimal? QuantidadeRecebida { get; set; }

    public Encomenda Encomenda { get; set; } = null!;
    public Produto Produto { get; set; } = null!;
}

/// <summary>Fatura emitida para um cliente empresarial.</summary>
public class Fatura
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty; // Sequencial único
    public int LojaId { get; set; }
    public int? VendaId { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public string NIFCliente { get; set; } = string.Empty;
    public string MoradaCliente { get; set; } = string.Empty;
    public DateTime DataEmissao { get; set; } = DateTime.UtcNow;
    public decimal Total { get; set; }
    public EstadoFatura Estado { get; set; } = EstadoFatura.Emitida;

    public Loja Loja { get; set; } = null!;
    public Venda? Venda { get; set; }
    public ICollection<LinhaFatura> Linhas { get; set; } = new List<LinhaFatura>();
}

/// <summary>Linha de produto numa fatura.</summary>
public class LinhaFatura
{
    public int Id { get; set; }
    public int FaturaId { get; set; }
    public string DescricaoProduto { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Desconto { get; set; }
    public decimal SubTotal => (Quantidade * PrecoUnitario) - Desconto;

    public Fatura Fatura { get; set; } = null!;
}

/// <summary>Resultado da consolidação diária de dados de uma loja.</summary>
public class Consolidacao
{
    public int Id { get; set; }
    public int LojaId { get; set; }
    public DateOnly DataConsolidacao { get; set; }
    public DateTime DataHoraExecucao { get; set; } = DateTime.UtcNow;
    public decimal TotalVendas { get; set; }
    public int NumeroTransacoes { get; set; }
    public decimal TotalDescontos { get; set; }
    public ResultadoConsolidacao Resultado { get; set; }
    public string? ErroDetalhes { get; set; }

    public Loja Loja { get; set; } = null!;
}

/// <summary>Registo de ajuste manual de stock.</summary>
public class AjusteStock
{
    public int Id { get; set; }
    public int LojaId { get; set; }
    public int ProdutoId { get; set; }
    public string UtilizadorId { get; set; } = string.Empty;
    public decimal Variacao { get; set; } // positivo = entrada, negativo = saída
    public string Motivo { get; set; } = string.Empty;
    public DateTime DataHora { get; set; } = DateTime.UtcNow;

    public Loja Loja { get; set; } = null!;
    public Produto Produto { get; set; } = null!;
}

/// <summary>Notificação enviada a um utilizador ou grupo (Gestores se DestinatarioId=null).</summary>
public class Notificacao
{
    public int Id { get; set; }
    public string? DestinatarioId { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public DateTime DataHora { get; set; } = DateTime.UtcNow;
    public bool Lida { get; set; } = false;
    public string Tipo { get; set; } = "Info"; // Info | Warning | Error
}

/// <summary>Utilizador do sistema com papel e loja associada.</summary>
public class Utilizador
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public PapelUtilizador Papel { get; set; }
    public int? LojaId { get; set; }
    public bool Ativo { get; set; } = true;
    public string? Telefone { get; set; }
    public string? Notas { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public Loja? Loja { get; set; }
}
