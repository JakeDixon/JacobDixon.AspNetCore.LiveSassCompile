using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace JacobDixon.AspNetCore.LiveSassCompile.Compilers
{
    public class SassCompiler : ICompiler
    {
        private const int _maxRetryAttempts = 2;
        private const string _compileFileExtension = ".css";
        private const int _msDelayBetweenRetries = 100;
        private SassFileWatcherOptions _options;
        public SassCompiler(SassFileWatcherOptions options)
        {
            _options = options;
        }

        public void Compile(string path)
        {
            var fileName = Path.GetFileName(path);
            var isDirectory = Directory.Exists(path);

            if (isDirectory || string.IsNullOrEmpty(fileName) || IsExcluded(fileName))
                CompileDirectory(_options.SourcePath);
            else
                CompileFile(path);
        }

        private void CompileDirectory(string path)
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                CompileFile(file);
            }

            var directories = Directory.GetDirectories(path);
            foreach (var directory in directories)
            {
                CompileDirectory(directory);
            }
        }

        private void CompileFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            var fileName = Path.GetFileName(filePath);

            if (IsExcluded(fileName))
                return;

            var cssFilePath = Path.ChangeExtension(filePath, _compileFileExtension);
            var cssFileName = Path.GetFileName(cssFilePath);

            var attempts = 0;
            var successful = false;

            while (!successful && attempts < _maxRetryAttempts)
            {
                try
                {
                    var result = LibSassHost.SassCompiler.CompileFile(
                        filePath,
                        cssFileName,
                        options: new LibSassHost.CompilationOptions
                        {
                            OutputStyle = LibSassHost.OutputStyle.Compressed,
                            IncludePaths = { _options.SourcePath }
                        });

                    var relativePath = Path.GetRelativePath(_options.SourcePath, cssFilePath);
                    var destinationFile = Path.Combine(_options.DestinationPath, relativePath);
                    var destinationPath = Path.GetDirectoryName(destinationFile);
                    Directory.CreateDirectory(destinationPath);
                    File.WriteAllText(Path.Combine(_options.DestinationPath, relativePath), result.CompiledContent);
                    successful = true;
                }
                catch (Exception e)
                {
                    if (attempts >= _maxRetryAttempts)
                        Console.WriteLine(e.ToString());
                    attempts++;
                    Thread.Sleep(_msDelayBetweenRetries);
                }
            }
        }

        public bool IsExcluded(string fileName)
        {
            foreach (var exclude in _options.FileNameExclusions)
            {
                if (fileName.MatchesGlob(exclude))
                    return true;
            }

            return false;
        }
    }
}
