using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Entities;

public class ProdottoOrdine
{
    public string Nome { get; set; } = string.Empty;
    public int Quantita { get; set; }
    public decimal PrezzoUnitario { get; set; }

    public ProdottoOrdine() { }

    public ProdottoOrdine(string nome, int quantita, decimal prezzoUnitario)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Il nome del prodotto non può essere vuoto.");
        if (quantita <= 0)
            throw new ArgumentException("La quantità deve essere maggiore di zero.");
        if (prezzoUnitario < 0)
            throw new ArgumentException("Il prezzo non può essere negativo.");

        Nome = nome;
        Quantita = quantita;
        PrezzoUnitario = prezzoUnitario;
    }

    public decimal TotaleParziale() => Quantita * PrezzoUnitario;
}
