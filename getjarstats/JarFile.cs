using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace getjarstats
{
    public class JarFile
    {
        private const string classExtension = ".class";
        private string jarFileName;
        private List<string> jarClasses = new List<string>();

        public JarFile(string file)
        {
            jarFileName = Path.GetFileName(file);
            using (var archive = ZipFile.OpenRead(file))
            {
                foreach (var entry in archive.Entries)
                {
                    var jarClass = entry.FullName;
                    if (jarClass.EndsWith(classExtension))
                    {
                        jarClass = jarClass.Substring(0, jarClass.Length - classExtension.Length);
                        jarClass = jarClass.Replace("/", ".");
                        jarClasses.Add(jarClass);
                    }
                }
            }
        }

        public List<string> JarClasses { get => jarClasses; }
        public string JarFileName { get => jarFileName; }
    }
}
