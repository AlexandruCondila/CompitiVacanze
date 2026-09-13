namespace Infrastructure.Dto;

public class ProdottoOrdinePersistenceDto
{
    public string Nome { get; set; } = string.Empty;
    public int Quantita { get; set; }
    public decimal PrezzoUnitario { get; set; }
}