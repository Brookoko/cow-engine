namespace CowLibrary.Models;

using System;
using System.Numerics;
using Mathematics.Sampler;

public readonly struct RealisticCameraModel : ICameraModel
{
    private readonly int width;
    private readonly int height;
    private readonly float aspectRatio;
    private readonly float tan;
    private readonly Lens lens;

    public RealisticCameraModel(int width, int height, float fov, Lens lens)
    {
        this.width = width;
        this.height = height;
        aspectRatio = (float)width / height;
        tan = (float)Math.Tan(Const.Deg2Rad * fov / 2);
        this.lens = lens;
    }

    public Ray ScreenPointToRay(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, ISampler sampler)
    {
        return Sample(in screenPoint, in localToWorldMatrix, sampler, 1)[0];
    }

    public Ray[] Sample(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, ISampler sampler, int samples)
    {
        var point = ViewportPoint(screenPoint);
        var lensCenter = new Vector3(0, 0, lens.distance);
        var dir = (point - lensCenter).Normalize();
        var focusPoint = lensCenter + dir * lens.focus;
        var rays = new Ray[samples];
        for (var i = 0; i < samples; i++)
        {
            var sample = Mathf.ConcentricSampleDisk(sampler.CreateSample()).Normalize();
            var lensPoint = lensCenter + new Vector3(sample * lens.radius, 0);
            var direction = focusPoint - lensPoint;
            lensPoint.Z = 0;
            var position = localToWorldMatrix.MultiplyPoint(lensPoint);
            direction = localToWorldMatrix.MultiplyVector(direction).Normalize();
            rays[i] = new Ray(position, direction);
        }
        return rays;
    }
    
    public Ray ScreenPointToRay(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, in LocalSampler sampler)
    {
        return Sample(in screenPoint, in localToWorldMatrix, sampler, 1)[0];
    }

    public Ray[] Sample(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, in LocalSampler sampler, int samples)
    {
        var point = ViewportPoint(screenPoint);
        var lensCenter = new Vector3(0, 0, lens.distance);
        var dir = (point - lensCenter).Normalize();
        var focusPoint = lensCenter + dir * lens.focus;
        var rays = new Ray[samples];
        for (var i = 0; i < samples; i++)
        {
            var sample = Mathf.ConcentricSampleDisk(sampler.CreateSample()).Normalize();
            var lensPoint = lensCenter + new Vector3(sample * lens.radius, 0);
            var direction = focusPoint - lensPoint;
            lensPoint.Z = 0;
            var position = localToWorldMatrix.MultiplyPoint(lensPoint);
            direction = localToWorldMatrix.MultiplyVector(direction).Normalize();
            rays[i] = new Ray(position, direction);
        }
        return rays;
    }

    private Vector3 ViewportPoint(Vector2 screenPoint)
    {
        var x = (2 * (screenPoint.X + 0.5f) / width - 1) * tan;
        var y = (1 - 2 * (screenPoint.Y + 0.5f) / height) / aspectRatio * tan;
        return new Vector3(x, y, 0);
    }
}
