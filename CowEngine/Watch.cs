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
        
        public void Stop(string label)
        {
            watch.Stop();
            Console.WriteLine($"{label}");
            var elapsed = watch.Elapsed;
            Console.WriteLine($"Seconds: {elapsed.TotalSeconds}");
            Console.WriteLine($"Milliseconds: {elapsed.TotalMilliseconds}");
        }
    }
}