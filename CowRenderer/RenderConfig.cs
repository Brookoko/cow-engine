namespace CowRenderer
{
    public class RenderConfig
    {
        public int width = 1920;
        public int height = 1080;
        public int fov = 60;

        public float bias = 0.00001f;

        public int numberOfThreadPerDimension = 8;
        public int numberOfRayPerPixel = 1;
        public int rayDepth = 2;
        public int numberOfRayPerLight = 2;
    }
}
