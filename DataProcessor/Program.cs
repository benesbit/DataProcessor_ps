using System;
using static System.Console;
using System.IO;
using System.Threading;
using System.Runtime.Caching;

namespace DataProcessor
{
    class Program
    {
        private static MemoryCache FilesToProcess = MemoryCache.Default;

        static void Main(string[] args)
        {
            WriteLine("Parsing command line options");

            // Command line validation omitted for now

            var directoryToWatch = args[0];

            if (!Directory.Exists(directoryToWatch))
            {
                WriteLine($"ERROR: {directoryToWatch} does not exist");
            }
            else
            {
                WriteLine($"Watching directory {directoryToWatch} for changes");

                using (var inputFileWatcher = new FileSystemWatcher(directoryToWatch))
                {
                    inputFileWatcher.IncludeSubdirectories = false;
                    inputFileWatcher.InternalBufferSize = 32768; // 32 KB
                    inputFileWatcher.Filter = "*.*"; // Monitor all files
                    inputFileWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

                    inputFileWatcher.Created += FileCreated;
                    inputFileWatcher.Changed += FileChanged;
                    inputFileWatcher.Deleted += FileDeleted;
                    inputFileWatcher.Renamed += FileRenamed;
                    inputFileWatcher.Error += WatcherError;

                    inputFileWatcher.EnableRaisingEvents = true;

                    WriteLine("Press enter to quit...");
                    ReadLine();
                }
            }
        }

        private static void FileCreated(object sender, FileSystemEventArgs e)
        {
            WriteLine($"* File created: {e.Name} - type: {e.ChangeType}");

            AddToCache(e.FullPath);
        }

        private static void FileChanged(object sender, FileSystemEventArgs e)
        {
            WriteLine($"* File changed: {e.Name} - type: {e.ChangeType}");

            AddToCache(e.FullPath);
        }

        private static void FileDeleted(object sender, FileSystemEventArgs e)
        {
            WriteLine($"* File deleted: {e.Name} - type: {e.ChangeType}");
        }
        private static void FileRenamed(object sender, RenamedEventArgs e)
        {
            WriteLine($"* File renamed: {e.OldName} to {e.Name} - type: {e.ChangeType}");
        }
        private static void WatcherError(object sender, ErrorEventArgs e)
        {
            WriteLine($"ERROR: file system watching may no longer be active: {e.GetException()}");
        }

        private static void AddToCache(string fullPath)
        {
            var item = new CacheItem(fullPath, fullPath);

            var policy = new CacheItemPolicy
            {
                RemovedCallback = ProcessFile,
                SlidingExpiration = TimeSpan.FromSeconds(2),
            };

            FilesToProcess.Add(item, policy);
        }

        private static void ProcessFile(CacheEntryRemovedArguments args)
        {
            WriteLine($"* Cahce item removed: {args.CacheItem.Key} because {args.RemovedReason}");

            if (args.RemovedReason == CacheEntryRemovedReason.Expired)
            {
                var fileProcessor = new FileProcessor(args.CacheItem.Key);
                fileProcessor.Process();
            }
            else
            {
                WriteLine($"WARNING: {args.CacheItem.Key} was removed unexpectedly and may not be...");
            }
        }

        //private static void ProcessFiles(Object stateInfo)
        //{
        //    foreach (var fileName in FilesToProcess.Keys) // May not be in order of adding
        //    {
        //        if (FilesToProcess.TryRemove(fileName, out _))
        //        {
        //            var fileProcessor = new FileProcessor(fileName);
        //            fileProcessor.Process();
        //        }
        //    }
        //}

        //private static void ProcessSingleFile(string filePath)
        //{
        //    var fileProcessor = new FileProcessor(filePath);
        //    fileProcessor.Process();
        //}

        //private static void ProcessDirectory(string directoryPath, string fileType)
        //{
        //    // Get all files
        //    // var allFiles = Directory.GetFiles(directoryPath);

        //    switch (fileType)
        //    {
        //        case "TEXT":
        //            string[] textFiles = Directory.GetFiles(directoryPath, "*.txt");
        //            foreach (var textFilePath in textFiles)
        //            {
        //                var fileProcessor = new FileProcessor(textFilePath);
        //                fileProcessor.Process();
        //            }
        //            break;
        //        default:
        //            WriteLine($"ERROR: {fileType} is not supported");
        //            return;
        //    }
        //}
    }
}
