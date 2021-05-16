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
            var directory = @"/Users/mario/Projects/getjarstats/getjarstats/data";
            var jarFiles = GetJarFiles(directory);
            foreach (var jarFile in jarFiles)
            {
                var jarFileName = Path.GetFileName(jarFile);
                Console.WriteLine($"file: {jarFileName}");
                var jarClasses = GetJarClasses(jarFile);
                int i = 0;
                foreach (var jarClass in jarClasses)
                {
                    i++;
                    Console.WriteLine($"class: {jarClass}");
                    if (i >= 5)
                    {
                        break;
                    }
                }
                Console.WriteLine($"number of classes found: {jarClasses.Count}");
            }
            Console.WriteLine("*** end ***");
        }

        private static List<string> GetJarClasses(string jarFile)
        {
            var jarClasses = new List<string>();
            using (var archive = ZipFile.OpenRead(jarFile))
            {
                foreach (var entry in archive.Entries)
                {
                    var classExtension = ".class";
                    var jarClass = entry.FullName;
                    if (jarClass.EndsWith(classExtension))
                    {
                        jarClass = jarClass.Substring(0, jarClass.Length - classExtension.Length);
                        jarClass = jarClass.Replace("/", ".");
                        jarClasses.Add(jarClass);
                    }
                }
            }
            return jarClasses;
        }

        private static List<string> GetJarFiles(string directory)
        {
            var jarFiles = new List<string>();
            var files = Directory.GetFiles(directory);
            foreach (var file in files)
            {
                if (file.EndsWith(".jar", StringComparison.OrdinalIgnoreCase))
                {
                    jarFiles.Add(file);
                }
            }
            return jarFiles;
        }
    }
}
