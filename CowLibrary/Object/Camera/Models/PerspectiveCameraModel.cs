namespace CowLibrary.Models;

using System;
using System.Numerics;

public readonly struct PerspectiveCameraModel : ICameraModel
{
    private readonly int width;
    private readonly int height;
    private readonly float aspectRatio;
    private readonly float tan;
    private readonly float nearPlane;

    public PerspectiveCameraModel(int width, int height, float fov, float nearPlane)
    {
        this.width = width;
        this.height = height;
        aspectRatio = (float)width / height;
        tan = (float)Math.Tan(Const.Deg2Rad * fov / 2);
        this.nearPlane = nearPlane;
    }
    
    public Ray ScreenPointToRay(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix)
    {
        var x = (2 * (screenPoint.X + 0.5f) / width - 1) * tan;
        var y = (1 - 2 * (screenPoint.Y + 0.5f) / height) / aspectRatio * tan;
        var dir = new Vector3(x, y, -nearPlane);
        dir = localToWorldMatrix.MultiplyVector(dir).Normalize();
        return new Ray(localToWorldMatrix.ExtractTranslation(), dir);
    }

    public Ray[] Sample(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, int samples)
    {
        var rays = new Ray[samples];
        for (var i = 0; i < samples; i++)
        {
            var sample = Mathf.CreateSample() - 0.5f * Vector2.One;
            rays[i] = ScreenPointToRay(screenPoint + sample, in localToWorldMatrix);
        }
        return rays;
    }
}
