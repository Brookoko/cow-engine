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

    public Color GetColor(in SceneView sceneView, in RenderData renderData, in RayHit hit, in Ray ray)
    {
        if (!hit.HasHit)
        {
            return GetLightColor(in sceneView, in ray);
        }
        var color = Color.Black;
        for (var i = 0; i < sceneView.light.environmentLights.Length; i++)
        {
            var light = sceneView.light.environmentLights[i];
            var matrix = sceneView.light.GetMatrix(light.Id);
            color += GetLighting(in sceneView, in renderData, in hit, in ray, in light, in matrix, 0);
        }
        return color;
    }

    private Color GetLightColor(in SceneView sceneView, in Ray ray)
    {
        var color = Color.Black;
        color += GetLightColor(in sceneView.light.environmentLights, in ray.direction);
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

    public Color GetLighting<T>(in SceneView sceneView, in RenderData renderData, in RayHit hit, in Ray ray,
        in T light, in Matrix4x4 matrix, int depth)
        where T : struct, ILightModel
    {
        // return GetDirectLighting(in sceneView, in hit, in ray, in light, in matrix);
        return GetDirectLighting(in sceneView, in hit, in ray, in light, in matrix) +
               GetIndirectLighting(in sceneView, in renderData, in hit, in ray, in light, in matrix, depth);
    }

    private Color GetDirectLighting<T>(in SceneView sceneView, in RayHit hit, in Ray ray, in T light,
        in Matrix4x4 matrix)
        where T : struct, ILightModel
    {
        var shading = light.GetShadingInfo(in hit, in matrix, sampler.CreateSample());
        var color = sceneView.material.GetMaterialColor(in hit.id, in ray.direction, in shading.direction);
        var dot = Vector3.Dot(hit.normal, shading.direction);
        dot = Math.Max(dot, 0);
        var multiplier = TraceShadowRay(in sceneView, in hit, in shading.direction, shading.distance);
        return multiplier * color * shading.color * dot;
    }

    private float TraceShadowRay(in SceneView sceneView, in RayHit hit, in Vector3 direction, float distance)
    {
        var position = hit.point + hit.normal * Const.Bias;
        var surfaceHit = raycaster.Raycast(in sceneView.mesh, new Ray(position, direction));
        return surfaceHit.HasHit && surfaceHit.t < distance ? 0 : 1;
    }

    private Color GetIndirectLighting<T>(in SceneView sceneView, in RenderData renderData, in RayHit hit, in Ray ray,
        in T light, in Matrix4x4 matrix, int depth)
        where T : struct, ILightModel
    {
        var result = Color.Black;
        var n = renderData.numberOfRayPerMaterial;
        for (var i = 0; i < n; i++)
        {
            var sample = sampler.CreateSample();
            var f = sceneView.material.Sample(in hit.id, in hit.normal, in ray.direction, out var wi, in sample,
                out var pdf);
            if (pdf > 0 && f > 0)
            {
                result += f * pdf * Trace(in sceneView, in renderData, in light, in hit, in wi, in matrix, depth);
            }
        }
        return result / renderData.numberOfRayPerMaterial;
    }

    private Color Trace<T>(in SceneView sceneView, in RenderData renderData, in T light, in RayHit hit,
        in Vector3 direction, in Matrix4x4 matrix, int depth)
        where T : struct, ILightModel
    {
        if (depth >= renderData.rayDepth)
        {
            return Color.Black;
        }
        var sign = Vector3.Dot(hit.normal, direction) >= 0 ? 1 : -1;
        var position = hit.point + sign * hit.normal * Const.Bias;
        var ray = new Ray(position, direction);
        var surfelHit = raycaster.Raycast(in sceneView.mesh, in ray);
        if (surfelHit.HasHit)
        {
            return Color.Black;
            return GetLighting(in sceneView, in renderData, in hit, in ray, in light, in matrix, depth + 1);
        }
        var lightning = light.Sample(in direction, sampler.CreateSample());
        var dot = Vector3.Dot(hit.normal, direction);
        dot = Math.Max(dot, 0);
        var color = sceneView.material.GetMaterialRawColor(in hit.id);
        return color * lightning * dot;
    }
}
