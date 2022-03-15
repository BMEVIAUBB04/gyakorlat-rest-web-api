using System.Collections.Generic;

namespace AcmeShop.Data;

public class Statusz
{
    public int Id { get; set; }
    public string? Nev { get; set; }

    public ICollection<Megrendeles> Megrendelesek { get; }
                                    = new List<Megrendeles>();
    public ICollection<MegrendelesTetel> MegrendelesTetelek { get; }
                                    = new List<MegrendelesTetel>();
}
