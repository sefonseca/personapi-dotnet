using System.ComponentModel.DataAnnotations;

namespace personapi_dotnet.Models.DTOs;

/// <summary>Datos de una profesión.</summary>
public class ProfesionDto
{
    [Required(ErrorMessage = "El identificador es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El identificador debe ser un número positivo.")]
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(90, ErrorMessage = "El nombre admite máximo 90 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nom { get; set; } = string.Empty;

    [Display(Name = "Descripción")]
    public string? Des { get; set; }
}
