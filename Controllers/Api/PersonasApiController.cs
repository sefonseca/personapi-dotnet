using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.DTOs;
using personapi_dotnet.Services.Interfaces;

namespace personapi_dotnet.Controllers.Api;

/// <summary>CRUD REST de personas.</summary>
[ApiController]
[Route("api/personas")]
[Produces("application/json")]
public class PersonasApiController : ControllerBase
{
    private readonly IPersonaService _service;

    public PersonasApiController(IPersonaService service)
    {
        _service = service;
    }

    /// <summary>Lista todas las personas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PersonaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PersonaDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    /// <summary>Obtiene una persona con sus teléfonos y estudios.</summary>
    /// <param name="cc">Cédula de la persona.</param>
    [HttpGet("{cc:int}")]
    [ProducesResponseType(typeof(PersonaDetalleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonaDetalleDto>> GetById(int cc)
        => Ok(await _service.GetDetalleAsync(cc));

    /// <summary>Crea una persona.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PersonaDto>> Create([FromBody] PersonaDto dto)
    {
        var creada = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { cc = creada.Cc }, creada);
    }

    /// <summary>Actualiza una persona (la cédula no se puede cambiar).</summary>
    [HttpPut("{cc:int}")]
    [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonaDto>> Update(int cc, [FromBody] PersonaDto dto)
        => Ok(await _service.UpdateAsync(cc, dto));

    /// <summary>Elimina una persona (solo si no tiene teléfonos ni estudios).</summary>
    [HttpDelete("{cc:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int cc)
    {
        await _service.DeleteAsync(cc);
        return NoContent();
    }
}
