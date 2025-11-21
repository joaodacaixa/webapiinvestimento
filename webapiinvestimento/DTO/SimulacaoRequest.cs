namespace webapiinvestimento.DTO
{
    public class SimulacaoRequest
    {
        public int IdCliente { get; set; }
        public decimal Valor { get; set; }
        public int Prazo { get; set; }
        public string TipoInvestimento { get; set; }
    }
}
