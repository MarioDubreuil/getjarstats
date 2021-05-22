using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace getjarstats
{
    class Program
    {
        static void Main(string[] args)
        {
            var directory = @"/Users/mario/Projects/getjarstats/getjarstats/data";
            var archivesDictionary = GetArchivesDictionary(directory);
            var classesDictionary = GetClassesDictionary(archivesDictionary);
            var archivesWithDuplicateClassesDictionary = GetArchivesWithDuplicateClassesDictionary(classesDictionary);
            WriteStats(archivesDictionary, classesDictionary, archivesWithDuplicateClassesDictionary);
        }

        private static Dictionary<string, List<string>> GetArchivesDictionary(string directory)
        {
            var archivesDictionary = new Dictionary<string, List<string>>();
            var files = Directory.GetFiles(directory);
            foreach (var file in files)
            {
                if (file.EndsWith(".jar", StringComparison.OrdinalIgnoreCase))
                {
                    var archive = Path.GetFileName(file);
                    var classes = GetArchiveClasses(file);
                    archivesDictionary.Add(archive, classes);
                }
            }
            return archivesDictionary;
        }

        private static List<string> GetArchiveClasses(string file)
        {
            var classes = new List<string>();
            using (var zipArchive = ZipFile.OpenRead(file))
            {
                foreach (var zipArchiveEntry in zipArchive.Entries)
                {
                    var className = zipArchiveEntry.FullName;
                    var classExtension = ".class";
                    if (className.EndsWith(classExtension, StringComparison.OrdinalIgnoreCase))
                    {
                        className = className.Substring(0, className.Length - classExtension.Length);
                        className = className.Replace("/", ".");
                        classes.Add(className);
                    }
                }
            }
            return classes;
        }
        private static Dictionary<string, List<string>> GetClassesDictionary(Dictionary<string, List<string>> archivesDictionary)
        {
            var classesDictionary = new Dictionary<string, List<string>>();
            foreach (var archivesDictionaryItem in archivesDictionary)
            {
                var archive = archivesDictionaryItem.Key;
                var classes = archivesDictionaryItem.Value;
                foreach (var className in classes)
                {
                    if (!classesDictionary.ContainsKey(className))
                    {
                        classesDictionary.Add(className, new List<string>());
                    }
                    classesDictionary[className].Add(archive);
                }
            }
            return classesDictionary;
        }
        private static Dictionary<string, List<string>> GetArchivesWithDuplicateClassesDictionary(Dictionary<string, List<string>> classesDictionary)
        {
            var archivesWithDuplicateClassesDictionary = new Dictionary<string, List<string>>();
            foreach (var classesDictionaryItem in classesDictionary)
            {
                var className = classesDictionaryItem.Key;
                var archives = classesDictionaryItem.Value;
                if (archives.Count > 1)
                {
                    var archiveNames = string.Join(",", archives);
                    if (!archivesWithDuplicateClassesDictionary.ContainsKey(archiveNames))
                    {
                        archivesWithDuplicateClassesDictionary.Add(archiveNames, new List<string>());
                    }
                    archivesWithDuplicateClassesDictionary[archiveNames].Add(className);
                }
            }
            return archivesWithDuplicateClassesDictionary;
        }
        private static void WriteStats(Dictionary<string, List<string>> archivesDictionary, Dictionary<string, List<string>> classesDictionary, Dictionary<string, List<string>> archivesWithDuplicateClassesDictionary)
        {
            Console.WriteLine($"Found {classesDictionary.Count} classes in {archivesDictionary.Count} archives.");
            int count = 0;
            var duplicateArchivesHasSet = new HashSet<string>();
            foreach (var classesDictionaryItem in classesDictionary)
            {
                var archives = classesDictionaryItem.Value;
                if (archives.Count > 1)
                {
                    count++;
                    foreach (var archive in archives)
                    {
                        duplicateArchivesHasSet.Add(archive);
                    }
                }
            }
            Console.WriteLine($"Found {duplicateArchivesHasSet.Count} archives with at least one class that is in another archive.");
            Console.WriteLine($"Found {count} classes that are in more than 1 archive.");
            foreach (var archivesWithDuplicateClassesDictionaryItem in archivesWithDuplicateClassesDictionary)
            {
                var archivesNames = archivesWithDuplicateClassesDictionaryItem.Key;
                var classes = archivesWithDuplicateClassesDictionaryItem.Value;
                Console.WriteLine($"Found {classes.Count} classes that are in each of those archives: {archivesNames}. One such class is {classes[0]}.");
            }
        }
    }
}
