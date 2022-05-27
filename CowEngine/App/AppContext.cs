namespace CowEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using Cowject;

public class AppContext
{
    private readonly DiContainer container;

    public AppContext()
    {
        container = new DiContainer();
    }

    public void Launch(ITypeProvider typeProvider)
    {
        var modules = FindModules(typeProvider);
        InstallModules(modules);
    }
    
    private List<IModule> FindModules(ITypeProvider typeProvider)
    {
        return typeProvider.Types
            .Where(IsNeededType)
            .Select(type => (IModule)Activator.CreateInstance(type))
            .OrderBy(module => module.Priority)
            .ToList();

        bool IsNeededType(Type type)
        {
            return typeof(IModule).IsAssignableFrom(type) &&
                   type != typeof(IModule);
        }
    }

    private void InstallModules(List<IModule> modules)
    {
        foreach (var module in modules)
        {
            container.Inject(module);
            module.Prepare(container);
        }
    }

    public T Get<T>() where T : class
    {
        return container.Get<T>();
    }
}
