using ExamenSATT.DTOs;

namespace ExamenSATT.Interfaces
{
    public interface IEmpleadoService
    {
        // El servicio devuelve la data procesada (DTO)
        Task<IEnumerable<EmpleadoDTO>> GetAllEmpleadosAsync(CancellationToken ct);
        Task<EmpleadoDTO?> GetEmpleadoByIdAsync(int id);
        Task<bool> RegisterEmpleadoAsync(EmpleadoCreateUpdateDTO dto);
        Task<bool> UpdateEmpleadoAsync(int id, EmpleadoCreateUpdateDTO dto);
        Task<bool> DeleteEmpleadoAsync(int id);

    }
}
