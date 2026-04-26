using ConvenienceChain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConvenienceChain.Data.Context;

/// <summary>DbContext principal da aplicação SGCLC.</summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Loja> Lojas => Set<Loja>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<Venda> Vendas => Set<Venda>();
    public DbSet<LinhaVenda> LinhasVenda => Set<LinhaVenda>();
    public DbSet<Fornecedor> Fornecedores => Set<Fornecedor>();
    public DbSet<Encomenda> Encomendas => Set<Encomenda>();
    public DbSet<LinhaEncomenda> LinhasEncomenda => Set<LinhaEncomenda>();
    public DbSet<Fatura> Faturas => Set<Fatura>();
    public DbSet<LinhaFatura> LinhasFatura => Set<LinhaFatura>();
    public DbSet<Consolidacao> Consolidacoes => Set<Consolidacao>();
    public DbSet<AjusteStock> AjustesStock => Set<AjusteStock>();
    public DbSet<Utilizador> Utilizadores => Set<Utilizador>();
    public DbSet<Notificacao> Notificacoes => Set<Notificacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Loja ---
        modelBuilder.Entity<Loja>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Nome).HasMaxLength(100).IsRequired();
            e.Property(x => x.Email).HasMaxLength(200);
        });

        // --- Categoria ---
        modelBuilder.Entity<Categoria>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Nome).HasMaxLength(100).IsRequired();
            e.HasOne(x => x.CategoriaPai)
             .WithMany(x => x.SubCategorias)
             .HasForeignKey(x => x.CategoriaPaiId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // --- Produto ---
        modelBuilder.Entity<Produto>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Codigo).IsUnique();
            e.Property(x => x.Codigo).HasMaxLength(50).IsRequired();
            e.Property(x => x.Nome).HasMaxLength(200).IsRequired();
            e.Property(x => x.PrecoCusto).HasPrecision(18, 2);
            e.Property(x => x.PrecoBaseVenda).HasPrecision(18, 2);
            e.HasMany(x => x.Fornecedores)
             .WithMany(x => x.Produtos)
             .UsingEntity(j => j.ToTable("FornecedorProduto"));
        });

        // --- Stock ---
        modelBuilder.Entity<Stock>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.LojaId, x.ProdutoId }).IsUnique();
            e.Property(x => x.Quantidade).HasPrecision(18, 3);
            e.Property(x => x.StockMinimo).HasPrecision(18, 3);
            e.Property(x => x.PrecoVendaLocal).HasPrecision(18, 2);
            e.Ignore(x => x.EmAlerta);
            e.Ignore(x => x.PrecoVendaEfetivo);
        });

        // --- Venda ---
        modelBuilder.Entity<Venda>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.SubTotal).HasPrecision(18, 2);
            e.Property(x => x.TotalDesconto).HasPrecision(18, 2);
            e.Property(x => x.Total).HasPrecision(18, 2);
            e.Property(x => x.Estado).HasConversion<string>();
            e.HasOne(x => x.Loja).WithMany(x => x.Vendas).HasForeignKey(x => x.LojaId);
            e.HasOne(x => x.Funcionario).WithMany().HasForeignKey(x => x.FuncionarioId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // --- LinhaVenda ---
        modelBuilder.Entity<LinhaVenda>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Quantidade).HasPrecision(18, 3);
            e.Property(x => x.PrecoUnitario).HasPrecision(18, 2);
            e.Property(x => x.Desconto).HasPrecision(18, 2);
            e.Ignore(x => x.SubTotal);
        });

        // --- Fornecedor ---
        modelBuilder.Entity<Fornecedor>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Nome).HasMaxLength(200).IsRequired();
            e.Property(x => x.NIF).HasMaxLength(20);
        });

        // --- Encomenda ---
        modelBuilder.Entity<Encomenda>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Estado).HasConversion<string>();
            e.HasOne(x => x.Loja).WithMany(x => x.Encomendas).HasForeignKey(x => x.LojaId);
            e.HasOne(x => x.Fornecedor).WithMany(x => x.Encomendas).HasForeignKey(x => x.FornecedorId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // --- Fatura ---
        modelBuilder.Entity<Fatura>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Numero).IsUnique();
            e.Property(x => x.Numero).HasMaxLength(50).IsRequired();
            e.Property(x => x.Total).HasPrecision(18, 2);
            e.Property(x => x.Estado).HasConversion<string>();
            e.HasOne(x => x.Venda).WithOne(x => x.Fatura)
             .HasForeignKey<Fatura>(x => x.VendaId).OnDelete(DeleteBehavior.Restrict);
        });

        // --- Consolidacao ---
        modelBuilder.Entity<Consolidacao>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.LojaId, x.DataConsolidacao }).IsUnique();
            e.Property(x => x.TotalVendas).HasPrecision(18, 2);
            e.Property(x => x.TotalDescontos).HasPrecision(18, 2);
            e.Property(x => x.Resultado).HasConversion<string>();
            e.HasOne(x => x.Loja).WithMany(x => x.Consolidacoes).HasForeignKey(x => x.LojaId);
        });

        // --- AjusteStock ---
        modelBuilder.Entity<AjusteStock>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Variacao).HasPrecision(18, 3);
            e.Property(x => x.Motivo).HasMaxLength(500).IsRequired();
        });

        // --- Utilizador ---
        modelBuilder.Entity<Utilizador>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Nome).HasMaxLength(200).IsRequired();
            e.Property(x => x.Email).HasMaxLength(300).IsRequired();
            e.Property(x => x.Papel).HasConversion<string>();
            e.HasOne(x => x.Loja).WithMany(x => x.Utilizadores)
             .HasForeignKey(x => x.LojaId).OnDelete(DeleteBehavior.Restrict);
        });

        // --- Notificacao ---
        modelBuilder.Entity<Notificacao>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Mensagem).HasMaxLength(1000).IsRequired();
            e.Property(x => x.Tipo).HasMaxLength(20);
            e.HasIndex(x => x.DestinatarioId);
            e.HasIndex(x => x.Lida);
        });
    }
}
