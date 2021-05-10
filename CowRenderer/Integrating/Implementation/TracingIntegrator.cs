namespace CowRenderer.Integration
{
    using Cowject;
    using CowLibrary;

    public class TracingIntegrator : IIntegrator
    {
        [Inject]
        public DiContainer DiContainer { get; set; }
        
        private readonly IIntegrator directIntegrator = new ShadowRayIntegrator();
        private readonly IIntegrator indirectIntegrator = new RandomIndirectIntegrator();
        
        private readonly Color backgroundColor = new Color(245, 245, 245);
        
        [PostConstruct]
        public void Prepare()
        {
            DiContainer.Inject(directIntegrator);
            DiContainer.Inject(indirectIntegrator);
        }
        
        public Color GetColor(Scene scene, Surfel surfel)
        {
            if (surfel == null)
            {
                return backgroundColor;
            }

            return directIntegrator.GetColor(scene, surfel) + indirectIntegrator.GetColor(scene, surfel);
        }
    }
}