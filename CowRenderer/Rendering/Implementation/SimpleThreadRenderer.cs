namespace CowRenderer.Rendering
{
    using System.Numerics;
    using CowLibrary;

    public class SimpleThreadRenderer : ThreadRenderer
    {
        public override void Render()
        {
            var surfels = GetPixelsRaycastSurfels(scene.MainCamera);
            IntegratePixelsSurfels(surfels);
        }

        protected Surfel[,] GetPixelsRaycastSurfels(Camera camera)
        {
            var w = (int)(to.X - from.X);
            var h = (int)(to.Y - from.Y);
            var surfels = new Surfel[h, w];
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    var cameraRay = camera.ScreenPointToRay(new Vector2(x + from.X + 0.5f, y + from.Y + 0.5f));
                    Raycaster.Raycast(cameraRay, out var surfel);
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
                    image[(int)(y + from.Y), (int)(x + from.X)] = Integrator.GetColor(scene, surfels[y, x]);
                }
            }
        }
    }
}
