namespace CowRenderer.Rendering
{
    using System.Linq;
    using System.Numerics;
    using Cowject;
    using CowLibrary;

    public class SimpleRenderer : IRenderer
    {
        [Inject]
        public IRaycaster Raycaster { get; set; }

        [Inject]
        private IIntegrator Integrator { get; set; }

        [Inject]
        public RenderConfig RenderConfig { get; set; }

        public string Tag => "cpu-simple";

        public Image Render(Scene scene)
        {
            Raycaster.Init(scene);
            var camera = scene.MainCamera;
            var w = camera.Width;
            var h = camera.Height;
            var image = new Image(w, h);

            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    var surfels = Raycast(camera, new Vector2(x, y));
                    image[y, x] = Integrate(scene, in surfels);
                }
            }

            return image;
        }

        private Surfel[] Raycast(Camera camera, Vector2 point)
        {
            var numberOfRay = RenderConfig.numberOfRayPerPixelDimension;
            var surfels = new Surfel[numberOfRay];
            var rays = camera.Sample(point, numberOfRay);
            for (var i = 0; i < numberOfRay; i++)
            {
                surfels[i] = Raycaster.Raycast(in rays[i]);
            }

            return surfels;
        }

        private Color Integrate(Scene scene, in Surfel[] surfels)
        {
            var color = surfels
                .Select(s => Integrator.GetColor(scene, in s))
                .Aggregate(Color.Black, (acc, c) => acc + c);
            return color / surfels.Length;
        }
    }
}
