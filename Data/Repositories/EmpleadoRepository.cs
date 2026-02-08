using ExamenSATT.Interfaces;
using ExamenSATT.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamenSATT.Data.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoRepository(ApplicationDbContext context) => _context = context;

        // Soporte para Cancelación (Pro++)
        // Esto permite que si el cliente cancela la petición (ej. cierra el navegador), la consulta a la
        // base de datos se detenga inmediatamente gracias al CancellationToken.
        // Todos los metodos async deben aceptar CancellationToken eso es lo Pro. Falta Implementar
        public async Task<IEnumerable<EmpleadoModel>> GetAllEmpleadosAsync(CancellationToken ct)
        {
            // Ejecución pro: EF Core mapea los resultados del SP automáticamente a tu modelo
            // Incluso si 'name_area' es [NotMapped],EF lo llenará si el SP devuelve una columna con ese nombre.
            // No está mal usar ADO.NET, pero EF Core es el estándar moderno de Microsoft
            return await _context.Empleados
                .FromSqlRaw("EXEC SP_LIST_ALL_EMPL")
                .AsNoTracking() // Mejora el rendimiento para consultas de solo lectura
                .ToListAsync(ct);
        }
        
        public async Task<EmpleadoModel?> GetEmpleadoByIdAsync(int id)
        {
            var result = await _context.Empleados
                .FromSqlInterpolated($"EXEC SP_LIST_EMPL @id_empl = {id}")
                .AsNoTracking()
                .ToListAsync();
            return result.FirstOrDefault();
        }

        public async Task<bool> RegisterEmpleadoAsync(EmpleadoModel model)
        {
            try
            {
                var result = await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC SP_REGI_EMPL @id_area={model.id_area}, @nomb_empl={model.nomb_empl}, @ape_empl={model.ape_empl}, @emai_empl={model.emai_empl}, @suel_empl={model.suel_empl}, @fech_ingr={model.fech_ingr}");
                return result != 0;
            }
            catch (Exception ex)
            {
                // 1 dev SR no pone try catch en todos los metodos solo donde es necesario
                throw new Exception("Error en SQL: " + ex.Message);
            }
        }

        public async Task<bool> UpdateEmpleadoAsync(int id, EmpleadoModel model)
        {
            var result = await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC SP_EDIT_EMPL @id_empl={id}, @id_area={model.id_area}, @nomb_empl={model.nomb_empl}, @ape_empl={model.ape_empl}, @emai_empl={model.emai_empl}, @suel_empl={model.suel_empl}, @fech_ingr={model.fech_ingr}");
            return result != 0;
        }

        public async Task<bool> DeleteEmpleadoAsync(int id)
        {
            var result = await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC SP_ELIM_EMPL @id_empl={id}");
            return result != 0;
        }



        // ADO.NET
        // El Repositorio "Pro" (Adiós ADO.NET manual)
        // Elimina el uso de SqlConnection manual. Si usas Entity Framework, úsalo de verdad.
        // Esto es más seguro contra inyecciones SQL y mucho más corto.
        // Mapeo manual: Hacer reader.GetString("columna") línea por línea es propenso a errores.
        // Si cambias el nombre de una columna en la DB, tu código explota y no te enteras hasta ejecutarlo
        
        //  Resumen de la Implementación Final
        //  Repository: Recibe y devuelve EmpleadoModel. Ejecuta los Stored Procedures usando
        //  ExecuteSqlInterpolatedAsync para mayor seguridad.
        //  Service: Recibe EmpleadoCreateUpdateDTO, usa AutoMapper para convertirlo a EmpleadoModel, llama
        //  al repositorio y, si es una consulta, devuelve un EmpleadoDTO.
        //  Controller: Solo inyecta IEmpleadoService. No conoce la BD, solo conoce los contratos de negocio
        //  Ojo: El code ya esta robusto pero los SP pueden fallar por restricciones de llave foránea o
        //  datos duplicados.

    }
}
