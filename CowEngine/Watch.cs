namespace CowEngine
{
    using System;
    using System.Diagnostics;

    public interface IWatch
    {
        void Start();

        double Stop(string info);
    }

    public class Watch : IWatch
    {
        private readonly Stopwatch watch = new Stopwatch();

        public void Start()
        {
            watch.Restart();
        }

        public double Stop(string info)
        {
            watch.Stop();
            var elapsed = watch.Elapsed;
            Console.WriteLine($"{info}: {elapsed.TotalSeconds}s");
            return elapsed.TotalSeconds;
        }
    }
}
