using System.ComponentModel.DataAnnotations;

namespace personapi_dotnet.Models.DTOs;

/// <summary>Datos de una persona (entrada y salida de la API y de las vistas).</summary>
public class PersonaDto
{
    /// <summary>Cédula de ciudadanía (llave primaria).</summary>
    [Required(ErrorMessage = "La cédula es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cédula debe ser un número positivo.")]
    [Display(Name = "Cédula")]
    public int Cc { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(45, ErrorMessage = "El nombre admite máximo 45 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(45, ErrorMessage = "El apellido admite máximo 45 caracteres.")]
    public string Apellido { get; set; } = string.Empty;

    /// <summary>M = masculino, F = femenino.</summary>
    [Required(ErrorMessage = "El género es obligatorio.")]
    [RegularExpression("^[MF]$", ErrorMessage = "El género debe ser 'M' o 'F'.")]
    [Display(Name = "Género")]
    public string Genero { get; set; } = string.Empty;

    [Range(0, 130, ErrorMessage = "La edad debe estar entre 0 y 130.")]
    public int? Edad { get; set; }
}

/// <summary>Persona con sus teléfonos y estudios (detalle).</summary>
public class PersonaDetalleDto : PersonaDto
{
    public List<TelefonoDto> Telefonos { get; set; } = new();

    public List<EstudioDto> Estudios { get; set; } = new();
}
