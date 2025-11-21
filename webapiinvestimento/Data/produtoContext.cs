using Microsoft.EntityFrameworkCore;
using webapiinvestimento.Context;
using webapiinvestimento.Models;
using Microsoft.EntityFrameworkCore;

namespace webapiinvestimento.Data
{
    public class produtoContext:DbContext
    {

        //contexto que salva as simulacoes de investimentos solicitadas
        public produtoContext(DbContextOptions<produtoContext> options)
                   : base(options) { }
        public DbSet<produtoInvestimento> produtoInvestimento { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }


    }
}
