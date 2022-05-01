namespace CowRenderer
{
    using System.Linq;
    using CowLibrary;

    public class ComplexScene : Scene
    {
        public override Camera MainCamera => camera;

        private Camera camera;

        public void SetMainCamera(int cameraId)
        {
            camera = cameras.First(c => c.Id == cameraId);
        }
    }
}
