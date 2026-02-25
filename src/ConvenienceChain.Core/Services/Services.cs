using ConvenienceChain.Core.DTOs;
using ConvenienceChain.Core.Entities;
using ConvenienceChain.Core.Enums;
using ConvenienceChain.Core.Interfaces;

namespace ConvenienceChain.Core.Services;

/// <summary>Serviço de autenticação simples baseado em utilizadores da BD.</summary>
public class AuthService : IAuthService
{
    private readonly IUtilizadorRepository _repo;
    public AuthService(IUtilizadorRepository repo) => _repo = repo;

    public async Task<LoginResultDto?> LoginAsync(string email, string password)
    {
        var user = await _repo.GetByEmailAsync(email);
        if (user is null || !user.Ativo) return null;
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;
        return new LoginResultDto(user.Id, user.Nome, user.Email, user.Papel, user.LojaId);
    }

    public async Task<bool> ChangePasswordAsync(string userId, string novaSenha)
    {
        var user = await _repo.GetByIdAsync(userId);
        if (user is null) return false;
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(novaSenha);
        await _repo.UpdateAsync(user);
        return true;
    }

    public Task LogoutAsync(string userId) => Task.CompletedTask;
}

/// <summary>Serviço de gestão de produtos e catálogo.</summary>
public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _repo;
    public ProdutoService(IProdutoRepository repo) => _repo = repo;

    public async Task<IEnumerable<Produto>> GetAllAsync() => await _repo.GetAllActiveAsync();
    public async Task<IEnumerable<Produto>> SearchAsync(string query, int? categoriaId = null)
    {
        var results = await _repo.SearchAsync(query);
        if (categoriaId.HasValue)
            results = results.Where(p => p.CategoriaId == categoriaId.Value);
        return results;
    }
    public async Task<Produto?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

    public async Task<Produto> CreateAsync(CreateProdutoDto dto)
    {
        var existing = await _repo.GetByCodigoAsync(dto.Codigo);
        if (existing is not null) throw new InvalidOperationException($"Produto com código '{dto.Codigo}' já existe.");
        var produto = new Produto
        {
            Codigo = dto.Codigo, Nome = dto.Nome, Descricao = dto.Descricao,
            PrecoCusto = dto.PrecoCusto, PrecoBaseVenda = dto.PrecoBaseVenda,
            UnidadeMedida = dto.UnidadeMedida, CategoriaId = dto.CategoriaId, Foto = dto.Foto
        };
        return await _repo.AddAsync(produto);
    }

    public async Task UpdateAsync(int id, UpdateProdutoDto dto)
    {
        var produto = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Produto {id} não encontrado.");
        produto.Nome = dto.Nome; produto.Descricao = dto.Descricao;
        produto.PrecoCusto = dto.PrecoCusto; produto.PrecoBaseVenda = dto.PrecoBaseVenda;
        produto.UnidadeMedida = dto.UnidadeMedida; produto.CategoriaId = dto.CategoriaId;
        produto.Foto = dto.Foto;
        await _repo.UpdateAsync(produto);
    }
    public async Task DeactivateAsync(int id)
    {
        var p = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException();
        p.Ativo = false;
        await _repo.UpdateAsync(p);
    }
}

/// <summary>Serviço de gestão de stock por loja.</summary>
public class StockService : IStockService
{
    private readonly IStockRepository _stockRepo;
    private readonly IAjusteStockRepository _ajusteRepo;

    public StockService(IStockRepository stockRepo, IAjusteStockRepository ajusteRepo)
    {
        _stockRepo = stockRepo; _ajusteRepo = ajusteRepo;
    }

