using System.Collections.Generic;

namespace AcmeShop.Data;

public class FizetesMod
{
    public int Id { get; set; }
    public string Mod { get; set; }
    public int? Hatarido { get; set; }

    public ICollection<Megrendeles> Megrendelesek { get; set; }
}
