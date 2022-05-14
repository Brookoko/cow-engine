namespace CowRenderer.Integration
{
    using System.Numerics;
    using CowLibrary;

    public class NormalsIntegrator : IIntegrator
    {
        private readonly Color negativeColor = Color.White;

        public Color GetColor(Scene scene, in Surfel surfel)
        {
            if (surfel.material == null)
            {
                return negativeColor;
            }

            var normal = surfel.normal;
            normal = (normal + Vector3.One) * 0.5f;
            return new Color(normal.X, normal.Y, normal.Z);
        }
    }
}
