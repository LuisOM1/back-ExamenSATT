using ExamenSATT.Models;

namespace ExamenSATT.Interfaces
{
    public interface IEmpleadoRepository
    {
        // El repositorio devuelve la data cruda (Modelo)
        Task<IEnumerable<EmpleadoModel>> GetAllEmpleadosAsync(CancellationToken ct);
        Task<EmpleadoModel?> GetEmpleadoByIdAsync(int id);
        Task<bool> RegisterEmpleadoAsync(EmpleadoModel model);
        Task<bool> UpdateEmpleadoAsync(int id, EmpleadoModel model);
        Task<bool> DeleteEmpleadoAsync(int id);
    }
}
