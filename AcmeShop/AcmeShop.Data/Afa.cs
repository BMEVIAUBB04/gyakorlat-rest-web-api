using System.Collections.Generic;

namespace AcmeShop.Data;

public class Afa
{
    public int Id { get; set; }
    public int? Kulcs { get; set; }

    public ICollection<Termek> Termekek { get; set; }
}
