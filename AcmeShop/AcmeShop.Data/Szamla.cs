using System;
using System.Collections.Generic;

namespace AcmeShop.Data;

public class Szamla
{
    public int Id { get; set; }
    public string MegrendeloNev { get; set; }
    public string MegrendeloIranyitoszam { get; set; }
    public string MegrendeloVaros { get; set; }
    public string MegrendeloUtca { get; set; }
    public int? NyomtatottPeldanyszam { get; set; }
    public bool? Sztorno { get; set; }
    public string FizetesiMod { get; set; }
    public DateTime? KiallitasDatum { get; set; }
    public DateTime? TeljesitesDatum { get; set; }
    public DateTime? FizetesiHatarido { get; set; }
    public int? KiallitoId { get; set; }
    public int? MegrendelesId { get; set; }

    public SzamlaKiallito Kiallito { get; set; }
    public Megrendeles Megrendeles { get; set; }
    public ICollection<SzamlaTetel> SzamlaTetelek { get; set; }
}
