using ConvenienceChain.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConvenienceChain.Web.Services;

/// <summary>
/// Job agendado que executa a consolidação diária de dados de todas as lojas
/// na hora configurada (default: 23:59).
/// Implementa RF33 e RF36 do SRS.
/// </summary>
public class ConsolidacaoBackgroundService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<ConsolidacaoBackgroundService> _logger;
    private readonly IConfiguration _config;

    public ConsolidacaoBackgroundService(
        IServiceProvider services,
        ILogger<ConsolidacaoBackgroundService> logger,
        IConfiguration config)
    {
        _services = services;
        _logger = logger;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ConsolidacaoBackgroundService iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var agora = DateTime.Now;
            var horaConfig = _config["Consolidacao:HoraExecucao"] ?? "23:59";
            var partes = horaConfig.Split(':');
            var horaAlvo = new TimeOnly(int.Parse(partes[0]), int.Parse(partes[1]));
            var agoraTime = TimeOnly.FromDateTime(agora);

            // Calcular tempo até à próxima execução
            var minutos = (horaAlvo.ToTimeSpan() - agoraTime.ToTimeSpan()).TotalMinutes;
            if (minutos < 0) minutos += 24 * 60; // próximo dia

            _logger.LogInformation("Consolidação agendada para {Hora}. Aguardando {Min:0} minutos.", horaConfig, minutos);

            try
            {
                await Task.Delay(TimeSpan.FromMinutes(minutos), stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }

            await ExecutarConsolidacaoAsync();

            // Aguardar 1 minuto para não repetir no mesmo minuto
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task ExecutarConsolidacaoAsync()
    {
        _logger.LogInformation("A iniciar consolidação diária de {Data}...", DateTime.Today.AddDays(-1));

        using var scope = _services.CreateScope();
        var consolidacaoService = scope.ServiceProvider.GetRequiredService<IConsolidacaoService>();

        try
        {
            var data = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)); // ontem
            await consolidacaoService.ConsolidarTodasAsync(data);
            _logger.LogInformation("Consolidação concluída com sucesso para {Data}.", data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante a consolidação diária automática.");
        }
    }
}