    public async Task<IEnumerable<StockDto>> GetStockLojaAsync(int lojaId)
    {
        var stocks = await _stockRepo.GetByLojaAsync(lojaId);
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        return stocks.Select(s =>
        {
            var validade = s.Produto.DataValidade;
            int? diasFim = validade.HasValue ? validade.Value.DayNumber - hoje.DayNumber : null;
            return new StockDto(
                s.ProdutoId, s.Produto.Codigo, s.Produto.Nome,
                s.Produto.Categoria?.Nome ?? "", s.Quantidade, s.StockMinimo,
                s.PrecoVendaLocal, s.Produto.PrecoBaseVenda, s.EmAlerta,
                validade, diasFim);
        });
    }

    public async Task<IEnumerable<StockAlertaDto>> GetAlertasAsync(int? lojaId = null)
    {
        IEnumerable<Stock> stocks = lojaId.HasValue
            ? await _stockRepo.GetAlertasByLojaAsync(lojaId.Value)
            : await _stockRepo.GetAllAlertasAsync();
        return stocks.Select(s => new StockAlertaDto(
            s.LojaId, s.Loja?.Nome ?? "", s.ProdutoId, s.Produto.Nome, s.Quantidade, s.StockMinimo));
    }

    public async Task AjustarStockAsync(int lojaId, int produtoId, decimal variacao, string motivo, string userId)
    {
        var stock = await _stockRepo.GetAsync(lojaId, produtoId);
        if (stock is null) throw new KeyNotFoundException("Stock não encontrado para este produto/loja.");
        stock.Quantidade += variacao;
        if (stock.Quantidade < 0) throw new InvalidOperationException("Stock não pode ficar negativo.");
        await _stockRepo.UpdateAsync(stock);
        await _ajusteRepo.AddAsync(new AjusteStock
        {
            LojaId = lojaId, ProdutoId = produtoId, UtilizadorId = userId,
            Variacao = variacao, Motivo = motivo
        });
    }

    public async Task DefinirStockMinimoAsync(int lojaId, int produtoId, decimal minimo)
    {
        var stock = await _stockRepo.GetAsync(lojaId, produtoId);
        if (stock is null) throw new KeyNotFoundException();
        stock.StockMinimo = minimo;
        await _stockRepo.UpdateAsync(stock);
    }

    public async Task DefinirPrecoLocalAsync(int lojaId, int produtoId, decimal? preco)
    {
        var stock = await _stockRepo.GetAsync(lojaId, produtoId);
        if (stock is null) throw new KeyNotFoundException();
        stock.PrecoVendaLocal = preco;
        await _stockRepo.UpdateAsync(stock);
    }

    public async Task<bool> CheckStockAsync(int lojaId, int produtoId, decimal quantidade)
    {
        var stock = await _stockRepo.GetAsync(lojaId, produtoId);
        return stock is not null && stock.Quantidade >= quantidade;
    }

    public async Task DeductStockAsync(int lojaId, IEnumerable<LinhaVendaDto> linhas)
    {
        foreach (var linha in linhas)
        {
            var stock = await _stockRepo.GetAsync(lojaId, linha.ProdutoId);
            if (stock is null) continue;
            stock.Quantidade -= linha.Quantidade;
            await _stockRepo.UpdateAsync(stock);
        }
    }

    public async Task ReporStockAsync(int lojaId, IEnumerable<LinhaVendaDto> linhas)
    {
        foreach (var linha in linhas)
        {
            var stock = await _stockRepo.GetAsync(lojaId, linha.ProdutoId);
            if (stock is null) continue;
            stock.Quantidade += linha.Quantidade;
            await _stockRepo.UpdateAsync(stock);
        }
    }
}

/// <summary>Serviço de gestão de vendas (POS).</summary>
public class SalesService : ISalesService
{
    private readonly IVendaRepository _vendaRepo;
    private readonly IStockService _stockSvc;

    public SalesService(IVendaRepository vendaRepo, IStockService stockSvc)
    {
        _vendaRepo = vendaRepo; _stockSvc = stockSvc;
    }

    public async Task<VendaDto?> GetByIdAsync(int id)
    {
        var v = await _vendaRepo.GetByIdAsync(id);
        return v is null ? null : MapToDto(v);
    }

