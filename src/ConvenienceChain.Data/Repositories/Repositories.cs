using ConvenienceChain.Core.Entities;
using ConvenienceChain.Core.Interfaces;
using ConvenienceChain.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ConvenienceChain.Data.Repositories;

public class LojaRepository : ILojaRepository
{
    private readonly AppDbContext _db;
    public LojaRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Loja>> GetAllActiveAsync() =>
        await _db.Lojas.Where(l => l.Ativa).ToListAsync();
    public async Task<Loja?> GetByIdAsync(int id) =>
        await _db.Lojas.FindAsync(id);
    public async Task<Loja> AddAsync(Loja loja)
    {
        _db.Lojas.Add(loja);
        await _db.SaveChangesAsync();
        return loja;
    }
    public async Task UpdateAsync(Loja loja)
    {
        _db.Lojas.Update(loja);
        await _db.SaveChangesAsync();
    }
}

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _db;
    public ProdutoRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Produto>> GetAllActiveAsync() =>
        await _db.Produtos.Include(p => p.Categoria).Where(p => p.Ativo).ToListAsync();
    public async Task<Produto?> GetByIdAsync(int id) =>
        await _db.Produtos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);
    public async Task<Produto?> GetByCodigoAsync(string codigo) =>
        await _db.Produtos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Codigo == codigo);
    public async Task<IEnumerable<Produto>> SearchAsync(string query) {
        var q = Normalizar(query.Trim());
        var all = await _db.Produtos.Include(p => p.Categoria)
            .Where(p => p.Ativo)
            .ToListAsync();
        return all.Where(p =>
            Normalizar(p.Nome).Contains(q) ||
            Normalizar(p.Codigo).Contains(q) ||
            (p.Categoria != null && Normalizar(p.Categoria.Nome).Contains(q)));
    }

    /// <summary>Remove acentos e normaliza para lowercase. "café" → "cafe", "Ação" → "acao".</summary>
    private static string Normalizar(string s) =>
        new string(s.Normalize(System.Text.NormalizationForm.FormD)
            .Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c)
                        != System.Globalization.UnicodeCategory.NonSpacingMark)
            .ToArray())
        .ToLowerInvariant();
    public async Task<IEnumerable<Produto>> GetByCategoriaAsync(int categoriaId) =>
        await _db.Produtos.Include(p => p.Categoria)
            .Where(p => p.Ativo && p.CategoriaId == categoriaId)
            .ToListAsync();
    public async Task<Produto> AddAsync(Produto produto)
    {
        _db.Produtos.Add(produto);
        await _db.SaveChangesAsync();
        return produto;
    }
    public async Task UpdateAsync(Produto produto)
    {
        _db.Produtos.Update(produto);
        await _db.SaveChangesAsync();
    }
}

public class StockRepository : IStockRepository
{
    private readonly AppDbContext _db;
    public StockRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Stock>> GetAllAsync() =>
        await _db.Stocks.Include(s => s.Produto).ThenInclude(p => p.Categoria)
            .Include(s => s.Loja).ToListAsync();
    public async Task<IEnumerable<Stock>> GetByLojaAsync(int lojaId) =>
        await _db.Stocks.Include(s => s.Produto).ThenInclude(p => p.Categoria)
            .Where(s => s.LojaId == lojaId).ToListAsync();
    public async Task<Stock?> GetAsync(int lojaId, int produtoId) =>
        await _db.Stocks.Include(s => s.Produto)
            .FirstOrDefaultAsync(s => s.LojaId == lojaId && s.ProdutoId == produtoId);
    public async Task<IEnumerable<Stock>> GetAlertasByLojaAsync(int lojaId) =>
        await _db.Stocks.Include(s => s.Produto)
            .Where(s => s.LojaId == lojaId && s.Quantidade <= s.StockMinimo).ToListAsync();
    public async Task<IEnumerable<Stock>> GetAllAlertasAsync() =>
        await _db.Stocks.Include(s => s.Produto).Include(s => s.Loja)
            .Where(s => s.Quantidade <= s.StockMinimo).ToListAsync();
    public async Task<Stock> AddAsync(Stock stock)
    {
        _db.Stocks.Add(stock);
        await _db.SaveChangesAsync();
        return stock;
    }
    public async Task UpdateAsync(Stock stock)
    {
        _db.Stocks.Update(stock);
        await _db.SaveChangesAsync();
    }
}

