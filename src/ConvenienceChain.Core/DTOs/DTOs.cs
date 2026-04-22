using ConvenienceChain.Core.Enums;

namespace ConvenienceChain.Core.DTOs;

// --- Auth ---
public record LoginResultDto(string UserId, string Nome, string Email, PapelUtilizador Papel, int? LojaId);

// --- Produto ---
public record CreateProdutoDto(string Codigo, string Nome, string Descricao, decimal PrecoCusto,
    decimal PrecoBaseVenda, string UnidadeMedida, int CategoriaId, string? Foto);
public record UpdateProdutoDto(string Nome, string Descricao, decimal PrecoCusto,
    decimal PrecoBaseVenda, string UnidadeMedida, int CategoriaId, string? Foto);

// --- Stock ---
public record StockDto(int ProdutoId, string CodigoProduto, string NomeProduto, string Categoria,
    decimal Quantidade, decimal StockMinimo, decimal? PrecoVendaLocal, decimal PrecoBaseVenda,
    bool EmAlerta, DateOnly? DataValidade, int? DiasFimValidade);
public record StockAlertaDto(int LojaId, string NomeLoja, int ProdutoId, string NomeProduto,
    decimal Quantidade, decimal StockMinimo);

// --- Venda ---
public record LinhaVendaDto(int ProdutoId, decimal Quantidade, decimal PrecoUnitario, decimal Desconto);
public record CreateVendaDto(int LojaId, string FuncionarioId, IEnumerable<LinhaVendaDto> Linhas);
public record VendaDto(int Id, int LojaId, string NomeLoja, string NomeFuncionario,
    DateTime DataHora, decimal SubTotal, decimal TotalDesconto, decimal Total, string Estado,
    IEnumerable<LinhaVendaDto> Linhas);
public record DevolutionDto(int ProdutoId, decimal Quantidade, string Motivo);

// --- Encomenda ---
public record CreateEncomendaDto(int LojaId, int FornecedorId, IEnumerable<LinhaEncomendaDto> Linhas, string Observacoes);
public record LinhaEncomendaDto(int ProdutoId, decimal QuantidadePedida);
public record RecepcionarLinhaDto(int ProdutoId, decimal QuantidadeRecebida);
public record EncomendaDto(int Id, int LojaId, string NomeLoja, int FornecedorId, string NomeFornecedor,
    DateTime DataCriacao, DateTime? DataRececao, string Estado, IEnumerable<LinhaEncomendaDto> Linhas);

// --- Fornecedor ---
public record CreateFornecedorDto(string Nome, string NIF, string Morada, string Telefone, string Email);
public record UpdateFornecedorDto(string Nome, string Morada, string Telefone, string Email);

// --- Fatura ---
public record EmitirFaturaDto(int LojaId, int? VendaId, string NomeCliente, string NIFCliente,
    string MoradaCliente, IEnumerable<LinhaFaturaDto> Linhas);
public record LinhaFaturaDto(string Descricao, decimal Quantidade, decimal PrecoUnitario, decimal Desconto);
public record FaturaDto(int Id, string Numero, string NomeLoja, string NomeCliente, string NIFCliente,
    DateTime DataEmissao, decimal Total, string Estado);

// --- Consolidacao ---
public record ConsolidacaoDto(int Id, int LojaId, string NomeLoja, DateOnly DataConsolidacao,
    DateTime DataHoraExecucao, decimal TotalVendas, int NumeroTransacoes, decimal TotalDescontos, string Resultado, string? Erro);
public record ConsolidacaoResumoDto(int Sucessos, int Falhas, IEnumerable<int> LojasComFalha);

// --- Dashboard ---
public record DashboardLojaDto(int LojaId, string NomeLoja, decimal TotalVendasHoje,
    int NrTransacoesHoje, int AlertasStock, int EncomendaePendentes,
    IEnumerable<VendasDiaDto> UltimosSete);
public record VendasDiaDto(DateOnly Data, decimal Total, int NrTransacoes);

// --- Relatórios ---
public record RelatorioVendasDto(decimal TotalVendas, int NrTransacoes, decimal TicketMedio,
    IEnumerable<VendasPorCategoriaDto> PorCategoria, IEnumerable<ProdutoMaisVendidoDto> TopProdutos,
    IEnumerable<VendasPorDiaDto> PorDia);
public record VendasPorCategoriaDto(string Categoria, decimal Total, int Quantidade);
public record ProdutoMaisVendidoDto(string Produto, decimal QuantidadeTotal, decimal TotalVendas);
public record VendasPorDiaDto(DateOnly Data, decimal Total);

// --- Utilizador ---
public record CreateUtilizadorDto(string Nome, string Email, string Password, PapelUtilizador Papel, int? LojaId,
    string? Telefone = null, string? Notas = null);
public record UpdateUtilizadorDto(string Nome, PapelUtilizador Papel, int? LojaId,
    string? Telefone = null, string? Notas = null);
