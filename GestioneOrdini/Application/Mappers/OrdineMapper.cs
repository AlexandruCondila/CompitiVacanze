using System;
using System.Linq;
using Application.Dto;
using Domain.Model.Entities;
using Domain.Model.Enums;

namespace Application.Mappers;

public static class OrdineMapper
{
    public static OrdineDto ToDto(Ordine domain)
    {
        return new OrdineDto
        {
            Id = domain.Id,
            Cliente = domain.Cliente,
            Data = domain.Data,
            Stato = domain.Stato.ToString(),
            TotaleOrdine = domain.TotaleOrdine(),
            Prodotti = domain.Prodotti.Select(ProdottoOrdineMapper.ToDto).ToList()
        };
    }

    public static Ordine ToDomain(OrdineDto dto)
    {
        var ordine = new Ordine(dto.Id, dto.Cliente)
        {
            Data = dto.Data
        };

        try
        {
            ordine.Stato = Enum.Parse<StatoOrdine>(dto.Stato);
        }
        catch
        {
            ordine.Stato = StatoOrdine.InAttesa;
        }

        foreach (var p in dto.Prodotti)
        {
            ordine.AggiungiProdotto(ProdottoOrdineMapper.ToDomain(p));
        }

        return ordine;
    }
}