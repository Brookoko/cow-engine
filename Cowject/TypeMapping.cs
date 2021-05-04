namespace Cowject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class TypeMapping
    {
        private readonly Dictionary<Type, List<Mapping>> mappings = new Dictionary<Type, List<Mapping>>();
        
        public Mapping Create(Type type)
        {
            if (mappings.TryGetValue(type, out var map) && map.Any(m => m.Name == null))
            {
                throw new BindingException($"Multiple bindings for: {type}");
            }
            var mapping = new Mapping {Type = type};
            if (map == null)
            {
                mappings.Add(type, new List<Mapping>() {mapping});
            }
            else
            {
                map.Add(mapping);
            }
            return mapping;
        }
        
        public Mapping GetMapping(Type type, object name)
        {
            if (TryGetMapping(type, name, out var implementation))
            {
                return implementation;
            }
            var nameInfo = name == null ? "" : $" with name {name}";
            throw new BindingException($"No binding for type: {type}{nameInfo}");
        }
        
        public bool TryGetMapping(Type type, object name, out Mapping mapping)
        {
            if (mappings.TryGetValue(type, out var map))
            {
                mapping = map.FirstOrDefault(m => name == null || name.Equals(m.Name));
                return true;
            }
            mapping = null;
            return false;
        }

        public void RemoveBindingFor(Type type)
        {
            mappings.Remove(type);
        }
    }
    
    public class Mapping
    {
        public Type Type { get; set; }
        
        public object Instance { get; set; }
        
        public object Name { get; set; }
        
        public bool ShouldInitialize { get; set; } = true;
    }
}