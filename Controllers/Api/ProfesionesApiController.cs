using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.DTOs;
using personapi_dotnet.Services.Interfaces;

namespace personapi_dotnet.Controllers.Api;

/// <summary>CRUD REST de profesiones.</summary>
[ApiController]
[Route("api/profesiones")]
[Produces("application/json")]
public class ProfesionesApiController : ControllerBase
{
    private readonly IProfesionService _service;

    public ProfesionesApiController(IProfesionService service)
    {
        _service = service;
    }

    /// <summary>Lista todas las profesiones.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProfesionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProfesionDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    /// <summary>Obtiene una profesión por su id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProfesionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfesionDto>> GetById(int id)
        => Ok(await _service.GetByIdAsync(id));

    /// <summary>Crea una profesión.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProfesionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProfesionDto>> Create([FromBody] ProfesionDto dto)
    {
        var creada = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
    }

    /// <summary>Actualiza una profesión (el id no se puede cambiar).</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProfesionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfesionDto>> Update(int id, [FromBody] ProfesionDto dto)
        => Ok(await _service.UpdateAsync(id, dto));

    /// <summary>Elimina una profesión (solo si no tiene estudios asociados).</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
