namespace ILGPURenderer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Converters;
    using Cowject;
    using CowLibrary;
    using CowRenderer;
    using ILGPU.Runtime;

    public class ILGPURenderer : IRenderer
    {
        [Inject]
        public IPrimaryRayKernel PrimaryRayKernel { get; set; }

        [Inject]
        public IHitKernel HitKernel { get; set; }

        [Inject]
        public IColorKernel ColorKernel { get; set; }

        [Inject]
        public ISceneConverter SceneConverter { get; set; }

        public Image Render(Scene scene)
        {
            var camera = scene.MainCamera;
            var w = camera.Width;
            var h = camera.Height;
            var image = new Image(w, h);
            var sceneData = SceneConverter.Convert(scene);
            
            var rays = PrimaryRayKernel.GeneratePrimaryRays(camera);
            var hits = HitKernel.GenerateHits(sceneData, rays);
            var colorsBuffer = ColorKernel.GenerateColors(hits);
            var colors = colorsBuffer.GetAsArray3D();
            for (var i = 0; i < w; i++)
            {
                for (var j = 0; j < h; j++)
                {
                    image[j, i] = colors[i, j, 0];
                }
            }
            return image;
        }
    }
}
