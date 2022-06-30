namespace CowLibrary;

using System.Numerics;

public readonly struct Basis
{
    public readonly Vector3 ss;
    public readonly Vector3 ts;
    public readonly Vector3 ns;

    public Basis(Vector3 ss, Vector3 ns, Vector3 ts)
    {
        this.ss = ss;
        this.ns = ns;
        this.ts = ts;
    }

    public Vector3 WorldToLocal(in Vector3 w)
    {
        return new Vector3(Vector3.Dot(w, ss), Vector3.Dot(w, ns), Vector3.Dot(w, ts));
    }

    public Vector3 LocalToWorld(in Vector3 w)
    {
        return new Vector3(
            ss.X * w.X + ns.X * w.Y + ts.X * w.Z,
            ss.Y * w.X + ns.Y * w.Y + ts.Y * w.Z,
            ss.Z * w.X + ns.Z * w.Y + ts.Z * w.Z);
    }
}
