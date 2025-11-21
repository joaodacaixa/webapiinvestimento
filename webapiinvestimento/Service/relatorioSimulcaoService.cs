using webapiinvestimento.Context;
using Microsoft.EntityFrameworkCore;
using webapiinvestimento.Models;

public interface IRelatorioSimulacaoService
{
    Task<List<object>> TotaisHojeAsync();
    Task<List<simulacao>> TodasAsync();
}

public class RelatorioSimulacaoService : IRelatorioSimulacaoService
{
    private readonly simulacaoContext _simContext;

    public RelatorioSimulacaoService(simulacaoContext simContext)
    {
        _simContext = simContext;
    }

    public async Task<List<object>> TotaisHojeAsync()
    {
        var hoje = DateTime.Today;

        return await _simContext.simulacoesInvestimentos
            .Where(s => s.DATA_SIMULACAO.Date == hoje)
            .GroupBy(s => s.NOME_PRODUTO)
            .Select(g => new
            {
                Produto = g.Key,
                Data = hoje,
                Quantidade = g.Count(),
                Media = g.Sum(x => (double)x.VALOR_FINAL) / g.Count(),
                TotalSimulado = g.Sum(x => (double)x.VALOR_FINAL)
            })
            .Cast<object>()
            .ToListAsync();
    }

    public async Task<List<simulacao>> TodasAsync()
    {
        return await _simContext.simulacoesInvestimentos.ToListAsync();
    }
}

