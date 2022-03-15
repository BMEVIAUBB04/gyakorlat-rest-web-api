using System;
using System.Collections.Generic;

namespace AcmeShop.Data;

public class Megrendeles
{
    public int Id { get; set; }
    public DateTime? Datum { get; set; }
    public DateTime? Hatarido { get; set; }
    public int? TelephelyId { get; set; }
    public int? StatuszId { get; set; }
    public int? FizetesModId { get; set; }

    public FizetesMod? FizetesMod { get; set; }
    public Statusz? Statusz { get; set; }
    public Telephely? Telephely { get; set; }
    public ICollection<MegrendelesTetel> MegrendelesTetelek { get; }
                                            = new List<MegrendelesTetel>();
    public ICollection<Szamla> Szamlak { get; } = new List<Szamla>();
}
