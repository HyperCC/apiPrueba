using Aplicacion.Contratos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Seguridad.TokenSeguridad
{
    // obtener datos del usuario en sesion actual
    public class UsuarioSesion : IUsuarioSesion
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        // busqueda del usuario en sesion
        public string ObtenerUsuarioSesion()
        {
            var userName = this._httpContextAccessor.HttpContext.User?.Claims?.
                FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            return userName;
        }
    }
}
