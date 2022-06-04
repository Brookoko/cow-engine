namespace CowLibrary
{
    using System.Numerics;

    public readonly struct RayHit
    {
        public bool HasHit => t < float.MaxValue;

        public readonly Vector3 point;
        public readonly Vector3 normal;
        public readonly Vector3 dpdu;
        public readonly Vector3 dpdv;
        public readonly float t;
        public readonly int id;

        public RayHit()
        {
            t = float.MaxValue;
            point = Vector3.Zero;
            normal = Vector3.Zero;
            dpdu = Vector3.Zero;
            dpdv = Vector3.Zero;
            id = -1;
        }

        public RayHit(float t, Vector3 point, Vector3 normal, Vector3 dpdu, Vector3 dpdv, int id)
        {
            this.t = t;
            this.point = point;
            this.normal = normal;
            this.dpdu = dpdu;
            this.dpdv = dpdv;
            this.id = id;
        }
        
        public Basis ExtractBasis()
        {
            var ns = normal;
            var ss = dpdu.Normalize();
            var ts = Vector3.Cross(ns, ss);
            return new Basis(ss, ns, ts);
        }
    }
}
