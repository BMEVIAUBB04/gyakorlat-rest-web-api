using System.Collections.Generic;

namespace AcmeShop.Data;

public class Kategoria
{
    public int Id { get; set; }
    public string Nev { get; set; }
    public int? SzuloKategoriaId { get; set; }

    public Kategoria SzuloKategoria { get; set; }
    public ICollection<Kategoria> GyerekKategoriak { get; set; }
    public ICollection<Termek> Termekek { get; set; }
}
