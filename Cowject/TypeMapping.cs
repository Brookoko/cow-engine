namespace Cowject
{
    using System;
    using System.Collections.Generic;
    
    internal class TypeMapping
    {
        private readonly Dictionary<Type, Mapping> mappings = new Dictionary<Type, Mapping>();
        
        public Mapping Create(Type type)
        {
            if (mappings.ContainsKey(type))
            {
                throw new BindingException($"Multiple bindings for: {type}");
            }
            var mapping = new Mapping {Type = type};
            mappings.Add(type, mapping);
            return mapping;
        }
        
        public Mapping GetMapping(Type type)
        {
            if (TryGetMapping(type, out var implementation))
            {
                return implementation;
            }
            throw new BindingException($"No binding for type: {type}");
        }
        
        public bool TryGetMapping(Type type, out Mapping mapping)
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
        
        public bool ShouldInitialize { get; set; } = true;
    }
}