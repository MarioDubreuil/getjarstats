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
            WriteJarFiles(jarFiles);
            Console.WriteLine("*** end ***");
        }

        private static void WriteJarFiles(List<JarFile> jarFiles)
        {
            foreach (var jarFile in jarFiles)
            {
                Console.WriteLine($"file: {jarFile.JarFileName}");
                var jarClasses = jarFile.JarClasses;
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
        }

        private static List<JarFile> GetJarFiles(string directory)
        {
            var jarFiles = new List<JarFile>();
            var files = Directory.GetFiles(directory);
            foreach (var file in files)
            {
                if (file.EndsWith(".jar", StringComparison.OrdinalIgnoreCase))
                {
                    var jarFile = new JarFile(file);
                    jarFiles.Add(jarFile);
                }
            }
            return jarFiles;
        }
    }
}
