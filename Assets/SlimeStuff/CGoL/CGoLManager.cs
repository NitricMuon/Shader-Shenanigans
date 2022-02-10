using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Experimental.Rendering;

public class CGoLManager : MonoBehaviour
{
    private const GraphicsFormat defaultGFormat = GraphicsFormat.R16G16B16A16_SFloat;

    [HeaderAttribute("Simulation Size")]
    public int width = 2560;
    public int height = 1440;

    [HeaderAttribute("Simulation Settings")]
    public int rangeStart = 2;
    public int rangeEnd = 3;
    public int spawn = 3;

    public ComputeShader compute;
    RenderTexture displayTexture;
    RenderTexture processTexture;

    ComputeBuffer randomData;

    private int updateKernel = 0;
    private int displayKernel = 1;

    void Init()
    {
        displayTexture = new RenderTexture(width, height, 0, defaultGFormat);
        displayTexture.enableRandomWrite = true;
        displayTexture.autoGenerateMips = false;
        displayTexture.Create();

        processTexture = new RenderTexture(width, height, 0, defaultGFormat);
        processTexture.enableRandomWrite = true;
        processTexture.autoGenerateMips = false;
        processTexture.Create();

        compute.SetTexture(updateKernel, "displayTexture", displayTexture);
        compute.SetTexture(updateKernel, "processTexture", processTexture);

        compute.SetTexture(displayKernel, "displayTexture", displayTexture);
        compute.SetTexture(displayKernel, "processTexture", processTexture);

        compute.SetInt("width", width);
        compute.SetInt("height", height);
        compute.SetInt("rangeStart", rangeStart);
        compute.SetInt("rangeEnd", rangeEnd);
        compute.SetInt("spawn", spawn);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        GetComponent<MeshRenderer>().material.mainTexture = displayTexture;

        int stride = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector2));
        int count = width * height;
        int initialCells = 1000000;
        randomData = new ComputeBuffer(count, stride);

        Vector2[] cells = new Vector2[initialCells];

        for (int i = 0; i < initialCells; i++)
        {
            cells[i] = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        }
        randomData.SetData(cells);
        compute.SetBuffer(2, "randomData", randomData);
        compute.SetTexture(2, "displayTexture", displayTexture);
        
        int threadGroupX = Mathf.CeilToInt(count/64);
        compute.Dispatch(2, threadGroupX, 1, 1);

        randomData.Release();
    }

    // Update is called once per frame
    void Update()
    {
        compute.SetInt("width", width);
        compute.SetInt("height", height);
        compute.SetInt("rangeStart", rangeStart);
        compute.SetInt("rangeEnd", rangeEnd);
        compute.SetInt("spawn", spawn);

        int threadGroupX = Mathf.CeilToInt(width/8);
        int threadGroupY = Mathf.CeilToInt(height/8);
        //compute.Dispatch(updateKernel, threadGroupX, threadGroupY, 1);

        compute.Dispatch(displayKernel, threadGroupX, threadGroupY, 1);
    }

    void FixedUpdate()
    {
        
    }

    void LateUpdate()
    {
        
    }
}
