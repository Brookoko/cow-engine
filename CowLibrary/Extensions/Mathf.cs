namespace CowLibrary
{
    using System;
    using System.Numerics;
    using System.Threading;

    public static class Mathf
    {
        private static int seed = Environment.TickCount;
        
        private static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));
        
        public static Vector3 OnUnitSphere()
        {
            var theta = random.Value.NextDouble() * 180;
            var phi = random.Value.NextDouble() * 360;
            var sin = Math.Sin(phi);
            return new Vector3((float) (sin * Math.Cos(theta)), (float) (sin * Math.Sin(theta)), (float) Math.Cos(phi));
        }
        
        public static Vector3 OnUnitHalfSphere(Vector3 up)
        {
            var theta = random.Value.NextDouble() * 90;
            var phi = random.Value.NextDouble() * 360;
            
            var m = Matrix4x4.Identity;
            var cosPhi = (float) Math.Cos(phi);
            var sinPhi = (float) Math.Sin(phi);
            var cosTheta = (float) Math.Cos(theta);
            var sinTheta = (float) Math.Sin(theta);
            m.M11 = cosPhi * cosTheta;
            m.M12 = -cosPhi * sinTheta;
            m.M13 = sinPhi;
            m.M21 = sinTheta;
            m.M22 = cosTheta;
            m.M23 = 0;
            m.M31 = -sinPhi * cosTheta;
            m.M32 = sinPhi * sinTheta;
            m.M33 = cosPhi;
            
            return m.MultiplyVector(up);
        }
    }
}