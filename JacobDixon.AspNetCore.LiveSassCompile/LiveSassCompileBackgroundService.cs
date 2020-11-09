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

        public LiveSassCompileBackgroundService(IOptions<LiveSassCompileOptions> options)
        {
            _options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_options.Value.EnableLiveCompile)
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
