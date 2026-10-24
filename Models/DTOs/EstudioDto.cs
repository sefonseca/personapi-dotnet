using System.ComponentModel.DataAnnotations;

namespace personapi_dotnet.Models.DTOs;

/// <summary>Estudio de una persona en una profesión (llave compuesta IdProf + CcPer).</summary>
public class EstudioDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una profesión.")]
    [Display(Name = "Profesión")]
    public int IdProf { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una persona.")]
    [Display(Name = "Persona")]
    public int CcPer { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de grado")]
    public DateOnly? Fecha { get; set; }

    [StringLength(50, ErrorMessage = "La universidad admite máximo 50 caracteres.")]
    [Display(Name = "Universidad")]
    public string? Univer { get; set; }

    /// <summary>Nombre de la profesión (solo lectura).</summary>
    [Display(Name = "Profesión")]
    public string? ProfesionNombre { get; set; }

    /// <summary>Nombre completo de la persona (solo lectura).</summary>
    [Display(Name = "Persona")]
    public string? PersonaNombre { get; set; }
}
