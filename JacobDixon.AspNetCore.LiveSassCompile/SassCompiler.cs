using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace JacobDixon.AspNetCore.LiveSassCompile
{
    public class SassCompiler
    {
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

            var cssFilePath = Path.ChangeExtension(filePath, ".css");
            var cssFileName = Path.GetFileName(cssFilePath);

            var attempts = 0;
            var successful = false;

            while (!successful && attempts < 2)
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
                    File.WriteAllText(Path.Combine(_options.DestinationPath, relativePath), result.CompiledContent);
                    successful = true;
                }
                catch
                {
                    if (attempts >= 2)
                        throw;
                    attempts++;
                    Thread.Sleep(100);
                }
            }
        }

        private bool IsExcluded(string fileName)
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
