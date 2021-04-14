namespace CowRenderer.Rendering.Impl
{
    using System.Numerics;
    using CowLibrary;
    using Integration.Impl;
    using Raycasting.Impl;

    public class SimpleRenderer : IRenderer
    {
        private readonly IRaycaster raycaster;

        private readonly IIntegrator integrator;

        public SimpleRenderer()
        {
            raycaster = new SimpleRaycaster();
            integrator = new NormalsIntegrator();
        }

        public Image Render(Scene scene)
        {
            var camera = scene.camera;
            var pixelsRaycastSurfels = GetPixelsRaycastSurfels(scene, camera);
            return IntegratePixelsSurfels(scene, pixelsRaycastSurfels);
        }

        private Surfel[,] GetPixelsRaycastSurfels(Scene scene, Camera targetCamera)
        {
            var cameraSize = (targetCamera.xResolution, targetCamera.yReslution);
            var resultSurfels = new Surfel[cameraSize.xResolution, cameraSize.yReslution];
            for (var x = 0; x < cameraSize.xResolution; x++)
            {
                for (var y = 0; y < cameraSize.yReslution; y++)
                {
                    var cameraRay = targetCamera.ScreenPointToRay(new Vector2(x, y));
                    var isSurfelHit = raycaster.Raycast(scene, cameraRay, out var surfel);
                    resultSurfels[x, y] = surfel;
                }
            }
            return resultSurfels;
        }

        private Image IntegratePixelsSurfels(Scene sourceScene, Surfel[,] surfels)
        {
            var outputResolution = (surfels.GetLength(0), surfels.GetLength(1));
            var outputImage = new Image(outputResolution.Item1, outputResolution.Item2);
            
            for (var x = 0; x < outputResolution.Item1; x++)
            {
                for (var y = 0; y < outputResolution.Item2; y++)
                {
                    outputImage[x, y] = integrator.GetColor(sourceScene, surfels[x, y]);
                }
            }

            return outputImage;
        }
    }
}