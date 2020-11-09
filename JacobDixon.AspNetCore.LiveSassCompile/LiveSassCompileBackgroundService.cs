using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace JacobDixon.AspNetCore.LiveSassCompile
{
    public class LiveSassCompileBackgroundService : IHostedService
    {
        private SassInitialiser _watcher;
        private readonly IOptions<LiveSassCompileOptions> _options;
        private readonly IWebHostEnvironment _environment;

        public LiveSassCompileBackgroundService(IOptions<LiveSassCompileOptions> options, IWebHostEnvironment environment)
        {
            _options = options;
            _environment = environment;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_options.Value.EnvironmentsToActivateIn.Contains(_environment.EnvironmentName))
            {
                _watcher = new SassInitialiser(_options);
                _watcher.StartFileWatchers();
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_watcher != null)
                _watcher.StopFileWatchers();
            return Task.CompletedTask;
        }
    }
}
