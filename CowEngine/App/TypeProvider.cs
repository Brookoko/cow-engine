namespace CowEngine;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

public interface ITypeLoader
{
    void LoadTypes();
}

public interface ITypeProvider
{
    List<Type> Types { get; }
}

public class TypeProvider : ITypeProvider, ITypeLoader
{
    public List<Type> Types { get; } = new();

    private readonly Regex assemblyRegex = new(@"(Cow|Renderer|Library|Worker)");

    public void LoadTypes()
    {
        LoadAssembliesToDomain();
        LoadFromAppDomain();
    }

    private void LoadAssembliesToDomain()
    {
        var directory = Directory.GetCurrentDirectory();
        var info = new DirectoryInfo(directory);
        var dlls = info.EnumerateFiles()
            .Where(f => f.Name.EndsWith(".dll") && assemblyRegex.IsMatch(f.Name))
            .Select(f => f.FullName)
            .ToArray();
        foreach (var dll in dlls)
        {
            LoadAssembly(dll);
        }
    }

    private void LoadAssembly(string assemblyPath)
    {
        Assembly.LoadFrom(assemblyPath);
    }

    private void LoadFromAppDomain()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(ass => !ass.IsDynamic)
            .SelectMany(assembly => assembly.GetExportedTypes());
        Types.AddRange(types);
    }
}