    public async Task<IEnumerable<VendaDto>> GetByLojaAsync(int lojaId, DateTime? de = null, DateTime? ate = null)
    {
        var vendas = await _vendaRepo.GetByLojaAsync(lojaId, de, ate);
        return vendas.Select(MapToDto);
    }

    public async Task<VendaDto> CreateSaleAsync(CreateVendaDto dto)
    {
        // Verificar stock para todas as linhas
        foreach (var linha in dto.Linhas)
        {
            if (!await _stockSvc.CheckStockAsync(dto.LojaId, linha.ProdutoId, linha.Quantidade))
                throw new InvalidOperationException($"Stock insuficiente para o produto {linha.ProdutoId}.");
        }

        var subTotal = dto.Linhas.Sum(l => l.Quantidade * l.PrecoUnitario);
        var totalDesc = dto.Linhas.Sum(l => l.Desconto);

        var venda = new Venda
        {
            LojaId = dto.LojaId, FuncionarioId = dto.FuncionarioId,
            SubTotal = subTotal, TotalDesconto = totalDesc, Total = subTotal - totalDesc,
            Linhas = dto.Linhas.Select(l => new LinhaVenda
            {
                ProdutoId = l.ProdutoId, Quantidade = l.Quantidade,
                PrecoUnitario = l.PrecoUnitario, Desconto = l.Desconto
            }).ToList()
        };

        venda = await _vendaRepo.AddAsync(venda);

        // Deduzir stock
        await _stockSvc.DeductStockAsync(dto.LojaId, dto.Linhas);

        var result = await _vendaRepo.GetByIdAsync(venda.Id);
        return MapToDto(result!);
    }

    public async Task<VendaDto> CancelSaleAsync(int vendaId, string motivo, string userId)
    {
        var venda = await _vendaRepo.GetByIdAsync(vendaId) ?? throw new KeyNotFoundException();
        if (venda.Estado != EstadoVenda.Concluida) throw new InvalidOperationException("Venda já foi cancelada.");
        venda.Estado = EstadoVenda.Cancelada;
        venda.MotivoAnulacao = motivo;
        await _vendaRepo.UpdateAsync(venda);
        // Repor stock
        var linhasDto = venda.Linhas.Select(l => new LinhaVendaDto(l.ProdutoId, l.Quantidade, l.PrecoUnitario, l.Desconto));
        await _stockSvc.ReporStockAsync(venda.LojaId, linhasDto);
        return MapToDto(venda);
    }

    public async Task<VendaDto> ProcessDevolucoesAsync(int vendaId, IEnumerable<DevolutionDto> devolutions, string userId)
    {
        var venda = await _vendaRepo.GetByIdAsync(vendaId) ?? throw new KeyNotFoundException();
        var linhasDevolvidas = devolutions.Select(d => new LinhaVendaDto(d.ProdutoId, d.Quantidade, 0, 0));
        await _stockSvc.ReporStockAsync(venda.LojaId, linhasDevolvidas);
        venda.Estado = EstadoVenda.Devolvida;
        await _vendaRepo.UpdateAsync(venda);
        return MapToDto(venda);
    }

    private static VendaDto MapToDto(Venda v) => new(
        v.Id, v.LojaId, v.Loja?.Nome ?? "", v.Funcionario?.Nome ?? "",
        v.DataHora, v.SubTotal, v.TotalDesconto, v.Total, v.Estado.ToString(),
        v.Linhas.Select(l => new LinhaVendaDto(l.ProdutoId, l.Quantidade, l.PrecoUnitario, l.Desconto)));
}

/// <summary>Serviço de consolidação diária de dados.</summary>
public class ConsolidacaoService : IConsolidacaoService
{
    private readonly IConsolidacaoRepository _consolidacaoRepo;
    private readonly IVendaRepository _vendaRepo;
    private readonly ILojaRepository _lojaRepo;

