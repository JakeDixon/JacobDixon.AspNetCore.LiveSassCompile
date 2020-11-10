using JacobDixon.AspNetCore.LiveSassCompile;
using JacobDixon.AspNetCore.LiveSassCompile.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LiveSassCompileUnitTests
{
    [TestClass]
    public class SassFileWatcherTests
    {
        private const string _testDirectoryName = "LiveSassCompileTest";
        private string _testRootPath;
        private string _testSourcePath;
        private string _testDestinationPath;
        private SassFileWatcherOptions _testOptions;

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

            _testOptions = new SassFileWatcherOptions { SourcePath = _testSourcePath, DestinationPath = _testDestinationPath };
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
        public void StartFileWatcher_FileCreated_CreatesDestinationFile()
        {
            // Arrange
            var watcher = new SassFileWatcher(_testOptions);
            watcher.StartFileWatcher();

            // Act
            File.WriteAllText(Path.Combine(_testSourcePath, "styles.scss"), @"body {  
color: red;
}");
            // Give the file system a second to catch up
            Thread.Sleep(200);

            watcher.StopFileWatcher();
            // Assert
            Assert.IsTrue(File.Exists(Path.Combine(_testDestinationPath, "styles.css")));
        }

        [TestMethod]
        public void StartFileWatcher_FileUpdated_UpdatesDestinationFile()
        {
            // Arrange
            string _updateContentsScssFileName = "update.scss";
            string _updateContentsCssFileName = "update.css";

            WriteScssFile(Path.Combine(_testSourcePath, _updateContentsScssFileName));
            var watcher = new SassFileWatcher(_testOptions);
            watcher.StartFileWatcher();

            // Act
            File.WriteAllText(Path.Combine(_testSourcePath, _updateContentsScssFileName), @"body {  
color: blue;
}");
            // Give the file system a second to catch up
            Thread.Sleep(200);

            watcher.StopFileWatcher();
            // Assert
            Assert.IsTrue(File.ReadAllText(Path.Combine(_testDestinationPath, _updateContentsCssFileName)).Contains("blue"));
        }

        [TestMethod]
        public void StartFileWatcher_FileRenamed_CreatesNewDestinationFile()
        {
            string _renameFileScssOldFileName = "rename.scss";
            string _renameFileScssNewFileName = "renameNew.scss";
            string _renameFileCssNewFileName = "renameNew.css";


            // Arrange
            WriteScssFile(Path.Combine(_testSourcePath, _renameFileScssOldFileName));
            var watcher = new SassFileWatcher(_testOptions);
            watcher.StartFileWatcher();

            // Act
            var oldPath = Path.Combine(_testSourcePath, _renameFileScssOldFileName);
            var newPath = Path.Combine(_testSourcePath, _renameFileScssNewFileName);
            File.Move(oldPath, newPath);

            // Give the file system a second to catch up
            Thread.Sleep(200);

            watcher.StopFileWatcher();
            // Assert
            Assert.IsTrue(File.Exists(Path.Combine(_testDestinationPath, _renameFileCssNewFileName)));
        }

        [TestMethod]
        public void StartFileWatcher_FileRenamed_DeletesOldDestinationFile()
        {
            // Arrange
            string _renameFileScssOldFileName = "renameOldDeleted.scss";
            string _renameFileCssOldFileName = "renameOldDeleted.css";
            string _renameFileScssNewFileName = "renameNewOldDeleted.scss";
            var oldPath = Path.Combine(_testSourcePath, _renameFileScssOldFileName);
            WriteScssFile(oldPath);
            WriteScssFile(Path.Combine(_testDestinationPath, _renameFileCssOldFileName));
            var watcher = new SassFileWatcher(_testOptions);
            watcher.StartFileWatcher();

            // Act
            var newPath = Path.Combine(_testSourcePath, _renameFileScssNewFileName);
            File.Move(oldPath, newPath);

            // Give the file system a second to catch up
            Thread.Sleep(200);

            watcher.StopFileWatcher();
            // Assert
            Assert.IsFalse(File.Exists(Path.Combine(_testDestinationPath, _renameFileCssOldFileName)));
        }

        [TestMethod]
        public void StartFileWatcher_FileDeleted_DeletesDestinationFile()
        {
            // Arrange
            string _deleteFileScssFileName = "delete.scss";
            string _deleteFileCssFileName = "delete.css";
            var scssPath = Path.Combine(_testSourcePath, _deleteFileScssFileName);
            var cssPath = Path.Combine(_testDestinationPath, _deleteFileCssFileName);
            WriteScssFile(scssPath);
            WriteScssFile(cssPath);
            var watcher = new SassFileWatcher(_testOptions);
            watcher.StartFileWatcher();

            // Act
            File.Delete(scssPath);

            // Give the file system a second to catch up
            Thread.Sleep(200);

            watcher.StopFileWatcher();
            // Assert
            Assert.IsFalse(File.Exists(cssPath));
        }

        [TestCleanup]
        public void CleanUpTestEnvironment()
        {
            Directory.Delete(_testRootPath, true);
        }

        private void WriteScssFile(string path, string content = null)
        {
            if (content == null)
            {
                content = @"body {  
color: blue;
}";
            }

            File.WriteAllText(path, content);
        }
    }
}
