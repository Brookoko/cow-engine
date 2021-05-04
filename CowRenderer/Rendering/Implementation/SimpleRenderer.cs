namespace CowRenderer.Rendering
{
    using System.Numerics;
    using Cowject;
    using CowLibrary;

    public class SimpleRenderer : IRenderer
    {
        [Inject]
        public  IRaycaster Raycaster { get; set; }

        [Inject]
        private IIntegrator Integrator { get; set; }

        public Image Render(Scene scene)
        {
            Raycaster.Init(scene);
            var pixelsRaycastSurfels = GetPixelsRaycastSurfels(scene.MainCamera);
            return IntegratePixelsSurfels(scene, pixelsRaycastSurfels);
        }

        private Surfel[,] GetPixelsRaycastSurfels(Camera targetCamera)
        {
            var w = targetCamera.width;
            var h = targetCamera.height;
            var resultSurfels = new Surfel[h, w];
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    var cameraRay = targetCamera.ScreenPointToRay(new Vector2(x, y));
                    var isSurfelHit = Raycaster.Raycast(cameraRay, out var surfel);
                    resultSurfels[y, x] = surfel;
                }
            }
            return resultSurfels;
        }

        private Image IntegratePixelsSurfels(Scene scene, Surfel[,] surfels)
        {
            var w = surfels.GetLength(1);
            var h = surfels.GetLength(0);
            var outputImage = new Image(w, h);
            
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    outputImage[y, x] = Integrator.GetColor(scene, surfels[y, x]);
                }
            }

            return outputImage;
        }
    }
}