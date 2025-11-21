using Microsoft.EntityFrameworkCore;
using webapiinvestimento.Context;
using webapiinvestimento.Data;
using webapiinvestimento.DTO;
using webapiinvestimento.Models;

public interface ISimulacaoService
{
    Task<(bool Sucesso, simulacao Simulacao, produtoInvestimento Produto, string Erro)> CriarSimulacaoAsync(SimulacaoRequest request);
}

public class SimulacaoService : ISimulacaoService
{
    private readonly simulacaoContext _simulacaoContext;
    private readonly produtoContext _produtoContext;

    public SimulacaoService(simulacaoContext simulacaoContext, produtoContext produtoContext)
    {
        _simulacaoContext = simulacaoContext;
        _produtoContext = produtoContext;
    }

    public async Task<(bool Sucesso, simulacao Simulacao, produtoInvestimento Produto, string Erro)> CriarSimulacaoAsync(SimulacaoRequest request)
    {
        var produto = await _produtoContext.produtoInvestimento
            .FirstOrDefaultAsync(p => p.TIPO_PRODUTO.ToLower().Trim() == request.TipoInvestimento.ToLower().Trim());

        if (produto == null)
            return (false, null!, null!, "Produto não encontrado");

        decimal valorFinal = request.Valor + (request.Valor * request.Prazo * produto.RENTABILIDADE);

        var simulacao = new simulacao
        {
            ID_CLIENTE = request.IdCliente,
            NOME_PRODUTO = produto.NOME_PRODUTO,
            VALOR_INVESTIMENTO = request.Valor,
            VALOR_FINAL = valorFinal,
            PRAZO = request.Prazo,
            DATA_SIMULACAO = DateTime.Now
        };

        _simulacaoContext.simulacoesInvestimentos.Add(simulacao);
        await _simulacaoContext.SaveChangesAsync();

        return (true, simulacao, produto, "");
    }
}

