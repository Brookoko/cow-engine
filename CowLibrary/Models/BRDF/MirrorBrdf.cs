namespace CowLibrary;

using System.Numerics;

public struct MirrorBrdf : IBrdf
{
    private readonly float r;
    private readonly FresnelNoOp fresnel;

    public MirrorBrdf(float r, FresnelNoOp fresnel)
    {
        this.r = r;
        this.fresnel = fresnel;
    }

    public float Evaluate(in Vector3 wo, in Vector3 wi)
    {
        return 0;
    }

    public float Sample(in Vector3 wo, in Vector2 sample, out Vector3 wi, out float pdf)
    {
        pdf = 1;
        wi = new Vector3(-wo.X, wo.Y, -wo.Z);
        return fresnel.Evaluate(Mathf.CosTheta(wi)) * r / Mathf.AbsCosTheta(wi);
    }

    public float Pdf(in Vector3 wo, in Vector3 wi)
    {
        return 0;
    }
}
