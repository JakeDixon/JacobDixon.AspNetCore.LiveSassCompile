using JacobDixon.AspNetCore.LiveSassCompile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit.Sdk;

namespace LiveSassCompileUnitTests
{
    public static class TestPaths
    {
        public static string RootDirectory { get; set; }
        public static string SourceDirectory { get; set; }
        public static string DestinationDirectory { get; set; }

        private static string _testDirectoryName = "LiveSassCompileTest";

        static TestPaths()
        {
            var tempDir = Environment.GetEnvironmentVariable("temp");
            if (tempDir.IsNullOrEmpty())
                throw new XunitException("Unable to load the temp directory from environment variables.");

            RootDirectory = Path.Combine(tempDir, _testDirectoryName);
            Directory.CreateDirectory(RootDirectory);

            SourceDirectory = Path.Combine(RootDirectory, "Source");
            Directory.CreateDirectory(SourceDirectory);

            DestinationDirectory = Path.Combine(RootDirectory, "Destination");
            Directory.CreateDirectory(DestinationDirectory);
        }
    }
}
