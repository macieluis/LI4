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
    private readonly Mock<IUtilizadorRepository> _mockUserRepo = new();

    public StockServiceTests()
    {
        // Por omissão mock devolve um Gestor ativo — as testes específicas podem sobrescrever.
        _mockUserRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new Utilizador { Id = "user1", Papel = PapelUtilizador.GestorCadeia, Ativo = true });
    }

    private StockService CreateSvc() =>
        new(_mockStockRepo.Object, _mockAjusteRepo.Object, _mockUserRepo.Object);

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
    private readonly Mock<IUtilizadorRepository> _mockUserRepo = new();

    public ProdutoServiceTests()
    {
        _mockUserRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new Utilizador { Id = "admin", Papel = PapelUtilizador.GestorCadeia, Ativo = true });
    }

    private ProdutoService CreateSvc() => new(_mockProdRepo.Object, _mockUserRepo.Object);

    [Fact]
    public async Task CreateProduto_CodigoDuplicado_LancaExcecao()
    {
        // Arrange
        var existing = new Produto { Codigo = "5601" };
        _mockProdRepo.Setup(r => r.GetByCodigoAsync("5601")).ReturnsAsync(existing);
        var svc = CreateSvc();
        var dto = new CreateProdutoDto("5601", "Água", "", 0.15m, 0.50m, "unidade", 1, null);

        // Act
        var act = async () => await svc.CreateAsync(dto, "admin");

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
        var act = async () => await svc.DeactivateAsync(999, "admin");

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task CreateProduto_ComFuncionario_DeveLancarUnauthorizedAccessException()
    {
        // Arrange — sobrescrever mock para devolver Funcionário
        _mockUserRepo.Setup(r => r.GetByIdAsync("func1"))
            .ReturnsAsync(new Utilizador { Id = "func1", Papel = PapelUtilizador.Funcionario, Ativo = true });
        var svc = CreateSvc();
        var dto = new CreateProdutoDto("NOVO", "Teste", "", 1m, 2m, "unidade", 1, null);

        // Act
        var act = async () => await svc.CreateAsync(dto, "func1");

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*Gestor*");
    }
}

/// <summary>Testes ao OrderService — cancelamento com motivo e RBAC.</summary>
public class OrderServiceTests
{
    private readonly Mock<IEncomendaRepository> _mockEncRepo = new();
    private readonly Mock<IStockRepository> _mockStockRepo = new();
    private readonly Mock<INotificationService> _mockNotif = new();
    private readonly Mock<IUtilizadorRepository> _mockUserRepo = new();

    public OrderServiceTests()
    {
        _mockUserRepo.Setup(r => r.GetByIdAsync("admin"))
            .ReturnsAsync(new Utilizador { Id = "admin", Papel = PapelUtilizador.GestorCadeia, Ativo = true });
        _mockUserRepo.Setup(r => r.GetByIdAsync("func"))
            .ReturnsAsync(new Utilizador { Id = "func", Papel = PapelUtilizador.Funcionario, Ativo = true });
        _mockUserRepo.Setup(r => r.GetByIdAsync("gerente_loja1"))
            .ReturnsAsync(new Utilizador
            {
                Id = "gerente_loja1",
                Papel = PapelUtilizador.GerenteLoja,
                LojaId = 1,
                Ativo = true
            });
    }

    private OrderService CreateSvc() =>
        new(_mockEncRepo.Object, _mockStockRepo.Object, _mockNotif.Object, _mockUserRepo.Object);

    [Fact]
    public async Task Cancel_SemMotivo_DeveLancarArgumentException()
    {
        // Arrange
        var svc = CreateSvc();

        // Act
        var act = async () => await svc.CancelAsync(1, "   ", "admin");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*motivo*");
    }

    [Fact]
    public async Task Cancel_ComFuncionario_DeveLancarUnauthorizedAccessException()
    {
        // Arrange
        var svc = CreateSvc();

        // Act
        var act = async () => await svc.CancelAsync(1, "motivo qualquer", "func");

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Cancel_EncomendaRececionada_DeveLancarInvalidOperation()
    {
        // Arrange
        var enc = new Encomenda { Id = 1, Estado = EstadoEncomenda.Rececionada, Observacoes = "" };
        _mockEncRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(enc);
        var svc = CreateSvc();

        // Act
        var act = async () => await svc.CancelAsync(1, "motivo", "admin");

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*rececionada*");
    }

    [Fact]
    public async Task Rececionar_ComGestor_DeveLancarUnauthorizedAccessException()
    {
        // Arrange
        var enc = new Encomenda { Id = 1, LojaId = 1, Estado = EstadoEncomenda.Pendente, Linhas = new List<LinhaEncomenda>() };
        _mockEncRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(enc);
        var svc = CreateSvc();

        // Act
        var act = async () => await svc.RecepcionarAsync(1, new List<RecepcionarLinhaDto>(), "admin");

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*Gerente*");
    }

    [Fact]
    public async Task Rececionar_ComGerenteDeOutraLoja_DeveLancarUnauthorizedAccessException()
    {
        // Arrange — Gerente da loja 2 a tentar receber encomenda da loja 1
        _mockUserRepo.Setup(r => r.GetByIdAsync("gerente_loja2"))
            .ReturnsAsync(new Utilizador
            {
                Id = "gerente_loja2",
                Papel = PapelUtilizador.GerenteLoja,
                LojaId = 2,
                Ativo = true
            });
        var enc = new Encomenda { Id = 1, LojaId = 1, Estado = EstadoEncomenda.Pendente, Linhas = new List<LinhaEncomenda>() };
        _mockEncRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(enc);
        var svc = CreateSvc();

        // Act
        var act = async () => await svc.RecepcionarAsync(1, new List<RecepcionarLinhaDto>(), "gerente_loja2");

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*sua própria loja*");
    }
}

/// <summary>Testes ao NotificationService.</summary>
public class NotificationServiceTests
{
    private readonly Mock<INotificacaoRepository> _mockRepo = new();

