namespace ILGPURenderer;

using System;
using System.Linq;
using System.Numerics;
using CowLibrary;
using CowLibrary.Lights;
using CowLibrary.Mathematics.Sampler;
using CowRenderer;
using Data;
using LocalRender;

public readonly struct LocalIntegrator
{
    private readonly LocalSampler sampler;
    private readonly LocalRaycaster raycaster;
    private readonly EnvironmentIntegrator environmentIntegrator;

    public LocalIntegrator(LocalSampler sampler, LocalRaycaster raycaster)
    {
        this.sampler = sampler;
        this.raycaster = raycaster;
        environmentIntegrator = new EnvironmentIntegrator(sampler, raycaster);
    }

    public Color GetColor(in SceneView sceneView, in RayHit hit, in Ray ray)
    {
        if (!hit.HasHit)
        {
            return GetLightColor(in sceneView, in hit, in ray);
        }
        var color = Color.Black;
        for (var i = 0; i < sceneView.light.environmentLights.Length; i++)
        {
            var light = sceneView.light.environmentLights[i];
            var matrix = sceneView.light.GetMatrix(light.Id);
            color += environmentIntegrator.GetLighting(in sceneView, in hit, in ray, in light, in matrix);
        }
        return color;
    }

    private Color GetLightColor(in SceneView sceneView, in RayHit hit, in Ray ray)
    {
        var color = Color.Black;
        for (var i = 0; i < sceneView.light.environmentLights.Length; i++)
        {
            color += sceneView.light.environmentLights[i].Sample(in ray.direction, sampler.CreateSample());
        }
        return color;
    }

    // private Color GetIndirectLighting(in Surfel surfel, Light light, int depth)
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
    //
    // private Color Trace(in Surfel surfel, Light light, Vector3 direction, int depth)
    // {
    //     if (depth >= RenderConfig.rayDepth)
    //     {
    //         return Color.Black;
    //     }
    //     var sign = Math.Sign(Vector3.Dot(surfel.hit.normal, direction));
    //     var position = surfel.hit.point + sign * surfel.hit.normal * RenderConfig.bias;
    //     var surfelHit = Raycaster.Raycast(new Ray(position, direction));
    //     if (surfelHit.hit.HasHit)
    //     {
    //         return GetLighting(in surfelHit, light, depth + 1);
    //     }
    //     var lightning = light.Sample(in direction);
    //     var dot = Vector3.Dot(surfel.hit.normal, direction);
    //     dot = Math.Max(dot, 0);
    //     return surfel.material.Color * lightning * dot;
    // }
}
