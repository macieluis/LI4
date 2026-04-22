using ConvenienceChain.Core.DTOs;
using ConvenienceChain.Core.Enums;

namespace ConvenienceChain.Web.Services;

/// <summary>
/// Serviço singleton que mantém o estado da sessão do utilizador autenticado no Blazor Server.
/// </summary>
public class SessionService
{
    public bool IsAuthenticated { get; private set; }
    public string? UserId { get; private set; }
    public string? Nome { get; private set; }
    public string? Email { get; private set; }
    public PapelUtilizador Papel { get; private set; }
    public int? LojaId { get; private set; }

    public bool IsGestorCadeia => Papel == PapelUtilizador.GestorCadeia;
    public bool IsGerenteLoja => Papel == PapelUtilizador.GerenteLoja;
    public bool IsFuncionario => Papel == PapelUtilizador.Funcionario;

    public void Login(LoginResultDto result)
    {
        IsAuthenticated = true;
        UserId = result.UserId;
        Nome = result.Nome;
        Email = result.Email;
        Papel = result.Papel;
        LojaId = result.LojaId;
        NotifyStateChanged();
    }

    public void Logout()
    {
        IsAuthenticated = false;
        UserId = null;
        Nome = null;
        Email = null;
        Papel = default;
        LojaId = null;
        NotifyStateChanged();
    }

    public event Action? OnChange;
    public void NotifyStateChanged() => OnChange?.Invoke();
}
