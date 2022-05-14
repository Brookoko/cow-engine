namespace CowLibrary.Models;

using System.Numerics;
using Mathematics.Sampler;

public readonly struct OrthographicCameraModel : ICameraModel
{
    private readonly int width;
    private readonly int height;
    private readonly float aspectRatio;

    public OrthographicCameraModel(int width, int height)
    {
        this.width = width;
        this.height = height;
        aspectRatio = (float)width / height;
    }

    public Ray ScreenPointToRay(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, ISampler sampler)
    {
        var x = (2 * (screenPoint.X + 0.5f) / width - 1) * aspectRatio;
        var y = 1 - 2 * (screenPoint.Y + 0.5f) / height;
        var origin = new Vector3(x, y, 0);
        return new Ray(origin, Vector3.UnitZ);
    }

    public Ray[] Sample(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, ISampler sampler, int samples)
    {
        var rays = new Ray[samples];
        for (var i = 0; i < samples; i++)
        {
            rays[i] = ScreenPointToRay(in screenPoint, in localToWorldMatrix, sampler);
        }
        return rays;
    }

    public Ray ScreenPointToRay(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, in LocalSampler sampler)
    {
        var x = (2 * (screenPoint.X + 0.5f) / width - 1) * aspectRatio;
        var y = 1 - 2 * (screenPoint.Y + 0.5f) / height;
        var origin = new Vector3(x, y, 0);
        return new Ray(origin, Vector3.UnitZ);
    }

    public Ray[] Sample(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, in  LocalSampler sampler, int samples)
    {
        var rays = new Ray[samples];
        for (var i = 0; i < samples; i++)
        {
            rays[i] = ScreenPointToRay(in screenPoint, in localToWorldMatrix, sampler);
        }
        return rays;
    }
}
