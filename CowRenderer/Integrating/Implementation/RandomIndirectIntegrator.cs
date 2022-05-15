namespace CowRenderer.Integration
{
    using System.Numerics;
    using Cowject;
    using CowLibrary;
    using CowLibrary.Mathematics.Sampler;

    public class RandomIndirectIntegrator : IIntegrator
    {
        [Inject]
        public DiContainer DiContainer { get; set; }

        [Inject]
        public IRaycaster Raycaster { get; set; }

        [Inject]
        public RenderConfig RenderConfig { get; set; }

        [Inject]
        public ISamplerProvider SamplerProvider { get; set; }
        
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
            var p = surfel.hit.point + surfel.hit.normal * RenderConfig.bias;
            var color = new Color(0f);
            for (var i = 0; i < RenderConfig.numberOfRayPerLight; i++)
            {
                var dir = Mathf.CosineSampleHemisphere(surfel.hit.normal, SamplerProvider.GetSampler().CreateSample());
                var ray = new Ray(p, dir);
                if (Raycaster.Raycast(ray, out var hit))
                {
                    var dot = Vector3.Dot(surfel.hit.normal, dir);
                    color += dot * (directIntegrator.GetColor(scene, hit) + TraceRecursive(scene, hit, depth + 1));
                }
            }
            return color / RenderConfig.numberOfRayPerLight;
        }
    }
}
