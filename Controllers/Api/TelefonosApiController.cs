using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.DTOs;
using personapi_dotnet.Services.Interfaces;

namespace personapi_dotnet.Controllers.Api;

/// <summary>CRUD REST de teléfonos.</summary>
[ApiController]
[Route("api/telefonos")]
[Produces("application/json")]
public class TelefonosApiController : ControllerBase
{
    private readonly ITelefonoService _service;

    public TelefonosApiController(ITelefonoService service)
    {
        _service = service;
    }

    /// <summary>Lista todos los teléfonos con su dueño.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TelefonoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TelefonoDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    /// <summary>Obtiene un teléfono por su número.</summary>
    [HttpGet("{num}")]
    [ProducesResponseType(typeof(TelefonoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TelefonoDto>> GetById(string num)
        => Ok(await _service.GetByIdAsync(num));

    /// <summary>Registra un teléfono para una persona existente.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TelefonoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TelefonoDto>> Create([FromBody] TelefonoDto dto)
    {
        var creado = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { num = creado.Num }, creado);
    }

    /// <summary>Actualiza operador y/o dueño de un teléfono (el número no se puede cambiar).</summary>
    [HttpPut("{num}")]
    [ProducesResponseType(typeof(TelefonoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TelefonoDto>> Update(string num, [FromBody] TelefonoDto dto)
        => Ok(await _service.UpdateAsync(num, dto));

    /// <summary>Elimina un teléfono.</summary>
    [HttpDelete("{num}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string num)
    {
        await _service.DeleteAsync(num);
        return NoContent();
    }
}
