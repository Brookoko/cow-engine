namespace CowRenderer.Rendering
{
    using System.Numerics;
    using CowLibrary;

    public class SimpleThreadRenderer : ThreadRenderer
    {
        protected override Surfel[,] GetPixelsRaycastSurfels(Camera camera)
        {
            var w = (int) (to.X - from.X);
            var h = (int) (to.Y - from.Y);
            var surfels = new Surfel[h, w];
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    var cameraRay = camera.ScreenPointToRay(new Vector2(x + from.X, y + from.Y));
                    Raycaster.Raycast(cameraRay, out var surfel);
                    surfels[y, x] = surfel;
                }
            }
            return surfels;
        }
    }
}