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

        public IBinder BindInterfacesTo<T>()
        {
            return BindInterfacesTo(typeof(T));
        }

        public IBinder BindInterfacesTo(Type type)
        {
            var interfaces = type.GetInterfaces();
            var binder = new Binder(interfaces[0], mapping);
            for (var i = 1; i < interfaces.Length; i++)
            {
                binder.Bind(interfaces[i]);
            }
            binder.To(type);
            return binder;
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
            return (T)Get(typeof(T), name);
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
