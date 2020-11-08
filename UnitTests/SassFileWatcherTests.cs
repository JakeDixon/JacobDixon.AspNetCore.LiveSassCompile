using JacobDixon.AspNetCore.LiveSassCompile;
using JacobDixon.AspNetCore.LiveSassCompile.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace LiveSassCompileUnitTests
{
    [TestClass]
    public class SassFileWatcherTests
    {
        private const string _testDirectoryName = "LiveSassCompileTest";
        private string _testRootPath;
        private string _testSourcePath;
        private string _testDestinationPath;

        [TestInitialize]
        public void InitialiseTestEnvironment()
        {
            var tempDir = Environment.GetEnvironmentVariable("temp");
            if (string.IsNullOrEmpty(tempDir))
                throw new AssertFailedException("Unable to load the temp directory from environment variables.");

            _testRootPath = Path.Combine(tempDir, _testDirectoryName);
            Directory.CreateDirectory(_testRootPath);

            _testSourcePath = Path.Combine(_testRootPath, "Source");
            Directory.CreateDirectory(_testSourcePath);

            _testDestinationPath = Path.Combine(_testRootPath, "Destination");
            Directory.CreateDirectory(_testDestinationPath);
        }


        [TestMethod]
        public void Constructor_NullSourcePath_ThrowsEmptyStringException()
        {
            // Arrange
            var options = new SassFileWatcherOptions { SourcePath = null };

            // Assert
            Assert.ThrowsException<EmptyStringException>(() => new SassFileWatcher(options));
        }

        [TestMethod]
        public void Constructor_EmptyFileNameFilters_ThrowsEmptyArrayException()
        {
            // Arrange
            var options = new SassFileWatcherOptions { SourcePath = "test", FileNameFilters = new List<string>() };

            // Assert
            Assert.ThrowsException<EmptyArrayException>(() => new SassFileWatcher(options));
        }

        [TestMethod]
        public void StartFileWatcher_FileChange_UpdatesDestinationFile()
        {
            // Arrange
            var options = new SassFileWatcherOptions { SourcePath = _testSourcePath, DestinationPath = _testDestinationPath };

            // Assert
            Assert.ThrowsException<EmptyArrayException>(() => new SassFileWatcher(options));
        }

        [TestCleanup]
        public void CleanUpTestEnvironment()
        {
            Directory.Delete(_testRootPath, true);
        }
    }
}
