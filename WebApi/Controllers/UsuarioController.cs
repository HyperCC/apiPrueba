using Aplicacion.ConfiguracionesLogin;
using Aplicacion.ConfiguracionesLogin.UsuarioLoginDTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Controllers.ControllerPersonalizado;

namespace WebApi.Controllers
{
    // controlador para operaciones con usaurios incluidos temas de LOGIN
    public class UsuarioController : PersonalControllerBase
    {
        //el endpoint para ejecutar este codigo es http://localhost:5000/api/Usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta parametros)
        {
            return await MediadorHerencia.Send(parametros);
        }

        // la url es http://localhost:5000/api/Usuario/registrar
        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> Registrar(Registrar.Ejecuta parametros)
        {
            return await MediadorHerencia.Send(parametros);
        }

        // ruta para obtener el usuario actualmente sesion http://localhost:5000/api/Usuario
        [HttpGet]
        public async Task<ActionResult<UsuarioData>> DevolverUsuarioActual()
        {
            return await MediadorHerencia.Send(new UsuarioActual.Ejecutar());
        }

        // actualizar los datos de un usuario ya registrado 
        [HttpPut]
        public async Task<ActionResult<UsuarioData>> Actualizar(UsuarioActualizar.Ejecuta parametros)
        {
            return await MediadorHerencia.Send(parametros);
        }
    }
}