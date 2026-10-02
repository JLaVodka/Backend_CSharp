using Backend_CSharp.Data;
using Backend_CSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_CSharp.Controllers;

[ApiController]
[Route("tareas")]
public class TareasController : ControllerBase
{
    private readonly AppDbContext _context;

    public TareasController(AppDbContext context)
    {
        _context = context;
    }

    // GET /tareas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas()
    {
        var tareas = await _context.Tareas
            .OrderBy(t => t.Id)
            .ToListAsync();

        return Ok(tareas);
    }

    // GET /tareas/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Tarea>> GetTarea(int id)
    {
        var tarea = await _context.Tareas
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tarea == null)
        {
            return NotFound(new
            {
                error = "Tarea no encontrada"
            });
        }

        return Ok(tarea);
    }

    // POST /tareas
    [HttpPost]
    public async Task<ActionResult<Tarea>> CrearTarea(
        [FromBody] Tarea tarea)
    {
        if (string.IsNullOrWhiteSpace(tarea.Titulo) ||
            string.IsNullOrWhiteSpace(tarea.Descripcion) ||
            string.IsNullOrWhiteSpace(tarea.Estado))
        {
            return BadRequest(new
            {
                error = "Título, descripción y estado son obligatorios"
            });
        }

        if (tarea.EmpleadoId.HasValue)
        {
            var empleadoExiste = await _context.Empleados
                .AnyAsync(e => e.Id == tarea.EmpleadoId.Value);

            if (!empleadoExiste)
            {
                return BadRequest(new
                {
                    error = "El empleado indicado no existe"
                });
            }
        }

        _context.Tareas.Add(tarea);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTarea),
            new { id = tarea.Id },
            tarea
        );
    }

    // PUT /tareas/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Tarea>> ActualizarTarea(
        int id,
        [FromBody] Tarea tareaActualizada)
    {
        var tarea = await _context.Tareas
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tarea == null)
        {
            return NotFound(new
            {
                error = "Tarea no encontrada"
            });
        }

        if (string.IsNullOrWhiteSpace(tareaActualizada.Titulo) ||
            string.IsNullOrWhiteSpace(tareaActualizada.Descripcion) ||
            string.IsNullOrWhiteSpace(tareaActualizada.Estado))
        {
            return BadRequest(new
            {
                error = "Título, descripción y estado son obligatorios"
            });
        }

        if (tareaActualizada.EmpleadoId.HasValue)
        {
            var empleadoExiste = await _context.Empleados
                .AnyAsync(e => e.Id == tareaActualizada.EmpleadoId.Value);

            if (!empleadoExiste)
            {
                return BadRequest(new
                {
                    error = "El empleado indicado no existe"
                });
            }
        }

        tarea.Titulo = tareaActualizada.Titulo;
        tarea.Descripcion = tareaActualizada.Descripcion;
        tarea.Estado = tareaActualizada.Estado;
        tarea.EmpleadoId = tareaActualizada.EmpleadoId;

        await _context.SaveChangesAsync();

        return Ok(tarea);
    }

    // DELETE /tareas/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarTarea(int id)
    {
        var tarea = await _context.Tareas
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tarea == null)
        {
            return NotFound(new
            {
                error = "Tarea no encontrada"
            });
        }

        _context.Tareas.Remove(tarea);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}