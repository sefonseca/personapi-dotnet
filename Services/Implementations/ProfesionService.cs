using personapi_dotnet.Exceptions;
using personapi_dotnet.Mappings;
using personapi_dotnet.Models.DTOs;
using personapi_dotnet.Repositories.Interfaces;
using personapi_dotnet.Services.Interfaces;

namespace personapi_dotnet.Services.Implementations;

public class ProfesionService : IProfesionService
{
    private readonly IProfesionRepository _repository;

    public ProfesionService(IProfesionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ProfesionDto>> GetAllAsync()
        => (await _repository.GetAllAsync()).Select(p => p.ToDto()).ToList();

    public async Task<ProfesionDto> GetByIdAsync(int id)
    {
        var profesion = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"No existe una profesión con el id {id}.");
        return profesion.ToDto();
    }

    public async Task<ProfesionDto> CreateAsync(ProfesionDto dto)
    {
        if (await _repository.ExistsAsync(dto.Id))
            throw new ConflictException($"Ya existe una profesión con el id {dto.Id}.");

        var profesion = dto.ToEntity();
        await _repository.AddAsync(profesion);
        return profesion.ToDto();
    }

    public async Task<ProfesionDto> UpdateAsync(int id, ProfesionDto dto)
    {
        if (dto.Id != id)
            throw new BusinessRuleException("El id de la profesión no se puede modificar.");

        var profesion = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"No existe una profesión con el id {id}.");

        dto.ApplyTo(profesion);
        await _repository.UpdateAsync(profesion);
        return profesion.ToDto();
    }

    public async Task DeleteAsync(int id)
    {
        var profesion = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"No existe una profesión con el id {id}.");

        if (await _repository.HasRelatedRecordsAsync(id))
            throw new ConflictException(
                "No se puede eliminar la profesión porque hay estudios registrados con ella. Elimínelos primero.");

        await _repository.DeleteAsync(profesion);
    }
}
