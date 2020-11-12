using JacobDixon.AspNetCore.LiveSassCompile.Compilers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Sdk;

namespace JacobDixon.AspNetCore.LiveSassCompile.Compilers.Tests
{
    public class SassCompilerTests
    {
        private const string _testDirectoryName = "LiveSassCompileTest.SassCompiler";
        private string _testRootPath;
        private string _testSourcePath;
        private string _testDestinationPath;
        private SassFileWatcherOptions _testOptions;

        private void InitialiseTestEnvironment()
        {
            var tempDir = Environment.GetEnvironmentVariable("temp");
            if (string.IsNullOrEmpty(tempDir))
                throw new XunitException("Unable to load the temp directory from environment variables.");

            _testRootPath = Path.Combine(tempDir, _testDirectoryName);
            Directory.CreateDirectory(_testRootPath);

            _testSourcePath = Path.Combine(_testRootPath, "Source");
            Directory.CreateDirectory(_testSourcePath);

            _testDestinationPath = Path.Combine(_testRootPath, "Destination");
            Directory.CreateDirectory(_testDestinationPath);

            _testOptions = new SassFileWatcherOptions { SourcePath = _testSourcePath, DestinationPath = _testDestinationPath, CompileOnStart = false };
        }

        [Fact]
        public void Compile_DirectoryWithSingleFile_CompilesSuccessfully()
        {
            bool fileExists = false;
            try
            {
                // Arrange
                InitialiseTestEnvironment();
                var folderPath = Path.Combine(_testSourcePath, "subfolder");
                Directory.CreateDirectory(folderPath);


                // Act

            }
            finally
            {
                CleanUpTestEnvironment();
            }
            // Assert
            throw new XunitException();
        }

        [Fact]
        public void IsExcludedTest()
        {
            throw new XunitException();
        }

        private void CleanUpTestEnvironment()
        {
            if (Directory.Exists(_testRootPath))
                Directory.Delete(_testRootPath, true);
        }

        private void WriteScssFile(string path, string content = null)
        {
            if (content == null)
            {
                content = @"body {  
color: red;
}";
            }

            File.WriteAllText(path, content);
        }
    }
}