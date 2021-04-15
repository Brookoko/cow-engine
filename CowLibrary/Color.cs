namespace CowLibrary
{
    using System;
    using System.Numerics;

    public readonly struct Color
    {
        public readonly byte r;
        public readonly byte g;
        public readonly byte b;
        public readonly byte a;

        public Color(byte v) : this(v, v, v)
        {
        }

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public byte[] ToBytes()
        {
            return new[] {r, g, b};
        }

        public override string ToString()
        {
            return $"Color R:{r} G:{g} B:{g} A:{a}";
        }

        public static Color LerpUnclamped(Color color1, Color color2, float a)
        {
            return new Color(
                (byte) Math.Ceiling(color1.r + (color2.r - color1.r) * a),
                (byte) Math.Ceiling(color1.g + (color2.g - color1.g) * a),
                (byte) Math.Ceiling(color1.b + (color2.b - color1.b) * a),
                (byte) Math.Ceiling(color1.a + (color2.a - color1.a) * a)
            );
        }
        
        public static Color LerpUnclamped(Color color1, Color color2, Vector3 a)
        {
            return new Color(
                (byte) Math.Ceiling(color1.r + (color2.r - color1.r) * a.X),
                (byte) Math.Ceiling(color1.g + (color2.g - color1.g) * a.Y),
                (byte) Math.Ceiling(color1.b + (color2.b - color1.b) * a.Z),
                (byte) Math.Ceiling(color1.a + (color2.a - color1.a) * 1f)
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

        public static Color operator *(Color color, float value)
        {
            return new Color(
            (byte) (color.r * value),
            (byte) (color.g * value),
            (byte) (color.b * value),
            (byte) (color.a * value)
                );
        }
    }
}