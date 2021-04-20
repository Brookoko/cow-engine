namespace CowRenderer.Rendering.Impl
{
    using System.Numerics;
    using CowLibrary;

    public class ThreadRenderer
    {
        private readonly IRaycaster raycaster;
        private readonly IIntegrator integrator;
        private readonly Scene scene;
        private readonly Image image;
        private readonly Vector2 from;
        private readonly Vector2 to;
        
        public ThreadRenderer(IRaycaster raycaster, IIntegrator integrator, Scene scene, Image image, Vector2 from, Vector2 to)
        {
            this.raycaster = raycaster;
            this.integrator = integrator;
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
        
        private Surfel[,] GetPixelsRaycastSurfels(Camera camera)
        {
            var w = (int) (to.X - from.X);
            var h = (int) (to.Y - from.Y);
            var surfels = new Surfel[h, w];
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    var cameraRay = camera.ScreenPointToRay(new Vector2(x + from.X, y + from.Y));
                    var isSurfelHit = raycaster.Raycast(scene, cameraRay, out var surfel);
                    surfels[y, x] = surfel;
                }
            }
            return surfels;
        }

        private void IntegratePixelsSurfels(Surfel[,] surfels)
        {
            var w = surfels.GetLength(1);
            var h = surfels.GetLength(0);
            
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    image[(int) (y + from.Y), (int) (x + from.X)] = integrator.GetColor(scene, surfels[y, x]);
                }
            }
        }
    }
}