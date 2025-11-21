using Microsoft.EntityFrameworkCore;
using webapiinvestimento.Context;
using webapiinvestimento.Data;

public interface IPerfilRiscoService
{
    Task<(bool Sucesso, string Perfil, double Pontuacao, int Total, string Descricao)> CalcularPerfilAsync(int clienteId);
}

public class PerfilRiscoService : IPerfilRiscoService
{
    private readonly simulacaoContext _simContext;
    private readonly produtoContext _prodContext;

    public PerfilRiscoService(simulacaoContext simContext, produtoContext prodContext)
    {
        _simContext = simContext;
        _prodContext = prodContext;
    }

    public async Task<(bool Sucesso, string Perfil, double Pontuacao, int Total, string Descricao)> CalcularPerfilAsync(int clienteId)
    {
        var simulacoes = await _simContext.simulacoesInvestimentos
            .Where(s => s.ID_CLIENTE == clienteId)
            .ToListAsync();

        if (!simulacoes.Any())
            return (false, "", 0, 0, "");

        var pontos = new List<int>();

        foreach (var sim in simulacoes)
        {
            var produto = await _prodContext.produtoInvestimento
                .FirstOrDefaultAsync(p => p.NOME_PRODUTO == sim.NOME_PRODUTO);

            if (produto == null)
                continue;

            var score = produto.RISCO_PRODUTO.Trim().ToLower() switch
            {
                "baixo" => 20,
                "médio" => 50,
                "alto" => 80,
                _ => 50
            };

            pontos.Add(score);
        }

        if (!pontos.Any())
            return (false, "", 0, 0, "");

        double media = pontos.Average();

        string perfil = media switch
        {
            <= 35 => "Conservador",
            <= 65 => "Moderado",
            _ => "Agressivo"
        };

        string descricao = perfil switch
        {
            "Conservador" => "baixa movimentação, foco em liquidez",
            "Moderado" => "perfil equilibrado entre segurança e rentabilidade",
            "Agressivo" => "busca por alta rentabilidade, maior risco",
            _ => ""
        };

        return (true, perfil, Math.Round(media), simulacoes.Count, descricao);
    }
}

