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
        
        public void Stop()
        {
            watch.Stop();
            var elapsed = watch.Elapsed;
            Console.WriteLine($"Seconds: {elapsed.TotalSeconds}");
        }
    }
}