using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto;

public class ProdottoOrdineDto
{
    public string Nome { get; set; } = string.Empty;
    public int Quantita { get; set; }
    public decimal PrezzoUnitario { get; set; }
    public decimal TotaleParziale { get; set; }
}
