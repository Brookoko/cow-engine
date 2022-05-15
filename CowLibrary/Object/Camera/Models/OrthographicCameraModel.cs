namespace CowLibrary.Models;

using System.Numerics;

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

    public Ray ScreenPointToRay(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, in Vector2 sample)
    {
        var point = screenPoint + sample - 0.5f * Vector2.One;
        var x = (2 * (point.X + 0.5f) / width - 1) * aspectRatio;
        var y = 1 - 2 * (point.Y + 0.5f) / height;
        var origin = new Vector3(x, y, 0);
        return new Ray(origin, Vector3.UnitZ);
    }

    public Ray[] Sample(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, in Vector2[] samples)
    {
        var rays = new Ray[samples.Length];
        for (var i = 0; i < samples.Length; i++)
        {
            rays[i] = ScreenPointToRay(in screenPoint, in localToWorldMatrix, in samples[i]);
        }
        return rays;
    }
}
