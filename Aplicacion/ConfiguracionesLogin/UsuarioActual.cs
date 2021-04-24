using Aplicacion.ConfiguracionesLogin.UsuarioLoginDTO;
using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.ConfiguracionesLogin
{
    public class UsuarioActual
    {
        // no requiere de parametros por que no hay necesidad de filtrar datos 
        public class Ejecutar : IRequest<UsuarioData> { }

        public class Manejador : IRequestHandler<Ejecutar, UsuarioData>
        {
            // instancia de variables necesarias para obtener la sesion de un usuario
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IUsuarioSesion _usuarioSesion;

            public Manejador(UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IUsuarioSesion sesion)
            {
                this._userManager = userManager;
                this._jwtGenerador = jwtGenerador;
                this._usuarioSesion = sesion;
            }

            // devolver los datos del usuario actual segun el modelo UserData
            public async Task<UsuarioData> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                var usuario = await this._userManager.FindByNameAsync(this._usuarioSesion.ObtenerUsuarioSesion());

                // obtener lista de roles para el objeto usuario "usuario"
                var listaRoles = new List<string>(await this._userManager.GetRolesAsync(usuario));

                return new UsuarioData
                {
                    NombreCompleto = usuario.NombreCompleto,
                    UserName = usuario.UserName,
                    Token = this._jwtGenerador.CreateToken(usuario, listaRoles),
                    Imagen = null,
                    Email = usuario.Email
                };
            }
        }
    }
}