    [Fact]
    public async Task NotifyGestores_DevePersistirComDestinatarioNull()
    {
        // Arrange
        Notificacao? persistida = null;
        _mockRepo.Setup(r => r.AddAsync(It.IsAny<Notificacao>()))
            .Callback<Notificacao>(n => persistida = n)
            .ReturnsAsync((Notificacao n) => n);
        var svc = new NotificationService(_mockRepo.Object);

        // Act
        await svc.NotifyGestoresAsync("Teste", "Warning");

        // Assert
        persistida.Should().NotBeNull();
        persistida!.DestinatarioId.Should().BeNull();
        persistida.Mensagem.Should().Be("Teste");
        persistida.Tipo.Should().Be("Warning");
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task MarcarComoLida_DeveAtualizarFlag()
    {
        // Arrange
        var n = new Notificacao { Id = 1, Mensagem = "x", Lida = false };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(n);
        var svc = new NotificationService(_mockRepo.Object);

        // Act
        await svc.MarcarComoLidaAsync(1);

        // Assert
        n.Lida.Should().BeTrue();
        _mockRepo.Verify(r => r.UpdateAsync(n), Times.Once);
    }
}

/// <summary>Testes aos exports PDF/CSV do ReportService e PDF da Fatura.</summary>
public class ExportServiceTests
{
    [Fact]
    public async Task ExportRelatorioVendasPdf_DeveRetornarBytesNaoVazios()
    {
        // Arrange — mocks vazios são suficientes, o relatório vem todo a zero
        var mockVendaRepo = new Mock<IVendaRepository>();
        var mockLojaRepo = new Mock<ILojaRepository>();
        var mockStockRepo = new Mock<IStockRepository>();
        var mockEncRepo = new Mock<IEncomendaRepository>();
        mockLojaRepo.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(new List<Loja>());
        mockVendaRepo.Setup(r => r.GetByLojaAsync(It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
            .ReturnsAsync(new List<Venda>());

        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        var svc = new ReportService(mockVendaRepo.Object, mockLojaRepo.Object, mockStockRepo.Object, mockEncRepo.Object);

        // Act
        var bytes = await svc.ExportRelatorioVendasPdfAsync(null, DateTime.Today.AddDays(-7), DateTime.Today);

        // Assert
        bytes.Should().NotBeNull();
        bytes.Length.Should().BeGreaterThan(100); // um PDF válido pesa muito mais
        // PDFs começam com a assinatura ASCII "%PDF"
        System.Text.Encoding.ASCII.GetString(bytes, 0, 4).Should().Be("%PDF");
    }

    [Fact]
    public async Task ExportRelatorioVendasCsv_DeveIncluirCabecalho()
    {
        // Arrange
        var mockVendaRepo = new Mock<IVendaRepository>();
        var mockLojaRepo = new Mock<ILojaRepository>();
        var mockStockRepo = new Mock<IStockRepository>();
        var mockEncRepo = new Mock<IEncomendaRepository>();
        mockLojaRepo.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(new List<Loja>());
        mockVendaRepo.Setup(r => r.GetByLojaAsync(It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
            .ReturnsAsync(new List<Venda>());
        var svc = new ReportService(mockVendaRepo.Object, mockLojaRepo.Object, mockStockRepo.Object, mockEncRepo.Object);

        // Act
        var bytes = await svc.ExportRelatorioVendasCsvAsync(null, DateTime.Today.AddDays(-7), DateTime.Today);
        var text = System.Text.Encoding.UTF8.GetString(bytes);

        // Assert
        text.Should().Contain("Relatório de Vendas SGCLC");
        text.Should().Contain("Top 10 Produtos");
    }

    [Fact]
    public async Task ExportFaturaPdf_DeveRetornarBytesNaoVazios()
    {
        // Arrange
        var mockRepo = new Mock<IFaturaRepository>();
        var fatura = new Fatura
        {
            Id = 1, Numero = "F2026/001", NomeCliente = "Cliente Teste",
            NIFCliente = "123456789", MoradaCliente = "R. Teste, 1",
            DataEmissao = DateTime.UtcNow, Total = 10m,
            Linhas = new List<LinhaFatura>
            {
                new() { DescricaoProduto = "Item A", Quantidade = 1, PrecoUnitario = 10m, Desconto = 0 }
            }
        };
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fatura);

        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        var svc = new FaturaService(mockRepo.Object);

        // Act
        var bytes = await svc.ExportPdfAsync(1);

        // Assert
        bytes.Length.Should().BeGreaterThan(100);
        System.Text.Encoding.ASCII.GetString(bytes, 0, 4).Should().Be("%PDF");
    }
}
