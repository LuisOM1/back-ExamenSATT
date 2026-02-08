using ExamenSATT.DTOs;
using ExamenSATT.Services;
using ExamenSATT.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamenSATT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoService _service;

        public EmpleadoController(IEmpleadoService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<APIResponse<IEnumerable<EmpleadoDTO>>>> Get(CancellationToken ct)
        {
            var result = await _service.GetAllEmpleadosAsync(ct);
            return Ok(new APIResponse<IEnumerable<EmpleadoDTO>> { Success = true, Data = result, Message = "Empleados recuperados con éxito" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _service.GetEmpleadoByIdAsync(id);
            return res != null ? Ok(new APIResponse<EmpleadoDTO> { Success = true, Data = res, Message = "Obtenido correctamente"}) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmpleadoCreateUpdateDTO dto)
        {
            var success = await _service.RegisterEmpleadoAsync(dto);
            return success
                ? Ok(new APIResponse<EmpleadoCreateUpdateDTO> { Success = true, Data = dto, Message = "Empleado registrado" })
                : BadRequest(new APIResponse<string> { Success = false, Message = "No se pudo registrar" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EmpleadoCreateUpdateDTO dto)
        {
            var success = await _service.UpdateEmpleadoAsync(id, dto);
            return success ? Ok(new APIResponse<EmpleadoCreateUpdateDTO> { Success = true, Data = dto, Message = "Actualizado" }) : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteEmpleadoAsync(id);
            return success ? Ok(new APIResponse<int> { Success = true, Data = id, Message = "Eliminado" }) : BadRequest();
        }
    }
}
