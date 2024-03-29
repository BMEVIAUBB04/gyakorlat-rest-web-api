﻿using System.Collections.Generic;

namespace AcmeShop.Data;

public class Vevo
{
    public int Id { get; set; }
    public string? Nev { get; set; }
    public string? Szamlaszam { get; set; }
    public string? Login { get; set; }
    public string? Jelszo { get; set; }
    public string? Email { get; set; }
    public int? KozpontiTelephelyId { get; set; }

    public Telephely? KozpontiTelephely { get; set; }

    public ICollection<Telephely> Telephelyek { get; } 
           = new List<Telephely>();
}
