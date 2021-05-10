namespace CowRenderer
{
    public class RenderConfig
    {
        public int width = 1920;
        public int height = 1080;
        public int fov = 60;
        
        public float bias = 0.01f;
        
        public int numberOfThreadPerDimension = 8;
        public int numberOfRayPerPixelDimension = 10;
        public int rayDepth = 3;
        public int numberOfIndirectRay = 1;
    }
}