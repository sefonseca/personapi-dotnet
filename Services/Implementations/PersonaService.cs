using personapi_dotnet.Exceptions;
using personapi_dotnet.Mappings;
using personapi_dotnet.Models.DTOs;
using personapi_dotnet.Repositories.Interfaces;
using personapi_dotnet.Services.Interfaces;

namespace personapi_dotnet.Services.Implementations;

public class PersonaService : IPersonaService
{
    private readonly IPersonaRepository _repository;

    public PersonaService(IPersonaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<PersonaDto>> GetAllAsync()
        => (await _repository.GetAllAsync()).Select(p => p.ToDto()).ToList();

    public async Task<PersonaDto> GetByIdAsync(int cc)
    {
        var persona = await _repository.GetByIdAsync(cc)
            ?? throw new NotFoundException($"No existe una persona con la cédula {cc}.");
        return persona.ToDto();
    }

    public async Task<PersonaDetalleDto> GetDetalleAsync(int cc)
    {
        var persona = await _repository.GetWithRelationsAsync(cc)
            ?? throw new NotFoundException($"No existe una persona con la cédula {cc}.");
        return persona.ToDetalleDto();
    }

    public async Task<PersonaDto> CreateAsync(PersonaDto dto)
    {
        if (await _repository.ExistsAsync(dto.Cc))
            throw new ConflictException($"Ya existe una persona con la cédula {dto.Cc}.");

        var persona = dto.ToEntity();
        await _repository.AddAsync(persona);
        return persona.ToDto();
    }

    public async Task<PersonaDto> UpdateAsync(int cc, PersonaDto dto)
    {
        if (dto.Cc != cc)
            throw new BusinessRuleException("La cédula no se puede modificar.");

        var persona = await _repository.GetByIdAsync(cc)
            ?? throw new NotFoundException($"No existe una persona con la cédula {cc}.");

        dto.ApplyTo(persona);
        await _repository.UpdateAsync(persona);
        return persona.ToDto();
    }

    public async Task DeleteAsync(int cc)
    {
        var persona = await _repository.GetByIdAsync(cc)
            ?? throw new NotFoundException($"No existe una persona con la cédula {cc}.");

        if (await _repository.HasRelatedRecordsAsync(cc))
            throw new ConflictException(
                "No se puede eliminar la persona porque tiene teléfonos o estudios registrados. Elimínelos primero.");

        await _repository.DeleteAsync(persona);
    }
}
