namespace ILGPURenderer;

using System;
using System.Numerics;
using CowLibrary;
using CowLibrary.Lights.Models;
using CowLibrary.Mathematics.Sampler;
using Data;
using ILGPU;

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
        for (var i = 0; i < sceneView.light.environmentLights.Length; i++)
        {
            var light = sceneView.light.environmentLights[i];
            color += GetLighting(in sceneView, in renderData, raycast, in light);
        }
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
            color += lights[i].Sample(in direction, sampler.CreateSample());
        }
        return color;
    }

    private Color GetLighting<T>(in SceneView sceneView, in RenderData renderData, Raycast raycast, in T light)
        where T : struct, ILightModel
    {
        var color = GetDirectLighting(in sceneView, in raycast, in light);
        var colors = new Color[2 + 1];
        var indirectRays = new Raycast[2];
        var indirectCount = new int[2];
        indirectRays[0] = raycast;
        var depth = 0;
        while (indirectCount[0] < renderData.numberOfRayPerMaterial + 1)
        {
            if (depth == renderData.rayDepth)
            {
                depth--;
                continue;
            }
            indirectCount[depth]++;
            colors[depth] += colors[depth + 1] / renderData.numberOfRayPerMaterial;
            colors[depth + 1] = Color.Black;
            if (indirectCount[depth] > renderData.numberOfRayPerMaterial)
            {
                depth--;
                continue;
            }
            raycast = indirectRays[depth];
            var f = sceneView.material.Sample(in raycast.hit.id, in raycast.hit.normal, in raycast.ray.direction,
                sampler.CreateSample(), out var wi, out var pdf);
            if (f * pdf == 0)
            {
                continue;
            }
            var surfelHit = Trace(in sceneView, in raycast, in wi);
            if (!surfelHit.hit.HasHit)
            {
                colors[depth] += f * pdf * SampleLight(in sceneView, in raycast, in light, in wi);
                continue;
            }
            indirectRays[depth] = raycast;
            depth++;
            raycast = surfelHit;
            colors[depth] = GetDirectLighting(in sceneView, in raycast, in light);
        }
        return color + colors[0] / renderData.numberOfRayPerMaterial;
    }

    private Color GetDirectLighting<T>(in SceneView sceneView, in Raycast raycast, in T light)
        where T : struct, ILightModel
    {
        var matrix = sceneView.light.GetMatrix(light.Id);
        var shading = light.GetShadingInfo(in raycast.hit, in matrix, sampler.CreateSample());
        var color = sceneView.material.GetMaterialColor(in raycast.hit.id, in raycast.ray.direction,
            in shading.direction);
        var dot = Vector3.Dot(raycast.hit.normal, shading.direction);
        dot = Math.Max(dot, 0);
        var multiplier = TraceShadowRay(in sceneView, in raycast.hit, in shading.direction, shading.distance);
        return multiplier * color * shading.color * dot;
    }

    private float TraceShadowRay(in SceneView sceneView, in RayHit hit, in Vector3 direction, float distance)
    {
        var position = hit.point + hit.normal * Const.Bias;
        var surfaceHit = raycaster.Raycast(in sceneView.mesh, new Ray(position, direction));
        return surfaceHit.hit.HasHit && surfaceHit.hit.t < distance ? 0 : 1;
    }

    private Raycast Trace(in SceneView sceneView, in Raycast raycast, in Vector3 direction)
    {
        var sign = Vector3.Dot(raycast.hit.normal, direction) >= 0 ? 1 : -1;
        var position = raycast.hit.point + sign * raycast.hit.normal * Const.Bias;
        var ray = new Ray(position, direction);
        return raycaster.Raycast(in sceneView.mesh, in ray);
    }

    private Color SampleLight<T>(in SceneView sceneView, in Raycast raycast, in T light, in Vector3 direction)
        where T : struct, ILightModel
    {
        var lightning = light.Sample(in direction, sampler.CreateSample());
        var dot = Vector3.Dot(raycast.hit.normal, direction);
        dot = Math.Max(dot, 0);
        var color = sceneView.material.GetMaterialRawColor(in raycast.hit.id);
        return color * lightning * dot;
    }
}
