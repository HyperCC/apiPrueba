using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    // contratos a cumplir para las operaciones con Instructor medinte procedimiento almacenados
    public interface IInstructor
    {
        Task<IEnumerable<InstructorModel>> ObtenerLista();
        Task<InstructorModel> ObtenerPorId(Guid id);

        // estos metodos deben retornar tipo int porue la DB retorna la lista de transacciones exitosas (un numero)
        Task<int> Nuevo(string nombre, string apellidos, string titulos);
        Task<int> Editar(Guid instructorId, string nombre, string apellidos, string titulo);
        Task<int> Eliminar(Guid id);
    }
}
