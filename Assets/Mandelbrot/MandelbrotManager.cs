using System;
using System.Numerics;
using UnityEngine;

public class MandelbrotManager : MonoBehaviour
{
    [HeaderAttribute("Simulation Size")]
    public int width = 2560;
    public int height = 1440;

    [HeaderAttribute("Simulation Settings")]
    public ComputeShader compute;
    public int maxInterations = 100;
    public int scalar = 1;
    public UnityEngine.Vector2 minimums;
    public UnityEngine.Vector2 maximums;

    private int threadGroupX;
    private int threadGroupY;

    private RenderTexture result;

    // Initialize stuff
    void Init()
    {
        threadGroupX = Mathf.CeilToInt(width/8);
        threadGroupY = Mathf.CeilToInt(height/8);

        Helper.CreateRenderTexture(ref result, width, height);

        compute.SetVector("minimums", minimums);
        compute.SetVector("maximums", maximums);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