public class VendaRepository : IVendaRepository
{
    private readonly AppDbContext _db;
    public VendaRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Venda>> GetByLojaAsync(int lojaId, DateTime? de = null, DateTime? ate = null)
    {
        var query = _db.Vendas
            .Include(v => v.Linhas).ThenInclude(l => l.Produto).ThenInclude(p => p.Categoria)
            .Include(v => v.Funcionario)
            .Include(v => v.Loja)
            .Where(v => v.LojaId == lojaId);
        if (de.HasValue) query = query.Where(v => v.DataHora >= de.Value);
        if (ate.HasValue) query = query.Where(v => v.DataHora <= ate.Value);
        return await query.OrderByDescending(v => v.DataHora).ToListAsync();
    }
    public async Task<Venda?> GetByIdAsync(int id) =>
        await _db.Vendas.Include(v => v.Linhas).ThenInclude(l => l.Produto)
            .Include(v => v.Funcionario).Include(v => v.Loja)
            .FirstOrDefaultAsync(v => v.Id == id);
    public async Task<decimal> GetTotalVendasDiaAsync(int lojaId, DateTime dia)
    {
        var inicio = dia.Date;
        var fim = inicio.AddDays(1);
        var vendas = await _db.Vendas
            .Where(v => v.LojaId == lojaId &&
                v.DataHora >= inicio && v.DataHora < fim &&
                v.Estado == Core.Enums.EstadoVenda.Concluida)
            .ToListAsync();
        return vendas.Sum(v => v.Total);
    }
    public async Task<int> GetNumeroTransacoesDiaAsync(int lojaId, DateTime dia)
    {
        var inicio = dia.Date;
        var fim = inicio.AddDays(1);
        var vendas = await _db.Vendas
            .Where(v => v.LojaId == lojaId &&
                v.DataHora >= inicio && v.DataHora < fim &&
                v.Estado == Core.Enums.EstadoVenda.Concluida)
            .ToListAsync();
        return vendas.Count;
    }
    public async Task<Venda> AddAsync(Venda venda)
    {
        _db.Vendas.Add(venda);
        await _db.SaveChangesAsync();
        return venda;
    }
    public async Task UpdateAsync(Venda venda)
    {
        _db.Vendas.Update(venda);
        await _db.SaveChangesAsync();
    }
}

public class EncomendaRepository : IEncomendaRepository
{
    private readonly AppDbContext _db;
    public EncomendaRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Encomenda>> GetByLojaAsync(int lojaId) =>
        await _db.Encomendas.Include(e => e.Fornecedor).Include(e => e.Linhas).ThenInclude(l => l.Produto)
            .Where(e => e.LojaId == lojaId).OrderByDescending(e => e.DataCriacao).ToListAsync();
    public async Task<IEnumerable<Encomenda>> GetPendentesAsync() =>
        await _db.Encomendas.Include(e => e.Loja).Include(e => e.Fornecedor)
            .Where(e => e.Estado == Core.Enums.EstadoEncomenda.Pendente).ToListAsync();
    public async Task<Encomenda?> GetByIdAsync(int id) =>
        await _db.Encomendas.Include(e => e.Fornecedor).Include(e => e.Loja)
            .Include(e => e.Linhas).ThenInclude(l => l.Produto)
            .FirstOrDefaultAsync(e => e.Id == id);
    public async Task<Encomenda> AddAsync(Encomenda encomenda)
    {
        _db.Encomendas.Add(encomenda);
        await _db.SaveChangesAsync();
        return encomenda;
    }
    public async Task UpdateAsync(Encomenda encomenda)
    {
        _db.Encomendas.Update(encomenda);
        await _db.SaveChangesAsync();
    }
}

public class FornecedorRepository : IFornecedorRepository
{
    private readonly AppDbContext _db;
    public FornecedorRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Fornecedor>> GetAllActiveAsync() =>
        await _db.Fornecedores.Where(f => f.Ativo).ToListAsync();
    public async Task<Fornecedor?> GetByIdAsync(int id) => await _db.Fornecedores.FindAsync(id);
    public async Task<Fornecedor> AddAsync(Fornecedor fornecedor)
    {
        _db.Fornecedores.Add(fornecedor);
        await _db.SaveChangesAsync();
        return fornecedor;
    }
    public async Task UpdateAsync(Fornecedor fornecedor)
    {
        _db.Fornecedores.Update(fornecedor);
        await _db.SaveChangesAsync();
    }
}

public class UtilizadorRepository : IUtilizadorRepository
{
    private readonly AppDbContext _db;
    public UtilizadorRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Utilizador>> GetAllAsync() =>
        await _db.Utilizadores.Include(u => u.Loja).ToListAsync();
    public async Task<IEnumerable<Utilizador>> GetByLojaAsync(int lojaId) =>
        await _db.Utilizadores.Include(u => u.Loja)
            .Where(u => u.LojaId == lojaId && u.Papel == Core.Enums.PapelUtilizador.Funcionario)
            .ToListAsync();
    public async Task<Utilizador?> GetByIdAsync(string id) =>
        await _db.Utilizadores.Include(u => u.Loja).FirstOrDefaultAsync(u => u.Id == id);
    public async Task<Utilizador?> GetByEmailAsync(string email) =>
        await _db.Utilizadores.FirstOrDefaultAsync(u => u.Email == email);
    public async Task<Utilizador> AddAsync(Utilizador u)
    {
        _db.Utilizadores.Add(u);
        await _db.SaveChangesAsync();
        return u;
    }
    public async Task UpdateAsync(Utilizador u)
    {
        _db.Utilizadores.Update(u);
        await _db.SaveChangesAsync();
    }
    public async Task DeleteAsync(string id)
    {
        var u = await _db.Utilizadores.FindAsync(id);
        if (u is not null) { _db.Utilizadores.Remove(u); await _db.SaveChangesAsync(); }
    }
}

