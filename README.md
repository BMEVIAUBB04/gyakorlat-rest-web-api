# ASP.NET Core Web API és többrétegű alkalmazásarchitektúra

## Célkitűzés

Egyszerű REST- vagy webszolgáltatások készítésének alapszintű elsajátítása.

## Előfeltételek

A labor elvégzéséhez szükséges eszközök:

- Microsoft SQL Server (LocalDB vagy Express edition, Visual Studio telepítővel telepíthető)
- Visual Studio 2022 .NET 6 SDK-val telepítve

Amit érdemes átnézned:

- EF Core előadás anyaga
- ASP.NET Core előadások anyaga
- A használt adatbázis [sémája](https://BMEVIAUBB04.github.io/gyakorlat-mssql/sema.html)

## Feladat 0: Kiinduló projekt letöltése, indítása

Az előző laborokon megszokott adatmodellt fogjuk használni MS SQL LocalDB segítségével. Az adatbázis sémájában néhány mező a .NET-ben ismeretes konvencióknak megfelelően átnevezésre került, felépítése viszont megegyezik a korábban megismertekkel.

1. Töltsük le a GitHub repository-t a reposiory főoldaláról (https://github.com/BMEVIAUBB04/gyakorlat-rest-web-api > *Code* gomb, majd *Download ZIP*) vagy a közvetlen [letöltő link](https://github.com/BMEVIAUBB04/gyakorlat-rest-web-api/archive/refs/heads/master.zip) segítségével. 
2. Csomagoljuk ki
3. Nyissuk meg a kicsomagolt mappa AcmeShop alkönyvtárban lévő solution fájlt.

A kiinduló solution egyelőre egy projektből áll:`AcmeShop.Data`: EF modellt, a hozzá tartozó kontextust (`AcmeShopContext`) tartalmazza. Hasonló az EF Core gyakorlaton generált kódhoz, de ez Code-First migrációt is tartalmaz (`Migrations` almappa).

## Feladat 1: Webes projekt elkészítése

1. Adjunk a solutionhöz egy új web projektet
    - Típusa: ASP.NET Core Web API (**nem Web App!**)
    - Neve: *AcmeShop.Api*
    - Framework: .NET 6.0
    - Authentication type: *None*
    - HTTPS, Docker: kikapcsolni
    - Use controllers, Enable OpenAPI support: bekapcsolni

1. Függőségek felvétele az új projekthez
    - adjuk meg projektfüggőségként az `AcmeShop.Data`-t
    - adjuk hozzá a *Microsoft.EntityFrameworkCore.Design* NuGet csomagot

1. Adatbáziskapcsolat, EF beállítása
    - connection string beállítása a konfigurációs fájlban (appsettings.json). A nyitó `{` jel után
    ```javascript
     "ConnectionStrings": {
       "AcmeShopContext": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AcmeShop"
     },
    ```
   - connection string kiolvasása a konfigurációból, `AcmeShopContext` példány konfigurálása ezen connection string alapján, `AcmeShopContext` példány regisztrálása DI konténerbe. Program.cs-be, a `builder.Build()` sor elé:
    ```csharp
    builder.Services.AddDbContext<AcmeShopContext>(
        options => options.UseSqlServer(
            builder.Configuration.GetConnectionString(nameof(AcmeShopContext))));
    ```

1. Adatbázis inicializálása Package Manager Console (PMC)-ban
   - Indítandó projekt az `AcmeShop.Api` projekt legyen (jobbklikk az AcmeShop.Api-n > *Set as Startup Project*)
   - A PMC-ben a Defult projekt viszont az `AcmeShop.Data` legyen
   - PMC-ből generáltassuk az adatbázist az alábbi paranccsal
    ```powershell
    Update-Database
    ```

1. Projekt indítása

## Feladat 2: Webes projekt vizsgálata

Az első indulást követően egy Swagger UI fogad minket, amin a kiinduló projektbe generált `WeatherForecastController` egyetlen, HTTP GET igére reagáló végpontját láthatjuk, amit a `​/WeatherForecast` URL-en érhetünk el. A `WeatherForecastController.Get()` metódusa nem vár paramétert, és egy 5 db véletlenszerű WeatherForecast elemet tartalmazó `IEnumerable<WeatherForecast>` elemmel tér vissza.

A Swagger UI-t használhatjuk az API tesztelésére, nyomjuk meg a UI-on látható `GET`, `Try it out`, majd az `Execute` feliratú gombot!

![Swagger UI](assets/1-swagger-ui.png)

Láthatjuk alul a küldött üzenetet (jelenleg query paramétereket sem fogadó GET kérés esetén ez csak az URL-t jelenti), az arra kapott válasz státuszkódját és formázott JSON törzsét. Megnézhetjük alább a kérés kapcsán használt objektumok sematikus leírását is. Fontos észrevenni, hogy az egész alkalmazásban nem jelöltük sehol, hogy JSON alapú kommunikációt végzünk; az API végponton egyszerűen az eredményhalmazt tartalmazó objektummal tértünk vissza. A keretrendszer a kliens által küldött kérés `Accept*` fejlécmezői alapján döntötte el, hogy JSON sorosítást alkalmaz (ami egyébként az alapértelmezett), de volna lehetőség további sorosítókat is alkalmazni (pl. XML-t vagy akár CSV-t).

Az alkalmazás teljes konfigurációs kódja nem sok, a Program.cs fájlban láthatjuk a függőségek regisztrációját (`builder.Services.AddXXX` sorok) és a kiszolgálási csővezeték konfigurációját (`app.UseXXX`). A legtöbb third-party komponens szolgáltat számunkra szolgáltatások is, és elérhetővé tesz végpontokat is, ezt láthatjuk pl. a Swagger UI kapcsán:
- a `services.AddSwaggerGen` hívás a megfelelő szolgáltatástípusokat konfigurálja fel és teszi elérhetővé az alkalmazás többi része számára,
- az `app.UseSwagger` és `app.UseSwaggerUI` hívások pedig a Swagger JSON leíróját és a UI-t ajánlják ki egy-egy meghatározott HTTP végponton.

## Feladat 3: Adatbázis objektumok lekérdezése

A `WeatherForecastController` nem használta az adatbázisunkat. Vegyünk fel egy új Controllert, aminek segítségével manipulálni tudjuk az adatbázist egy REST API-n keresztül! A leggyorsabb módja ennek a kódgenerálás (scaffolding).

1. Adjunk hozzá az API projekthez a *Microsoft.VisualStudio.Web.CodeGeneration.Design* NuGet csomagot.
2. PMC-ben telepítsük az ASP.NET Core kódgeneráló eszközt
    ```powershell
    dotnet tool install -g dotnet-aspnet-codegenerator
    ```
3. Lépjünk be a projekt könyvtárába
    ```powershell
    cd .\AcmeShop.Api
    ```
4. Generáljunk a kódgenerálóval REST API (`-api`) controllert a `Termek` entitáshoz (`-m`), mely a `AcmeShopContext` kontextushoz  (`-dc`) tartozik. A generált osztály neve legyen `TermekController` (`-name`), az `AcmeShop.Api.Controllers` névtérbe  (`-namespace`) kerüljön. A generált fájl a *Controllers* mappába (`-outDir`) kerüljön. 
    ```powershell
    dotnet aspnet-codegenerator controller -m AcmeShop.Data.Termek -dc AcmeShop.Data.AcmeShopContext -outDir Controllers -name TermekekController -namespace AcmeShop.Api.Controllers -api
    ```

Indítsuk újra az alkalmazást, nézzük meg, milyen végpontokat látunk a Swagger UI szerint!

![Generált végpontok](assets/1-scaffolding-4.png)

Elég sok végpontot látunk, gyakorlatilag a CRUD műveletek mindegyikét megtaláljuk (R-ből kettő van, így összesen 5 végpont), valamint meglepően sok modell sémát. A GET-es kérésünk például bár csak egy terméket tartalmaz, amihez tartozik egy ÁFA és egy kategória, de tartoznak a termékekhez megrendelések is, amin keresztül gyakorlatilag a teljes fennmaradó adattartalma az adatbázisnak elérhető lesz nekünk... ide értve a felhasználó féltett `Jelszo` mezőjét is a `Termek.MegrendelesTetelek.Megrendeles.Telephely.Vevo` tulajdonságláncon át.

![Entitás séma](assets/1-entity-schema.png)

Futassuk az első GET lekérdezést!

Ezek után a helyes reakció, hogy az egyik szemünk sír, amíg a másik nevet. Az alábbi tanulságokat tudjuk levonni a forráskód vizsgálata után:
- a kérések nagyon könnyen legenerálódtak, sőt, olyan szélsőséges esetekre is felkészültünk, mint például idő közben törölt termék módosításának kísérlete.
- a navigation property-k nincsenek betöltve, ezért az összes ilyen tulajdonság az entitásban `null`. Ha ezeket be szeretnénk tölteni, arról magunknak kell gondoskodni.
- a módosítás PUT műveletben a `Termek termek` JSON objektumot deszerializálva validáció nélkül mentjük az adatbázisba. Ha a `Vevo` entitáshoz is generáltunk volna végpontokat, akkor egyszerű (és inkorrekt) volna megváltoztatni a vevők jelszavát ilyen módon.

A `GET /api/Termekek` végpontnak megfelelő kontroller művelet törzsében töltessük ki az EF-fel a `MegrendelesTetelek` navigációs property-t

``` C#
return await _context.Termek.Include(t => t.MegrendelesTetelek).ToListAsync();
```

Hibát kapunk, ugyanis a JSON objektumban végtelen ciklus keletkezett a navigation property hatására. Miért? A `Termek` és a `Termek.MegrendelesTetelek.Termek` ugyanarra az objektumra mutat, ezért ennek a sorosítása a klasszikus értelemben véve problémás. Ezt kiküszöbölhetnénk a .NET beépített JSON sorosítójának konfigurációjával, de ilyenkor erre a kliensoldali sorosítót is fel kell készíteni. Ha jobban átgondoljuk a helyzetet, nem a sorosítóval van a gond, sokkal inkább azzal, hogy közvetlenül az entitásmodell (egy részét) sorosítjuk - a problémáink is ebből adódnak.

Ebből is tászik, hogy a scaffolding ebben az esetben legfeljebb gyors prototipizálásra jó, **változtatás nélkül ne használjuk**! Gyakorlatilag közvetlen elérést engedünk a végfelhasználónak az adatbázishoz (egy kevésbé optimális absztrakción keresztül).

## Feladat 3: DTO-k lekérdezése


1. Hozzunk létre az API projektben egy új "osztályt" _DTOs.cs_ fájlban, a fájl teljes tartalmát pedig cseréljük le az alábbira:
    ``` C#
    namespace AcmeShop.Models;

    public record TermekDto(int Id, string Nev, double? NettoAr, int? Raktarkeszlet, int? AfaKulcs, int? KategoriaId, string Leiras);

    public record KategoriaDto(int Id, string Nev, int? SzuloKategoriaId);
    ```
    
1. A `GET /api/Termekek` végpontnak megfelelő kontroller művelet törzsében `TermekDto`-t adjunk vissza `Temek` helyett
    ``` C#
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TermekDto>>> GetTermek()
    {
       //return await _context.Termek.Include(t => t.MegrendelesTetelek).ToListAsync();
       return await _context.Termek.Select(t => new TermekDto(t.Id, t.Nev, t.NettoAr, t.Raktarkeszlet, t.Afa.Kulcs, t.KategoriaId, t.Leiras )).ToListAsync();
    }
    ```
  
1. Próbáljuk ki, hogy a Swagger felületen most már TermekDto-nak megfelelő JSON-t kapunk-e válaszként.

## Feladat 3: Lekérdezés specifikáció szerint

A [specifikáció minta](https://en.wikipedia.org/wiki/Specification_pattern) egyik variánsa, amikor egy olyan objektumot készítünk, ami a szűrési feltételeket tartalmazza. Ez kombinálható különböző aspektusorientált műveletekkel (pl. attribútumok), amivel akár automatikusan felületet is generálhatunk. Nekünk most a Swagger UI lesz a "felületünk", ezért mi csak a műveletet fogjuk megvalósítani.

A specifikációs objektumunk egyszerű adat objektum, amit paraméterül fogunk várni az API-n. Hozzuk létre a `Bll`-ben.
``` C#
namespace AcmeShop.Bll
{
    public record TermekSpec(
        int Oldal,
        int Oldalmeret,
        int? Id, 
        string NevContains, 
        double? NettoArMinimum, 
        double? NettoArMaximum, 
        int? KategoriaId, 
        bool? Raktaron) 
    { }
}
```

Módosítsuk ennek megfelelően a `TermekService`-t és interfészét, egyúttal implementáljuk a szolgáltatást! Trükközhetünk a konstruktor injektálással jóval rövidebb szintaxissal, ha a szolgáltatásunk is `record` típusú.

``` C#
using AcmeShop.Bll.Models;
using AcmeShop.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeShop.Bll
{
    public interface ITermekService
    {
        Task<IReadOnlyCollection<KategoriaDto>> GetKategoriakAsync();
        Task<IReadOnlyCollection<TermekDto>> GetTermekekAsync(TermekSpec spec);
    }
    public record TermekService(AcmeShopContext DbContext) : ITermekService
    {
        public async Task<IReadOnlyCollection<KategoriaDto>> GetKategoriakAsync() =>
            await DbContext.Kategoria.Select(k => new KategoriaDto(k.Id, k.Nev, k.SzuloKategoriaId)).ToListAsync();

        public async Task<IReadOnlyCollection<TermekDto>> GetTermekekAsync(TermekSpec spec)
        {
            var termekek = DbContext.Termek.AsQueryable();
            if (spec.Id != null)
                termekek = termekek.Where(t => t.Id == spec.Id);
            if (spec.KategoriaId != null)
                termekek = termekek.Where(t => t.KategoriaId == spec.KategoriaId);
            if (spec.NettoArMaximum != null)
                termekek = termekek.Where(t => t.NettoAr <= spec.NettoArMaximum);
            if (spec.NettoArMinimum != null)
                termekek = termekek.Where(t => t.NettoAr >= spec.NettoArMaximum);
            if (!string.IsNullOrWhiteSpace(spec.NevContains))
                termekek = termekek.Where(t => t.Nev.Contains(spec.NevContains));
            if (spec.Raktaron != null)
                termekek = termekek.Where(t => spec.Raktaron == true ? t.Raktarkeszlet > 0 : t.Raktarkeszlet <= 0);
            var (oldal, oldalmeret) = (Math.Max(0, spec.Oldal), Math.Min(Math.Max(1, spec.Oldalmeret), 100));
            return await termekek
                .OrderBy(t => t.Nev)
                .Skip(oldal * oldalmeret)
                .Take(oldalmeret)
                .Select(t => new TermekDto(t.Id, t.Nev, t.NettoAr, t.Raktarkeszlet, t.Afa.Kulcs, t.KategoriaId, t.Leiras))
                .ToListAsync();
        }
    }
}

```

Ennél gyönyörűbb megoldást talán csak ritkán látunk. Az utolsó lépés a Controllerek létrehozása és a service megfelelő meghívása. Az egyszerűbb a `KategoriakController`, kezdjük tehát azzal (legyen egy új üres API Controller):
``` C#
using AcmeShop.Bll;
using AcmeShop.Bll.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KategoriakController : ControllerBase
    {
        public KategoriakController(ITermekService termekService)
        {
            TermekService = termekService;
        }

        private ITermekService TermekService { get; }

        [HttpGet]
        public async Task<IReadOnlyCollection<KategoriaDto>> GetKategoriakAsync() => await TermekService.GetKategoriakAsync();
    }
}

```

Tesztelhetjük, működik.

Végül pedig a `TermekekController`t valósítjuk meg...

``` C#
using AcmeShop.Bll;
using AcmeShop.Bll.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermekekController : ControllerBase
    {
        public TermekekController(ITermekService termekService)
        {
            TermekService = termekService;
        }

        private ITermekService TermekService { get; }

        [HttpGet]
        public async Task<IReadOnlyCollection<TermekDto>> GetTermekekAsync(TermekSpec spec) => await TermekService.GetTermekekAsync(spec);
    }
}

```

Próbáljuk ki a lekérdezésünket!

---

Az itt található oktatási segédanyagok a BMEVIAUBB04 tárgy hallgatóinak készültek. Az anyagok oly módú felhasználása, amely a tárgy oktatásához nem szorosan kapcsolódik, csak a szerző(k) és a forrás megjelölésével történhet.

Az anyagok a tárgy keretében oktatott kontextusban értelmezhetőek. Az anyagokért egyéb felhasználás esetén a szerző(k) felelősséget nem vállalnak.
