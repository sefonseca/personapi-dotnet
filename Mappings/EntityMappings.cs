using personapi_dotnet.Models.DTOs;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Mappings;

/// <summary>
/// Conversión entre entidades (capa de datos) y DTOs (capa de presentación).
/// Mapeo manual: explícito, sin dependencias externas.
/// </summary>
public static class EntityMappings
{
    // ---------- Persona ----------
    public static PersonaDto ToDto(this Persona e) => new()
    {
        Cc = e.Cc,
        Nombre = e.Nombre,
        Apellido = e.Apellido,
        Genero = e.Genero,
        Edad = e.Edad
    };

    public static PersonaDetalleDto ToDetalleDto(this Persona e) => new()
    {
        Cc = e.Cc,
        Nombre = e.Nombre,
        Apellido = e.Apellido,
        Genero = e.Genero,
        Edad = e.Edad,
        Telefonos = e.Telefonos.Select(t => t.ToDto()).ToList(),
        Estudios = e.Estudios.Select(s => s.ToDto()).ToList()
    };

    public static Persona ToEntity(this PersonaDto d) => new()
    {
        Cc = d.Cc,
        Nombre = d.Nombre.Trim(),
        Apellido = d.Apellido.Trim(),
        Genero = d.Genero.Trim().ToUpperInvariant(),
        Edad = d.Edad
    };

    public static void ApplyTo(this PersonaDto d, Persona e)
    {
        e.Nombre = d.Nombre.Trim();
        e.Apellido = d.Apellido.Trim();
        e.Genero = d.Genero.Trim().ToUpperInvariant();
        e.Edad = d.Edad;
    }

    // ---------- Profesion ----------
    public static ProfesionDto ToDto(this Profesion e) => new()
    {
        Id = e.Id,
        Nom = e.Nom,
        Des = e.Des
    };

    public static Profesion ToEntity(this ProfesionDto d) => new()
    {
        Id = d.Id,
        Nom = d.Nom.Trim(),
        Des = d.Des?.Trim()
    };

    public static void ApplyTo(this ProfesionDto d, Profesion e)
    {
        e.Nom = d.Nom.Trim();
        e.Des = d.Des?.Trim();
    }

    // ---------- Telefono ----------
    public static TelefonoDto ToDto(this Telefono e) => new()
    {
        Num = e.Num,
        Oper = e.Oper,
        Duenio = e.Duenio,
        DuenioNombre = e.DuenioNavigation is null
            ? null
            : $"{e.DuenioNavigation.Nombre} {e.DuenioNavigation.Apellido}"
    };

    public static Telefono ToEntity(this TelefonoDto d) => new()
    {
        Num = d.Num.Trim(),
        Oper = d.Oper.Trim(),
        Duenio = d.Duenio
    };

    public static void ApplyTo(this TelefonoDto d, Telefono e)
    {
        e.Oper = d.Oper.Trim();
        e.Duenio = d.Duenio;
    }

    // ---------- Estudio ----------
    public static EstudioDto ToDto(this Estudio e) => new()
    {
        IdProf = e.IdProf,
        CcPer = e.CcPer,
        Fecha = e.Fecha,
        Univer = e.Univer,
        ProfesionNombre = e.IdProfNavigation?.Nom,
        PersonaNombre = e.CcPerNavigation is null
            ? null
            : $"{e.CcPerNavigation.Nombre} {e.CcPerNavigation.Apellido}"
    };

    public static Estudio ToEntity(this EstudioDto d) => new()
    {
        IdProf = d.IdProf,
        CcPer = d.CcPer,
        Fecha = d.Fecha,
        Univer = d.Univer?.Trim()
    };

    public static void ApplyTo(this EstudioDto d, Estudio e)
    {
        e.Fecha = d.Fecha;
        e.Univer = d.Univer?.Trim();
    }
}
