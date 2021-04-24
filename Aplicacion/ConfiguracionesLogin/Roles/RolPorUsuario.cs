using Aplicacion.ExcepcionesPersonalizadas;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.ConfiguracionesLogin.Roles
{
    public class RolPorUsuario
    {
        // devolver todos los roles para un usuario buscado por nombre
        public class Ejecuta : IRequest<List<string>>
        {
            // para devolver la lista de nombre se necesita el nombre del usuario
            public string Username { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, List<string>>
        {
            private readonly UserManager<Usuario> _userManager;
            // modelo de rol por defecto
            private readonly RoleManager<IdentityRole> _roleManager;
            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<List<string>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // buscar al usuairo por us username y verificar que exista
                var usuarioIdem = await this._userManager.FindByNameAsync(request.Username);
                if (usuarioIdem == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No existe el usuario buscado por nombre en ObtenerRolesPorUsuario" });

                // obtener todos los roles asociados al usuario ya encontrado
                var results = await this._userManager.GetRolesAsync(usuarioIdem);
                return (List<string>)results;
            }
        }
    }
}
