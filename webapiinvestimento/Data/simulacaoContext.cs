using System.Collections.Generic;
using System.Reflection.Emit;
using System;
using webapiinvestimento.Models;
using Microsoft.EntityFrameworkCore;


namespace webapiinvestimento.Context
{
    public class simulacaoContext:DbContext
    {

        //contexto que salva as simulacoes de investimentos solicitadas
        public simulacaoContext(DbContextOptions<simulacaoContext> options)
                    : base(options) { }
        public DbSet<simulacao> simulacoesInvestimentos { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<simulacao>()
                .Property(e => e.ID_SIMULACAO)
                .ValueGeneratedOnAdd(); // simula autoincrement
        }


    }


}

