
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public static class Helper
{
    private const GraphicsFormat defaultGFormat = GraphicsFormat.R16G16B16A16_SFloat;

    public static void CreateStructuredBuffer<T>(ref ComputeBuffer buffer, int count)
    {
        int stride = System.Runtime.InteropServices.Marshal.SizeOf<T>();
        buffer = new ComputeBuffer(count, stride);
    }

    public static void CreateRenderTexture(ref RenderTexture texture, int width, int height, int depth = 0, GraphicsFormat format = defaultGFormat)
    {
        texture = new RenderTexture(width, height, depth, format);
        texture.enableRandomWrite = true;
        texture.autoGenerateMips = true;
        texture.filterMode = FilterMode.Point;
        texture.Create();
    }
}
