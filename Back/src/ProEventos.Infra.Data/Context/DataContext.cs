using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.API;

namespace ProEventos.Infra.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}
        public DbSet<Evento> Eventos { get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     if (!optionsBuilder.IsConfigured)
        //     {
        //         optionsBuilder.UseSqlite.UseMySql("Server=localhost;User Id=root;Password=coderp;Database=proeventos", new MySqlServerVersion(new Version(8, 0, 11)));
        //     }
        // }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {}     


    }
}