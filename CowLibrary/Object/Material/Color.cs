namespace CowLibrary
{
    public readonly struct Color
    {
        public readonly byte r;
        public readonly byte g;
        public readonly byte b;
        public readonly byte a;
        
        public Color(byte r, byte g, byte b, byte a = 255)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
    }
}