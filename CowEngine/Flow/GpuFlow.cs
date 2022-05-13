namespace CowEngine
{
    using Cowject;

    public class GpuFlow : IFlow<GpuOption>
    {
        [Inject]
        public IWatch Watch { get; set; }
        
        [Inject]
        public ISceneLoader SceneLoader { get; set; }
        
        public int Process(GpuOption option)
        {
            Watch.Start();
            var scene = SceneLoader.LoadSceneFromOptions(option);
            Watch.Stop("Loading scene");
            return 0;
        }
    }
}
