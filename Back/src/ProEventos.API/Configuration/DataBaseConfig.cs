using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Infra.Data.Context;

namespace ProEventos.API.Configuration
{
    public class DataBaseConfig: DbContext
    {
    //     public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    //     {
    //         if (services == null) throw new ArgumentNullException(nameof(services));

    //    //     services.AddDbContext<DataContext>(options =>
    //    //         options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    //         // services.AddDbContext<EventStoreSqlContext>(options =>
    //         //     options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    //     }        
    }
}