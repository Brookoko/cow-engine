namespace CowEngine
{
    using Cowject;
    using CowLibrary;
    using CowLibrary.Lights;
    using CowLibrary.Mathematics.Sampler;
    using CowRenderer;
    using SceneWorker;

    public interface ISceneLoader
    {
        Scene LoadSceneFromOptions(Option option);
    }

    public class SceneLoader : ISceneLoader
    {
        [Inject]
        public ISceneWorker SceneWorker { get; set; }

        [Inject]
        public IRenderableObjectWorker RenderableObjectWorker { get; set; }

        [Inject]
        public ISamplerProvider SamplerProvider { get; set; }
        
        [Inject]
        public DiContainer DiContainer { get; set; }
        
        public Scene LoadSceneFromOptions(Option option)
        {
            if (HasSceneParameter(option))
            {
                return ExtractFromSceneFile(option.Scene);
            }
            return ExtractFromModelFile(option.Model);
        }

        private bool HasSceneParameter(Option option)
        {
            return !string.IsNullOrEmpty(option.Scene);
        }

        private Scene ExtractFromSceneFile(string source)
        {
            return SceneWorker.Parse(source);
        }

        private Scene ExtractFromModelFile(string source)
        {
            var model = RenderableObjectWorker.Parse(source);
            return PrepareScene(model);
        }

        private Scene PrepareScene(RenderableObject model)
        {
            var scene = new AutoAdjustScene(SamplerProvider.Sampler);
            DiContainer.Inject(scene);

            var light = new EnvironmentLight(new Color(255, 255, 255), 3, SamplerProvider.Sampler);
            scene.lights.Add(light);

            scene.objects.Add(model);
            scene.PrepareScene();
            return scene;
        }
    }
}
