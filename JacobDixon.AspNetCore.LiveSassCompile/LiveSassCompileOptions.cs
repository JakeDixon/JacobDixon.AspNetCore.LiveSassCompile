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
        /// An array of environments LiveSassCompile should activate in.
        /// Default: [ "Development" ]
        /// </summary>
        public List<string> EnvironmentsToActivateIn { get; set; } = new List<string> { "Development" };

        /// <summary>
        /// The folders to monitor for sass/scss file changes 
        /// and the matching destination folders.
        /// </summary>
        public List<SassFileWatcherOptions> SassFileWatchers { get; set; }
    }
}
