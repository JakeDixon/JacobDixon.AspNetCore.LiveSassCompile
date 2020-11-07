using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JacobDixon.AspNetCore.LiveSassCompile
{
    public class SassFileWatcher
    {
        private FileSystemWatcher _fileWatcher;
        private readonly SassFileWatcherOptions _options;

        public SassFileWatcher(SassFileWatcherOptions options)
        {
            _options = options;
        }

        public void StartFileWatcher()
        {
            _fileWatcher = new FileSystemWatcher(_options.SourcePath);

            foreach (var filter in _options.FileNameFilters)
                _fileWatcher.Filters.Add(filter);

            _fileWatcher.EnableRaisingEvents = true;
            _fileWatcher.IncludeSubdirectories = true;

            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite
                                   | NotifyFilters.FileName
                                   | NotifyFilters.DirectoryName;

            _fileWatcher.Changed += FileWatcher_Changed;
            _fileWatcher.Created += FileWatcher_Changed;
            _fileWatcher.Renamed += FileWatcher_Renamed;
        }

        public void StopFileWatcher()
        {
            _fileWatcher.Changed -= FileWatcher_Changed;
            _fileWatcher.Created -= FileWatcher_Changed;
            _fileWatcher.Renamed -= FileWatcher_Renamed;
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher?.Dispose();
            _fileWatcher = null;
        }

        private void FileChanged(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            var fileName = Path.GetFileName(filePath);

            if (IsExcluded(fileName))
                return;

            var cssFilePath = Path.ChangeExtension(filePath, ".css");
            var cssFileName = Path.GetFileName(cssFilePath);

            var result = LibSassHost.SassCompiler.CompileFile(
                filePath,
                cssFileName,
                options: new LibSassHost.CompilationOptions
                {
                    OutputStyle = LibSassHost.OutputStyle.Compressed
                });

            var relativePath = Path.GetRelativePath(_options.SourcePath, cssFilePath);

            File.WriteAllText(Path.Combine(_options.DestinationPath, relativePath), result.CompiledContent);
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

        private void FileWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            FileChanged(e.FullPath);
        }

        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            FileChanged(e.FullPath);
        }
    }
}
