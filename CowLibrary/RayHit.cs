namespace CowLibrary
{
    using System.Numerics;

    public readonly struct RayHit
    {
        public bool HasHit => t < float.MaxValue;

        public readonly Vector3 point;
        public readonly Vector3 normal;
        public readonly float t;
        public readonly int id;

        public RayHit()
        {
            t = float.MaxValue;
            point = Vector3.Zero;
            normal = Vector3.Zero;
            id = -1;
        }

        public RayHit(float t, Vector3 point, Vector3 normal, int id)
        {
            this.t = t;
            this.point = point;
            this.normal = normal;
            this.id = id;
        }
    }
}
