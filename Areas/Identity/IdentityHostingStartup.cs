using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(territory.mobi.Areas.Identity.IdentityHostingStartup))]
namespace territory.mobi.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}