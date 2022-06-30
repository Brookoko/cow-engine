namespace CowLibrary.Models.Microfacet;

using System.Numerics;

public interface IMicrofacetDistribution
{
    float D(in Vector3 w);

    float Lambda(in Vector3 w);

    Vector3 Sample(in Vector3 wo, in Vector2 sample);

    float G1(in Vector3 w)
    {
        return 1 / (1 + Lambda(in w));
    }

    float G(in Vector3 wo, in Vector3 wi)
    {
        return 1 / (1 + Lambda(in wo) + Lambda(in wi));
    }

    float Pdf(in Vector3 wo, in Vector3 wi)
    {
        if (Const.SampleVisibleArea)
        {
            return D(wi) * G1(wo) * Mathf.AbsDot(wo, wi) / Mathf.AbsCosTheta(wo);
        }
        return D(wi) * Mathf.AbsCosTheta(wi);
    }
}
