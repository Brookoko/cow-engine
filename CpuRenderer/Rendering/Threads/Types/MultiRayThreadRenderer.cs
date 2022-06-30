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
                    image[y, x] = Integrate(in surfels);
                }
            }
        }

        private Surfel[] Raycast(Camera camera, in Vector2 point)
        {
            var numberOfRay = RenderConfig.numberOfRayPerPixelDimension;
            var surfels = new Surfel[numberOfRay];
            var rays = camera.Sample(in point, numberOfRay);
            for (var i = 0; i < numberOfRay; i++)
            {
                surfels[i] = Raycaster.Raycast(in rays[i]);
            }

            return surfels;
        }

        private Color Integrate(in Surfel[] surfels)
        {
            var color = Color.Black;
            foreach (var surfel in surfels)
            {
                color += Integrator.GetColor(scene, in surfel);
            }
            return color / surfels.Length;
        }
    }
}
