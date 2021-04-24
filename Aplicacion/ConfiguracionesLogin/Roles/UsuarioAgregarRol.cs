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

namespace Aplicacion.ConfiguracionesLogin.Roles
{
    // agregar roles a un usuario comun
    public class UsuarioAgregarRol
    {
        public class Ejecuta : IRequest
        {
            // nombre del usuario y el nombre del rol para agregar al usuario
            public string Username { get; set; }
            public string RolNombre { get; set; }
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador()
            {
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.RolNombre).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly UserManager<Usuario> _userManager;
            // modelo de rol por defecto
            private readonly RoleManager<IdentityRole> _roleManager;

            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // verificar que el rol exista
                var role = await this._roleManager.FindByNameAsync(request.RolNombre);
                if (role == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "el rol buscado por nombre en UsuarioRolAgregar no existe" });

                // verificar que el usuario exista
                var usuarioIden = await this._userManager.FindByNameAsync(request.Username);
                if (usuarioIden == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "el usuario buscado por nombre en UsuarioRolAgregar no existe" });

                // agregar el rol al usuario (al objeto usuario se le agrega el rol por nombre )
                var result = await this._userManager.AddToRoleAsync(usuarioIden, request.RolNombre);
                
                // en caso de resultar exitosa la operacion de agregar el rol al usuario
                if (result.Succeeded)
                    return Unit.Value;

                throw new Exception("No se pudo agregar el Rol al Usuario");
            }
        }
    }
}
