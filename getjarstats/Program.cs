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
                            var percentageFirst =  duplicateClasses.Count * 100 / classesFirst.Count;
                            var percentageSecond =  duplicateClasses.Count * 100 / classesSecond.Count;
                            Console.WriteLine($"Found {duplicateClasses.Count:N0} duplicate classes between {archiveFirst} and {archiveSecond}.");
                            Console.WriteLine($"Those duplicate classes represent {percentageFirst}% of the {classesFirst.Count:N0} classes in {archiveFirst} and {percentageSecond}% of the {classesSecond.Count:N0} classes in {archiveSecond}.");
                            Console.WriteLine($"One such duplicate class is {duplicateClasses[0]}.");
                            // TODO: display no duplicate found message when no duplicate
                            // TODO: sort archives by name (ie don't use dictionary)
                            // TODO: randomly select a duplicate class
                            // TODO: singular vs plural - Found x duplicate classes vs Found 1 duplicate class
                            // TODO: singular vs plural - Those duplicate classes vs This duplicate class
                            // TODO: singular vs plural - of the x classes vs of the 1 class
                            // TODO: fix local git because the hashset and getjarstats/hashset branches are still visible in vscode even though I deleted from github
                            // TODO: test merging a branch locally instead of doing a pull request in github
                        }
                    }
                }
            }
        }
    }
}
