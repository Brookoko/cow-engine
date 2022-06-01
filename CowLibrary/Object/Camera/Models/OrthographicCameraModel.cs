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
        var x = (2 * screenPoint.X / width - 1) * aspectRatio;
        var y = 1 - 2 * screenPoint.Y / height;
        var origin = new Vector3(x, y, 0);
        var position = localToWorldMatrix.MultiplyPoint(origin);
        var direction = localToWorldMatrix.MultiplyVector(-Vector3.UnitZ);
        return new Ray(position, direction);
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
