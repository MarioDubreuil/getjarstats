using System;
using System.Collections.Generic;
using System.IO.Compression;

namespace getjarstats
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** begin ***");
            string zipPath = @"/Users/mario/Projects/getjarstats/getjarstats/data/junit-platform-console-standalone-1.6.2.jar";
            List<string> classes = new List<string>();
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                Console.WriteLine("archive has been opened");
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".class"))
                    {
                        classes.Add(entry.FullName);
                    }
                }
            }
            Console.WriteLine($"classed found: {classes.Count}");
            Console.WriteLine("*** end ***");
        }
    }
}
