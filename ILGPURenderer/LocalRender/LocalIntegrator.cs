namespace ILGPURenderer;

using System;
using System.Numerics;
using CowLibrary;
using CowLibrary.Lights.Models;
using CowLibrary.Mathematics.Sampler;
using Data;
using ILGPU;
using ILGPU.Algorithms;

public readonly struct LocalIntegrator
{
    private readonly LocalSampler sampler;
    private readonly LocalRaycaster raycaster;

    public LocalIntegrator(LocalSampler sampler, LocalRaycaster raycaster)
    {
        this.sampler = sampler;
        this.raycaster = raycaster;
    }

    public Color GetColor(in SceneView sceneView, in RenderData renderData, in Raycast raycast)
    {
        if (!raycast.hit.HasHit)
        {
            return GetLightColor(in sceneView, in raycast);
        }
        var color = Color.Black;
        GetLighting(in sceneView, in renderData, in raycast, in sceneView.light.environmentLights, ref color);
        GetLighting(in sceneView, in renderData, in raycast, in sceneView.light.directionalLights, ref color);
        GetLighting(in sceneView, in renderData, in raycast, in sceneView.light.pointLights, ref color);
        return color;
    }

    private Color GetLightColor(in SceneView sceneView, in Raycast raycast)
    {
        var color = Color.Black;
        color += GetLightColor(in sceneView.light.environmentLights, in raycast.ray.direction);
        return color;
    }

    private Color GetLightColor<T>(in ArrayView<T> lights, in Vector3 direction)
        where T : unmanaged, ILightModel
    {
        var color = Color.Black;
        for (var i = 0; i < lights.Length; i++)
        {
            color += SampleLight(in lights[i], direction);
        }
        return color;
    }

    private void GetLighting<T>(in SceneView sceneView, in RenderData renderData, in Raycast raycast,
        in ArrayView<T> lights, ref Color color)
        where T : unmanaged, ILightModel
    {
        for (var i = 0; i < lights.Length; i++)
        {
            var light = lights[i];
            color += GetLighting(in sceneView, in renderData, in raycast, in light);
        }
    }

    private Color GetLighting<T>(in SceneView sceneView, in RenderData renderData, in Raycast primaryRaycast,
        in T light)
        where T : struct, ILightModel
    {
        var color = Color.Black;
        var beta = Color.White;
        var raycast = primaryRaycast;
        for (var bounces = 0; bounces < renderData.rayDepth; bounces++)
        {
            var f = sceneView.material.Sample(in raycast.hit.id, in raycast.hit.normal, in raycast.ray.direction,
                sampler.CreateSample(), out var wi, out var pdf);
            var matColor = sceneView.material.GetMaterialRawColor(in raycast.hit.id);
            beta *= f * matColor * XMath.Abs(Vector3.Dot(wi, raycast.hit.normal)) / pdf;
            if (f == 0 || pdf == 0)
            {
                break;
            }
            raycast = Trace(in sceneView, in raycast, in wi);
            if (!raycast.hit.HasHit)
            {
                color += beta * SampleLight(in light, in wi);
                break;
            }
        }
        return color;
    }

    private Raycast Trace(in SceneView sceneView, in Raycast raycast, in Vector3 direction)
    {
        var sign = XMath.Sign(Vector3.Dot(raycast.hit.normal, direction));
        var position = raycast.hit.point + sign * raycast.hit.normal * Const.Bias;
        var ray = new Ray(position, direction);
        return raycaster.Raycast(in sceneView.mesh, in ray);
    }

    private Color SampleLight<T>(in T light, in Vector3 direction) where T : struct, ILightModel
    {
        return light.Sample(in direction, sampler.CreateSample());
    }
}
