using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;

namespace ProEventos.Infra.Data.Context
{
    public class ProEventosContext : DbContext
    {
        public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options) { }
        public DbSet<Evento> eventos { get; set; }
        public DbSet<Lote> lotes { get; set; }
        public DbSet<Palestrante> palestrantes { get; set; }
        public DbSet<PalestranteEvento> palestrantes_eventos { get; set; }
        public DbSet<RedeSocial> redes_sociais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<PalestranteEvento>()
            .HasKey(pe => new {pe.EventoId, pe.PalestranteId});
        }
    }
}