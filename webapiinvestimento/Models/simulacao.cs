using System.ComponentModel.DataAnnotations;

namespace webapiinvestimento.Models
{
    public class simulacao
    {
         
        [Key]
        public int ID_SIMULACAO { get; set; }
        public int ID_CLIENTE { get; set; }
        public string? NOME_PRODUTO { get; set; }
        public decimal VALOR_INVESTIMENTO { get; set; }
        public decimal VALOR_FINAL { get; set; }
        public int PRAZO { get; set; }
        public DateTime DATA_SIMULACAO { get; set; }
    }
}

