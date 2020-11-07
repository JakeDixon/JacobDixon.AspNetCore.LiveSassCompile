using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JacobDixon.AspNetCore.LiveSassCompile
{
    public class SassInitialiser
    {
        private List<SassFileWatcher> _sassFileWatchers = new List<SassFileWatcher>();
        private readonly IOptions<LiveSassCompileOptions> _options;

        public SassInitialiser(IOptions<LiveSassCompileOptions> options)
        {
            _options = options;
        }

        public void StartFileWatchers()
        {
            var sassFileWatchersOptions = _options.Value.SassFileWatchers;

            foreach(var sassFileWatcherOptions in sassFileWatchersOptions)
            {
                var sassFileWatcher = new SassFileWatcher(sassFileWatcherOptions);
                sassFileWatcher.StartFileWatcher();
                _sassFileWatchers.Add(sassFileWatcher);
            }

        }

        public void StopFileWatchers()
        {
            foreach(var fw in _sassFileWatchers)
            {
                fw.StopFileWatcher();
            }
        }       
    }
}
