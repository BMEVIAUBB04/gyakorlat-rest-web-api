using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcmeShop.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AcmeShop.Api
{
    public static class Program
    {
        public static async Task Main(string[] args) =>
            (await CreateHostBuilder(args)
                    .Build()
                    .MigrateOrReacreateDatabaseAsync<AcmeShopContext>())
                .Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task<IHost> MigrateOrReacreateDatabaseAsync<TContext>(this IHost host) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
            var allMigrations = dbContext.Database.GetMigrations().ToHashSet();
            var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();
            if (appliedMigrations.Any(m => !allMigrations.Contains(m)))
            {
                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.MigrateAsync();
            }
            else if (allMigrations.Any(m => !appliedMigrations.Contains(m)))
                await dbContext.Database.MigrateAsync();
            return host;
        }
    }
}
