using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace AcmeShop.Data;

public class AcmeShopContext : DbContext
{
    public AcmeShopContext(DbContextOptions<AcmeShopContext> options)
        : base(options)
    {
    }

    public DbSet<Afa> Afa => Set<Afa>();
    public DbSet<FizetesMod> FizetesMod => Set<FizetesMod>();
    public DbSet<Kategoria> Kategoria => Set<Kategoria>();
    public DbSet<Megrendeles> Megrendeles => Set<Megrendeles>();
    public DbSet<MegrendelesTetel> MegrendelesTetel => Set<MegrendelesTetel>();
    public DbSet<Statusz> Statusz => Set<Statusz>();
    public DbSet<Szamla> Szamla => Set<Szamla>();
    public DbSet<SzamlaKiallito> SzamlaKiallito => Set<SzamlaKiallito>();
    public DbSet<SzamlaTetel> SzamlaTetel => Set<SzamlaTetel>();
    public DbSet<Telephely> Telephely => Set<Telephely>();
    public DbSet<Termek> Termek => Set<Termek>();
    public DbSet<Vevo> Vevo => Set<Vevo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Afa>(entity =>
        {
            entity.HasData(
                new Afa { Id = 1, Kulcs = 0 },
                new Afa { Id = 2, Kulcs = 15 },
                new Afa { Id = 3, Kulcs = 20 });
        });

        modelBuilder.Entity<FizetesMod>(entity =>
        {
            entity.Property(e => e.Mod).HasMaxLength(20);

            entity.HasData(
                new FizetesMod { Id = 1, Mod = "Készpénz", Hatarido = 0 },
                new FizetesMod { Id = 2, Mod = "Átutalás 8", Hatarido = 8 },
                new FizetesMod { Id = 3, Mod = "Átutalás 15", Hatarido = 15 },
                new FizetesMod { Id = 4, Mod = "Átutalás 30", Hatarido = 30 },
                new FizetesMod { Id = 5, Mod = "Kártya", Hatarido = 0 },
                new FizetesMod { Id = 6, Mod = "Utánvét", Hatarido = 0 });
        });

        modelBuilder.Entity<Kategoria>(entity =>
        {
            entity.Property(e => e.Nev).HasMaxLength(50);

            entity.HasOne(d => d.SzuloKategoria)
                .WithMany(p => p.GyerekKategoriak)
                .HasForeignKey(d => d.SzuloKategoriaId);

            entity.HasData(
                new Kategoria { Id = 1, Nev = "Játék" },
                new Kategoria { Id = 2, Nev = "Játszóház" },
                new Kategoria { Id = 3, Nev = "Bébijáték", SzuloKategoriaId = 1 },
                new Kategoria { Id = 4, Nev = "Építőjáték", SzuloKategoriaId = 1 },
                new Kategoria { Id = 5, Nev = "Fajáték", SzuloKategoriaId = 1 },
                new Kategoria { Id = 6, Nev = "Plüss figurák", SzuloKategoriaId = 1 },
                new Kategoria { Id = 7, Nev = "Közlekedési eszközök", SzuloKategoriaId = 1 },
                new Kategoria { Id = 8, Nev = "0-6 hónapos kor", SzuloKategoriaId = 3 },
                new Kategoria { Id = 9, Nev = "6-18 hónapos kor", SzuloKategoriaId = 3 },
                new Kategoria { Id = 10, Nev = "18-24 hónapos kor", SzuloKategoriaId = 3 },
                new Kategoria { Id = 11, Nev = "DUPLO", SzuloKategoriaId = 4 },
                new Kategoria { Id = 13, Nev = "LEGO", SzuloKategoriaId = 4 },
                new Kategoria { Id = 14, Nev = "Építő elemek", SzuloKategoriaId = 4 },
                new Kategoria { Id = 15, Nev = "Építő kockák", SzuloKategoriaId = 5 },
                new Kategoria { Id = 16, Nev = "Készségfejlesztő játékok", SzuloKategoriaId = 5 },
                new Kategoria { Id = 17, Nev = "Logikai játékok", SzuloKategoriaId = 5 },
                new Kategoria { Id = 18, Nev = "Ügyességi játékok", SzuloKategoriaId = 5 },
                new Kategoria { Id = 19, Nev = "Bébi taxik", SzuloKategoriaId = 7 },
                new Kategoria { Id = 20, Nev = "Motorok", SzuloKategoriaId = 7 },
                new Kategoria { Id = 21, Nev = "Triciklik", SzuloKategoriaId = 7 });
        });

