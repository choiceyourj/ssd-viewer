using AnnotationWebApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AnnotationWebApp
{
    public class Program
    {
        public static int Main(string[] args)
        {

            try
            {
                var seed = args.Contains("-seedUser");
                if (seed)
                {
                    args = args.Except(new[] { "-seedUser" }).ToArray();
                }

                var host = CreateHostBuilder(args).Build();

                if (seed)
                {
                    Debug.WriteLine("Seeding users to database ...");
                    var config = host.Services.GetRequiredService<IConfiguration>();
                    var connectionString = config.GetConnectionString("AppDbConnString");

                    InitialDbSeed.SeedUserData(connectionString).Wait();

                    Debug.WriteLine("Done seeding users to database.");
                    return 0;
                }

                Debug.WriteLine("Starting IPMSM.Api host...");
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Host terminated unexpectedly.");

                return 1;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
