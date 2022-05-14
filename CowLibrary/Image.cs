namespace CowLibrary
{
    public readonly struct Image
    {
        public int Height => data.GetLength(0);
        public int Width => data.GetLength(1);

        private readonly Color[,] data;

        public Image(int w, int h)
        {
            data = new Color[h, w];
        }

        public Color this[int i, int j]
        {
            get => data[i, j];
            set => data[i, j] = value;
        }
    }
}