        modelBuilder.Entity<Megrendeles>(entity =>
        {
            entity.HasOne(d => d.FizetesMod)
                .WithMany(p => p.Megrendelesek)
                .HasForeignKey(d => d.FizetesModId);

            entity.HasOne(d => d.Statusz)
                .WithMany(p => p.Megrendelesek)
                .HasForeignKey(d => d.StatuszId);

            entity.HasOne(d => d.Telephely)
                .WithMany(p => p.Megrendelesek)
                .HasForeignKey(d => d.TelephelyId);

            entity.HasData(
                new Megrendeles { Id = 1, Datum = new DateTime(2008, 01, 18), Hatarido = new DateTime(2008, 01, 30), TelephelyId = 3, StatuszId = 5, FizetesModId = 1 },
                new Megrendeles { Id = 2, Datum = new DateTime(2008, 02, 13), Hatarido = new DateTime(2008, 02, 15), TelephelyId = 2, StatuszId = 5, FizetesModId = 2 },
                new Megrendeles { Id = 3, Datum = new DateTime(2008, 02, 15), Hatarido = new DateTime(2008, 02, 20), TelephelyId = 1, StatuszId = 2, FizetesModId = 1 },
                new Megrendeles { Id = 4, Datum = new DateTime(2008, 02, 15), Hatarido = new DateTime(2008, 02, 20), TelephelyId = 2, StatuszId = 3, FizetesModId = 5 });
        });

        modelBuilder.Entity<MegrendelesTetel>(entity =>
        {
            entity.HasOne(d => d.Megrendeles)
                .WithMany(p => p.MegrendelesTetelek)
                .HasForeignKey(d => d.MegrendelesId);

            entity.HasOne(d => d.Statusz)
                .WithMany(p => p.MegrendelesTetelek)
                .HasForeignKey(d => d.StatuszId);

            entity.HasOne(d => d.Termek)
                .WithMany(p => p.MegrendelesTetelek)
                .HasForeignKey(d => d.TermekId);

            entity.HasData(
                new MegrendelesTetel { Id = 1, Mennyiseg = 2, NettoAr = 8356, MegrendelesId = 1, TermekId = 4, StatuszId = 5 },
                new MegrendelesTetel { Id = 2, Mennyiseg = 1, NettoAr = 1854, MegrendelesId = 1, TermekId = 6, StatuszId = 5 },
                new MegrendelesTetel { Id = 3, Mennyiseg = 5, NettoAr = 1738, MegrendelesId = 1, TermekId = 2, StatuszId = 5 },
                new MegrendelesTetel { Id = 4, Mennyiseg = 2, NettoAr = 7488, MegrendelesId = 2, TermekId = 1, StatuszId = 5 },
                new MegrendelesTetel { Id = 5, Mennyiseg = 3, NettoAr = 3725, MegrendelesId = 2, TermekId = 3, StatuszId = 5 },
                new MegrendelesTetel { Id = 6, Mennyiseg = 1, NettoAr = 4362, MegrendelesId = 3, TermekId = 7, StatuszId = 3 },
                new MegrendelesTetel { Id = 7, Mennyiseg = 6, NettoAr = 1854, MegrendelesId = 3, TermekId = 6, StatuszId = 2 },
                new MegrendelesTetel { Id = 8, Mennyiseg = 2, NettoAr = 6399, MegrendelesId = 3, TermekId = 9, StatuszId = 3 },
                new MegrendelesTetel { Id = 9, Mennyiseg = 5, NettoAr = 1738, MegrendelesId = 3, TermekId = 2, StatuszId = 1 },
                new MegrendelesTetel { Id = 10, Mennyiseg = 23, NettoAr = 3725, MegrendelesId = 4, TermekId = 3, StatuszId = 3 },
                new MegrendelesTetel { Id = 11, Mennyiseg = 12, NettoAr = 1738, MegrendelesId = 4, TermekId = 2, StatuszId = 3 },
                new MegrendelesTetel { Id = 12, Mennyiseg = 10, NettoAr = 27563, MegrendelesId = 4, TermekId = 8, StatuszId = 3 },
                new MegrendelesTetel { Id = 13, Mennyiseg = 25, NettoAr = 7488, MegrendelesId = 4, TermekId = 1, StatuszId = 3 });
        });

        modelBuilder.Entity<Statusz>(entity =>
        {
            entity.Property(e => e.Nev).HasMaxLength(20);
            entity.HasData(
                new Statusz { Id = 1, Nev = "Rögzítve" },
                new Statusz { Id = 2, Nev = "Várakozik" },
                new Statusz { Id = 3, Nev = "Csomagolva" },
                new Statusz { Id = 4, Nev = "Szállítás alatt" },
                new Statusz { Id = 5, Nev = "Kiszállítva" });
        });

