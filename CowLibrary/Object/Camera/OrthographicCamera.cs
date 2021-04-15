namespace CowLibrary
{
    using System.Numerics;

    public class OrthographicCamera : Camera
    {
        public override Ray ScreenPointToRay(Vector2 screenPoint)
        {
            var x = (2 * (screenPoint.X + 0.5f) / width - 1) * AspectRatio;
            var y = 1 - 2 * (screenPoint.Y + 0.5f) / height;
            var origin = new Vector3(x, y, 0);
            return new Ray(origin, Vector3.UnitZ);
        }
    }
}