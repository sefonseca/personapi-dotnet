using personapi_dotnet.Exceptions;
using personapi_dotnet.Mappings;
using personapi_dotnet.Models.DTOs;
using personapi_dotnet.Repositories.Interfaces;
using personapi_dotnet.Services.Interfaces;

namespace personapi_dotnet.Services.Implementations;

public class EstudioService : IEstudioService
{
    private readonly IEstudioRepository _repository;
    private readonly IPersonaRepository _personaRepository;
    private readonly IProfesionRepository _profesionRepository;

    public EstudioService(
        IEstudioRepository repository,
        IPersonaRepository personaRepository,
        IProfesionRepository profesionRepository)
    {
        _repository = repository;
        _personaRepository = personaRepository;
        _profesionRepository = profesionRepository;
    }

    public async Task<IReadOnlyList<EstudioDto>> GetAllAsync()
        => (await _repository.GetAllAsync()).Select(e => e.ToDto()).ToList();

    public async Task<EstudioDto> GetByIdAsync(int idProf, int ccPer)
    {
        var estudio = await _repository.GetByIdAsync(idProf, ccPer)
            ?? throw new NotFoundException($"No existe el estudio de la persona {ccPer} en la profesión {idProf}.");
        return estudio.ToDto();
    }

    public async Task<EstudioDto> CreateAsync(EstudioDto dto)
    {
        if (!await _profesionRepository.ExistsAsync(dto.IdProf))
            throw new BusinessRuleException($"No existe una profesión con el id {dto.IdProf}.");

        if (!await _personaRepository.ExistsAsync(dto.CcPer))
            throw new BusinessRuleException($"No existe una persona con la cédula {dto.CcPer}.");

        if (await _repository.ExistsAsync(dto.IdProf, dto.CcPer))
            throw new ConflictException("Esta persona ya tiene registrado un estudio en esa profesión.");

        ValidateFecha(dto.Fecha);

        await _repository.AddAsync(dto.ToEntity());
        return await GetByIdAsync(dto.IdProf, dto.CcPer);
    }

    public async Task<EstudioDto> UpdateAsync(int idProf, int ccPer, EstudioDto dto)
    {
        if (dto.IdProf != idProf || dto.CcPer != ccPer)
            throw new BusinessRuleException("La persona y la profesión de un estudio no se pueden modificar.");

        var estudio = await _repository.GetByIdAsync(idProf, ccPer)
            ?? throw new NotFoundException($"No existe el estudio de la persona {ccPer} en la profesión {idProf}.");

        ValidateFecha(dto.Fecha);

        dto.ApplyTo(estudio);
        await _repository.UpdateAsync(estudio);
        return estudio.ToDto();
    }

    public async Task DeleteAsync(int idProf, int ccPer)
    {
        var estudio = await _repository.GetByIdAsync(idProf, ccPer)
            ?? throw new NotFoundException($"No existe el estudio de la persona {ccPer} en la profesión {idProf}.");

        await _repository.DeleteAsync(estudio);
    }

    private static void ValidateFecha(DateOnly? fecha)
    {
        if (fecha.HasValue && fecha.Value > DateOnly.FromDateTime(DateTime.Today))
            throw new BusinessRuleException("La fecha de grado no puede ser futura.");
    }
}
