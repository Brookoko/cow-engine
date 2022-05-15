namespace CowLibrary.Lights
{
    using System;
    using System.Numerics;

    public class PointLight : Light
    {
        private readonly Color color;
        private readonly float intensity;

        public PointLight(Color color, float intensity)
        {
            this.color = color;
            this.intensity = intensity;
        }

        public override ShadingInfo GetShadingInfo(in RayHit rayHit)
        {
            var direction = Transform.Position - rayHit.point;
            var sqrtDistance = direction.LengthSquared();
            var distance = (float)Math.Sqrt(sqrtDistance);
            return new ShadingInfo
            {
                direction = direction / distance,
                distance = distance,
                color = color * (intensity / (4 * Math.PI * sqrtDistance))
            };
        }

        public override Color Sample(in Vector3 wi)
        {
            return Color.Black;
        }
    }
}
