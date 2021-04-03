namespace Cowject.Commands
{
    using Signals;
    using System;
    
    internal interface ICommandInjector
    {
        ICommandBinder Bind<T>() where T : BaseSignal;
    }
    
    internal class CommandInjector : ICommandInjector
    {
        private readonly DiContainer container;
        private readonly IInjector injector;
        
        public CommandInjector(DiContainer container, IInjector injector)
        {
            this.container = container;
            this.injector = injector;
        }
        
        public ICommandBinder Bind<T>() where T : BaseSignal
        {
            return Bind(typeof(T));
        }
        
        private ICommandBinder Bind(Type type)
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);
            var signal = (BaseSignal) constructor?.Invoke(new object[] { });
            container.Bind(type).ToInstance(signal);
            return new CommandBinder(injector, signal);
        }
    }
}