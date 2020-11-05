using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JacobDixon.AspNetCore.LiveSassCompile
{
    public class SassFileWatcher
    {
        private readonly string[] _extensions = { ".scss", ".sass" };
        private FileSystemWatcher _fileWatcher;
        //private List<FileSystemWatcher> FolderWatchers;
        private string _sourceFolder;
        private string _destinationFolder;
        private bool _compileFilesWithLeadingUnderscores;

        public SassFileWatcher(string sourceFolder, string destinationFolder, bool compileFilesWithLeadingUnderscores)
        {
            _sourceFolder = sourceFolder;
            _destinationFolder = destinationFolder;
            _compileFilesWithLeadingUnderscores = compileFilesWithLeadingUnderscores;
        }

        public void StartFileWatcher()
        {
            //FolderToMonitorPath = Path.GetFullPath(path);
            //FolderToMonitorName = Path.GetFileName(FolderToMonitorPath);
            StartFilesWatcher();
            //StartFolderWatcher();
        }

        public void StopFileWatcher()
        {
            DisposeFilesWatcher();
            //DisposeFolderWatcher();
        }

        private void StartFilesWatcher()
        {
            _fileWatcher = new FileSystemWatcher(_sourceFolder);
            _fileWatcher.Filter = "*.*";
            _fileWatcher.EnableRaisingEvents = true;
            _fileWatcher.IncludeSubdirectories = true;

            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite
                                   | NotifyFilters.FileName
                                   | NotifyFilters.DirectoryName;

            _fileWatcher.Changed += FileWatcher_Changed;
            _fileWatcher.Created += FileWatcher_Changed;
            _fileWatcher.Renamed += FileWatcher_Renamed;
        }

        //private void StartFolderWatcher()
        //{
        //    var parentPath = Path.GetDirectoryName(FolderToMonitorPath);
        //    var folderName = Path.GetFileName(FolderToMonitorPath);
        //    FolderWatcher = new FileSystemWatcher(parentPath);
        //    FolderWatcher.Filter = folderName;
        //    FolderWatcher.EnableRaisingEvents = true;
        //    FolderWatcher.IncludeSubdirectories = false;


        //    FolderWatcher.Created += FolderWatcher_Created;
        //    FolderWatcher.Deleted += FolderWatcher_Deleted;
        //    FolderWatcher.Renamed += FolderWatcher_Renamed;
        //}

        private void DisposeFilesWatcher()
        {
            _fileWatcher.Changed -= FileWatcher_Changed;
            _fileWatcher.Created -= FileWatcher_Changed;
            _fileWatcher.Renamed -= FileWatcher_Renamed;
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher?.Dispose();
            _fileWatcher = null;
        }

        //private void DisposeFolderWatcher()
        //{
        //    FolderWatcher.Created -= FolderWatcher_Created;
        //    FolderWatcher.Deleted -= FolderWatcher_Deleted;
        //    FolderWatcher.Renamed -= FolderWatcher_Renamed;
        //    FolderWatcher.EnableRaisingEvents = false;
        //    FolderWatcher?.Dispose();
        //    FolderWatcher = null;
        //}

        private void FileChanged(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || fileName.Contains("\\node_modules\\"))
                return;

            var ext = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(ext))
                return;  // we don't care about extensionless files

            if (_extensions.Contains(ext))
            {
                var cssFilePath = Path.ChangeExtension(fileName, ".css");
                var cssFileName = Path.GetFileName(cssFilePath);

                var result = LibSassHost.SassCompiler.CompileFile(
                    fileName, 
                    cssFileName, 
                    options: new LibSassHost.CompilationOptions { 
                        OutputStyle = LibSassHost.OutputStyle.Compressed
                    });

                var relativePath = Path.GetRelativePath(_sourceFolder, cssFilePath);

                File.WriteAllText(Path.Combine(_destinationFolder, relativePath), result.CompiledContent);
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

        //private void FolderWatcher_Created(object sender, FileSystemEventArgs e)
        //{
        //    _isFolderCreated = true;
        //    StartFilesWatcher();
        //}

        //private void FolderWatcher_Deleted(object sender, System.IO.FileSystemEventArgs e)
        //{
        //    DisposeFilesWatcher();
        //}

        //private void FolderWatcher_Renamed(object sender, RenamedEventArgs e)
        //{
        //    if (string.Compare(e.Name, FolderToMonitorName, StringComparison.OrdinalIgnoreCase) == 0)
        //    {
        //        StartFileWatcher();
        //    }
        //    else if (string.Compare(e.OldName, FolderToMonitorName, StringComparison.OrdinalIgnoreCase) == 0)
        //    {
        //        DisposeFilesWatcher();
        //    }
        //}
    }
}