    public ConsolidacaoService(IConsolidacaoRepository consolidacaoRepo,
        IVendaRepository vendaRepo, ILojaRepository lojaRepo)
    {
        _consolidacaoRepo = consolidacaoRepo;
        _vendaRepo = vendaRepo;
        _lojaRepo = lojaRepo;
    }

    public async Task ConsolidarTodasAsync(DateOnly data)
    {
        var lojas = await _lojaRepo.GetAllActiveAsync();
        foreach (var loja in lojas)
            await ConsolidarLojaAsync(loja.Id, data);
    }

    public async Task<ConsolidacaoDto> ConsolidarLojaAsync(int lojaId, DateOnly data)
    {
        var dia = data.ToDateTime(TimeOnly.MinValue);
        try
        {
            var totalVendas = await _vendaRepo.GetTotalVendasDiaAsync(lojaId, dia);
            var nrTransacoes = await _vendaRepo.GetNumeroTransacoesDiaAsync(lojaId, dia);

            // Remove consolidação prévia se existir e recalcula
            var existing = await _consolidacaoRepo.GetAsync(lojaId, data);
            if (existing is not null)
            {
                existing.TotalVendas = totalVendas;
                existing.NumeroTransacoes = nrTransacoes;
                existing.DataHoraExecucao = DateTime.UtcNow;
                existing.Resultado = ResultadoConsolidacao.Sucesso;
                await _consolidacaoRepo.UpdateAsync(existing);
                return MapToDto(existing);
            }

            var consolidacao = new Consolidacao
            {
                LojaId = lojaId, DataConsolidacao = data,
                TotalVendas = totalVendas, NumeroTransacoes = nrTransacoes,
                Resultado = ResultadoConsolidacao.Sucesso
            };
            consolidacao = await _consolidacaoRepo.AddAsync(consolidacao);
            return MapToDto(consolidacao);
        }
        catch (Exception ex)
        {
            var falha = new Consolidacao
            {
                LojaId = lojaId, DataConsolidacao = data,
                Resultado = ResultadoConsolidacao.Falha, ErroDetalhes = ex.Message
            };
            await _consolidacaoRepo.AddAsync(falha);
            return MapToDto(falha);
        }
    }

    public async Task<IEnumerable<ConsolidacaoDto>> GetHistoricoAsync(int? lojaId = null)
    {
        IEnumerable<Consolidacao> consolidacoes = lojaId.HasValue
            ? await _consolidacaoRepo.GetByLojaAsync(lojaId.Value)
            : await _consolidacaoRepo.GetAllByDataAsync(DateOnly.FromDateTime(DateTime.Today));
        return consolidacoes.Select(MapToDto);
    }

    private static ConsolidacaoDto MapToDto(Consolidacao c) => new(
        c.Id, c.LojaId, c.Loja?.Nome ?? "", c.DataConsolidacao, c.DataHoraExecucao,
        c.TotalVendas, c.NumeroTransacoes, c.TotalDescontos,
        c.Resultado.ToString(), c.ErroDetalhes);
}

/// <summary>Serviço de encomendas a fornecedores.</summary>
public class OrderService : IOrderService
{
    private readonly IEncomendaRepository _repo;
    private readonly IStockRepository _stockRepo;

    public OrderService(IEncomendaRepository repo, IStockRepository stockRepo)
    {
        _repo = repo; _stockRepo = stockRepo;
    }

    public async Task<IEnumerable<EncomendaDto>> GetByLojaAsync(int lojaId) =>
        (await _repo.GetByLojaAsync(lojaId)).Select(MapToDto);

    public async Task<IEnumerable<EncomendaDto>> GetPendentesAsync() =>
        (await _repo.GetPendentesAsync()).Select(MapToDto);

    public async Task<EncomendaDto?> GetByIdAsync(int id)
    {
        var e = await _repo.GetByIdAsync(id);
        return e is null ? null : MapToDto(e);
    }

