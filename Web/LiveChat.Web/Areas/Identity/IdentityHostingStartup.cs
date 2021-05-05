using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(LiveChat.Web.Areas.Identity.IdentityHostingStartup))]

namespace LiveChat.Web.Areas.Identity
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
