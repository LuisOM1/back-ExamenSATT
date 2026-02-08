using ExamenSATT.DTOs;
using ExamenSATT.Interfaces;
using ExamenSATT.Models;
using AutoMapper;

namespace ExamenSATT.Services
{
    // El Servicio(Lógica y Mapping),usamos Primary Constructor(repository y mapper se inyectan
    // directamente)
    // Aquí es donde AutoMapper brilla, convirtiendo el DTO que llega del cliente a Modelo, y viceversa.
    // Actualizamos el primary constructor para incluir el iloger
    public class EmpleadoService(IEmpleadoRepository repository, IMapper mapper, ILogger<EmpleadoService> 
        logger) : IEmpleadoService
    {
        public async Task<IEnumerable<EmpleadoDTO>> GetAllEmpleadosAsync(CancellationToken ct)
        {
            logger.LogInformation("Consultando lista completa de empleados");
            var empleados = await repository.GetAllEmpleadosAsync(ct);
            // ¡Adiós al .Select(e => new EmpleadoDTO { ... }) manual!
            // AutoMapper se encarga de todo el arreglo automáticamente
            return mapper.Map<IEnumerable<EmpleadoDTO>>(empleados);
        }

        public async Task<EmpleadoDTO?> GetEmpleadoByIdAsync(int id)
        {
            logger.LogInformation("Consultando empleado ID: {Id} ", id);
            var data = await repository.GetEmpleadoByIdAsync(id);
            return data == null ? null : mapper.Map<EmpleadoDTO>(data);
        }

        public async Task<bool> RegisterEmpleadoAsync(EmpleadoCreateUpdateDTO dto)
        {
            // Usamos {@dto} para que Serilog serialize el objeto a JSON en el log
            logger.LogInformation("Iniciando registro de empleado: {@EmpleadoDTO}", dto);
            var model = mapper.Map<EmpleadoModel>(dto);
            return await repository.RegisterEmpleadoAsync(model);
        }

        public async Task<bool> UpdateEmpleadoAsync(int id, EmpleadoCreateUpdateDTO dto)
        {
            logger.LogInformation("Actualizando empleado ID: {Id} con datos: {@Datos}", id, dto);
            var model = mapper.Map<EmpleadoModel>(dto);
            return await repository.UpdateEmpleadoAsync(id, model);
        }

        public async Task<bool> DeleteEmpleadoAsync(int id)
        {
            logger.LogWarning("Se solicitó eliminar/inactivar al empleado con ID: {Id}", id);
            return await repository.DeleteEmpleadoAsync(id);
        }

    }
}