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
            var javaClasses = new List<JavaClass>();
            foreach (var jarFile in jarFiles)
            {
                var javaArchive = new JavaArchive(jarFile);
                javaArchives.Add(javaArchive);
                foreach (var jarClass in jarFile.JarClasses)
                {
                    var javaClass = new JavaClass(jarClass);
                    javaClasses.Add(javaClass);
                }
            }
            WriteStatsJarFiles(jarFiles);
            WriteStatsJavaArchives(javaArchives);
            WriteStatsJavaClasses(javaClasses);
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
                    if (i > 5)
                    {
                        Console.WriteLine("...");
                        break;
                    }
                    Console.WriteLine($"class: {jarClass}");
                }
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

        private static void WriteStatsJavaClasses(List<JavaClass> javaClasses)
        {
            Console.WriteLine($"number of java classes: {javaClasses.Count}");
            int i = 0;
            foreach (var javaClass in javaClasses)
            {
                i++;
                if (i > 5)
                {
                    Console.WriteLine("...");
                    break;
                }
                Console.WriteLine($"class: {javaClass.Name}");
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
