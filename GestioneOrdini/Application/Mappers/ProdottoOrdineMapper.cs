using Application.Dto;
using Domain.Model.Entities;

namespace Application.Mappers;

public static class ProdottoOrdineMapper
{
    public static ProdottoOrdineDto ToDto(ProdottoOrdine domain)
    {
        return new ProdottoOrdineDto
        {
            Nome = domain.Nome,
            Quantita = domain.Quantita,
            PrezzoUnitario = domain.PrezzoUnitario,
            TotaleParziale = domain.TotaleParziale()
        };
    }

    public static ProdottoOrdine ToDomain(ProdottoOrdineDto dto)
    {
        return new ProdottoOrdine(dto.Nome, dto.Quantita, dto.PrezzoUnitario);
    }
}