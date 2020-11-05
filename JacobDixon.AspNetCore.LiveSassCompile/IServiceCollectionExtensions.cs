using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace JacobDixon.AspNetCore.LiveSassCompile
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddLiveSassCompile(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LiveSassCompileOptions>(configuration.GetSection(LiveSassCompileOptions.OptionsName));
            services.AddHostedService<LiveSassCompileBackgroundService>();
            return services;
        }
    }
}
