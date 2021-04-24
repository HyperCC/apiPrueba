using Aplicacion.Instructores;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Controllers.ControllerPersonalizado;

namespace WebApi.Controllers
{
    public class InstructorController : PersonalControllerBase
    {
        // notar que por parametros o durante la funcion siempre se llaman otra funciones para devolver
        // los datos en json, nunca se pasan datos en crudo a excepsion del ID

        [Authorize(Roles = "Admin")] // autorizacion por roles de los usuarios - solo para admn
        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> ObtenerInstructores()
        {
            return await MediadorHerencia.Send(new TodosInstructores.Lista());
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await MediadorHerencia.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Actualizar(Guid id, Editar.Ejecuta datos)
        {
            datos.InstructorId = id;
            return await MediadorHerencia.Send(datos);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {
            // recordar ue siempre se pasa un json por el .Send() sino, no lo poria leer.
            return await MediadorHerencia.Send(
                new Eliminar.Ejecuta { id = id }
                );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorModel>> ObtenerPorId(Guid id)
        {
            return await MediadorHerencia.Send(new BuscarPorId.Ejecuta { id = id });
        }
    }
}
