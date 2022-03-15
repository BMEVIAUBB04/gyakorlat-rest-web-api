using System.Collections.Generic;

namespace AcmeShop.Data;

public class SzamlaKiallito
{
    public int Id { get; set; }
    public string? Nev { get; set; }
    public string? Iranyitoszam { get; set; }
    public string? Varos { get; set; }
    public string? Utca { get; set; }
    public string? Adoszam { get; set; }
    public string? Szamlaszam { get; set; }

    public ICollection<Szamla> Szamlak { get; } = new List<Szamla>();
}
