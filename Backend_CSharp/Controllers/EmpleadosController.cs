using Backend_CSharp.Data;
using Backend_CSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_CSharp.Controllers;

[ApiController]
[Route("empleados")]
public class EmpleadosController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmpleadosController(AppDbContext context)
    {
        _context = context;
    }

    // GET /empleados
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
    {
        var empleados = await _context.Empleados
            .OrderBy(e => e.Id)
            .ToListAsync();

        return Ok(empleados);
    }

    // GET /empleados/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Empleado>> GetEmpleado(int id)
    {
        var empleado = await _context.Empleados
            .FirstOrDefaultAsync(e => e.Id == id);

        if (empleado == null)
        {
            return NotFound(new
            {
                error = "Empleado no encontrado"
            });
        }

        return Ok(empleado);
    }

    // POST /empleados
    [HttpPost]
    public async Task<ActionResult<Empleado>> CrearEmpleado(
        [FromBody] Empleado empleado)
    {
        if (string.IsNullOrWhiteSpace(empleado.Nombre) ||
            string.IsNullOrWhiteSpace(empleado.Especialidad))
        {
            return BadRequest(new
            {
                error = "Nombre y especialidad son obligatorios"
            });
        }

        _context.Empleados.Add(empleado);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEmpleado),
            new { id = empleado.Id },
            empleado
        );
    }

    // PUT /empleados/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Empleado>> ActualizarEmpleado(
        int id,
        [FromBody] Empleado empleadoActualizado)
    {
        var empleado = await _context.Empleados
            .FirstOrDefaultAsync(e => e.Id == id);

        if (empleado == null)
        {
            return NotFound(new
            {
                error = "Empleado no encontrado"
            });
        }

        if (string.IsNullOrWhiteSpace(empleadoActualizado.Nombre) ||
            string.IsNullOrWhiteSpace(empleadoActualizado.Especialidad))
        {
            return BadRequest(new
            {
                error = "Nombre y especialidad son obligatorios"
            });
        }

        empleado.Nombre = empleadoActualizado.Nombre;
        empleado.Especialidad = empleadoActualizado.Especialidad;

        await _context.SaveChangesAsync();

        return Ok(empleado);
    }

    // DELETE /empleados/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarEmpleado(int id)
    {
        var empleado = await _context.Empleados
            .FirstOrDefaultAsync(e => e.Id == id);

        if (empleado == null)
        {
            return NotFound(new
            {
                error = "Empleado no encontrado"
            });
        }

        _context.Empleados.Remove(empleado);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new
            {
                error = "No se puede eliminar el empleado porque tiene datos relacionados"
            });
        }

        return NoContent();
    }
}