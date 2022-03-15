using System.Collections.Generic;

namespace AcmeShop.Data;

public class Telephely
{
    public int Id { get; set; }
    public string Iranyitoszam { get; set; }
    public string Varos { get; set; }
    public string Utca { get; set; }
    public string Telefonszam { get; set; }
    public string Fax { get; set; }
    public int? VevoId { get; set; }

    public Vevo Vevo { get; set; }
    public ICollection<Megrendeles> Megrendelesek { get; set; }
    public ICollection<Vevo> VevoKozpontiTelephelyek { get; set; }
}
