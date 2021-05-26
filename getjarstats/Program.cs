using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace getjarstats
{
    class Program
    {
        static void Main(string[] args)
        {
            var directory = @"/Users/mario/Projects/getjarstats/getjarstats/data";
            var archivesDictionary = GetArchivesDictionary(directory);
            WriteStats(archivesDictionary);
            // var classesDictionary = GetClassesDictionary(archivesDictionary);
            // var archivesWithDuplicateClassesDictionary = GetArchivesWithDuplicateClassesDictionary(classesDictionary);
            // WriteStats(archivesDictionary, classesDictionary, archivesWithDuplicateClassesDictionary);
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
        private static void WriteStats(Dictionary<string, List<string>> archivesDictionary)
        {
            bool duplicateFound = false;
            foreach (var archivesDictionaryFirstItem in archivesDictionary)
            {
                var archiveFirst = archivesDictionaryFirstItem.Key;
                var classesFirst = archivesDictionaryFirstItem.Value;
                foreach (var archivesDictionarySecondItem in archivesDictionary)
                {
                    var archiveSecond = archivesDictionarySecondItem.Key;
                    var classesSecond = archivesDictionarySecondItem.Value;
                    if (string.Compare(archiveSecond, archiveFirst) > 0) // archiveSecond > archiveFirst
                    {
                        var duplicateClasses = classesFirst.Intersect(classesSecond).ToList();
                        if (duplicateClasses.Count > 0)
                        {
                            duplicateFound = true;
                            var percentageFirst =  duplicateClasses.Count * 100 / classesFirst.Count;
                            var percentageSecond =  duplicateClasses.Count * 100 / classesSecond.Count;
                            var randomDuplicateIndex = new Random().Next(duplicateClasses.Count);
                            Console.WriteLine($"Found {duplicateClasses.Count:N0} duplicate class{(duplicateClasses.Count > 1 ? "es" : "")} between {archiveFirst} and {archiveSecond}.");
                            Console.WriteLine($" {(duplicateClasses.Count > 1 ? "Those duplicate classes represent" : "This duplicate class represents")} {percentageFirst}% of the {classesFirst.Count:N0} class{(classesFirst.Count > 1 ? "es" : "")} in {archiveFirst} and {percentageSecond}% of the {classesSecond.Count:N0} class{(classesSecond.Count > 1 ? "es" : "")} in {archiveSecond}.");
                            Console.WriteLine($" One such duplicate class is {duplicateClasses[randomDuplicateIndex]}.");
                        }
                    }
                }
            }
            if (!duplicateFound)
            {
                Console.WriteLine($"Did not find a duplicate class in {archivesDictionary.Count:N0} jar file{(archivesDictionary.Count > 1 ? "s" : "")}.");
            }
        }
    }
}
