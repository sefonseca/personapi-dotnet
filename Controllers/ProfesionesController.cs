using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.Exceptions;
using personapi_dotnet.Models.DTOs;
using personapi_dotnet.Services.Interfaces;

namespace personapi_dotnet.Controllers;

/// <summary>Vistas MVC del CRUD de profesiones.</summary>
[ApiExplorerSettings(IgnoreApi = true)]
public class ProfesionesController : Controller
{
    private readonly IProfesionService _service;

    public ProfesionesController(IProfesionService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
        => View(await _service.GetAllAsync());

    public async Task<IActionResult> Details(int id)
    {
        try { return View(await _service.GetByIdAsync(id)); }
        catch (NotFoundException) { return NotFound(); }
    }

    public IActionResult Create() => View(new ProfesionDto());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProfesionDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _service.CreateAsync(dto);
            TempData["Success"] = $"Profesión {dto.Nom} creada correctamente.";
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
    public async Task<IActionResult> Edit(int id, ProfesionDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _service.UpdateAsync(id, dto);
            TempData["Success"] = "Profesión actualizada correctamente.";
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
            TempData["Success"] = "Profesión eliminada correctamente.";
        }
        catch (AppException ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }
}
