namespace CowRenderer
{
    public class RenderConfig
    {
        public int width = 1920;
        public int height = 1080;
        public int fov = 60;

        public int numberOfThreadPerDimension = 8;
        public int numberOfRayPerPixelDimension = 4;
        public int rayDepth = 2;
        public int numberOfRayPerMaterial = 2;
    }
}
