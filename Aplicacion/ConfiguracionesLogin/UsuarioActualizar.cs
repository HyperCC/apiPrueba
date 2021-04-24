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
    public class UsuarioActualizar
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            // parametros para poder actualizar el usuario
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
        }

        public class EjecutaValidator : AbstractValidator<Ejecuta>
        {
            public EjecutaValidator()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }
        }

        // se procesan los datos en Ejecuta pero se devuelve un UsuarioData
        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            // obtener datos desde la DB
            private readonly CursosOnlineContext _context;
            // instancia del ususario de core identity
            private readonly UserManager<Usuario> _userManager;
            // interface permite crear nuevo token para el usuario 
            private readonly IJwtGenerador _jwtGenerator;
            // creador de hash para una nueva contraseña a actualizar
            private readonly PasswordHasher<Usuario> _passwordHasher;

            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerator, PasswordHasher<Usuario> passwordHasher)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _passwordHasher = passwordHasher;
            }

            // actualizar los datos del usuario implica cambiar el contenido del token
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuarioIdem = await this._userManager.FindByNameAsync(request.Nombre);
                // en caso de no encontrarse el usuario buscado por nombre
                if (usuarioIdem == null)
                {
                    throw new ManejadorExcepcion(
                        HttpStatusCode.NotFound,
                        new { mensaje = "No existe el usuario buscado por nombre en UsuarioActualizar" });
                }

                // en caso de ue el Email ya existe para otro usuario
                var restult = await this._context.Users.Where(x => x.Email == request.Email && x.UserName != request.Username).AnyAsync();
                if (restult)
                {
                    // si el Email ingresado existe no se podra continuar con la actualizacion del Usuario
                    throw new ManejadorExcepcion(HttpStatusCode.InternalServerError, new { mensaje = "El Email ingresado ya pertenece a otro usuario registrado en el metodo UsuarioActualizar" });
                }

                // actualizacion de los datos 
                usuarioIdem.NombreCompleto = request.Nombre + " " + request.Apellidos;
                usuarioIdem.PasswordHash = this._passwordHasher.HashPassword(usuarioIdem, request.Password);
                usuarioIdem.Email = request.Email;
                // resultado de la operacion de guardar los cambios en el usuario
                var resultadoUpdate = await this._userManager.UpdateAsync(usuarioIdem);

                // obtencion de todos los roles
                var rolesUsuario = (List<string>)await this._userManager.GetRolesAsync(usuarioIdem);

                if (resultadoUpdate.Succeeded)
                {
                    // devolver todos los datos ya generados y validados
                    return new UsuarioData
                    {
                        NombreCompleto = usuarioIdem.NombreCompleto,
                        UserName = usuarioIdem.UserName,
                        Email = usuarioIdem.Email,
                        Token = this._jwtGenerator.CreateToken(usuarioIdem, rolesUsuario)
                    };
                }

                // de haber un eror con la actualizacion de datos
                throw new Exception("No se pudo actualizar el Usuario. Error actualizando en UsuarioActualizar.");
            }
        }
    }
}
