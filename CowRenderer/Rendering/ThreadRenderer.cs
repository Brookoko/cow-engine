namespace CowRenderer
{
    using System.Numerics;
    using Cowject;
    using CowLibrary;

    public abstract class ThreadRenderer
    {
        [Inject]
        public IRaycaster Raycaster { get; set; }
        
        [Inject]
        public IIntegrator Integrator { get; set; }
        
        [Inject]
        public RenderConfig RenderConfig { get; set; }
        
        protected Scene scene;
        protected Image image;
        protected Vector2 from;
        protected Vector2 to;
        
        public void Init(Scene scene, Image image, Vector2 from, Vector2 to)
        {
            this.scene = scene;
            this.image = image;
            this.from = from;
            this.to = to;
        }
        
        public void Render()
        {
            var surfels = GetPixelsRaycastSurfels(scene.camera);
            IntegratePixelsSurfels(surfels);
        }
        
        protected abstract Surfel[,] GetPixelsRaycastSurfels(Camera camera);
        
        private void IntegratePixelsSurfels(Surfel[,] surfels)
        {
            var w = surfels.GetLength(1);
            var h = surfels.GetLength(0);
            
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    image[(int) (y + from.Y), (int) (x + from.X)] = Integrator.GetColor(scene, surfels[y, x]);
                }
            }
        }
    }
}