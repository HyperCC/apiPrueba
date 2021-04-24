using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.ConfiguracionesLogin.Roles
{
    public class TodosRoles
    {
        public class Ejecuta : IRequest<List<IdentityRole>>
        {
            // no se agregarn parametros por que no se quiere filtrar, se necesita todos los parametros
        }

        public class Manejador : IRequestHandler<Ejecuta, List<IdentityRole>>
        {
            // variables para retornar todos los roles de la DB  
            public readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                this._context = context;
            }

            // devolucion de todos los roles 
            public async Task<List<IdentityRole>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var roles = await this._context.Roles.ToListAsync();
                return roles;
            }
        }
    }
}
