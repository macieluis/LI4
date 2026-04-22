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

            await ExecutarConsolidacaoAsync(stoppingToken);

            // Aguardar 1 minuto para não repetir no mesmo minuto
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task ExecutarConsolidacaoAsync(CancellationToken ct)
    {
        var data = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)); // ontem
        var retryMinutes = int.TryParse(_config["Consolidacao:RetryIntervalMinutes"], out var m) ? m : 30;
        const int MAX_TENTATIVAS = 3;

        for (var tentativa = 1; tentativa <= MAX_TENTATIVAS && !ct.IsCancellationRequested; tentativa++)
        {
            _logger.LogInformation("Consolidação tentativa {Tent}/{Max} para {Data}...",
                tentativa, MAX_TENTATIVAS, data);

            using var scope = _services.CreateScope();
            var consolidacaoService = scope.ServiceProvider.GetRequiredService<IConsolidacaoService>();

            try
            {
                var resumo = await consolidacaoService.ConsolidarTodasAsync(data);
                _logger.LogInformation("Consolidação: {S} sucessos, {F} falhas.",
                    resumo.Sucessos, resumo.Falhas);

                if (resumo.Falhas == 0)
                {
                    _logger.LogInformation("Consolidação concluída com sucesso total.");
                    return;
                }

                if (tentativa == MAX_TENTATIVAS)
                {
                    _logger.LogError("Consolidação falhou em {F} loja(s) após {Max} tentativas: {Lojas}",
                        resumo.Falhas, MAX_TENTATIVAS, string.Join(",", resumo.LojasComFalha));

                    try
                    {
                        var notifSvc = scope.ServiceProvider.GetRequiredService<INotificationService>();
                        await notifSvc.NotifyGestoresAsync(
                            $"Consolidação de {data:dd/MM/yyyy} falhou em {resumo.Falhas} loja(s) após {MAX_TENTATIVAS} tentativas.",
                            "Error");
                    }
                    catch (Exception nex)
                    {
                        _logger.LogError(nex, "Falha ao enviar notificação de consolidação aos Gestores.");
                    }
                    return;
                }

                _logger.LogWarning("Aguardando {Min} minuto(s) antes da próxima tentativa...", retryMinutes);
                await Task.Delay(TimeSpan.FromMinutes(retryMinutes), ct);
            }
            catch (TaskCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na tentativa {Tent} da consolidação.", tentativa);
                if (tentativa == MAX_TENTATIVAS) return;
                try { await Task.Delay(TimeSpan.FromMinutes(retryMinutes), ct); }
                catch (TaskCanceledException) { return; }
            }
        }
    }
}
