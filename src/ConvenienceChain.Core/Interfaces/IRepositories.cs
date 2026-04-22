using ConvenienceChain.Core.Entities;

namespace ConvenienceChain.Core.Interfaces;

// ---------- Repositories ----------

public interface ILojaRepository
{
    Task<IEnumerable<Loja>> GetAllActiveAsync();
    Task<Loja?> GetByIdAsync(int id);
    Task<Loja> AddAsync(Loja loja);
    Task UpdateAsync(Loja loja);
}

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> GetAllActiveAsync();
    Task<Produto?> GetByIdAsync(int id);
    Task<Produto?> GetByCodigoAsync(string codigo);
    Task<IEnumerable<Produto>> SearchAsync(string query);
    Task<IEnumerable<Produto>> GetByCategoriaAsync(int categoriaId);
    Task<Produto> AddAsync(Produto produto);
    Task UpdateAsync(Produto produto);
}

public interface IStockRepository
{
    Task<IEnumerable<Stock>> GetAllAsync();
    Task<IEnumerable<Stock>> GetByLojaAsync(int lojaId);
    Task<Stock?> GetAsync(int lojaId, int produtoId);
    Task<IEnumerable<Stock>> GetAlertasByLojaAsync(int lojaId);
    Task<IEnumerable<Stock>> GetAllAlertasAsync(); // Para gestor cadeia
    Task<Stock> AddAsync(Stock stock);
    Task UpdateAsync(Stock stock);
}

public interface IVendaRepository
{
    Task<IEnumerable<Venda>> GetByLojaAsync(int lojaId, DateTime? de = null, DateTime? ate = null);
    Task<Venda?> GetByIdAsync(int id);
    Task<decimal> GetTotalVendasDiaAsync(int lojaId, DateTime dia);
    Task<int> GetNumeroTransacoesDiaAsync(int lojaId, DateTime dia);
    Task<Venda> AddAsync(Venda venda);
    Task UpdateAsync(Venda venda);
}

public interface IEncomendaRepository
{
    Task<IEnumerable<Encomenda>> GetByLojaAsync(int lojaId);
    Task<IEnumerable<Encomenda>> GetPendentesAsync();
    Task<Encomenda?> GetByIdAsync(int id);
    Task<Encomenda> AddAsync(Encomenda encomenda);
    Task UpdateAsync(Encomenda encomenda);
}

public interface IFornecedorRepository
{
    Task<IEnumerable<Fornecedor>> GetAllActiveAsync();
    Task<Fornecedor?> GetByIdAsync(int id);
    Task<Fornecedor> AddAsync(Fornecedor fornecedor);
    Task UpdateAsync(Fornecedor fornecedor);
}

public interface IFaturaRepository
{
    Task<IEnumerable<Fatura>> GetByLojaAsync(int lojaId, DateTime? de = null, DateTime? ate = null);
    Task<IEnumerable<Fatura>> GetAllFaturasAsync(DateTime? de = null, DateTime? ate = null);
    Task<Fatura?> GetByIdAsync(int id);
    Task<string> GetNextNumeroAsync(int lojaId);
    Task<Fatura> AddAsync(Fatura fatura);
    Task UpdateAsync(Fatura fatura);
}

public interface IConsolidacaoRepository
{
    Task<IEnumerable<Consolidacao>> GetByLojaAsync(int lojaId);
    Task<Consolidacao?> GetAsync(int lojaId, DateOnly data);
    Task<IEnumerable<Consolidacao>> GetAllByDataAsync(DateOnly data);
    Task<Consolidacao> AddAsync(Consolidacao consolidacao);
    Task UpdateAsync(Consolidacao consolidacao);
}

public interface IUtilizadorRepository
{
    Task<IEnumerable<Utilizador>> GetAllAsync();
    Task<IEnumerable<Utilizador>> GetByLojaAsync(int lojaId);
    Task<Utilizador?> GetByIdAsync(string id);
    Task<Utilizador?> GetByEmailAsync(string email);
    Task<Utilizador> AddAsync(Utilizador utilizador);
    Task UpdateAsync(Utilizador utilizador);
    Task DeleteAsync(string id);
}

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllAsync();
    Task<Categoria?> GetByIdAsync(int id);
    Task<Categoria> AddAsync(Categoria categoria);
    Task UpdateAsync(Categoria categoria);
}

public interface IAjusteStockRepository
{
    Task<IEnumerable<AjusteStock>> GetByLojaAsync(int lojaId, DateTime? de = null, DateTime? ate = null);
    Task<AjusteStock> AddAsync(AjusteStock ajuste);
}

public interface INotificacaoRepository
{
    Task<IEnumerable<Notificacao>> GetNaoLidasParaUtilizadorAsync(string userId);
    Task<Notificacao?> GetByIdAsync(int id);
    Task<Notificacao> AddAsync(Notificacao notificacao);
    Task UpdateAsync(Notificacao notificacao);
}
