namespace CowEngine
{
    using System;
    using System.Diagnostics;

    public class Watch
    {
        private readonly Stopwatch watch = new Stopwatch();
        
        public void Start()
        {
            watch.Restart();
        }
        
        public void Stop(string info)
        {
            watch.Stop();
            var elapsed = watch.Elapsed;
            Console.WriteLine($"{info}: {elapsed.TotalSeconds}");
        }
    }
}