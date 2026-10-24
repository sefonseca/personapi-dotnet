using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Models.DTOs;
using personapi_dotnet.Services.Interfaces;

namespace personapi_dotnet.Controllers.Api;

/// <summary>CRUD REST de estudios (llave compuesta: profesión + persona).</summary>
[ApiController]
[Route("api/estudios")]
[Produces("application/json")]
public class EstudiosApiController : ControllerBase
{
    private readonly IEstudioService _service;

    public EstudiosApiController(IEstudioService service)
    {
        _service = service;
    }

    /// <summary>Lista todos los estudios.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EstudioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EstudioDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    /// <summary>Obtiene un estudio por profesión y cédula.</summary>
    /// <param name="idProf">Id de la profesión.</param>
    /// <param name="ccPer">Cédula de la persona.</param>
    [HttpGet("{idProf:int}/{ccPer:int}")]
    [ProducesResponseType(typeof(EstudioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EstudioDto>> GetById(int idProf, int ccPer)
        => Ok(await _service.GetByIdAsync(idProf, ccPer));

    /// <summary>Registra un estudio.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(EstudioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EstudioDto>> Create([FromBody] EstudioDto dto)
    {
        var creado = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { idProf = creado.IdProf, ccPer = creado.CcPer }, creado);
    }

    /// <summary>Actualiza fecha y universidad de un estudio.</summary>
    [HttpPut("{idProf:int}/{ccPer:int}")]
    [ProducesResponseType(typeof(EstudioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EstudioDto>> Update(int idProf, int ccPer, [FromBody] EstudioDto dto)
        => Ok(await _service.UpdateAsync(idProf, ccPer, dto));

    /// <summary>Elimina un estudio.</summary>
    [HttpDelete("{idProf:int}/{ccPer:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int idProf, int ccPer)
    {
        await _service.DeleteAsync(idProf, ccPer);
        return NoContent();
    }
}
