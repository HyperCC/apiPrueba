using Dominio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostserver = CreateHostBuilder(args).Build();

            // agregar las migraciones creadas con EF
            using (var ambiente = hostserver.Services.CreateScope())
            {
                var services = ambiente.ServiceProvider;
                try
                {
                    // obetener el usermanager para el modelo Usuario
                    var usuarioManager = services.GetRequiredService<UserManager<Usuario>>();

                    // llamar el uso de CursosOnlineContext
                    var context = services.GetRequiredService<CursosOnlineContext>();
                    // hacer la migracion directamente a la DB
                    context.Database.Migrate();

                    // instancia del seeder de datos creado en Persistencia
                    DataSeeder.InsertarData(context, usuarioManager).Wait();
                }
                catch (Exception ex)
                {
                    // lanzar log con los errores encontrados durante la migracion en la DB
                    var logging = services.GetRequiredService<ILogger<Program>>();
                    logging.LogError(ex, "Ocurrio un error durante la migracion y no se efectuó correctamente.");
                }

                hostserver.Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
