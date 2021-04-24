using Aplicacion.ConfiguracionesLogin.UsuarioLoginDTO;
using Aplicacion.Contratos;
using Aplicacion.ExcepcionesPersonalizadas;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.ConfiguracionesLogin
{
    public class Login
    {
        // obtener los datos de UsuarioData que se requieren para hacer el login
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        // validar los datos de entrada del ususario
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _usuarioManager;
            private readonly SignInManager<Usuario> _signInManager;
            private readonly IJwtGenerador _jwtGenerator;

            public Manejador(UserManager<Usuario> userManager,
                SignInManager<Usuario> signInManager,
                IJwtGenerador jwtGenerador)
            {
                this._usuarioManager = userManager;
                this._signInManager = signInManager;
                this._jwtGenerator = jwtGenerador;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await this._usuarioManager.FindByEmailAsync(request.Email);

                // verificar ue el usuario buscado exista
                if (usuario == null)
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);

                var resultado = await this._signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                // obtener lista de roles para el objeto usuario "usuario"
                var listaRoles = new List<string>(await this._usuarioManager.GetRolesAsync(usuario));

                // verificar si el password entregado coincide con el real
                if (resultado.Succeeded)
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = this._jwtGenerator.CreateToken(usuario, listaRoles),
                        UserName = usuario.UserName,
                        Email = usuario.Email,
                        Imagen = null
                    };

                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
            }
        }
    }
}
