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
            Console.WriteLine("*** begin ***");
            Console.WriteLine("*** begin ***");
            Console.WriteLine("*** begin ***");
            // TODO - Fix names (methods and variables)
            var directory = @"/Users/mario/Projects/getjarstats/getjarstats/data";
            var classes = GetArchivesClasses(directory);
            WriteStats(classes);
            Console.WriteLine("*** end ***");
            Console.WriteLine("*** end ***");
            Console.WriteLine("*** end ***");
        }

        private static Dictionary<string, List<string>> GetArchivesClasses(string directory)
        {
            var classes = new Dictionary<string, List<string>>();
            var files = Directory.GetFiles(directory);
            foreach (var file in files)
            {
                if (file.EndsWith(".jar", StringComparison.OrdinalIgnoreCase))
                {
                    var archiveName = Path.GetFileName(file);
                    var archiveClasses = GetArchiveClasses(file);
                    classes.Add(archiveName, archiveClasses);
                }
            }
            return classes;
        }

        private static List<string> GetArchiveClasses(string file)
        {
            var classes = new List<string>();
            using (var archive = ZipFile.OpenRead(file))
            {
                foreach (var entry in archive.Entries)
                {
                    var className = entry.FullName;
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

        private static void WriteStats(Dictionary<string, List<string>> classes)
        {
            Console.WriteLine($"number of jar files: {classes.Count}");
            foreach (var jarFile in classes)
            {
                Console.WriteLine($"file: {jarFile.Key}");
                var jarClasses = jarFile.Value;
                Console.WriteLine($"number of classes: {jarClasses.Count}");
                int i = 0;
                foreach (var jarClass in jarClasses)
                {
                    i++;
                    if (i > 5)
                    {
                        Console.WriteLine("...");
                        break;
                    }
                    Console.WriteLine($"class: {jarClass}");
                }
            }
        }
    }
}
