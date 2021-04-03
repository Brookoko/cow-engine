namespace Cowject.Commands
{
    using Signals;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface ICommandBinder
    {
        ICommandBinder To<T>() where T : Command;
    }

    public class CommandBinder : ICommandBinder
    {
        private readonly BaseSignal signal;
        private readonly List<Type> commands = new List<Type>();

        private readonly IInjector binder;
        
        internal CommandBinder(IInjector binder, BaseSignal signal)
        {
            this.binder = binder;
            this.signal = signal;
            signal.AddListener(ExecuteCommands);
        }

        public ICommandBinder To<T>() where T : Command
        {
            return To(typeof(T));
        }

        private ICommandBinder To(Type commandType)
        {
            commands.Add(commandType);
            return this;
        }

        private void ExecuteCommands(object[] parameters)
        {
            var sequencer = new Sequencer(binder, commands, parameters.ToList());
            sequencer.Execute();
        }
    }
}