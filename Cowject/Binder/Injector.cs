namespace Cowject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal class Injector : IInjector
    {
        private readonly TypeMapping mapping;
        
        public Injector(TypeMapping mapping)
        {
            this.mapping = mapping;
        }
        
        public object Inject(object obj)
        {
            return Inject(obj, new List<object>());
        }
        
        public object Inject(object obj, List<object> parameters)
        {
            var type = obj.GetType();
            parameters = parameters.ToList();
            var properties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(property => CanInject(type, property));
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                if (TryGetParameter(propertyType, parameters, out var value))
                {
                    property.SetValue(obj, value);
                }
                else
                {
                    property.SetValue(obj, Get(propertyType));
                }
            }
            return obj;
        }
        
        private bool TryGetParameter(Type type, List<object> parameters, out object value)
        {
            value = parameters.FirstOrDefault(type.IsInstanceOfType);
            parameters.Remove(value);
            return value != null;
        }
        
        private bool CanInject(Type type, PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<InjectAttribute>();
            if (attribute == null) return false;
            if (!property.CanWrite)
            {
                throw new InjectionException($"Failed to inject into readonly property: {type}");
            }
            return true;
        }
        
        public IBinder Bind<T>()
        {
            return Bind(typeof(T));
        }

        public IBinder Bind(Type type)
        {
            return new Binder(type, mapping);
        }

        public void Unbind<T>()
        {
            Unbind(typeof(T));
        }

        public void Unbind(Type type)
        {
            mapping.RemoveBindingFor(type);
        }

        public T Get<T>() where T : class
        {
            return Get<T>(new object[] { });
        }
        
        public T Get<T>(IEnumerable<object> parameters) where T : class
        {
            return (T) Get(typeof(T), parameters);
        }
        
        public object Get(Type type)
        {
            return Get(type, new object[] { });
        }

        public object Get(Type type, IEnumerable<object> parameters)
        {
            var mapped = mapping.GetMapping(type);
            var obj = InitializeComponent(mapped.ShouldInitialize, mapped.Instance ?? CreateInstance(mapped.Type), parameters);
            mapped.ShouldInitialize = mapped.Instance == null;
            return obj;
        }
        
        private object InitializeComponent(bool shouldInitialize, object component, IEnumerable<object> parameters)
        {
            if (shouldInitialize)
            {
                component = Inject(component, parameters.ToList());
                PrepareComponent(component);
            }
            return component;
        }

        private void PrepareComponent(object component)
        {
            var type = component.GetType();
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute<PostConstructAttribute>();
                if (attribute != null)
                {
                    method.Invoke(component, new object[] { });
                }
            }
        }
        
        private object CreateInstance(Type type)
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);
            var instance = constructor?.Invoke(new object[] { });
            if (instance == null)
            {
                throw new InjectionException($"Failed to create instance for: {type}");
            }
            return instance;
        }
    }
}