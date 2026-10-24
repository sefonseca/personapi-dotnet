using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Exceptions;
using personapi_dotnet.Models.DTOs;
using personapi_dotnet.Services.Interfaces;

namespace personapi_dotnet.Controllers;

/// <summary>Vistas MVC del CRUD de personas.</summary>
[ApiExplorerSettings(IgnoreApi = true)]
public class PersonasController : Controller
{
    private readonly IPersonaService _service;

    public PersonasController(IPersonaService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
        => View(await _service.GetAllAsync());

    public async Task<IActionResult> Details(int id)
    {
        try { return View(await _service.GetDetalleAsync(id)); }
        catch (NotFoundException) { return NotFound(); }
    }

    public IActionResult Create() => View(new PersonaDto());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PersonaDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _service.CreateAsync(dto);
            TempData["Success"] = $"Persona {dto.Nombre} {dto.Apellido} creada correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (AppException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        try { return View(await _service.GetByIdAsync(id)); }
        catch (NotFoundException) { return NotFound(); }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PersonaDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _service.UpdateAsync(id, dto);
            TempData["Success"] = "Persona actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (NotFoundException) { return NotFound(); }
        catch (AppException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        try { return View(await _service.GetByIdAsync(id)); }
        catch (NotFoundException) { return NotFound(); }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "Persona eliminada correctamente.";
        }
        catch (AppException ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }
}
