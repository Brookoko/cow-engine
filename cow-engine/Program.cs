using System;

namespace cow_engine
{
    using System.Numerics;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var v = new Vector4(1, 1, 1, 1);
            var m = Matrix4x4.CreateTranslation(1, 1, 1);
        }
    }
}
