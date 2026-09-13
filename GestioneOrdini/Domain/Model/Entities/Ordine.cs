using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Model.Enums;

namespace Domain.Model.Entities;

public class Ordine
{
    public int Id { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public DateTime Data { get; set; } = DateTime.Now;
    public List<ProdottoOrdine> Prodotti { get; set; } = new();
    public StatoOrdine Stato { get; set; } = StatoOrdine.InAttesa;

    public Ordine() { }

    public Ordine(int id, string cliente)
    {
        Id = id;
        Cliente = string.IsNullOrWhiteSpace(cliente) ? "Cliente Anonimo" : cliente;
        Data = DateTime.Now;
        Stato = StatoOrdine.InAttesa;
    }

    public void AggiungiProdotto(ProdottoOrdine prodotto)
    {
        if (prodotto != null)
        {
            Prodotti.Add(prodotto);
        }
    }

    public decimal TotaleOrdine()
    {
        return Prodotti.Sum(p => p.TotaleParziale());
    }
}