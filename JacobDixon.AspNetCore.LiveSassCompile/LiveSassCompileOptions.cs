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
        /// Default: { "Development" }
        /// </summary>
        public List<string> EnvironmentsToActivateIn { get; set; } = new List<string> { "Development" };

        /// <summary>
        /// Determines whether scss compile is enabled. If off, there's no
        /// overhead as this background service will not start any file watchers.
        /// Default: false
        /// </summary>
        public bool SassCompileEnabled { get; set; }

        /// <summary>
        /// The folders to monitor for sass/scss file changes 
        /// and the matching destination folders.
        /// </summary>
        public List<FileOrFolderMap> FileAndFolderMaps { get; set; }

        /// <summary>
        /// Should the system compile files with leading underscores?
        /// Standard naming conventions for SASS state that partial
        /// files are prefixed with underscores.
        /// Default: false
        /// </summary>
        public bool CompileFilesWithLeadingUnderscores { get; set; }
    }
}
