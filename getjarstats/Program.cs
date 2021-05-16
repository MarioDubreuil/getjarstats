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
            var directory = @"/Users/mario/Projects/getjarstats/getjarstats/data";
            var jarFiles = GetJarFiles(directory);
            var javaArchives = new List<JavaArchive>();
            foreach (var jarFile in jarFiles)
            {
                var javaArchive = new JavaArchive(jarFile);
                javaArchives.Add(javaArchive);
            }
            WriteStatsJarFiles(jarFiles);
            WriteStatsJavaArchives(javaArchives);
            Console.WriteLine("*** end ***");
            Console.WriteLine("*** end ***");
            Console.WriteLine("*** end ***");
        }

        private static void WriteStatsJarFiles(List<JarFile> jarFiles)
        {
            Console.WriteLine($"number of jar files: {jarFiles.Count}");
            foreach (var jarFile in jarFiles)
            {
                Console.WriteLine($"file: {jarFile.JarFileName}");
                var jarClasses = jarFile.JarClasses;
                Console.WriteLine($"number of classes: {jarClasses.Count}");
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
                Console.WriteLine("...");
            }
        }

        private static void WriteStatsJavaArchives(List<JavaArchive> javaArchives)
        {
            Console.WriteLine($"number of java archives: {javaArchives.Count}");
            foreach (var javaArchive in javaArchives)
            {
                Console.WriteLine($"archive: {javaArchive.FileName}");
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
