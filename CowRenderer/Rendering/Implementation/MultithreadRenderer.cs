namespace CowRenderer.Rendering.Impl
{
    using System;
    using System.Numerics;
    using System.Threading;
    using Cowject;
    using CowLibrary;

    public class MultithreadRenderer : IRenderer
    {
        [Inject]
        public  IRaycaster Raycaster { get; set; }
        
        [Inject]
        private IIntegrator Integrator { get; set; }
        
        private const int NumberOfThread = 8;
        
        public Image Render(Scene scene)
        {
            var w = scene.camera.width;
            var h = scene.camera.height;
            var image = new Image(w, h);
            var xStep = w / NumberOfThread;
            var yStep = h / NumberOfThread;
            var threads = new Thread[NumberOfThread * NumberOfThread];
            for (var i = 0; i < NumberOfThread; i++)
            {
                for (var j = 0; j < NumberOfThread; j++)
                {
                    var from = new Vector2(i * xStep, j * yStep);
                    var to = new Vector2((i + 1) * xStep, (j + 1) * yStep);
                    var renderer = new ThreadRenderer(Raycaster, Integrator, scene, image, from, to);
                    var thread = new Thread(renderer.Render);
                    thread.Start();
                    threads[i * NumberOfThread + j] = thread;
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