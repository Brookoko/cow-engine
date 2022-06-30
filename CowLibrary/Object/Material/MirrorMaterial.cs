namespace CowLibrary;

using System.Numerics;

public readonly struct MirrorMaterial : IMaterial
{
    public Color Color { get; }

    public int Id { get; }

    private readonly MirrorBrdf brdf;

    public MirrorMaterial(Color color, float r, int id) :
        this(color, new MirrorBrdf(r, new FresnelNoOp()), id)
    {
    }

    private MirrorMaterial(Color color, MirrorBrdf brdf, int id)
    {
        Color = color;
        this.brdf = brdf;
        Id = id;
    }

    public Color GetColor(in Vector3 wo, in Vector3 wi)
    {
        return brdf.Evaluate(in wo, in wi) * Color;
    }

    public Color Sample(in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        return brdf.Sample(in wo, in sample, out wi, out pdf) * Color;
    }

    public IMaterial Copy(int id)
    {
        return new MirrorMaterial(Color, brdf, id);
    }
}
