using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using personapi_dotnet.Exceptions;
using personapi_dotnet.Models.DTOs;
using personapi_dotnet.Services.Interfaces;

namespace personapi_dotnet.Controllers;

/// <summary>Vistas MVC del CRUD de estudios (llave compuesta idProf + ccPer).</summary>
[ApiExplorerSettings(IgnoreApi = true)]
public class EstudiosController : Controller
{
    private readonly IEstudioService _service;
    private readonly IPersonaService _personaService;
    private readonly IProfesionService _profesionService;

    public EstudiosController(
        IEstudioService service,
        IPersonaService personaService,
        IProfesionService profesionService)
    {
        _service = service;
        _personaService = personaService;
        _profesionService = profesionService;
    }

    public async Task<IActionResult> Index()
        => View(await _service.GetAllAsync());

    public async Task<IActionResult> Details(int idProf, int ccPer)
    {
        try { return View(await _service.GetByIdAsync(idProf, ccPer)); }
        catch (NotFoundException) { return NotFound(); }
    }

    public async Task<IActionResult> Create()
    {
        await CargarListasAsync();
        return View(new EstudioDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EstudioDto dto)
    {
        if (!ModelState.IsValid)
        {
            await CargarListasAsync(dto.IdProf, dto.CcPer);
            return View(dto);
        }

        try
        {
            await _service.CreateAsync(dto);
            TempData["Success"] = "Estudio registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (AppException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await CargarListasAsync(dto.IdProf, dto.CcPer);
            return View(dto);
        }
    }

    public async Task<IActionResult> Edit(int idProf, int ccPer)
    {
        try { return View(await _service.GetByIdAsync(idProf, ccPer)); }
        catch (NotFoundException) { return NotFound(); }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int idProf, int ccPer, EstudioDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _service.UpdateAsync(idProf, ccPer, dto);
            TempData["Success"] = "Estudio actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (NotFoundException) { return NotFound(); }
        catch (AppException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    public async Task<IActionResult> Delete(int idProf, int ccPer)
    {
        try { return View(await _service.GetByIdAsync(idProf, ccPer)); }
        catch (NotFoundException) { return NotFound(); }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int idProf, int ccPer)
    {
        try
        {
            await _service.DeleteAsync(idProf, ccPer);
            TempData["Success"] = "Estudio eliminado correctamente.";
        }
        catch (AppException ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarListasAsync(int? idProf = null, int? ccPer = null)
    {
        var profesiones = await _profesionService.GetAllAsync();
        var personas = await _personaService.GetAllAsync();

        ViewBag.Profesiones = new SelectList(profesiones, "Id", "Nom", idProf);
        ViewBag.Personas = new SelectList(
            personas.Select(p => new { p.Cc, Texto = $"{p.Cc} - {p.Nombre} {p.Apellido}" }),
            "Cc", "Texto", ccPer);
    }
}
