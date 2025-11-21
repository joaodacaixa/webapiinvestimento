using apiHackthon.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using webapiinvestimento.DTO;

namespace webapiinvestimento.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SimulacaoController : ControllerBase
{
    private readonly ISimulacaoService _simService;
    private readonly IPerfilRiscoService _perfilService;
    private readonly IRelatorioSimulacaoService _relatorioService;

    private readonly TelemetriaService _telemetria;

    public SimulacaoController(
        ISimulacaoService simService,
        IPerfilRiscoService perfilService,
        IRelatorioSimulacaoService relatorioService,
        TelemetriaService telemetria)
    {
        _simService = simService;
        _perfilService = perfilService;
        _relatorioService = relatorioService;
        _telemetria = telemetria;
    }

    [HttpPost]
    public async Task<IActionResult> CriarSimulacao([FromBody] SimulacaoRequest request)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        var result = await _simService.CriarSimulacaoAsync(request);

        sw.Stop();
        _telemetria.Registrar("CriarSimulacao", sw.Elapsed);

        if (!result.Sucesso)
            return NotFound(new { mensagem = result.Erro });

        return Ok(new
        {
            produtoValidado = new
            {
                id = result.Produto.ID_PRODUTO,
                nome = result.Produto.NOME_PRODUTO,
                tipo = result.Produto.TIPO_PRODUTO,
                rentabilidade = result.Produto.RENTABILIDADE,
                risco = result.Produto.RISCO_PRODUTO
            },
            resultadoSimulacao = new
            {
                valorFinal = result.Simulacao.VALOR_FINAL,
                rentabilidadeEfetiva = result.Produto.RENTABILIDADE,
                prazoMeses = result.Simulacao.PRAZO
            },
            dataSimulacao = result.Simulacao.DATA_SIMULACAO
        });
    }

    [HttpGet("perfil-risco/{clienteId}")]
    public async Task<IActionResult> GetPerfilRisco(int clienteId)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var result = await _perfilService.CalcularPerfilAsync(clienteId);
        sw.Stop();

        _telemetria.Registrar("PerfilRisco", sw.Elapsed);

        if (!result.Sucesso)
            return NotFound(new { mensagem = "Não foi possível calcular o perfil." });

        return Ok(new
        {
            clienteId,
            result.Perfil,
            pontuacaoMedia = result.Pontuacao,
            result.Descricao,
            totalSimulacoes = result.Total
        });
    }

    [HttpGet("totais-hoje")]
    public async Task<IActionResult> TotaisHoje()
    {
        var sw = Stopwatch.StartNew();

        var totais = await _relatorioService.TotaisHojeAsync();

        sw.Stop();
        _telemetria.Registrar("TotaisHoje", sw.Elapsed);

        return Ok(totais);
    }


    [HttpGet("todas-as-siumulacoes")]
    public async Task<IActionResult> TodasSimulacoes()
    {
        var sw = Stopwatch.StartNew();

        var lista = await _relatorioService.TodasAsync();

        sw.Stop();
        _telemetria.Registrar("TodasSimulacoes", sw.Elapsed);

        return Ok(lista);
    }


    [HttpGet("telemetriaAcumulativa")]
    public IActionResult TelemetriaAcumulada()
    {
        return Ok(_telemetria.ObterResumo());
    }

    [HttpGet("telemetria/servico")]
    public IActionResult TelemetriaServico()
    {
        var ultima = _telemetria.ObterUltimaExecucao("CriarSimulacao");

        if (ultima == null)
            return NotFound(new { Mensagem = "Nenhum registro encontrado." });

        return Ok(new { Servico = "CriarSimulacao", UltimaExecucaoMs = ultima });
    }
}
