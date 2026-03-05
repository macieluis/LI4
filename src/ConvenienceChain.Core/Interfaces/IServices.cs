using ConvenienceChain.Core.DTOs;
using ConvenienceChain.Core.Entities;

namespace ConvenienceChain.Core.Interfaces;

// ---------- Service Interfaces ----------

public interface IAuthService
{
    Task<LoginResultDto?> LoginAsync(string email, string password);
    Task<bool> ChangePasswordAsync(string userId, string novaSenha);
    Task LogoutAsync(string userId);
}

public interface IProdutoService
{
    Task<IEnumerable<Produto>> GetAllAsync();
    Task<IEnumerable<Produto>> SearchAsync(string query, int? categoriaId = null);
    Task<Produto?> GetByIdAsync(int id);
    Task<Produto> CreateAsync(CreateProdutoDto dto);
    Task UpdateAsync(int id, UpdateProdutoDto dto);
    Task DeactivateAsync(int id);
}

public interface IStockService
{
    Task<IEnumerable<StockDto>> GetStockLojaAsync(int lojaId);
    Task<IEnumerable<StockDto>> GetStockAllLojasAsync();
    Task<IEnumerable<StockAlertaDto>> GetAlertasAsync(int? lojaId = null);
    Task AjustarStockAsync(int lojaId, int produtoId, decimal variacao, string motivo, string userId);
    Task DefinirStockMinimoAsync(int lojaId, int produtoId, decimal minimo);
    Task DefinirPrecoLocalAsync(int lojaId, int produtoId, decimal? preco);
    Task<bool> CheckStockAsync(int lojaId, int produtoId, decimal quantidade);
    Task DeductStockAsync(int lojaId, IEnumerable<LinhaVendaDto> linhas);
    Task ReporStockAsync(int lojaId, IEnumerable<LinhaVendaDto> linhas);
}

public interface ISalesService
{
    Task<VendaDto?> GetByIdAsync(int id);
    Task<IEnumerable<VendaDto>> GetByLojaAsync(int lojaId, DateTime? de = null, DateTime? ate = null);
    Task<VendaDto> CreateSaleAsync(CreateVendaDto dto);
    Task<VendaDto> CancelSaleAsync(int vendaId, string motivo, string userId);
    Task<VendaDto> ProcessDevolucoesAsync(int vendaId, IEnumerable<DevolutionDto> devoluctions, string userId);
}

public interface IOrderService
{
    Task<IEnumerable<EncomendaDto>> GetByLojaAsync(int lojaId);
    Task<IEnumerable<EncomendaDto>> GetPendentesAsync();
    Task<EncomendaDto?> GetByIdAsync(int id);
    Task<EncomendaDto> CreateAsync(CreateEncomendaDto dto);
    Task<EncomendaDto> RecepcionarAsync(int encomendaId, IEnumerable<RecepcionarLinhaDto> linhas);
    Task CancelAsync(int encomendaId);
}

public interface IFornecedorService
{
    Task<IEnumerable<Fornecedor>> GetAllAsync();
    Task<Fornecedor?> GetByIdAsync(int id);
    Task<Fornecedor> CreateAsync(CreateFornecedorDto dto);
    Task UpdateAsync(int id, UpdateFornecedorDto dto);
    Task DeactivateAsync(int id);
}

public interface IFaturaService
{
    Task<IEnumerable<FaturaDto>> GetByLojaAsync(int lojaId, DateTime? de = null, DateTime? ate = null);
    Task<IEnumerable<FaturaDto>> GetAllFaturasAsync(DateTime? de = null, DateTime? ate = null);
    Task<FaturaDto?> GetByIdAsync(int id);
    Task<FaturaDto> EmitirAsync(EmitirFaturaDto dto);
    Task<byte[]> ExportPdfAsync(int faturaId);
}

public interface IConsolidacaoService
{
    Task ConsolidarTodasAsync(DateOnly data);
    Task<ConsolidacaoDto> ConsolidarLojaAsync(int lojaId, DateOnly data);
    Task<IEnumerable<ConsolidacaoDto>> GetHistoricoAsync(int? lojaId = null);
}

public interface IReportService
{
    Task<RelatorioVendasDto> GetRelatorioVendasAsync(int? lojaId, DateTime de, DateTime ate, int? categoriaId = null);
    Task<IEnumerable<DashboardLojaDto>> GetDashboardCentralAsync();
    Task<DashboardLojaDto> GetDashboardLojaAsync(int lojaId);
    Task<byte[]> ExportRelatorioVendasPdfAsync(int? lojaId, DateTime de, DateTime ate);
    Task<byte[]> ExportRelatorioVendasCsvAsync(int? lojaId, DateTime de, DateTime ate);
}

public interface IUtilizadorService
{
    Task<IEnumerable<Utilizador>> GetAllAsync();
    Task<IEnumerable<Utilizador>> GetByLojaAsync(int lojaId);
    Task<Utilizador?> GetByIdAsync(string id);
    Task<Utilizador> CreateAsync(CreateUtilizadorDto dto);
    Task UpdateAsync(string id, UpdateUtilizadorDto dto);
    Task DeactivateAsync(string id);
    Task ReactivateAsync(string id);
    Task DeleteAsync(string id);
    Task ResetPasswordAsync(string id, string novaPassword);
}
