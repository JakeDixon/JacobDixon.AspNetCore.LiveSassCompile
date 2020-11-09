using JacobDixon.AspNetCore.LiveSassCompile.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace JacobDixon.AspNetCore.LiveSassCompile
{
    public class SassFileWatcher
    {
        private FileSystemWatcher _fileWatcher;
        private readonly SassFileWatcherOptions _options;
        private Dictionary<string, DateTime> _lastRead = new Dictionary<string, DateTime>();

        public SassFileWatcher(SassFileWatcherOptions options)
        {
            _options = options;

            if (string.IsNullOrEmpty(_options.SourcePath))
                throw new EmptyStringException("SourcePath option must not be empty or null");

            if (string.IsNullOrEmpty(_options.DestinationPath))
                throw new EmptyStringException("DestinationPath option must not be empty or null");

            if (_options.FileNameFilters.Count == 0)
                throw new EmptyArrayException("FileNameFilters must contain atleast one filter");
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

            if (_options.CompileOnStart)
            {
                var sassCompiler = new SassCompiler(_options);
                sassCompiler.Compile(_options.SourcePath);
            }
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

            var lastWriteTime = File.GetLastWriteTime(filePath);

            if (!_lastRead.ContainsKey(filePath) || _lastRead[filePath] < lastWriteTime)
            {
                var sassCompiler = new SassCompiler(_options);
                sassCompiler.Compile(filePath);

                _lastRead[filePath] = DateTime.Now;
            }
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
