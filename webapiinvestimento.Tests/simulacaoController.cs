using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using webapiinvestimento.Context;
using webapiinvestimento.Models;
using webapiinvestimento.Controllers;
using webapiinvestimento.DTO;
using Microsoft.AspNetCore.Mvc;
using apiHackthon.Service;
using webapiinvestimento.Data;

public class SimulacaoControllerTests
{
    private simulacaoContext CreateSimContext()
    {
        var options = new DbContextOptionsBuilder<simulacaoContext>()
            .UseInMemoryDatabase(databaseName: "SimDB")
            .Options;

        return new simulacaoContext(options);
    }

    private produtoContext CreateProdContext()
    {
        var options = new DbContextOptionsBuilder<produtoContext>()
            .UseInMemoryDatabase(databaseName: "ProdDB")
            .Options;

        return new produtoContext(options);
    }

    [Fact]
    public async Task CriarSimulacao_DeveRetornarNotFound_SeProdutoNaoExiste()
    {
        var simContext = CreateSimContext();
        var prodContext = CreateProdContext();
        var simService = new SimulacaoService(simContext, prodContext);
        var perfilService = new PerfilRiscoService(simContext, prodContext);
        var relatorioService = new RelatorioSimulacaoService(simContext);
        var telemetriaMock = new TelemetriaService();

         var controller = new SimulacaoController(
                        simService,
                        perfilService,
                        relatorioService,
                        telemetriaMock
                        );

        var request = new SimulacaoRequest
        {
            IdCliente = 1,
            Valor = 1000,
            Prazo = 10,
            TipoInvestimento = "Inexistente"
        };

        var result = await controller.CriarSimulacao(request);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task CriarSimulacao_DeveCalcularValorFinalCorretamente()
    {
        var simContext = CreateSimContext();
        var prodContext = CreateProdContext();

        prodContext.produtoInvestimento.Add(new produtoInvestimento
        {
            ID_PRODUTO = 1,
            TIPO_PRODUTO = "CDB",
            NOME_PRODUTO = "CDB Prime",
            RENTABILIDADE = 0.02m
        });
        prodContext.SaveChanges();

       
        var simService = new SimulacaoService(simContext, prodContext);
        var perfilService = new PerfilRiscoService(simContext, prodContext);
        var relatorioService = new RelatorioSimulacaoService(simContext);
        var telemetriaMock = new TelemetriaService();
        var controller = new SimulacaoController(simService,perfilService,relatorioService,telemetriaMock);

        var request = new SimulacaoRequest
        {
            IdCliente = 1,
            Valor = 1000,
            Prazo = 10,
            TipoInvestimento = "CDB"
        };

        decimal esperado = 1000 + (1000 * 10 * 0.02m);
        var result = await controller.CriarSimulacao(request) as OkObjectResult;
        var json = System.Text.Json.JsonSerializer.Serialize(result.Value);
        var obj = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        var resultadoSimulacaoJson = obj["resultadoSimulacao"].ToString();
        var resultado = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(resultadoSimulacaoJson);
        decimal valorFinal = ((System.Text.Json.JsonElement)resultado["valorFinal"]).GetDecimal();


        Assert.Equal(esperado, valorFinal);
    }
}

