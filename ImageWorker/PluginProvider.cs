namespace CowEngine.ImageWorker
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Cowject;

    internal interface IPluginProvider
    {
        List<Type> Types { get; }
    }
    
    public class PluginProvider : IPluginProvider
    {
        public List<Type> Types { get; } = new List<Type>();
        
        [PostConstruct]
        public void Prepare()
        {
            var directory = Directory.GetCurrentDirectory();
            var info = new DirectoryInfo(directory);
            foreach (var dll in info.EnumerateFiles().Where(f => f.Name.EndsWith(".dll")))
            {
                var assembly = Assembly.LoadFile(dll.FullName);
                Types.AddRange(assembly.GetExportedTypes());
            }
        }
    }
}