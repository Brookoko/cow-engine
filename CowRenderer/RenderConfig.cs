namespace CowRenderer
{
    public class RenderConfig
    {
        public int width = 1920;
        public int height = 1080;
        public int fov = 60;
        
        public float bias = 0.01f;
        
        public int numberOfThread = 8;
        public int numberOfRayPerPixel = 2;
        public int rayDepth = 2;
        public int numberOfIndirectRay = 8;
    }
}