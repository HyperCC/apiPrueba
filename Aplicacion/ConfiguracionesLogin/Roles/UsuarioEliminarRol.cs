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
    public class UsuarioEliminarRol
    {
        public class Ejecuta : IRequest
        {
            // nombres de usuario y rol para buscarlos por esos parametros
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
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "el rol buscado por nombre en UsuarioRolEliminar no existe" });

                // verificar que el usuario exista
                var usuarioIden = await this._userManager.FindByNameAsync(request.Username);
                if (usuarioIden == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "el usuario buscado por nombre en UsuarioRolEliminar no existe" });

                // eliminar el rol del ussuario segun el objeto de usuario y el nombre del rol
                var result = await this._userManager.RemoveFromRoleAsync(usuarioIden, request.RolNombre);
                // en caso de resultar exitosa la operacion de elimiar un rol del usuario
                if (result.Succeeded)
                    return Unit.Value;

                throw new Exception("No se pudo agregar el Rol al Usuario");
            }
        }
    }
}
