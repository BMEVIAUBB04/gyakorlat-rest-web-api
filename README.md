# ASP.NET Core Razor Pages, Web API

## Célkitűzés

Egyszerű REST- vagy RPC-jellegű webszolgáltatások készítésének alapszintű elsajátítása.

## Előfeltételek

A labor elvégzéséhez szükséges eszközök:

- Microsoft SQL Server (LocalDB vagy Express edition, Visual Studio telepítővel telepíthető)
- Visual Studio 2019 .NET 5 SDK-val telepítve

Amit érdemes átnézned:

- EF Core előadás anyaga
- ASP.NET Core előadások anyaga (elsősorban Web API, esetleg MVC és Razor Pages)
- A használt adatbázis [sémája](https://BMEVIAUBB04.github.io/gyakorlat-mssql/sema.html)

## Gyakorlat menete

A gyakorlatok laborvezetővel közösen és önállóan is elvégezhetők jelen útmutató segítségével. A feladatok megoldásai megtalálhatók az útmutatóban, rövid magyarázattal.

## Feladat 0: Kiinduló projekt letöltése, indítása

Megjegyzés: a kiinduló projekt felépítése, a benne történt módosítások analógak az ASP.NET Razor Pages és az ASP.NET Web API laborok között.

Az előző laborokon megszokott adatmodellt fogjuk használni MS SQL LocalDB segítségével, de egy másik elérési útvonalon, hogy tiszta adatbázissal induljunk. Az adatbázis sémájában néhány mező a .NET-ben ismeretes konvencióknak megfelelően átnevezésre került, felépítése viszont megegyezik a korábban megismertekkel.

1. Nyissuk meg a Visual Studio-t, és a megnyíló nyitóképernyőn válasszuk a "Clone or check out code" lehetőséget, és adjuk meg jelen repository git URL-jét és egy megfelelő (otthoni munkavégzés esetén tetszőleges) munkamappát: `https://github.com/BMEVIAUBB04/gyakorlat-rest-web-api.git`
2. A klónozást a Visual Studio el fogja végezni nekünk, megnyitja a repository mappáját. Duplaklikkeljünk a megjelenő Solution Explorer ablakban az `AcmeShop\AcmeShop.sln` fájlra, ami megnyitja a kiinduló projektünket.

A kiinduló solution 2 projektből áll:
- `AcmeShop.Data`: az `AcmeShopContext`-et, a hozzá kapcsolódó entitásokat és a kiinduló migrációt tartalmazó projekt,
- `AcmeShop.Api`: a szokásos ASP.NET Core kiinduló API projekt, amiben az alábbi bővítések történtek:
  - A projekt referálja az `AcmeShop.Data` projektet.
  - Az alkalmazás DI konténerébe regisztrálásra került az `AcmeShopContext` a `Startup` osztály `ConfigureServices` metódusában az alábbi módon:
  ``` C#
  public void ConfigureServices(IServiceCollection services)
  {
      services.AddDbContext<AcmeShopContext>(options => options.UseSqlServer(Configuration.GetConnectionString(nameof(AcmeShopContext))));
  // ...
  ```
  - Az adatbázis connection stringje bekerült az `appsettings.json`-be, az `appsettings.Development.json` törlésre került.
  - A projekt induláskor (`Program.Main` metódus) a szokásos kiszolgáló felépítésén túl az adatbázis automatikus létrehozását/migrációját is elvégzi, így az első indításkor létre fog jönni az adatbázisunk (további migrációk esetén azokat indításkor alkalmazza az adatbázison, inkompatibilis migrációk esetén pedig újra létrehozza az adatbázist).
  - A `Properties\launchSettings.json` fájlból eltávolításra került az IIS Expresstől való függőség, így az alkalmazás indításakor csak a Kestrel szerver fog futni egy konzolalkalmazás formájában.
3. Indítsuk el a projektet az `F5` megnyomásával! Ez első alkalommal tovább tarthat, ilyenkor ugyanis a teljes alkalmazás fordítása és az adatbázis létrehozása történik.

**Figyelem!** A szerveroldali kód módosítása során (néhány kivételtől eltekintve) szükséges újraindítani a szervert, hogy a változtatások érvényesüljenek.

## Feladat 1: Generált Controllerek

Az első indulást követően egy Swagger UI fogad minket, amin a kiinduló projektbe generált `WeatherForecastController` egyetlen, HTTP GET igére reagáló végpontját láthatjuk, amit a `​/WeatherForecast` URL-en érhetünk el.


---

Az itt található oktatási segédanyagok a BMEVIAUBB04 tárgy hallgatóinak készültek. Az anyagok oly módú felhasználása, amely a tárgy oktatásához nem szorosan kapcsolódik, csak a szerző(k) és a forrás megjelölésével történhet.

Az anyagok a tárgy keretében oktatott kontextusban értelmezhetőek. Az anyagokért egyéb felhasználás esetén a szerző(k) felelősséget nem vállalnak.
