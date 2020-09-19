using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using Regira.Extensions;
using Regira.Stakeholders.Core.Entities;
using Regira.Stakeholders.Library.Data;
using Regira.Stakeholders.Library.Extensions;
using Regira.TreeList;
using System;
using System.Linq;

namespace Regira.Stakeholders.ConsoleApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press 'c' to drop and create DB.");
            Console.WriteLine("Press 's' to seed.");
            Console.WriteLine("Or press 'cs' to drop, create and seed.");
            var input = Console.ReadLine() ?? string.Empty;
            var create = input.Contains('c');
            var seed = input.Contains('s');

            var start = DateTime.Now;

            // configuration
            var host = CreateHostBuilder(args)
                .Build();

            // initialisation
            using var scope = host.Services.CreateScope();
            var p = scope.ServiceProvider;
            var targetDb = p.GetRequiredService<StakeholdersContext>();


            if (create)
            {
                Console.WriteLine("(Re)creating DB");
                // start with a new DB
                targetDb.Database.EnsureDeleted();
                // Create DB from Context and apply Migrations
                targetDb.Database.Migrate();
                foreach (var sp in StoredProcedures.CREATE_ALL)
                {
                    targetDb.Database.ExecuteSqlRaw(sp);
                }
            }

            if (seed)
            {
                Console.WriteLine("Seeding DB");
                var seeder = p.GetRequiredService<Seeder>();
                seeder.Seed(2500);
            }

            var tree = Query(targetDb, 100);
            Console.Clear();
            foreach (var node in tree)
            {
                Console.WriteLine($"{node.Level}. {new string("....".Repeat(node.Level).ToArray())}{node.Value.GetTitle()} (#{node.Value.Id})");
            }

            Console.WriteLine();
            Console.WriteLine($"Finished in {(DateTime.Now - start).TotalSeconds} sec.");
        }


        // used by EF Core to access the DbContext
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(SetupConfig)
                .ConfigureServices(ConfigureServices);
        }
        static void SetupConfig(HostBuilderContext context, IConfigurationBuilder builder)
        {
            builder.Sources.Clear();
            // add configuration
            builder
                .AddJsonFile("appsettings.json", true, true)
                .AddUserSecrets(typeof(Program).Assembly, true);
        }
        static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var config = context.Configuration;

            var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
            services
                .AddDbContext<StakeholdersContext>(db =>
                {
                    db.UseMySql(config["ConnectionStrings:Stakeholders"], o =>
                    {
                        o.MigrationsAssembly(migrationsAssembly);
                        o.CommandTimeout(60 * 60);
                    }).EnableSensitiveDataLogging();
                })
                .AddTransient<Seeder>();
        }

        // Sample query
        static TreeList<Stakeholder> Query(StakeholdersContext db, int n = 10)
        {
            var randomizer = new Randomizer();
            var maxId = db.Stakeholders.Max(x => x.Id);
            var ids = Enumerable.Range(0, n).Select(_ => randomizer.Number(1, maxId)).ToArray();
            var param = new MySqlParameter("stakeholder_id", string.Join(",", ids));
            var start = DateTime.Now;

            var resultIds = db.StakeholderContacts.FromSqlInterpolated($"CALL contacts_offspring({param},{null})")
                .ToList()
                .SelectMany(c => new[] { c.RoleGiverId, c.RoleBearerId })
                .Distinct()
                .ToList();
            var end1 = (DateTime.Now - start).TotalSeconds;
            var stakeholders = db.Stakeholders.Where(x => resultIds.Contains(x.Id))
                .Include(x => x.Contacts)
                .ThenInclude(c => c.RoleGiver)
                .Include(x => x.Contacts)
                .ThenInclude(c => c.RoleBearer)
                .ToList();
            start = DateTime.Now;
            var tree = stakeholders.ToStakeholdersTree();
            var end2 = (DateTime.Now - start).TotalSeconds;
            return tree;
        }
    }
}
