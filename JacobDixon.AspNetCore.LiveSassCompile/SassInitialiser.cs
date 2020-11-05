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
        private readonly string[] _extensions = { ".scss", ".sass" };
        private List<SassFileWatcher> _sassFileWatchers = new List<SassFileWatcher>();
        private readonly IOptions<LiveSassCompileOptions> _options;

        public SassInitialiser(IOptions<LiveSassCompileOptions> options)
        {
            _options = options;
        }

        public void StartFileWatchers()
        {
            if (!_options.Value.SassCompileEnabled)
                return;

            var fileAndFolderMaps = _options.Value.FileAndFolderMaps;

            foreach(var fileOrFolderMap in fileAndFolderMaps)
            {
                var sassFileWatcher = new SassFileWatcher(fileOrFolderMap.Source, fileOrFolderMap.Destination, _options.Value.CompileFilesWithLeadingUnderscores);
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
