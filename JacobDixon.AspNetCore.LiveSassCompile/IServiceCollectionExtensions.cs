using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JacobDixon.AspNetCore.LiveSassCompile
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddLiveSassCompile(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            services.Configure<LiveSassCompileOptions>(configuration.GetSection(LiveSassCompileOptions.OptionsName));
            services.AddHostedService<LiveSassCompileBackgroundService>();
            return services;
        }
    }
}
