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
        
        public Color GetColor(Scene scene, Surfel surfel)
        {
            if (surfel.material == null)
            {
                return backgroundColor;
            }
            return TraceRecursive(scene, surfel, 0);
        }
        
        private Color TraceRecursive(Scene scene, Surfel surfel, int depth)
        {
            if (depth >= RenderConfig.rayDepth)
            {
                return new Color(0f);
            }
            var dir = surfel.ray.Reflect(surfel.normal);
            var p = surfel.point + surfel.normal * RenderConfig.bias;
            var ray = new Ray(p, dir);
            if (Raycaster.Raycast(ray, out surfel))
            {
                var dot = Vector3.Dot(surfel.normal, dir);
                return dot * (directIntegrator.GetColor(scene, surfel) + TraceRecursive(scene, surfel, depth + 1));
            }
            return new Color(0f);
        }
    }
}