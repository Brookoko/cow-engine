namespace CowRenderer.Integration
{
    using System.Numerics;
    using Cowject;
    using CowLibrary;

    public class ReflectionIndirectIntegrator : IIntegrator
    {
        [Inject]
        public DiContainer DiContainer { get; set; }

        [Inject]
        public IRaycaster Raycaster { get; set; }

        [Inject]
        public RenderConfig RenderConfig { get; set; }

        private readonly IIntegrator directIntegrator = new ShadowRayIntegrator();
        private readonly Color backgroundColor = new Color(245, 245, 245);

        [PostConstruct]
        public void Prepare()
        {
            DiContainer.Inject(directIntegrator);
        }

        public Color GetColor(Scene scene, in Surfel surfel)
        {
            if (surfel.material == null)
            {
                return backgroundColor;
            }
            return TraceRecursive(scene, in surfel, 0);
        }

        private Color TraceRecursive(Scene scene, in Surfel surfel, int depth)
        {
            if (depth >= RenderConfig.rayDepth)
            {
                return new Color(0f);
            }
            var dir = surfel.ray.Reflect(surfel.hit.normal);
            var p = surfel.hit.point + surfel.hit.normal * RenderConfig.bias;
            var ray = new Ray(p, dir);
            if (Raycaster.Raycast(ray, out var surfelHit))
            {
                var dot = Vector3.Dot(surfelHit.hit.normal, dir);
                return dot * (directIntegrator.GetColor(scene, in surfelHit) + TraceRecursive(scene, in surfelHit, depth + 1));
            }
            return new Color(0f);
        }
    }
}
