namespace CowRenderer.Rendering
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Numerics;
    using CowLibrary;

    public class MultiRayThreadRenderer : ThreadRenderer
    {
        public override void Render()
        {
            var w = (int) (to.X - from.X);
            var h = (int) (to.Y - from.Y);
            var camera = scene.MainCamera;
            for (var i = 0; i < h; i++)
            {
                for (var j = 0; j < w; j++)
                {
                    var y = i + (int) from.Y;
                    var x = j + (int) from.X;
                    var surfels = Raycast(camera, x, y);
                    image[y, x] = Integrate(surfels);
                }
            }
        }
        
        private Surfel[] Raycast(Camera camera, int x, int y)
        {
            var numberOfRay = RenderConfig.numberOfRayPerPixelDimension;
            var surfels = new Surfel[numberOfRay * numberOfRay];
            for (var i = 0; i < numberOfRay; i++)
            {
                var xStep = (i + 1) / (numberOfRay + 1);
                for (var j = 0; j < numberOfRay; j++)
                {
                    var yStep = (j + 1) / (numberOfRay + 1);
                    var cameraRay = camera.ScreenPointToRay(new Vector2(x + xStep, y + yStep));
                    Raycaster.Raycast(cameraRay, out var surfel);
                    surfels[i * numberOfRay + j] = surfel;
                }
            }
            return surfels;
        }
        
        private Color Integrate(Surfel[] surfels)
        {
            var color = surfels
                .Select(s => Integrator.GetColor(scene, s))
                .Aggregate(new Color(0), (acc, c) => acc + c);
            return color / surfels.Length;
        }
    }
}