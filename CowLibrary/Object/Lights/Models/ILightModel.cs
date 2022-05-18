namespace CowLibrary.Lights.Models;

using System.Numerics;

public interface ILightModel
{
    public int Id { get; }

    public ShadingInfo GetShadingInfo(in RayHit rayHit, in Matrix4x4 localToWorldMatrix, in Vector2 sample);

    public Color Sample(in Vector3 wi, in Vector2 sample);
}
