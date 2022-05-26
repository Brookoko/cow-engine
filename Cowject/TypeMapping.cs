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
            var mapping = new Mapping { Type = type };
            if (mappings.TryGetValue(type, out var map))
            {
                map.Add(mapping);
            }
            else
            {
                mappings.Add(type, new List<Mapping>() { mapping });
            }
            return mapping;
        }

        public List<Mapping> GetAllMapping(Type type)
        {
            if (TryGetAllMapping(type, out var implementation))
            {
                return implementation;
            }
            throw new BindingException($"No bindings for type: {type}");
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

        private bool TryGetMapping(Type type, object name, out Mapping mapping)
        {
            if (TryGetAllMapping(type, out var map))
            {
                mapping = map.First(m => name == null || name.Equals(m.Name));
                return true;
            }
            mapping = null;
            return false;
        }

        private bool TryGetAllMapping(Type type, out List<Mapping> mapping)
        {
            return mappings.TryGetValue(type, out mapping);
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
