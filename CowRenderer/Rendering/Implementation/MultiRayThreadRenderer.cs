namespace CowRenderer.Rendering
{
    using System.Linq;
    using System.Numerics;
    using CowLibrary;

    public class MultiRayThreadRenderer : ThreadRenderer
    {
        public override void Render()
        {
            var w = (int)(to.X - from.X);
            var h = (int)(to.Y - from.Y);
            var fromY = (int)from.Y;
            var fromX = (int)from.X;
            var camera = scene.MainCamera;

            for (var i = 0; i < h; i++)
            {
                for (var j = 0; j < w; j++)
                {
                    var y = i + fromY;
                    var x = j + fromX;
                    var surfels = Raycast(camera, new Vector2(x, y));
                    image[y, x] = Integrate(surfels);
                }
            }
        }

        private Surfel[] Raycast(Camera camera, Vector2 point)
        {
            var numberOfRay = RenderConfig.numberOfRayPerPixel;
            var surfels = new Surfel[numberOfRay];
            var rays = camera.Sample(point, numberOfRay);
            for (var i = 0; i < numberOfRay; i++)
            {
                Raycaster.Raycast(in rays[i], out var surfel);
                surfels[i] = surfel;
            }

            return surfels;
        }

        private Color Integrate(Surfel[] surfels)
        {
            var color = surfels
                .Select(s => Integrator.GetColor(scene, s))
                .Aggregate(Color.Black, (acc, c) => acc + c);
            return color / surfels.Length;
        }
    }
}
