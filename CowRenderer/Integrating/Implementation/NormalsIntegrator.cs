namespace CowRenderer.Integration.Impl
{
    using System.Numerics;
    using CowLibrary;

    public class NormalsIntegrator : IIntegrator
    {
        private readonly Color negativeColor = new Color(255, 255, 255);

        public Color GetColor(Scene scene, Surfel surfel)
        {
            if (surfel == null)
            {
                return negativeColor;
            }

            var normal = surfel.normal;
            normal = (normal + Vector3.One) * 0.5f;
            return new Color(normal.X, normal.Y, normal.Z);
        }
    }
}