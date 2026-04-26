using ConvenienceChain.Core.Entities;
using ConvenienceChain.Core.Enums;
using ConvenienceChain.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ConvenienceChain.Web.Services;

/// <summary>
/// Popula a base de dados com dados iniciais de demonstração.
/// Apenas executa se a BD estiver vazia.
/// </summary>
public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        if (await db.Lojas.AnyAsync()) return; // Já inicializado

        // ── Categorias ────────────────────────────────────────────
        var catBebidas = new Categoria { Nome = "Bebidas" };
        var catBebAl = new Categoria { Nome = "Bebidas Alcoólicas", CategoriaPai = catBebidas };
        var catSnacks = new Categoria { Nome = "Snacks" };
        var catLact = new Categoria { Nome = "Lacticínios" };
        var catHig = new Categoria { Nome = "Higiene" };
        var catCongelados = new Categoria { Nome = "Congelados" };
        db.Categorias.AddRange(catBebidas, catBebAl, catSnacks, catLact, catHig, catCongelados);

        // ── Lojas ─────────────────────────────────────────────────
        var loja1 = new Loja { Nome = "QuickMart Braga-Centro", Morada = "Rua do Souto, 45, Braga", Telefone = "253123456", Email = "braga@quickmart.pt" };
        var loja2 = new Loja { Nome = "QuickMart Palmeira",     Morada = "Av. da Europa, 12, Braga",  Telefone = "253234567", Email = "palmeira@quickmart.pt" };
        var loja3 = new Loja { Nome = "QuickMart Maximinos",    Morada = "Rua da Igreja, 3, Braga",   Telefone = "253345678", Email = "maximinos@quickmart.pt" };
        var loja4 = new Loja { Nome = "QuickMart Barcelos",     Morada = "Praça da República, 5, Barcelos", Telefone = "253456789", Email = "barcelos@quickmart.pt" };
        var loja5 = new Loja { Nome = "QuickMart Guimarães",    Morada = "Av. D. João IV, 20, Guimarães",   Telefone = "253567890", Email = "guimaraes@quickmart.pt" };
        db.Lojas.AddRange(loja1, loja2, loja3, loja4, loja5);

        // ── Produtos ──────────────────────────────────────────────
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        var produtos = new[]
        {
            new Produto { Codigo = "5601227001001", Nome = "Água Mineral 500ml",     PrecoCusto = 0.15m, PrecoBaseVenda = 0.60m, UnidadeMedida = "unidade", Categoria = catBebidas,    DataValidade = hoje.AddDays(365) },
            new Produto { Codigo = "5601227001002", Nome = "Água Mineral 1,5L",      PrecoCusto = 0.25m, PrecoBaseVenda = 0.95m, UnidadeMedida = "unidade", Categoria = catBebidas,    DataValidade = hoje.AddDays(365) },
            new Produto { Codigo = "5601227001003", Nome = "Coca-Cola 330ml",         PrecoCusto = 0.60m, PrecoBaseVenda = 1.50m, UnidadeMedida = "unidade", Categoria = catBebidas,    DataValidade = hoje.AddDays(240) },
            new Produto { Codigo = "5601227001004", Nome = "Red Bull 250ml",          PrecoCusto = 0.95m, PrecoBaseVenda = 1.90m, UnidadeMedida = "unidade", Categoria = catBebidas,    DataValidade = hoje.AddDays(180) },
            new Produto { Codigo = "5601227001005", Nome = "Sumo Laranja 1L",         PrecoCusto = 0.80m, PrecoBaseVenda = 1.80m, UnidadeMedida = "unidade", Categoria = catBebidas,    DataValidade = hoje.AddDays(20) }, // ⚠ expira em breve
            new Produto { Codigo = "5601227002001", Nome = "Cerveja SuperBock 330ml", PrecoCusto = 0.45m, PrecoBaseVenda = 1.10m, UnidadeMedida = "unidade", Categoria = catBebAl,      DataValidade = hoje.AddDays(300) },
            new Produto { Codigo = "5601227002002", Nome = "Vinho Tinto 750ml",       PrecoCusto = 2.50m, PrecoBaseVenda = 5.99m, UnidadeMedida = "unidade", Categoria = catBebAl,      DataValidade = null },             // sem validade
            new Produto { Codigo = "5601227003001", Nome = "Batatas Fritas 180g",     PrecoCusto = 0.85m, PrecoBaseVenda = 1.75m, UnidadeMedida = "unidade", Categoria = catSnacks,     DataValidade = hoje.AddDays(120) },
            new Produto { Codigo = "5601227003002", Nome = "Bolacha Maria",            PrecoCusto = 0.50m, PrecoBaseVenda = 1.20m, UnidadeMedida = "unidade", Categoria = catSnacks,     DataValidade = hoje.AddDays(90)  },
            new Produto { Codigo = "5601227003003", Nome = "Pão de Leite",             PrecoCusto = 0.20m, PrecoBaseVenda = 0.45m, UnidadeMedida = "unidade", Categoria = catSnacks,     DataValidade = hoje.AddDays(3)   }, // ⚠⚠ expira em 3 dias
            new Produto { Codigo = "5601227004001", Nome = "Leite Meio-Gordo 1L",     PrecoCusto = 0.55m, PrecoBaseVenda = 1.10m, UnidadeMedida = "unidade", Categoria = catLact,       DataValidade = hoje.AddDays(10)  }, // ⚠ expira em breve
            new Produto { Codigo = "5601227004002", Nome = "Iogurte Natural 125g",    PrecoCusto = 0.30m, PrecoBaseVenda = 0.65m, UnidadeMedida = "unidade", Categoria = catLact,       DataValidade = hoje.AddDays(-2)  }, // ⛔ EXPIRADO
            new Produto { Codigo = "5601227005001", Nome = "Champô 400ml",             PrecoCusto = 1.20m, PrecoBaseVenda = 2.80m, UnidadeMedida = "unidade", Categoria = catHig,        DataValidade = hoje.AddDays(730) },
            new Produto { Codigo = "5601227005002", Nome = "Pasta de Dentes 75ml",    PrecoCusto = 0.90m, PrecoBaseVenda = 2.10m, UnidadeMedida = "unidade", Categoria = catHig,        DataValidade = hoje.AddDays(730) },
            new Produto { Codigo = "5601227006001", Nome = "Pizza Margherita 400g",   PrecoCusto = 1.80m, PrecoBaseVenda = 3.99m, UnidadeMedida = "unidade", Categoria = catCongelados,  DataValidade = hoje.AddDays(45)  },
        };
        db.Produtos.AddRange(produtos);

        // ── Utilizadores ──────────────────────────────────────────
        var gestor = new Utilizador { Id = Guid.NewGuid().ToString(), Nome = "João Martins", Email = "gestor@quickmart.pt", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), Papel = PapelUtilizador.GestorCadeia };
        var gerente1 = new Utilizador { Id = Guid.NewGuid().ToString(), Nome = "Ana Rodrigues",   Email = "ana@quickmart.pt",    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Gerente@1"), Papel = PapelUtilizador.GerenteLoja, Loja = loja1 };
        var gerente2 = new Utilizador { Id = Guid.NewGuid().ToString(), Nome = "Pedro Silva",      Email = "pedro@quickmart.pt",  PasswordHash = BCrypt.Net.BCrypt.HashPassword("Gerente@2"), Papel = PapelUtilizador.GerenteLoja, Loja = loja2 };
        var func1   = new Utilizador { Id = Guid.NewGuid().ToString(), Nome = "Carlos Mendes",    Email = "carlos@quickmart.pt", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Func@1234"),  Papel = PapelUtilizador.Funcionario,  Loja = loja1 };
        var func2   = new Utilizador { Id = Guid.NewGuid().ToString(), Nome = "Sofia Costa",      Email = "sofia@quickmart.pt",  PasswordHash = BCrypt.Net.BCrypt.HashPassword("Func@1234"),  Papel = PapelUtilizador.Funcionario,  Loja = loja1 };
        db.Utilizadores.AddRange(gestor, gerente1, gerente2, func1, func2);

        await db.SaveChangesAsync();

        // ── Stocks (após guardar produtos e lojas) ────────────────
        var stk = new List<Stock>();
        foreach (var loja in new[] { loja1, loja2, loja3, loja4, loja5 })
        {
            foreach (var p in produtos)
            {
                stk.Add(new Stock
                {
                    Loja = loja, Produto = p,
                    Quantidade = Random.Shared.Next(20, 200),
                    StockMinimo = 15
                });
            }
        }
        db.Stocks.AddRange(stk);

        // ── Fornecedores (cada um com o seu catálogo de produtos) ─
        var forn1 = new Fornecedor
        {
            Nome = "Distribuidora Norte Lda.", NIF = "509876543",
            Telefone = "253456789", Email = "encomendas@distnorte.pt",
            // Bebidas alcoólicas + lacticínios + congelados
            Produtos = produtos.Where(p =>
                p.Categoria == catBebAl || p.Categoria == catLact || p.Categoria == catCongelados
            ).ToList()
        };
        var forn2 = new Fornecedor
        {
            Nome = "Águas de Portugal S.A.", NIF = "501234567",
            Telefone = "210234567", Email = "comercial@aguasportugal.pt",
            // Bebidas não alcoólicas
            Produtos = produtos.Where(p => p.Categoria == catBebidas).ToList()
        };
        var forn3 = new Fornecedor
        {
            Nome = "SnackWorld Iberia", NIF = "508765432",
            Telefone = "219876543", Email = "vendas@snackworld.pt",
            // Snacks + higiene
            Produtos = produtos.Where(p => p.Categoria == catSnacks || p.Categoria == catHig).ToList()
        };
        db.Fornecedores.AddRange(forn1, forn2, forn3);

        await db.SaveChangesAsync();

        // ── Vendas e Faturas de demonstração ─────────────────────
        // Criar vendas das últimas 2 semanas para loja1 (func1 e gerente1)
        var vendas = new List<Venda>();
        var faturas = new List<Fatura>();
        var rng = new Random(42);
        var prod0 = produtos[0]; // Agua 500ml
        var prod1 = produtos[2]; // Coca-Cola
        var prod2 = produtos[7]; // Batatas Fritas

        for (int i = 13; i >= 0; i--)
        {
            var data = DateTime.Now.AddDays(-i).Date.AddHours(10 + rng.Next(0, 8));
            var funcionario = i % 2 == 0 ? func1 : gerente1;
            var nrVendas = rng.Next(2, 6);
            for (int v = 0; v < nrVendas; v++)
            {
                var linhas = new List<LinhaVenda>
                {
                    new() { Produto = prod0, Quantidade = rng.Next(1, 5), PrecoUnitario = prod0.PrecoBaseVenda, Desconto = 0 },
                    new() { Produto = prod1, Quantidade = rng.Next(1, 3), PrecoUnitario = prod1.PrecoBaseVenda, Desconto = 0 },
                };
                if (rng.Next(0, 2) == 0)
                    linhas.Add(new() { Produto = prod2, Quantidade = 1, PrecoUnitario = prod2.PrecoBaseVenda, Desconto = 0 });

                var sub = linhas.Sum(l => l.Quantidade * l.PrecoUnitario);
                var venda = new Venda
                {
                    Loja = loja1, Funcionario = funcionario,
                    DataHora = data.AddMinutes(v * 20),
                    SubTotal = sub, TotalDesconto = 0, Total = sub,
                    Estado = EstadoVenda.Concluida,
                    Linhas = linhas
                };
                vendas.Add(venda);

                // Fatura correspondente
                var lojaSeq = faturas.Count(f => f.Loja == loja1) + 1;
                faturas.Add(new Fatura
                {
                    Loja = loja1, Venda = venda,
                    Numero = $"FAT-001-{venda.DataHora.Year}-{lojaSeq:D5}",
                    NomeCliente = "", NIFCliente = "", MoradaCliente = "",
                    DataEmissao = venda.DataHora,
                    Total = sub,
                    Estado = EstadoFatura.Emitida,
                    Linhas = linhas.Select(l => new LinhaFatura
                    {
                        DescricaoProduto = l.Produto.Nome,
                        Quantidade = l.Quantidade,
                        PrecoUnitario = l.PrecoUnitario,
                        Desconto = 0
                    }).ToList()
                });
            }
        }
        // Algumas vendas na loja2 (para o Gestor ver dados diferentes por loja)
        for (int i = 6; i >= 0; i--)
        {
            var data = DateTime.Now.AddDays(-i).Date.AddHours(9 + rng.Next(0, 6));
            var sub = Math.Round((decimal)(rng.NextDouble() * 15 + 5), 2);
            var venda = new Venda
            {
                Loja = loja2, Funcionario = gerente2,
                DataHora = data,
                SubTotal = sub, TotalDesconto = 0, Total = sub,
                Estado = EstadoVenda.Concluida,
                Linhas = new List<LinhaVenda>
                {
                    new() { Produto = prod0, Quantidade = 2, PrecoUnitario = prod0.PrecoBaseVenda, Desconto = 0 }
                }
            };
            vendas.Add(venda);
            var lojaSeq = faturas.Count(f => f.Loja == loja2) + 1;
            faturas.Add(new Fatura
            {
                Loja = loja2, Venda = venda,
                Numero = $"FAT-002-{data.Year}-{lojaSeq:D5}",
                NomeCliente = "", NIFCliente = "", MoradaCliente = "",
                DataEmissao = data, Total = sub,
                Estado = EstadoFatura.Emitida,
                Linhas = new List<LinhaFatura>
                {
                    new() { DescricaoProduto = prod0.Nome, Quantidade = 2, PrecoUnitario = prod0.PrecoBaseVenda, Desconto = 0 }
                }
            });
        }

        db.Vendas.AddRange(vendas);
        db.Faturas.AddRange(faturas);
        await db.SaveChangesAsync();
    }
}
