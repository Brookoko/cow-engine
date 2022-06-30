namespace CowTest;

using System;
using System.Collections.Generic;
using System.Numerics;
using CowLibrary;
using CowLibrary.Models.Microfacet;
using CowLibrary.Views;
using NUnit.Framework;

public class BrdfTests
{
    private const int Estimates = 10000000;
    private Random random;

    private List<(IBrdf brdf, string description)> brdfs = new()
    {
        (new LambertianBrdf(1), "Lambertian"),
        (new OrenNayar(1, 0), "Oren-Nayar (0)"),
        (new OrenNayar(1, 2f / 9), "Oren-Nayar (20)"),
        (new MetalBrdf(1,
            new ConductorFresnel(1, 1.5f, 2),
            new TrowbridgeReitzDistribution(Mathf.RoughnessToAlpha(0.5f), Mathf.RoughnessToAlpha(0.5f))),
            "Metal"),
        (new PlasticBrdf(1,
                new DielectricFresnel(1.5f, 1),
                new TrowbridgeReitzDistribution(Mathf.RoughnessToAlpha(0.5f), Mathf.RoughnessToAlpha(0.5f))),
            "Plastic"),
        (new MicrofacetReflection(1,
                new DielectricFresnel(1, 1.5f),
                new TrowbridgeReitzDistribution(Mathf.RoughnessToAlpha(0.5f), Mathf.RoughnessToAlpha(0.5f))),
            "Trowbridge-Reitz (roughness 0.5, sample visible mf area)")
    };

    [Test]
    public void BrdfDistribution()
    {
        random = new Random();
        var (hit, ray) = Raycast();
        var basis = hit.ExtractBasis();
        var wo = basis.LocalToWorld(-ray.direction);
        var n = basis.LocalToWorld(Vector3.UnitY);
        foreach (var (brdf, description) in brdfs)
        {
            var sum = 0f;
            var numHistoBins = 10;
            var histogram = new double[numHistoBins, numHistoBins];
            var badSamples = 0;
            var outsideSamples = 0;

            for (var j = 0; j < Estimates; j++)
            {
                Sample(in brdf, in wo, out var wi, out var pdf, out var f);
                var x = Mathf.Clamp(wi.X, -1, 1);
                var y = Mathf.Clamp(wi.Z, -1, 1);
                var phi = (Math.Atan2(y, x) + Const.Pi) / (2.0 * Const.Pi);
                var cosTheta = wi.Y;
                var validSample = cosTheta > 1e-7;
                if (phi < -0.0001 || phi > 1.0001 || cosTheta > 1.0001)
                {
                    Console.WriteLine($"Bad wi {wi}, {phi}, {cosTheta}");
                }
                else if (validSample)
                {
                    var histoPhi = (int)(phi * numHistoBins);
                    if (histoPhi == numHistoBins) --histoPhi;
                    var histoCosTheta = (int)(cosTheta * numHistoBins);
                    if (histoCosTheta == numHistoBins) --histoCosTheta;
                    histogram[histoCosTheta, histoPhi] += 1.0 / pdf;
                }
                if (!validSample)
                {
                    outsideSamples++;
                }
                else if (pdf == 0 || float.IsNaN(pdf) || f < 0 || float.IsNaN(f))
                {
                    badSamples++;
                    Console.WriteLine($"Bad sample {j} {pdf} {f}");
                }
                else
                {
                    sum += f * Mathf.AbsDot(wi, n) / pdf;
                }
            }
            var goodSamples = Estimates - badSamples;

            Console.WriteLine($"*** BRDF: {description}\n\n" +
                              $"wi histogram showing the relative weight in each bin\n" +
                              $"all entries should be close to 2pi = {Const.Pi * 2}:\n" +
                              $"({badSamples} bad samples, {outsideSamples} outside samples)\n\nphi bins\n");

            var totalSum = 0.0;
            for (var i = 0; i < numHistoBins; i++)
            {
                Console.Write($"  cos(theta) bin {i}:");
                for (var j = 0; j < numHistoBins; j++)
                {
                    var f = histogram[i, j] * numHistoBins * numHistoBins / goodSamples;
                    Console.Write($"  {f:0.00}");
                    totalSum += histogram[i, j];
                }
                Console.WriteLine();
            }
            var average = totalSum / goodSamples;
            var error = totalSum / goodSamples - Const.Pi * 2.0;
            var radiance = sum / goodSamples;
            Console.WriteLine($"\n  final average : {average:F5} (error {error:F5})\n\n" +
                              $"  radiance = {radiance:F5}\n\n");
            Assert.That(error, Is.LessThan(0.01));
        }
    }

    private void Sample(in IBrdf brdf, in Vector3 wo, out Vector3 wi, out float pdf, out float f)
    {
        var sample = CreateSample();
        f = brdf.Sample(in wo, sample, out wi, out pdf);
    }

    private Vector2 CreateSample()
    {
        return new Vector2(random.NextSingle(), random.NextSingle());
    }

    private (RayHit hit, Ray ray) Raycast()
    {
        var disk = new DiskView(Vector3.Zero, Vector3.UnitY, 5, 0);
        var ray = new Ray(Vector3.One, (Vector3.UnitX * 0.1f - Vector3.One).Normalize());
        var hit = new RayHit();
        disk.Intersect(in ray, ref hit);
        return (hit, ray);
    }
}