public class ConsolidacaoRepository : IConsolidacaoRepository
{
    private readonly AppDbContext _db;
    public ConsolidacaoRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Consolidacao>> GetByLojaAsync(int lojaId) =>
        await _db.Consolidacoes.Where(c => c.LojaId == lojaId)
            .OrderByDescending(c => c.DataConsolidacao).ToListAsync();
    public async Task<Consolidacao?> GetAsync(int lojaId, DateOnly data) =>
        await _db.Consolidacoes.FirstOrDefaultAsync(c => c.LojaId == lojaId && c.DataConsolidacao == data);
    public async Task<IEnumerable<Consolidacao>> GetAllByDataAsync(DateOnly data) =>
        await _db.Consolidacoes.Include(c => c.Loja)
            .Where(c => c.DataConsolidacao == data).ToListAsync();
    public async Task<Consolidacao> AddAsync(Consolidacao c)
    {
        _db.Consolidacoes.Add(c);
        await _db.SaveChangesAsync();
        return c;
    }
    public async Task UpdateAsync(Consolidacao c)
    {
        _db.Consolidacoes.Update(c);
        await _db.SaveChangesAsync();
    }
}

public class FaturaRepository : IFaturaRepository
{
    private readonly AppDbContext _db;
    public FaturaRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Fatura>> GetByLojaAsync(int lojaId, DateTime? de = null, DateTime? ate = null)
    {
        var q = _db.Faturas.Include(f => f.Loja).Where(f => f.LojaId == lojaId);
        if (de.HasValue) q = q.Where(f => f.DataEmissao >= de.Value);
        if (ate.HasValue) q = q.Where(f => f.DataEmissao <= ate.Value);
        return await q.OrderByDescending(f => f.DataEmissao).ToListAsync();
    }
    public async Task<IEnumerable<Fatura>> GetAllFaturasAsync(DateTime? de = null, DateTime? ate = null)
    {
        var q = _db.Faturas.Include(f => f.Loja).AsQueryable();
        if (de.HasValue) q = q.Where(f => f.DataEmissao >= de.Value);
        if (ate.HasValue) q = q.Where(f => f.DataEmissao <= ate.Value);
        return await q.OrderByDescending(f => f.DataEmissao).ToListAsync();
    }
    public async Task<Fatura?> GetByIdAsync(int id) =>
        await _db.Faturas.Include(f => f.Linhas).Include(f => f.Loja).FirstOrDefaultAsync(f => f.Id == id);
    public async Task<string> GetNextNumeroAsync(int lojaId)
    {
        var count = await _db.Faturas.CountAsync(f => f.LojaId == lojaId);
        return $"FAT-{lojaId:D3}-{DateTime.Now.Year}-{(count + 1):D5}";
    }
    public async Task<Fatura> AddAsync(Fatura f)
    {
        _db.Faturas.Add(f);
        await _db.SaveChangesAsync();
        return f;
    }
    public async Task UpdateAsync(Fatura f)
    {
        _db.Faturas.Update(f);
        await _db.SaveChangesAsync();
    }
}

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _db;
    public CategoriaRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Categoria>> GetAllAsync() =>
        await _db.Categorias.Include(c => c.SubCategorias).ToListAsync();
    public async Task<Categoria?> GetByIdAsync(int id) => await _db.Categorias.FindAsync(id);
    public async Task<Categoria> AddAsync(Categoria c)
    {
        _db.Categorias.Add(c);
        await _db.SaveChangesAsync();
        return c;
    }
    public async Task UpdateAsync(Categoria c)
    {
        _db.Categorias.Update(c);
        await _db.SaveChangesAsync();
    }
}

public class AjusteStockRepository : IAjusteStockRepository
{
    private readonly AppDbContext _db;
    public AjusteStockRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<AjusteStock>> GetByLojaAsync(int lojaId, DateTime? de = null, DateTime? ate = null)
    {
        var q = _db.AjustesStock.Include(a => a.Produto).Where(a => a.LojaId == lojaId);
        if (de.HasValue) q = q.Where(a => a.DataHora >= de.Value);
        if (ate.HasValue) q = q.Where(a => a.DataHora <= ate.Value);
        return await q.OrderByDescending(a => a.DataHora).ToListAsync();
    }
    public async Task<AjusteStock> AddAsync(AjusteStock a)
    {
        _db.AjustesStock.Add(a);
        await _db.SaveChangesAsync();
        return a;
    }
}
