using Aplicacion.ConfiguracionesLogin.Roles;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Controllers.ControllerPersonalizado;

namespace WebApi.Controllers
{
    public class RolController : PersonalControllerBase
    {
        // operaciones directamente sobre los roles
        [HttpPost("crear")]
        public async Task<ActionResult<Unit>> Crear(NuevoRol.Ejecuta parametros)
        {
            return await MediadorHerencia.Send(parametros);
        }

        [HttpDelete("eliminar")]
        public async Task<ActionResult<Unit>> Eliminar(EliminarRol.Ejecuta parametros)
        {
            return await MediadorHerencia.Send(parametros);
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<IdentityRole>>> Lista()
        {
            return await MediadorHerencia.Send(new TodosRoles.Ejecuta());
        }

        // operaciones para enrutar acciones de los usuario con los roles 
        [HttpPost("agregarRoleUsuario")]
        public async Task<ActionResult<Unit>> AgregarRoleUsuario(UsuarioAgregarRol.Ejecuta parametros)
        {
            return await MediadorHerencia.Send(parametros);
        }

        [HttpPost("elimiarRoleUsuario")]
        public async Task<ActionResult<Unit>> ElimiarRolUsuario(UsuarioEliminarRol.Ejecuta parametros)
        {
            return await MediadorHerencia.Send(parametros);
        }

        [HttpGet("{username}")] // obtener todos los roles asociados a un usuario
        public async Task<ActionResult<List<string>>> ObtenerRolesPorUsuario(string username)
        {
            return await MediadorHerencia.Send(new RolPorUsuario.Ejecuta { Username = username });
        }
    }
}
