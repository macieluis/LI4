namespace ConvenienceChain.Core.Enums;

public enum PapelUtilizador
{
    GestorCadeia,
    GerenteLoja,
    Funcionario
}

public enum EstadoVenda
{
    Concluida,
    Cancelada,
    Devolvida
}

public enum EstadoEncomenda
{
    Pendente,
    Enviada,
    Rececionada,
    Cancelada
}

public enum EstadoFatura
{
    Emitida,
    Paga,
    Anulada
}

public enum ResultadoConsolidacao
{
    Sucesso,
    Falha,
    Parcial
}
