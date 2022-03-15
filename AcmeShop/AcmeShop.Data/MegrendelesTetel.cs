using System.Collections.Generic;

namespace AcmeShop.Data;

public class MegrendelesTetel
{
    public int Id { get; set; }
    public int? Mennyiseg { get; set; }
    public double? NettoAr { get; set; }
    public int? MegrendelesId { get; set; }
    public int? TermekId { get; set; }
    public int? StatuszId { get; set; }

    public Megrendeles Megrendeles { get; set; }
    public Statusz Statusz { get; set; }
    public Termek Termek { get; set; }
    public ICollection<SzamlaTetel> SzamlaTetelek { get; set; }
}
