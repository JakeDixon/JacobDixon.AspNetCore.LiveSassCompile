using JacobDixon.AspNetCore.LiveSassCompile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LiveSassCompileUnitTests
{
    [TestClass]
    public class SassFileWatcherTests
    {
        [TestMethod]
        public void StartFileWatcher_NullSourcePath()
        {
            // set up
            var watcher = new SassFileWatcher(new SassFileWatcherOptions { SourcePath = null });

            // Assert
            // broken
            Assert.ThrowsException<ArgumentNullException>(watcher.StartFileWatcher);

        }
    }
}
