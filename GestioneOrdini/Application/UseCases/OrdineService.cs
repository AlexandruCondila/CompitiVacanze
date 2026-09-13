using System.Collections.Generic;
using System.Linq;
using Application.Dto;
using Application.Interfaces;
using Application.Mappers;

namespace Application.UseCases;

public class OrdineService
{
    private readonly IOrdineRepository _repository;

    public OrdineService(IOrdineRepository repository)
    {
        _repository = repository;
    }

    public List<OrdineDto> OttieniTuttiGliOrdini()
    {
        return _repository.GetAll()
                          .Select(OrdineMapper.ToDto)
                          .ToList();
    }

    public OrdineDto? OttieniOrdinePerId(int id)
    {
        var ordine = _repository.GetById(id);
        return ordine != null ? OrdineMapper.ToDto(ordine) : null;
    }

    public int CreaOrdine(OrdineDto dto)
    {
        var tutti = _repository.GetAll();

        
        int nuovoId = 1;
        if (tutti.Count > 0)
        {
            nuovoId = tutti.Max(o => o.Id) + 1;
        }

        dto.Id = nuovoId;

        var entity = OrdineMapper.ToDomain(dto);
        _repository.Save(entity);

        return nuovoId;
    }

    public bool CancellaOrdine(int id)
    {
        return _repository.Delete(id);
    }
}