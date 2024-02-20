using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Purchases.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:7002").UseStartup<Startup>();
                });
    }
}
