using ConvenienceChain.Core.Interfaces;
using ConvenienceChain.Core.Services;
using ConvenienceChain.Data.Context;
using ConvenienceChain.Data.Repositories;
using ConvenienceChain.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Blazor Server ──────────────────────────────────────────────
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ── Base de Dados ──────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=sgclc.db"));

// ── Repositórios ───────────────────────────────────────────────
builder.Services.AddScoped<ILojaRepository, LojaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();
builder.Services.AddScoped<IEncomendaRepository, EncomendaRepository>();
builder.Services.AddScoped<IFornecedorRepository, FornecedorRepository>();
builder.Services.AddScoped<IFaturaRepository, FaturaRepository>();
builder.Services.AddScoped<IConsolidacaoRepository, ConsolidacaoRepository>();
builder.Services.AddScoped<IUtilizadorRepository, UtilizadorRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IAjusteStockRepository, AjusteStockRepository>();

// ── Serviços de Aplicação ─────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IFornecedorService, FornecedorService>();
builder.Services.AddScoped<IFaturaService, FaturaService>();
builder.Services.AddScoped<IConsolidacaoService, ConsolidacaoService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IUtilizadorService, UtilizadorService>();

// ── Sessão do utilizador ──────────────────────────────────────
builder.Services.AddSingleton<SessionService>();

// ── Job de Consolidação (background) ─────────────────────────
builder.Services.AddHostedService<ConsolidacaoBackgroundService>();

// ── Sessão HTTP ──────────────────────────────────────────────
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromHours(8);
    opt.Cookie.HttpOnly = true;
    opt.Cookie.IsEssential = true;
});

var app = builder.Build();

// ── Middleware ────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseAntiforgery();

// ── Seed da Base de Dados ─────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    await SeedData.InitializeAsync(db);
}

app.MapRazorComponents<ConvenienceChain.Web.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
