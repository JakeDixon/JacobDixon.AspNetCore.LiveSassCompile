using System;
using System.Collections.Generic;
using System.Threading;

namespace JacobDixon.AspNetCore.LiveSassCompile
{
    public class LiveSassCompileOptions
    {
        /// <summary>
        /// The const string that defined the options section in settings 
        /// </summary>
        public const string OptionsName = "LiveSassCompileOptions";

        /// <summary>
        /// A boolean value which controls whether live compile is on (true) or off (false).
        /// Default: false
        /// </summary>
        public bool EnableLiveCompile { get; set; }

        /// <summary>
        /// The folders to monitor for sass/scss file changes 
        /// and the matching destination folders.
        /// </summary>
        public List<SassFileWatcherOptions> SassFileWatchers { get; set; }
    }
}
