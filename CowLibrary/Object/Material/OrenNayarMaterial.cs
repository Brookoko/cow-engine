namespace CowLibrary;

using System.Numerics;

public readonly struct OrenNayarMaterial : IMaterial
{
    public Color Color { get; }

    public int Id { get; }

    private readonly OrenNayar brdf;

    public OrenNayarMaterial(Color color, float r, float sigma, int id) : this(color, new OrenNayar(r, sigma), id)
    {
    }

    private OrenNayarMaterial(Color color, OrenNayar brdf, int id)
    {
        Color = color;
        this.brdf = brdf;
        Id = id;
    }

    public Color GetColor(in Vector3 wo, in Vector3 wi, in Vector3 normal)
    {
        return brdf.Evaluate(in wo, in wi, in normal) * Color;
    }

    public Color Sample(in Vector3 normal, in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        return brdf.Sample(in normal, in wo, in sample, out wi, out pdf) * Color;
    }

    public IMaterial Copy(int id)
    {
        return new OrenNayarMaterial(Color, brdf, id);
    }
}
