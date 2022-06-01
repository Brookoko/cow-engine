namespace CowLibrary.Models;

using System;
using System.Numerics;

public readonly struct RealisticCameraModel : ICameraModel
{
    private readonly int width;
    private readonly int height;
    private readonly float aspectRatio;
    private readonly float tan;
    private readonly Lens lens;
    private readonly Vector3 lensCenter;

    public RealisticCameraModel(int width, int height, float fov, Lens lens)
    {
        this.width = width;
        this.height = height;
        aspectRatio = (float)width / height;
        tan = (float)Math.Tan(Const.Deg2Rad * fov / 2);
        this.lens = lens;
        lensCenter = new Vector3(0, 0, lens.distance);
    }

    public Ray ScreenPointToRay(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, in Vector2 sample)
    {
        var focusPoint = GetPerspectiveDirection(in screenPoint);
        return Sample(in localToWorldMatrix, in sample, in focusPoint);
    }

    public Ray[] Sample(in Vector2 screenPoint, in Matrix4x4 localToWorldMatrix, in Vector2[] samples)
    {
        var direction = GetPerspectiveDirection(in screenPoint);
        var rays = new Ray[samples.Length];
        for (var i = 0; i < samples.Length; i++)
        {
            rays[i] = Sample(in localToWorldMatrix, in samples[i], in direction);
        }
        return rays;
    }

    private Ray Sample(in Matrix4x4 localToWorldMatrix, in Vector2 sample, in Vector3 focusDirection)
    {
        var sampleDisk = Mathf.ConcentricSampleDisk(sample);
        var lensPoint = new Vector3(sampleDisk * lens.radius, 0);
        var ft = lens.distance / -focusDirection.Z;
        var focusPoint = ft * focusDirection;
        var direction = focusPoint - lensPoint;
        var position = localToWorldMatrix.MultiplyPoint(lensPoint);
        direction = localToWorldMatrix.MultiplyVector(direction).Normalize();
        return new Ray(position, direction);
    }

    private Vector3 GetPerspectiveDirection(in Vector2 screenPoint)
    {
        var point = ViewportPoint(in screenPoint);
        point.Z = -lens.focus;
        return point.Normalize();
    }

    private Vector3 ViewportPoint(in Vector2 screenPoint)
    {
        var x = (2 * screenPoint.X / width - 1) * tan;
        var y = (1 - 2 * screenPoint.Y / height) / aspectRatio * tan;
        return new Vector3(x, y, 0);
    }
}
