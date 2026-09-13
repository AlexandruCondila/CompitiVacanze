using Domain.Model.Entities;
using Infrastructure.Dto;

namespace Infrastructure.Mappers;

public static class ProdottoOrdinePersistenceMapper
{
    public static ProdottoOrdinePersistenceDto ToPersistence(ProdottoOrdine domain)
    {
        return new ProdottoOrdinePersistenceDto
        {
            Nome = domain.Nome,
            Quantita = domain.Quantita,
            PrezzoUnitario = domain.PrezzoUnitario
        };
    }

    public static ProdottoOrdine ToDomain(ProdottoOrdinePersistenceDto dto)
    {
        return new ProdottoOrdine(dto.Nome, dto.Quantita, dto.PrezzoUnitario);
    }
}