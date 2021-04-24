using Aplicacion.ExcepcionesPersonalizadas;
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
    public class EliminarRol
    {
        public class Ejecuta : IRequest
        {
            // parametro para un rol
            public string Nombre { get; set; }
        }

        public class EjecutaValida : AbstractValidator<Ejecuta>
        {
            public EjecutaValida()
            {
                RuleFor(x => x.Nombre).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            public Manejador(RoleManager<IdentityRole> roleManager)
            {
                this._roleManager = roleManager;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // buscar el rol
                var role = await this._roleManager.FindByNameAsync(request.Nombre);
                // en caso de no existir el rol
                if (role == null)
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "No existe el rol a eliminar" });

                // eliminacion del rol encontrado
                var result = await this._roleManager.DeleteAsync(role);

                // operacion de eliminacion completada correctamente
                if (result.Succeeded)
                    return Unit.Value;

                throw new Exception("No se pudo elimiar el rol");
            }
        }
    }
}
