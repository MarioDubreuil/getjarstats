﻿using System;
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
            var classesByArchive = GetClassesByArchive(directory);
            var archivesByClass = GetArchivesByClass(classesByArchive);
            // WriteStats(classesByArchive);
            Console.WriteLine("*** end ***");
            Console.WriteLine("*** end ***");
            Console.WriteLine("*** end ***");
        }

        private static Dictionary<string, List<string>> GetClassesByArchive(string directory)
        {
            var classesByArchive = new Dictionary<string, List<string>>();
            var files = Directory.GetFiles(directory);
            foreach (var file in files)
            {
                if (file.EndsWith(".jar", StringComparison.OrdinalIgnoreCase))
                {
                    var archiveName = Path.GetFileName(file);
                    var archiveClasses = GetArchiveClasses(file);
                    classesByArchive.Add(archiveName, archiveClasses);
                }
            }
            return classesByArchive;
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
        private static Dictionary<string, List<string>> GetArchivesByClass(Dictionary<string, List<string>> javaClassesByArchiveDictionary)
        {
            var javaArchivesByclassDict = new Dictionary<string, List<string>>();
            foreach (var javaArchive in javaClassesByArchiveDictionary)
            {
                var javaArchiveName = javaArchive.Key;
                var javaArchiveClasses = javaArchive.Value;
                foreach (var javaArchiveClass in javaArchiveClasses)
                {
                    // Console.WriteLine($"archive={javaArchiveName} class={javaArchiveClass}");
                    if (!javaArchivesByclassDict.ContainsKey(javaArchiveClass))
                    {
                        javaArchivesByclassDict.Add(javaArchiveClass, new List<string>());
                    }
                    javaArchivesByclassDict[javaArchiveClass].Add(javaArchiveName);
                }
            }
            return javaArchivesByclassDict;
        }
        private static void WriteStats(Dictionary<string, List<string>> classesByArchive)
        {
            Console.WriteLine($"number of jar files: {classesByArchive.Count}");
            foreach (var archive in classesByArchive)
            {
                Console.WriteLine($"file: {archive.Key}");
                var archiveClasses = archive.Value;
                Console.WriteLine($"number of classes: {archiveClasses.Count}");
                int i = 0;
                foreach (var archiveClass in archiveClasses)
                {
                    i++;
                    if (i > 5)
                    {
                        Console.WriteLine("...");
                        break;
                    }
                    Console.WriteLine($"class: {archiveClass}");
                }
            }
        }
    }
}
