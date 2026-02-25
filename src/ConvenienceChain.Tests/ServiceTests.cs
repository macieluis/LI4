using ConvenienceChain.Core.DTOs;
using ConvenienceChain.Core.Entities;
using ConvenienceChain.Core.Enums;
using ConvenienceChain.Core.Interfaces;
using ConvenienceChain.Core.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConvenienceChain.Tests;

/// <summary>
/// Testes unitários ao StockService.
/// Validam a lógica de negócio de ajuste de stock, alertas e deduções de venda.
/// </summary>
public class StockServiceTests
{
    private readonly Mock<IStockRepository> _mockStockRepo = new();
    private readonly Mock<IAjusteStockRepository> _mockAjusteRepo = new();
    private StockService CreateSvc() => new(_mockStockRepo.Object, _mockAjusteRepo.Object);

    [Fact]
    public async Task AjustarStock_VariacaoNegativaValida_AtualizaQuantidade()
    {
        // Arrange
        var stock = new Stock { LojaId = 1, ProdutoId = 1, Quantidade = 100 };
        _mockStockRepo.Setup(r => r.GetAsync(1, 1)).ReturnsAsync(stock);
        _mockAjusteRepo.Setup(r => r.AddAsync(It.IsAny<AjusteStock>())).ReturnsAsync(new AjusteStock());
        var svc = CreateSvc();

        // Act
        await svc.AjustarStockAsync(1, 1, -10, "Quebra teste", "user1");

        // Assert
        stock.Quantidade.Should().Be(90);
        _mockStockRepo.Verify(r => r.UpdateAsync(stock), Times.Once);
    }

    [Fact]
    public async Task AjustarStock_VariacaoNegativaMaiorQueStock_LancaExcecao()
    {
        // Arrange
        var stock = new Stock { LojaId = 1, ProdutoId = 1, Quantidade = 5 };
        _mockStockRepo.Setup(r => r.GetAsync(1, 1)).ReturnsAsync(stock);
        var svc = CreateSvc();

        // Act
        var act = async () => await svc.AjustarStockAsync(1, 1, -10, "Quebra", "user1");

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*negativo*");
    }

    [Fact]
    public async Task CheckStock_StockSuficiente_RetornaTrue()
    {
        // Arrange
        var stock = new Stock { LojaId = 1, ProdutoId = 1, Quantidade = 50 };
        _mockStockRepo.Setup(r => r.GetAsync(1, 1)).ReturnsAsync(stock);
        var svc = CreateSvc();

        // Act
        var result = await svc.CheckStockAsync(1, 1, 30);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CheckStock_StockInsuficiente_RetornaFalse()
    {
        // Arrange
        var stock = new Stock { LojaId = 1, ProdutoId = 1, Quantidade = 5 };
        _mockStockRepo.Setup(r => r.GetAsync(1, 1)).ReturnsAsync(stock);
        var svc = CreateSvc();

        // Act
        var result = await svc.CheckStockAsync(1, 1, 10);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeductStock_DeduceQuantidadesCorretamente()
    {
        // Arrange
        var stock1 = new Stock { LojaId = 1, ProdutoId = 1, Quantidade = 100 };
        var stock2 = new Stock { LojaId = 1, ProdutoId = 2, Quantidade = 50 };
        _mockStockRepo.Setup(r => r.GetAsync(1, 1)).ReturnsAsync(stock1);
        _mockStockRepo.Setup(r => r.GetAsync(1, 2)).ReturnsAsync(stock2);
        var svc = CreateSvc();
        var linhas = new[] { new LinhaVendaDto(1, 5, 1.0m, 0), new LinhaVendaDto(2, 3, 2.0m, 0) };

        // Act
        await svc.DeductStockAsync(1, linhas);

        // Assert
        stock1.Quantidade.Should().Be(95);
        stock2.Quantidade.Should().Be(47);
    }
}

/// <summary>
/// Testes unitários ao SalesService.
/// Validam a lógica de criação e cancelamento de vendas.
/// </summary>
public class SalesServiceTests
{
    private readonly Mock<IVendaRepository> _mockVendaRepo = new();
    private readonly Mock<IStockService> _mockStockSvc = new();
    private SalesService CreateSvc() => new(_mockVendaRepo.Object, _mockStockSvc.Object);

    [Fact]
    public async Task CreateSale_StockInsuficiente_LancaExcecao()
    {
        // Arrange
        _mockStockSvc.Setup(s => s.CheckStockAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()))
            .ReturnsAsync(false);
        var svc = CreateSvc();
        var dto = new CreateVendaDto(1, "userId", new[] { new LinhaVendaDto(1, 10, 1.0m, 0) });

        // Act
        var act = async () => await svc.CreateSaleAsync(dto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Stock insuficiente*");
    }

    [Fact]
    public async Task CreateSale_StockSuficiente_CriaVenda()
    {
        // Arrange
        _mockStockSvc.Setup(s => s.CheckStockAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()))
            .ReturnsAsync(true);
        var vendaCriada = new Venda
        {
            Id = 1, LojaId = 1, FuncionarioId = "userId",
            SubTotal = 10, TotalDesconto = 0, Total = 10,
            Estado = EstadoVenda.Concluida,
            Linhas = new List<LinhaVenda>()
        };
        _mockVendaRepo.Setup(r => r.AddAsync(It.IsAny<Venda>())).ReturnsAsync(vendaCriada);
        _mockVendaRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(vendaCriada);
        var svc = CreateSvc();
        var dto = new CreateVendaDto(1, "userId", new[] { new LinhaVendaDto(1, 2, 5.0m, 0) });

        // Act
        var result = await svc.CreateSaleAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Total.Should().Be(10);
        _mockStockSvc.Verify(s => s.DeductStockAsync(1, It.IsAny<IEnumerable<LinhaVendaDto>>()), Times.Once);
    }

    [Fact]
    public async Task CancelSale_VendaJaCancelada_LancaExcecao()
    {
        // Arrange
        var venda = new Venda { Id = 1, Estado = EstadoVenda.Cancelada, Linhas = new List<LinhaVenda>() };
        _mockVendaRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(venda);
        var svc = CreateSvc();

        // Act
        var act = async () => await svc.CancelSaleAsync(1, "motivo", "userId");

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}

/// <summary>
/// Testes ao ProdutoService.
/// </summary>
public class ProdutoServiceTests
{
    private readonly Mock<IProdutoRepository> _mockProdRepo = new();
    private ProdutoService CreateSvc() => new(_mockProdRepo.Object);

    [Fact]
    public async Task CreateProduto_CodigoDuplicado_LancaExcecao()
    {
        // Arrange
        var existing = new Produto { Codigo = "5601" };
        _mockProdRepo.Setup(r => r.GetByCodigoAsync("5601")).ReturnsAsync(existing);
        var svc = CreateSvc();
        var dto = new CreateProdutoDto("5601", "Água", null, 0.15m, 0.50m, "unidade", 1, null);

        // Act
        var act = async () => await svc.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*já existe*");
    }

    [Fact]
    public async Task DeactivateProduto_ProdutoNaoEncontrado_LancaKeyNotFoundException()
    {
        // Arrange
        _mockProdRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Produto?)null);
        var svc = CreateSvc();

        // Act
        var act = async () => await svc.DeactivateAsync(999);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
