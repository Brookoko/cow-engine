namespace CowLibrary
{
    using System;
    using System.Numerics;

    public static class ColorExtensions
    {
        public static Color LerpUnclamped(Color color1, Color color2, float a)
        {
            return new Color(
                (float)Math.Ceiling(color1.r + (color2.r - color1.r) * a),
                (float)Math.Ceiling(color1.g + (color2.g - color1.g) * a),
                (float)Math.Ceiling(color1.b + (color2.b - color1.b) * a),
                (float)Math.Ceiling(color1.a + (color2.a - color1.a) * a)
            );
        }

        public static Color LerpUnclamped(Color color1, Color color2, Vector3 a)
        {
            return new Color(
                (float)Math.Ceiling(color1.r + (color2.r - color1.r) * a.X),
                (float)Math.Ceiling(color1.g + (color2.g - color1.g) * a.Y),
                (float)Math.Ceiling(color1.b + (color2.b - color1.b) * a.Z),
                (float)Math.Ceiling(color1.a + (color2.a - color1.a) * 1f)
            );
        }

        public static Color MaxComponents(Color color1, Color color2)
        {
            return new Color
            (
                Math.Max(color1.r, color2.r),
                Math.Max(color1.g, color2.g),
                Math.Max(color1.b, color2.b)
            );
        }

        public static Color MinComponents(Color color1, Color color2)
        {
            return new Color
            (
                Math.Min(color1.r, color2.r),
                Math.Min(color1.g, color2.g),
                Math.Min(color1.b, color2.b)
            );
        }
    }
}
