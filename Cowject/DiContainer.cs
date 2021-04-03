namespace Cowject
{
    using System;
    using Commands;
    using Signals;

    public class DiContainer
    {
        private readonly TypeMapping mapping;
        private readonly IInjector injector;
        private readonly ICommandInjector commandInjector;
        
        public DiContainer()
        {
            mapping = new TypeMapping();
            injector = new Injector(mapping);
            commandInjector = new CommandInjector(this, injector);
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
        
        public ICommandBinder BindSignal<T>() where T : BaseSignal
        {
            return commandInjector.Bind<T>();
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
            return (T) Get(typeof(T));
        }
        
        public object Get(Type type)
        {
            return injector.Get(type);
        }
        
        public object Inject(object obj)
        {
            return injector.Inject(obj);
        }
    }
}