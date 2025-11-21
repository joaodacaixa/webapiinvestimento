using System.ComponentModel.DataAnnotations;

namespace webapiinvestimento.Models
{
    public class produtoInvestimento
    {
        [Key]
        public int ID_PRODUTO { get; set; }
        public string? NOME_PRODUTO { get; set; }
        public string? TIPO_PRODUTO { get; set; }
        public string? RISCO_PRODUTO { get; set; }
        public decimal RENTABILIDADE { get; set; }

    }
}
