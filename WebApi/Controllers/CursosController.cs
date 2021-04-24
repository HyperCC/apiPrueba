using Aplicacion.Cursos;
using Aplicacion.PaginacionPorModelo;
using Dominio;
using Dominio.ModelosDto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Paginacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Controllers.ControllerPersonalizado;

namespace WebApi.Controllers
{
    // acceso mediante http://localhost:5000/api/Cursos
    public class CursosController : PersonalControllerBase
    {
        // obtener datos por http://localhost:5000/api/Cursos
        [HttpGet]
        public async Task<ActionResult<List<CursoDto>>> Get()
        {
            return await MediadorHerencia.Send(new Consulta.CursosTodos());
        }

        // obtener dato por http://localhost:5000/api/Cursos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDto>> GetPorId(Guid id)
        {
            return await this.MediadorHerencia.Send(new CursoPorId.CursoUnico { Id = id });
        }

        // mandar parametros por http://localhost:5000/api/Cursos
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(NuevoCurso.Ejecuta data)
        {
            return await this.MediadorHerencia.Send(data);
        }

        // mandar parametros por http://localhost:5000/api/Cursos
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(Guid id, EditarCurso.Ejecuta datos)
        {
            datos.CursoId = id;
            return await this.MediadorHerencia.Send(datos);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Elimiar(Guid id)
        {
            return await this.MediadorHerencia.Send(new EliminarCurso.Ejecuta { Id = id });
        }

        // obtener la paginacion para algun modelo
        [HttpPost("report")]
        public async Task<ActionResult<PaginacionModel>> Report(PaginacionCurso.Ejecuta datos)
        {
            // el numero de paginas y la cantidad de elementos siempre se revuelven
            return await MediadorHerencia.Send(datos);
        }
    }
}
