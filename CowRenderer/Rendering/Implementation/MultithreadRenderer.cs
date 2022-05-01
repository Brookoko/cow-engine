namespace CowRenderer.Rendering
{
    using System.Numerics;
    using System.Threading;
    using Cowject;
    using CowLibrary;

    public class MultithreadRenderer : IRenderer
    {
        [Inject]
        public RenderConfig RenderConfig { get; set; }

        [Inject]
        public DiContainer DiContainer { get; set; }

        [Inject]
        public IRaycaster Raycaster { get; set; }

        public Image Render(Scene scene)
        {
            Raycaster.Init(scene);
            var numberOfThread = RenderConfig.numberOfThreadPerDimension;
            var threads = new Thread[numberOfThread * numberOfThread];

            var w = scene.MainCamera.width;
            var h = scene.MainCamera.height;
            var xStep = w / numberOfThread;
            var yStep = h / numberOfThread;
            var image = new Image(w, h);

            for (var i = 0; i < numberOfThread; i++)
            {
                var fromX = i * xStep;
                var toX = (i + 1) * xStep;
                for (var j = 0; j < numberOfThread; j++)
                {
                    var from = new Vector2(fromX, j * yStep);
                    var to = new Vector2(toX, (j + 1) * yStep);

                    var renderer = DiContainer.Get<ThreadRenderer>();
                    renderer.Init(scene, image, from, to);

                    var thread = new Thread(renderer.Render);
                    thread.Start();
                    threads[i * numberOfThread + j] = thread;
                }
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
            return image;
        }
    }
}
