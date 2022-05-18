namespace ILGPURenderer.Utils;

using System.Collections.Generic;
using System.Linq;
using ILGPU;
using ILGPU.Runtime;

public static class Utils
{
    public static TDerived[] FindDerived<TDerived, TBase>(IEnumerable<TBase> meshes) where TDerived : TBase
    {
        return meshes.Where(m => m is TDerived).Cast<TDerived>().ToArray();
    }
}
