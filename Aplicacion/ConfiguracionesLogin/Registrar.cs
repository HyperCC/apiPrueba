using Aplicacion.ConfiguracionesLogin.UsuarioLoginDTO;
using Aplicacion.Contratos;
using Aplicacion.ExcepcionesPersonalizadas;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.ConfiguracionesLogin
{
    public class Registrar
    {
        // obtener los datos de los usuarios unicamente para registrar uno nuevo
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string NombreCompleto { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
        }

        // todas las validaciones necesarias 
        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador()
            {
                RuleFor(x => x.NombreCompleto).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            // variables necesarias para generar un registro de nuevo usuario 
            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;

            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador)
            {
                this._context = context;
                this._userManager = userManager;
                this._jwtGenerador = jwtGenerador;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // verificar que el email sea unico o no exista ya en la DB
                var existe = await this._context.Users.Where(x => x.Email == request.Email).AnyAsync();
                if (existe)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest,
                        new { mensaje = "Ya existe un usuario registrado con este Email" });
                }

                // verificar que el username sea unico o no exista ya en la DB
                var existeUsername = await this._context.Users.Where(x => x.UserName == request.Username).AnyAsync();
                if (existeUsername)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest,
                        new { mensaje = "Ya existe un usuario con ese Username" });
                }

                // se crea un nuevo Usuario con los datos para entregar como UsuarioData
                var usuario = new Usuario
                {
                    NombreCompleto = request.NombreCompleto,
                    Email = request.Email,
                    UserName = request.Username
                };

                // verificar si se pudo crear el UsuarioData y retornarlo
                var resultado = await this._userManager.CreateAsync(usuario, request.Password);
                if (resultado.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = this._jwtGenerador.CreateToken(usuario, null),
                        UserName = usuario.UserName,
                        Email = usuario.Email
                    };
                }

                throw new Exception("No se pudo agregar el nuevo usuario");
            }
        }
    }
}
