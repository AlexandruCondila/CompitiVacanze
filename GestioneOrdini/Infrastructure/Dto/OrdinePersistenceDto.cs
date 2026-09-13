using System;
using System.Collections.Generic;

namespace Infrastructure.Dto;

public class OrdinePersistenceDto
{
    public int Id { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public int Stato { get; set; }
    public List<ProdottoOrdinePersistenceDto> Prodotti { get; set; } = new();
}