using System.ComponentModel.DataAnnotations;

namespace personapi_dotnet.Models.DTOs;

/// <summary>Datos de un teléfono.</summary>
public class TelefonoDto
{
    /// <summary>Número telefónico (llave primaria). Solo dígitos, opcionalmente con + inicial.</summary>
    [Required(ErrorMessage = "El número es obligatorio.")]
    [StringLength(15, ErrorMessage = "El número admite máximo 15 caracteres.")]
    [RegularExpression(@"^\+?\d{7,14}$", ErrorMessage = "El número solo puede contener dígitos (7 a 14), con un '+' opcional al inicio.")]
    [Display(Name = "Número")]
    public string Num { get; set; } = string.Empty;

    [Required(ErrorMessage = "El operador es obligatorio.")]
    [StringLength(45, ErrorMessage = "El operador admite máximo 45 caracteres.")]
    [Display(Name = "Operador")]
    public string Oper { get; set; } = string.Empty;

    /// <summary>Cédula de la persona dueña del teléfono.</summary>
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar el dueño del teléfono.")]
    [Display(Name = "Dueño (cédula)")]
    public int Duenio { get; set; }

    /// <summary>Nombre completo del dueño (solo lectura).</summary>
    [Display(Name = "Dueño")]
    public string? DuenioNombre { get; set; }
}
