namespace CowEngine
{
    using System;

    public class Program
    {
        private static readonly AppContext Context = new();
        private static readonly TypeProvider TypeProvider = new();

        public static void Main(string[] args)
        {
            Initialize();
            Process(args);
        }

        private static void Initialize()
        {
            var watch = new Watch();
            watch.Start();
            TypeProvider.LoadTypes();
            Context.Launch(TypeProvider);
            watch.Stop("Launch");
        }

        private static void Process(string[] args)
        {
            var argumentParser = Context.Get<IArgumentsParser>();
            try
            {
                argumentParser.Parse(args);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to render. See next exception for more details");
                throw;
            }
        }
    }
}
