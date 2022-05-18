namespace ILGPURenderer.LocalRender;

using System;
using System.Numerics;
using CowLibrary;
using CowLibrary.Lights.Models;
using CowLibrary.Mathematics.Sampler;
using Data;

public readonly struct EnvironmentIntegrator
{
    private readonly LocalSampler sampler;
    private readonly LocalRaycaster raycaster;

    public EnvironmentIntegrator(LocalSampler sampler, LocalRaycaster raycaster)
    {
        this.sampler = sampler;
        this.raycaster = raycaster;
    }

    public Color GetLighting(in SceneView sceneView, in RayHit hit, in Ray ray, in EnvironmentLightModel light,
        in Matrix4x4 matrix)
    {
        return GetDirectLighting(in sceneView, in hit, in ray, in light, in matrix);
        // GetIndirectLighting(in surfel, light, depth);
    }

    private Color GetDirectLighting(in SceneView sceneView, in RayHit hit, in Ray ray, in EnvironmentLightModel light,
        in Matrix4x4 matrix)
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

    // private Color GetIndirectLighting(in SceneView sceneView, in RayHit hit, in Ray ray, in DirectionalLightModel light)
    // {
    //     var result = Color.Black;
    //     var n = RenderConfig.numberOfRayPerLight;
    //     for (var i = 0; i < n; i++)
    //     {
    //         var sample = SamplerProvider.Sampler.CreateSample();
    //         var f = surfel.material.Sample(in surfel.hit.normal, in surfel.ray, out var wi, in sample, out var pdf);
    //         if (pdf > 0 && f > 0)
    //         {
    //             result += f * pdf * Trace(in surfel, light, wi, depth);
    //         }
    //     }
    //     return result / RenderConfig.numberOfRayPerLight;
    // }
}
