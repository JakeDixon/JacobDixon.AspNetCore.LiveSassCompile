using JacobDixon.AspNetCore.LiveSassCompile.Compilers;
using LiveSassCompileUnitTests.Compilers;
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
        private string _testRootPath;

        [Theory]
        [ClassData(typeof(SassCompilerTestDataCollection))]
        public void Compile(List<SassCompilerTestData> filesToCreate, string sourceDirectory, string destinationDirectory)
        {
            bool fileExists = false;
            try
            {
                // Arrange
                foreach (var file in filesToCreate)
                {
                    WriteScssFile(file.SourceLocation, file.FileContent);
                }
                SassFileWatcherOptions options = new SassFileWatcherOptions() { CompileOnStart = false, DestinationPath = destinationDirectory, SourcePath = sourceDirectory };
                ICompiler compiler = new SassCompiler(options);

                // Act
                compiler.Compile(sourceDirectory);

                foreach(var file in filesToCreate)
                {
                    if (!compiler.IsExcluded(Path.GetFileName(file.SourceLocation)))
                    {
                        fileExists = File.Exists(file.DestinationLocation);
                        if (fileExists == false)
                            break;
                    }
                }
            }
            finally
            {
                CleanUpTestEnvironment();
            }
            // Assert
            Assert.True(fileExists);
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

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, content);
        }
    }
}