namespace CowLibrary
{
    using System;

    public readonly struct Color
    {
        public readonly float r;
        public readonly float g;
        public readonly float b;
        public readonly float a;
        
        public Color(byte v) : this(v, v, v)
        {
        }

        public Color(byte r, byte g, byte b, byte a = 255) : this(r / 255f, g / 255f, b / 255f, a / 255f)
        {
        }

        public Color(float v) : this(v, v, v)
        {
        }
        
        public Color(float r, float g, float b, float a = 1)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
        
        public byte[] ToBytes()
        {
            return new[] {(byte) (Math.Clamp(r, 0, 1) * 255), (byte) (Math.Clamp(g, 0, 1) * 255), (byte) (Math.Clamp(b, 0, 1) * 255)};
        }

        public override string ToString()
        {
            return $"Color R:{r} G:{g} B:{g} A:{a}";
        }
        
        public static Color operator *(Color color, double value)
        {
            return new Color
            (
                (float) (color.r * value),
                (float) (color.g * value),
                (float) (color.b * value),
                (float) (color.a * value)
            );
        }
        
        public static Color operator *(double value, Color color) => color * value;
        
        public static Color operator /(Color color, double value)
        {
            return new Color
            (
                (float) (color.r / value),
                (float) (color.g / value),
                (float) (color.b / value),
                (float) (color.a / value)
            );
        }
        
        public static Color operator *(Color left, Color right)
        {
            return new Color
            (
                left.r * right.r,
                left.g * right.g,
                left.b * right.b,
                left.a * right.a
            );
        }
        
        public static Color operator +(Color color1, Color color2)
        {
            return new Color
            (
                color1.r + color2.r,
                color1.g + color2.g,
                color1.b + color2.b,
                color1.a + color2.a
            );
        }
        
        public bool Equals(Color other)
        {
            return r.Equals(other.r) && g.Equals(other.g) && b.Equals(other.b) && a.Equals(other.a);
        }
        
        public override bool Equals(object obj)
        {
            return obj is Color other && Equals(other);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(r, g, b, a);
        }
        
        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(Color left, Color right)
        {
            return !left.Equals(right);
        }
    }
}