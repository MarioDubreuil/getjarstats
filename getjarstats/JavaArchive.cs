using System;
namespace getjarstats
{
    public class JavaArchive
    {
        private string fileName;
        public JavaArchive(JarFile jarFile)
        {
            fileName = jarFile.JarFileName;
        }

        public string FileName { get => fileName; }
    }
}
