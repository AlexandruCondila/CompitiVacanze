using System;
using System.Linq;
using Domain.Model.Entities;
using Domain.Model.Enums;
using Domain.Model.ValueObjects;
using Infrastructure.Dto;

namespace Infrastructure.Mappers;

public static class OrdinePersistenceMapper
{
    public static OrdinePersistenceDto ToPersistence(Ordine domain)
    {
        return new OrdinePersistenceDto
        {
            Id = domain.Id,
            Cliente = domain.Cliente,
            Data = domain.Data,
            Stato = (int)domain.Stato,
            Prodotti = domain.Prodotti.Select(ProdottoOrdinePersistenceMapper.ToPersistence).ToList()
        };
    }

    public static Ordine ToDomain(OrdinePersistenceDto dto)
    {
        var ordine = new Ordine(dto.Id, dto.Cliente)
        {
            Data = dto.Data,
            Stato = (StatoOrdine)dto.Stato
        };

        foreach (var p in dto.Prodotti)
        {
            ordine.AggiungiProdotto(ProdottoOrdinePersistenceMapper.ToDomain(p));
        }

        return ordine;
    }
}