    public async Task<EncomendaDto> CreateAsync(CreateEncomendaDto dto)
    {
        var enc = new Encomenda
        {
            LojaId = dto.LojaId, FornecedorId = dto.FornecedorId,
            Observacoes = dto.Observacoes,
            Linhas = dto.Linhas.Select(l => new LinhaEncomenda
            {
                ProdutoId = l.ProdutoId, QuantidadePedida = l.QuantidadePedida
            }).ToList()
        };
        enc = await _repo.AddAsync(enc);
        var result = await _repo.GetByIdAsync(enc.Id);
        return MapToDto(result!);
    }

    public async Task<EncomendaDto> RecepcionarAsync(int encomendaId, IEnumerable<RecepcionarLinhaDto> linhas)
    {
        var enc = await _repo.GetByIdAsync(encomendaId) ?? throw new KeyNotFoundException();
        enc.Estado = EstadoEncomenda.Rececionada;
        enc.DataRececao = DateTime.UtcNow;

        foreach (var linha in linhas)
        {
            var linhaEnc = enc.Linhas.FirstOrDefault(l => l.ProdutoId == linha.ProdutoId);
            if (linhaEnc is not null) linhaEnc.QuantidadeRecebida = linha.QuantidadeRecebida;

            // Atualizar stock
            var stock = await _stockRepo.GetAsync(enc.LojaId, linha.ProdutoId);
            if (stock is not null)
            {
                stock.Quantidade += linha.QuantidadeRecebida;
                await _stockRepo.UpdateAsync(stock);
            }
        }

        await _repo.UpdateAsync(enc);
        return MapToDto(enc);
    }

    public async Task CancelAsync(int encomendaId)
    {
        var enc = await _repo.GetByIdAsync(encomendaId) ?? throw new KeyNotFoundException();
        enc.Estado = EstadoEncomenda.Cancelada;
        await _repo.UpdateAsync(enc);
    }

    private static EncomendaDto MapToDto(Encomenda e) => new(
        e.Id, e.LojaId, e.Loja?.Nome ?? "", e.FornecedorId, e.Fornecedor?.Nome ?? "",
        e.DataCriacao, e.DataRececao, e.Estado.ToString(),
        e.Linhas.Select(l => new LinhaEncomendaDto(l.ProdutoId, l.QuantidadePedida)));
}

/// <summary>Serviço de relatórios e dashboard.</summary>
public class ReportService : IReportService
{
    private readonly IVendaRepository _vendaRepo;
    private readonly ILojaRepository _lojaRepo;
    private readonly IStockRepository _stockRepo;
    private readonly IEncomendaRepository _encomendaRepo;

    public ReportService(IVendaRepository vendaRepo, ILojaRepository lojaRepo,
        IStockRepository stockRepo, IEncomendaRepository encomendaRepo)
    {
        _vendaRepo = vendaRepo; _lojaRepo = lojaRepo;
        _stockRepo = stockRepo; _encomendaRepo = encomendaRepo;
    }

    public async Task<DashboardLojaDto> GetDashboardLojaAsync(int lojaId)
    {
        var loja = await _lojaRepo.GetByIdAsync(lojaId);
        var hoje = DateTime.Today;
        var totalHoje = await _vendaRepo.GetTotalVendasDiaAsync(lojaId, hoje);
        var nrHoje = await _vendaRepo.GetNumeroTransacoesDiaAsync(lojaId, hoje);
        var alertas = (await _stockRepo.GetAlertasByLojaAsync(lojaId)).Count();
        var encPendentes = (await _encomendaRepo.GetByLojaAsync(lojaId))
            .Count(e => e.Estado == EstadoEncomenda.Pendente);

        // Últimos 7 dias
        var ultimos7 = new List<VendasDiaDto>();
        for (int i = 6; i >= 0; i--)
        {
            var dia = DateTime.Today.AddDays(-i);
            var total = await _vendaRepo.GetTotalVendasDiaAsync(lojaId, dia);
            var nr = await _vendaRepo.GetNumeroTransacoesDiaAsync(lojaId, dia);
            ultimos7.Add(new VendasDiaDto(DateOnly.FromDateTime(dia), total, nr));
        }

        return new DashboardLojaDto(lojaId, loja?.Nome ?? "", totalHoje, nrHoje, alertas, encPendentes, ultimos7);
    }

