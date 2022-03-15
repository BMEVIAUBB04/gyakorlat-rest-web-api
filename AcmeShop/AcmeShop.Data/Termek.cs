using System.Collections.Generic;

namespace AcmeShop.Data;

public class Termek
{
    public int Id { get; set; }
    public string? Nev { get; set; }
    public double? NettoAr { get; set; }
    public int? Raktarkeszlet { get; set; }
    public int? AfaId { get; set; }
    public int? KategoriaId { get; set; }
    public string? Leiras { get; set; }
    public byte[]? Kep { get; set; }

    public Afa? Afa { get; set; }
    public Kategoria? Kategoria { get; set; }
    public ICollection<MegrendelesTetel> MegrendelesTetelek { get; } 
                                        = new List<MegrendelesTetel>();
}
