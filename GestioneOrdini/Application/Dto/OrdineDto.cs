using System;
using System.Collections.Generic;

namespace Application.Dto;

public class OrdineDto
{
    public int Id { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Stato { get; set; } = string.Empty;
    public List<ProdottoOrdineDto> Prodotti { get; set; } = new();
    public decimal TotaleOrdine { get; set; }
}