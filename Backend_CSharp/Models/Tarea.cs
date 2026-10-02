namespace Backend_CSharp.Models;

public class Tarea
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public int? EmpleadoId { get; set; }
}