    public async Task<IEnumerable<DashboardLojaDto>> GetDashboardCentralAsync()
    {
        var lojas = await _lojaRepo.GetAllActiveAsync();
        var result = new List<DashboardLojaDto>();
        foreach (var loja in lojas)
            result.Add(await GetDashboardLojaAsync(loja.Id));
        return result;
    }

    public async Task<RelatorioVendasDto> GetRelatorioVendasAsync(int? lojaId, DateTime de, DateTime ate, int? categoriaId = null)
    {
        IEnumerable<Core.Entities.Venda> vendas;
        if (lojaId.HasValue)
            vendas = await _vendaRepo.GetByLojaAsync(lojaId.Value, de, ate);
        else
        {
            var lojas = await _lojaRepo.GetAllActiveAsync();
            var todasVendas = new List<Core.Entities.Venda>();
            foreach (var l in lojas)
                todasVendas.AddRange(await _vendaRepo.GetByLojaAsync(l.Id, de, ate));
            vendas = todasVendas;
        }

        vendas = vendas.Where(v => v.Estado == EstadoVenda.Concluida);

        var totalVendas = vendas.Sum(v => v.Total);
        var nrTransacoes = vendas.Count();
        var ticketMedio = nrTransacoes > 0 ? totalVendas / nrTransacoes : 0;

        var porDia = vendas
            .GroupBy(v => DateOnly.FromDateTime(v.DataHora))
            .Select(g => new VendasPorDiaDto(g.Key, g.Sum(v => v.Total)))
            .OrderBy(x => x.Data).ToList();

        var todasLinhas = vendas.SelectMany(v => v.Linhas).ToList();

        var porCategoria = todasLinhas
            .GroupBy(l => l.Produto?.Categoria?.Nome ?? "Sem categoria")
            .Select(g => new VendasPorCategoriaDto(g.Key, g.Sum(l => l.SubTotal), (int)g.Sum(l => l.Quantidade)))
            .OrderByDescending(x => x.Total).ToList();

        var topProdutos = todasLinhas
            .GroupBy(l => l.Produto?.Nome ?? "Desconhecido")
            .Select(g => new ProdutoMaisVendidoDto(g.Key, g.Sum(l => l.Quantidade), g.Sum(l => l.SubTotal)))
            .OrderByDescending(x => x.TotalVendas).Take(10).ToList();

        return new RelatorioVendasDto(totalVendas, nrTransacoes, ticketMedio, porCategoria, topProdutos, porDia);
    }

    public Task<byte[]> ExportRelatorioVendasPdfAsync(int? lojaId, DateTime de, DateTime ate) =>
        Task.FromResult(Array.Empty<byte>()); // Implementar com QuestPDF

    public Task<byte[]> ExportRelatorioVendasCsvAsync(int? lojaId, DateTime de, DateTime ate) =>
        Task.FromResult(Array.Empty<byte>()); // Implementar com CsvHelper
}

/// <summary>Serviço de gestão de fornecedores.</summary>
public class FornecedorService : IFornecedorService
{
    private readonly IFornecedorRepository _repo;
    public FornecedorService(IFornecedorRepository repo) => _repo = repo;

    public async Task<IEnumerable<Fornecedor>> GetAllAsync() => await _repo.GetAllActiveAsync();
    public async Task<Fornecedor?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

