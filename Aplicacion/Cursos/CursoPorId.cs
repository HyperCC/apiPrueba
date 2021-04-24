using Aplicacion.ExcepcionesPersonalizadas;
using AutoMapper;
using Dominio;
using Dominio.ModelosDto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// el directorio Cursos indica actividades para con el modelo Curso
namespace Aplicacion.Cursos
{
    // obtener un curso guardado de la DB
    public class CursoPorId
    {
        public class CursoUnico : IRequest<CursoDto>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<CursoUnico, CursoDto>
        {
            // instancia de la DB
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<CursoDto> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                // encontrar un curso y para tal curso obtener todos los instructores relacionados  
                var curso = await this._context.Curso
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.PrecioPromocion)
                    .Include(x => x.InstructoresLinks)
                    .ThenInclude(y => y.Instructor)
                    .FirstOrDefaultAsync(a => a.CursoId == request.Id);

                // en caso de no existir el curso
                if (curso == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el curso buscado" });

                // mapeo desde modelo curso a cursoDTO
                var cursoDTO = this._mapper.Map<Curso, CursoDto>(curso);

                return cursoDTO;
            }
        }

    }
}