        modelBuilder.Entity<Szamla>(entity =>
        {
            entity.Property(e => e.FizetesiMod).HasMaxLength(20);

            entity.Property(e => e.MegrendeloIranyitoszam)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.MegrendeloNev).HasMaxLength(50);

            entity.Property(e => e.MegrendeloUtca).HasMaxLength(50);

            entity.Property(e => e.MegrendeloVaros).HasMaxLength(50);

            entity.HasOne(d => d.Kiallito)
                .WithMany(p => p.Szamlak)
                .HasForeignKey(d => d.KiallitoId);

            entity.HasOne(d => d.Megrendeles)
                .WithMany(p => p.Szamlak)
                .HasForeignKey(d => d.MegrendelesId);

            entity.HasData(
                new Szamla { Id = 1, MegrendeloNev = "Hajdú-Nagy Katalin", MegrendeloIranyitoszam = "3000", MegrendeloVaros = "Hatvan", MegrendeloUtca = "Vörösmarty tér. 5.", NyomtatottPeldanyszam = 2, Sztorno = false, FizetesiMod = "Készpénz", KiallitasDatum = new DateTime(2008, 01, 30), TeljesitesDatum = new DateTime(2008, 01, 30), FizetesiHatarido = new DateTime(2008, 01, 30), KiallitoId = 1, MegrendelesId = 1 },
                new Szamla { Id = 2, MegrendeloNev = "Puskás Norbert", MegrendeloIranyitoszam = "1051", MegrendeloVaros = "Budapest", MegrendeloUtca = "Hercegprímás u. 22.", NyomtatottPeldanyszam = 2, Sztorno = false, FizetesiMod = "Átutalás 8", KiallitasDatum = new DateTime(2008, 02, 14), TeljesitesDatum = new DateTime(2008, 02, 15), FizetesiHatarido = new DateTime(2008, 02, 23), KiallitoId = 1, MegrendelesId = 2 });
        });

        modelBuilder.Entity<SzamlaKiallito>(entity =>
        {
            entity.Property(e => e.Adoszam)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.Iranyitoszam)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Nev).HasMaxLength(50);

            entity.Property(e => e.Szamlaszam)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Utca).HasMaxLength(50);

            entity.Property(e => e.Varos).HasMaxLength(50);

            entity.HasData(
                new SzamlaKiallito { Id = 1, Nev = "Regio Játék Áruház Kft", Iranyitoszam = "1119", Varos = "Budapest", Utca = "Nándorfejérvári u. 23", Adoszam = "15684995-2-32", Szamlaszam = "259476332-15689799-10020065" },
                new SzamlaKiallito { Id = 2, Nev = "Regio Játék Áruház Zrt", Iranyitoszam = "1119", Varos = "Budapest", Utca = "Nándorfejérvári u. 23", Adoszam = "68797867-1-32", Szamlaszam = "259476332-15689799-10020065" });
        });

        modelBuilder.Entity<SzamlaTetel>(entity =>
        {
            entity.Property(e => e.Nev).HasMaxLength(50);

            entity.HasOne(d => d.MegrendelesTetel)
                .WithMany(p => p.SzamlaTetelek)
                .HasForeignKey(d => d.MegrendelesTetelId);

            entity.HasOne(d => d.Szamla)
                .WithMany(p => p.SzamlaTetelek)
                .HasForeignKey(d => d.SzamlaId);

            entity.HasData(
                new SzamlaTetel { Id = 1, Nev = "Fisher Price kalapáló", Mennyiseg = 2, NettoAr = 8356, Afakulcs = 20, SzamlaId = 1, MegrendelesTetelId = 1 },
                new SzamlaTetel { Id = 2, Nev = "Maxi Blocks 56 db-os", Mennyiseg = 1, NettoAr = 1854, Afakulcs = 20, SzamlaId = 1, MegrendelesTetelId = 2 },
                new SzamlaTetel { Id = 3, Nev = "Színes bébikönyv", Mennyiseg = 5, NettoAr = 1738, Afakulcs = 20, SzamlaId = 1, MegrendelesTetelId = 3 },
                new SzamlaTetel { Id = 4, Nev = "Activity playgim", Mennyiseg = 2, NettoAr = 7488, Afakulcs = 20, SzamlaId = 2, MegrendelesTetelId = 4 },
                new SzamlaTetel { Id = 5, Nev = "Zenélő bébitelefon", Mennyiseg = 3, NettoAr = 3725, Afakulcs = 20, SzamlaId = 2, MegrendelesTetelId = 5 });
        });

        modelBuilder.Entity<Telephely>(entity =>
        {
            entity.Property(e => e.Fax)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.Property(e => e.Iranyitoszam)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Telefonszam)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.Property(e => e.Utca).HasMaxLength(50);

            entity.Property(e => e.Varos).HasMaxLength(50);

            entity.HasOne(d => d.Vevo)
                .WithMany(p => p.Telephelyek)
                .HasForeignKey(d => d.VevoId);

            entity.HasData(
                new Telephely { Id = 1, Iranyitoszam = "1114", Varos = "Budapest", Utca = "Baranyai u. 16.", Telefonszam = "061-569-23-99", VevoId = 2 },
                new Telephely { Id = 2, Iranyitoszam = "1051", Varos = "Budapest", Utca = "Hercegprímás u. 22.", Telefonszam = "061-457-11-03", Fax = "061-457-11-04", VevoId = 1 },
                new Telephely { Id = 3, Iranyitoszam = "3000", Varos = "Hatvan", Utca = "Vörösmarty tér. 5.", Telefonszam = "0646-319-169", Fax = "0646-319-168", VevoId = 2 },
                new Telephely { Id = 4, Iranyitoszam = "2045", Varos = "Törökbálint", Utca = "Határ u. 17.", Telefonszam = "0623-200-156", Fax = "0623-200-155", VevoId = 3 });
        });

        modelBuilder.Entity<Termek>(entity =>
        {
            entity.Property(e => e.Kep).HasColumnType("image");

            //entity.Property(e => e.Leiras).HasColumnType("xml");

            entity.Property(e => e.Nev).HasMaxLength(50);

            entity.HasOne(d => d.Afa)
                .WithMany(p => p.Termekek)
                .HasForeignKey(d => d.AfaId);

            entity.HasOne(d => d.Kategoria)
                .WithMany(p => p.Termekek)
                .HasForeignKey(d => d.KategoriaId);

            entity.HasData(
                new Termek
                {
                    Id = 1,
                    Nev = "Activity playgim",
                    NettoAr = 7488,
                    Raktarkeszlet = 21,
                    AfaId = 3,
                    KategoriaId = 8,
                    Leiras =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<termek>
	<termek_meret>
		<mertekegyseg>cm</mertekegyseg>
		<szelesseg>150</szelesseg>
		<magassag>50</magassag>
		<melyseg>150</melyseg>
	</termek_meret>
	<csomag_parameterek>
		<csomag_darabszam>1</csomag_darabszam>
		<csomag_meret>
			<mertekegyseg>cm</mertekegyseg>
			<szelesseg>150</szelesseg>
			<magassag>20</magassag>
			<melyseg>20</melyseg>
		</csomag_meret>
	</csomag_parameterek>
	<leiras>
		Elemmel mukodik, a csomag nem tartalmay elemet.
	</leiras>
	<ajanlott_kor>0-18 hónap</ajanlott_kor>
</termek>"
                },
                new Termek
                {
                    Id = 2,
                    Nev = "Színes bébikönyv",
                    NettoAr = 1738,
                    Raktarkeszlet = 58,
                    AfaId = 3,
                    KategoriaId = 8,
                    Leiras =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<termek>
	<termek_meret>
		<mertekegyseg>cm</mertekegyseg>
		<szelesseg>15</szelesseg>
		<magassag>2</magassag>
		<melyseg>15</melyseg>
	</termek_meret>
	<csomag_parameterek>
		<csomag_darabszam>1</csomag_darabszam>
		<csomag_meret>
			<mertekegyseg>cm</mertekegyseg>
			<szelesseg>15</szelesseg>
			<magassag>2</magassag>
			<melyseg>15</melyseg>
		</csomag_meret>
	</csomag_parameterek>
	<leiras>
		Tiszta pamut oldalak, élénk színek, vastag kontúrok.
		Ez a mini világ termék a babák életkori sajátosságainak megfelelően fejleszti a látást, tapintást. Motiválja a babát, hogy megtanulja környezete felismerését.
		Felerősíthető a gyerekágyra, járókára vagy a babakocsira.
	</leiras>
	<ajanlott_kor>0-18 hónap</ajanlott_kor>
</termek>"
                },
                new Termek
                {
                    Id = 3,
                    Nev = "Zenélő bébitelefon",
                    NettoAr = 3725,
                    Raktarkeszlet = 18,
                    AfaId = 3,
                    KategoriaId = 9,
                    Leiras =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<termek>
	<termek_meret>
		<mertekegyseg>cm</mertekegyseg>
		<szelesseg>20</szelesseg>
		<magassag>12</magassag>
		<melyseg>35</melyseg>
	</termek_meret>
	<csomag_parameterek>
		<csomag_darabszam>1</csomag_darabszam>
		<csomag_meret>
			<mertekegyseg>cm</mertekegyseg>
			<szelesseg>40</szelesseg>
			<magassag>25</magassag>
			<melyseg>50</melyseg>
		</csomag_meret>
	</csomag_parameterek>
	<leiras>
		9-36 hónaposan a zajok és a zene izgatja a gyermeki fantáziát. A gombok különböző hangélményekkel lepik meg a gyermeket a dallamok és csengetések segítségével. A 3 gomb megnyomásával vidám képmotívumok kezdenek forogni.
	</leiras>
	<ajanlott_kor>9-36 hónap</ajanlott_kor>
</termek>"
                },
                new Termek { Id = 4, Nev = "Fisher Price kalapáló", NettoAr = 8356, Raktarkeszlet = 58, AfaId = 3, KategoriaId = 10 },
                new Termek { Id = 5, Nev = "Mega Bloks 24 db-os", NettoAr = 4325, Raktarkeszlet = 47, AfaId = 3, KategoriaId = 14 },
                new Termek { Id = 6, Nev = "Maxi Blocks 56 db-os", NettoAr = 1854, Raktarkeszlet = 36, AfaId = 3, KategoriaId = 14 },
                new Termek { Id = 7, Nev = "Building Blocks 80 db-os", NettoAr = 4362, Raktarkeszlet = 25, AfaId = 3, KategoriaId = 14 },
                new Termek
                {
                    Id = 8,
                    Nev = "Lego City kikötője",
                    NettoAr = 27563,
                    Raktarkeszlet = 12,
                    AfaId = 3,
                    KategoriaId = 13,
                    Leiras =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<termek>
	<csomag_parameterek>
		<csomag_darabszam>1</csomag_darabszam>
		<csomag_meret>
			<mertekegyseg>cm</mertekegyseg>
			<szelesseg>80</szelesseg>
			<magassag>20</magassag>
			<melyseg>40</melyseg>
		</csomag_meret>
	</csomag_parameterek>
	<leiras>
		Elemek száma: 695 db.
	</leiras>
	<ajanlott_kor>5-12 év</ajanlott_kor>
</termek>"
                },
                new Termek { Id = 9, Nev = "Lego Duplo Ásógép", NettoAr = 6399, Raktarkeszlet = 26, AfaId = 3, KategoriaId = 11 },
                new Termek { Id = 10, Nev = "Egy óra gyerekfelügyelet", NettoAr = 800, Raktarkeszlet = 0, AfaId = 2, KategoriaId = 2 });
        });

        modelBuilder.Entity<Vevo>(entity =>
        {
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Jelszo).HasMaxLength(50);

            entity.Property(e => e.Login).HasMaxLength(50);

            entity.Property(e => e.Nev).HasMaxLength(50);

            entity.Property(e => e.Szamlaszam)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.KozpontiTelephely)
                .WithMany(p => p.VevoKozpontiTelephelyek)
                .HasForeignKey(d => d.KozpontiTelephelyId);

            
            entity.HasData(
                new Vevo { Id = 1, Nev = "Puskás Norbert", Szamlaszam = "16489665-05899845-10000038", Login = "pnorbert", Jelszo = "huti9haj1s", Email = "puskasnorbert@freemail.hu", KozpontiTelephelyId = 2 },
                new Vevo { Id = 2, Nev = "Hajdú-Nagy Katalin", Szamlaszam = "54255831-15615432-25015126", Login = "katinka", Jelszo = "gandalf67j", Email = "hajdunagyk@hotmail.com", KozpontiTelephelyId = 1 },
                new Vevo { Id = 3, Nev = "Grosz János", Szamlaszam = "25894467-12005362-59815126", Login = "jano", Jelszo = "jag7guFs", Email = "janos.grosz@gmail.com", KozpontiTelephelyId = 4 });            
        });
    }
}

// Csak a dotnet aspnet-codegenerator parancsnak van erre szüksége a lenti osztályra, valószínűleg egy bug miatt
// Csak a parancs lefutás idejére aktiváljuk.
/*
public class AcmeShopContextFactory : IDesignTimeDbContextFactory<AcmeShopContext>
{
    public AcmeShopContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AcmeShopContext>();
        optionsBuilder.UseSqlServer("connectionstring");

        return new AcmeShopContext(optionsBuilder.Options);
    }
}
*/
