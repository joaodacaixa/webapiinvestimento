namespace apiHackthon.Service
{
    //service criada para fins de gerar informações de telemetria
    public class TelemetriaService
    {
        private readonly List<TelemetriaRegistro> _registros = new();
        private readonly Dictionary<string, TimeSpan> _ultimaExecucao = new();

        public void Registrar(string servico, TimeSpan tempoExecucao)
        {
            _registros.Add(new TelemetriaRegistro
            {
                Servico = "Simulação",// servico,
                TempoExecucao = tempoExecucao,
                DataHora = DateTime.UtcNow
            });
            _ultimaExecucao[servico] = tempoExecucao;
        }

        public IEnumerable<object> ObterResumo()
        {
            return _registros
                .GroupBy(r => r.Servico)
                .Select(g => new
                {
                    Servico = g.Key,
                    TotalChamadas = g.Count(),
                    TempoMedioMs = g.Average(r => r.TempoExecucao.TotalMilliseconds),
                    TempoMaxMs = g.Max(r => r.TempoExecucao.TotalMilliseconds)
                })
                .ToList();
        }


        public double? ObterUltimaExecucao(string nomeServico)
        {
            if (_ultimaExecucao.ContainsKey(nomeServico))
                return _ultimaExecucao[nomeServico].TotalMilliseconds;
            return null;
        }

        public class TelemetriaRegistro
        {
            public string Servico { get; set; }
            public TimeSpan TempoExecucao { get; set; }
            public DateTime DataHora { get; set; }
        }

    }
}
