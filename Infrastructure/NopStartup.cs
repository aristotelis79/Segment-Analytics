using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.SegmentAnalytics.ActionsFilters;

namespace Nop.Plugin.Widgets.SegmentAnalytics.Infrastructure
{
    public class NopStartup : INopStartup
    {

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MvcOptions>(config =>{
                                                        config.Filters.Add<IdentityActionFilter>();
                                                    });
        }

        public void Configure(IApplicationBuilder application)
        {
        }

        public int Order => 1001;
    }
}