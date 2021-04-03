namespace Cowject.Commands
{
    using System;
    using System.Collections.Generic;

    internal class Sequencer
    {
        private readonly IInjector binder;
        private readonly List<Type> commands;
        private readonly List<object> parameters;
        
        private int currentCommandIndex;
        
        public Sequencer(IInjector binder, List<Type> commands, List<object> parameters)
        {
            this.binder = binder;
            this.commands = commands;
            parameters.Insert(0, this);
            this.parameters = parameters;
        }

        public void Execute()
        {
            if (commands.Count > 0)
            {
                InvokeCommand(commands[0]);
            }
        }
        
        private void InvokeCommand(Type type)
        {
            var command = CreateCommand(type);
            command.Execute();
            ReleaseCommand(command);
        }

        private Command CreateCommand(Type type)
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);
            var command = constructor?.Invoke(new object[] { });
            return (Command) binder.Inject(command, parameters);
        }
        
        public void ReleaseCommand(Command command)
        {
            if (!command.retain)
            {
                currentCommandIndex++;
                command.Sequencer = null;
                if (currentCommandIndex < commands.Count)
                {
                    InvokeCommand(commands[currentCommandIndex]);
                }
            }
        }
    }
}