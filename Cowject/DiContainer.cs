namespace Cowject
{
    using System;

    public class DiContainer
    {
        private readonly TypeMapping mapping;
        private readonly IInjector injector;
        
        public DiContainer()
        {
            mapping = new TypeMapping();
            injector = new Injector(mapping);
            Bind<DiContainer>().ToInstance(this);
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
        
        public T Get<T>(object name = null) where T : class
        {
            return (T) Get(typeof(T), name);
        }
        
        public object Get(Type type, object name = null)
        {
            return injector.Get(type, name);
        }
        
        public object Inject(object obj)
        {
            return injector.Inject(obj);
        }
    }
}