    public async Task<Fornecedor> CreateAsync(CreateFornecedorDto dto)
    {
        var f = new Fornecedor { Nome = dto.Nome, NIF = dto.NIF, Telefone = dto.Telefone, Email = dto.Email };
        return await _repo.AddAsync(f);
    }

    public async Task UpdateAsync(int id, UpdateFornecedorDto dto)
    {
        var f = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException();
        f.Nome = dto.Nome; f.Telefone = dto.Telefone; f.Email = dto.Email;
        await _repo.UpdateAsync(f);
    }

    public async Task DeactivateAsync(int id)
    {
        var f = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException();
        f.Ativo = false;
        await _repo.UpdateAsync(f);
    }
}

/// <summary>Serviço de gestão de utilizadores.</summary>
public class UtilizadorService : IUtilizadorService
{
    private readonly IUtilizadorRepository _repo;
    public UtilizadorService(IUtilizadorRepository repo) => _repo = repo;

    public async Task<IEnumerable<Utilizador>> GetAllAsync() => await _repo.GetAllAsync();
    public async Task<Utilizador?> GetByIdAsync(string id) => await _repo.GetByIdAsync(id);

    public async Task<Utilizador> CreateAsync(CreateUtilizadorDto dto)
    {
        var u = new Utilizador
        {
            Id = Guid.NewGuid().ToString(), Nome = dto.Nome, Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Papel = dto.Papel, LojaId = dto.LojaId
        };
        return await _repo.AddAsync(u);
    }

    public async Task UpdateAsync(string id, UpdateUtilizadorDto dto)
    {
        var u = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException();
        u.Nome = dto.Nome; u.Papel = dto.Papel; u.LojaId = dto.LojaId;
        await _repo.UpdateAsync(u);
    }

    public async Task DeactivateAsync(string id)
    {
        var u = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException();
        u.Ativo = false;
        await _repo.UpdateAsync(u);
    }
}

/// <summary>Serviço de faturação.</summary>
public class FaturaService : IFaturaService
{
    private readonly IFaturaRepository _repo;
    public FaturaService(IFaturaRepository repo) => _repo = repo;

    public async Task<IEnumerable<FaturaDto>> GetByLojaAsync(int lojaId, DateTime? de = null, DateTime? ate = null)
    {
        var faturas = await _repo.GetByLojaAsync(lojaId, de, ate);
        return faturas.Select(MapToDto);
    }

    public async Task<FaturaDto?> GetByIdAsync(int id)
    {
        var f = await _repo.GetByIdAsync(id);
        return f is null ? null : MapToDto(f);
    }

    public async Task<FaturaDto> EmitirAsync(EmitirFaturaDto dto)
    {
        var numero = await _repo.GetNextNumeroAsync(dto.LojaId);
        var fatura = new Fatura
        {
            Numero = numero, LojaId = dto.LojaId, VendaId = dto.VendaId,
            NomeCliente = dto.NomeCliente, NIFCliente = dto.NIFCliente,
            MoradaCliente = dto.MoradaCliente,
            Linhas = dto.Linhas.Select(l => new LinhaFatura
            {
                DescricaoProduto = l.Descricao, Quantidade = l.Quantidade,
                PrecoUnitario = l.PrecoUnitario, Desconto = l.Desconto
            }).ToList()
        };
        fatura.Total = fatura.Linhas.Sum(l => l.Quantidade * l.PrecoUnitario - l.Desconto);
        fatura = await _repo.AddAsync(fatura);
        return MapToDto(fatura);
    }

    public Task<byte[]> ExportPdfAsync(int faturaId) =>
        Task.FromResult(Array.Empty<byte>()); // Implementar com QuestPDF

    private static FaturaDto MapToDto(Fatura f) => new(
        f.Id, f.Numero, f.Loja?.Nome ?? "", f.NomeCliente ?? "", f.NIFCliente ?? "",
        f.DataEmissao, f.Total, f.Estado.ToString());
}
