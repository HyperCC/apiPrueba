using Dominio;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    // clase en forma de seeder para subir datos en la DB 
    public class DataSeeder
    {
        // metodo para insertar datos cuando la DB este vacia
        // siempre es necesario obtener los datos desde el context. UserManager es opcional.
        public static async Task InsertarData(CursosOnlineContext context, UserManager<Usuario> usuarioManager)
        {
            // en caso de no haber usuarios en la DB
            if (usuarioManager.Users.Any() == false)
            {
                // creacion de nuevo usuario para poblar la DB
                var usuario = new Usuario
                {
                    NombreCompleto = "Domain Belchello",
                    UserName = "domainB",
                    Email = "belchello@gmail.com"
                };

                await usuarioManager.CreateAsync(usuario, "P@ssw0rd");
            }
        }
    }
}
