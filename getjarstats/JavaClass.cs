using System;
namespace getjarstats
{
    public class JavaClass
    {
        private string name;
        public JavaClass(string name)
        {
            this.name = name;
        }

        public string Name { get => name; }
    }
}
