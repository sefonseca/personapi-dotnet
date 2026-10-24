using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using personapi_dotnet.Exceptions;
using personapi_dotnet.Models.DTOs;
using personapi_dotnet.Services.Interfaces;

namespace personapi_dotnet.Controllers;

/// <summary>Vistas MVC del CRUD de teléfonos.</summary>
[ApiExplorerSettings(IgnoreApi = true)]
public class TelefonosController : Controller
{
    private readonly ITelefonoService _service;
    private readonly IPersonaService _personaService;

    public TelefonosController(ITelefonoService service, IPersonaService personaService)
    {
        _service = service;
        _personaService = personaService;
    }

    public async Task<IActionResult> Index()
        => View(await _service.GetAllAsync());

    public async Task<IActionResult> Details(string id)
    {
        try { return View(await _service.GetByIdAsync(id)); }
        catch (NotFoundException) { return NotFound(); }
    }

    public async Task<IActionResult> Create()
    {
        await CargarPersonasAsync();
        return View(new TelefonoDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TelefonoDto dto)
    {
        if (!ModelState.IsValid)
        {
            await CargarPersonasAsync(dto.Duenio);
            return View(dto);
        }

        try
        {
            await _service.CreateAsync(dto);
            TempData["Success"] = $"Teléfono {dto.Num} registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (AppException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await CargarPersonasAsync(dto.Duenio);
            return View(dto);
        }
    }

    public async Task<IActionResult> Edit(string id)
    {
        try
        {
            var dto = await _service.GetByIdAsync(id);
            await CargarPersonasAsync(dto.Duenio);
            return View(dto);
        }
        catch (NotFoundException) { return NotFound(); }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, TelefonoDto dto)
    {
        if (!ModelState.IsValid)
        {
            await CargarPersonasAsync(dto.Duenio);
            return View(dto);
        }

        try
        {
            await _service.UpdateAsync(id, dto);
            TempData["Success"] = "Teléfono actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (NotFoundException) { return NotFound(); }
        catch (AppException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await CargarPersonasAsync(dto.Duenio);
            return View(dto);
        }
    }

    public async Task<IActionResult> Delete(string id)
    {
        try { return View(await _service.GetByIdAsync(id)); }
        catch (NotFoundException) { return NotFound(); }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        try
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Teléfono eliminado correctamente.";
        }
        catch (AppException ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarPersonasAsync(int? seleccionada = null)
    {
        var personas = await _personaService.GetAllAsync();
        ViewBag.Personas = new SelectList(
            personas.Select(p => new { p.Cc, Texto = $"{p.Cc} - {p.Nombre} {p.Apellido}" }),
            "Cc", "Texto", seleccionada);
    }
}
