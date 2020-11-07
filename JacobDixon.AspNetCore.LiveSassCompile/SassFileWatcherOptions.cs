using System;
using System.Collections.Generic;
using System.Text;

namespace JacobDixon.AspNetCore.LiveSassCompile
{
    public class SassFileWatcherOptions
    {
        /// <summary>
        /// The source path to watch for file changes.
        /// </summary>
        public string SourcePath { get; set; }

        /// <summary>
        /// The destination path to write compiles css files out to.
        /// Default: wwwroot\css
        /// </summary>
        public string DestinationPath { get; set; } = "wwwroot\\css";

        /// <summary>
        /// The file extensions to watch for changes. 
        /// Accepts an array of glob patterns
        /// Default: [ "*.scss", "*.sass" ]
        /// </summary>
        public List<string> FileNameFilters { get; set; } = new List<string> { "*.scss", "*.sass" };

        /// <summary>
        /// The file name patters to exclude from compiling. 
        /// Accepts an array of glob patterns.
        /// Default: [ "_*" ]
        /// </summary>
        public List<string> FileNameExclusions { get; set; } = new List<string> { "_*" };
    }
}
