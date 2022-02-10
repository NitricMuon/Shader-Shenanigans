using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class CGoLManager : MonoBehaviour
{
    public const GraphicsFormat defaultGFormat = GraphicsFormat.R16G16B16A16_SFloat;
    public int width = 2560;
    public int height = 1440;

    public ComputeShader compute;
    RenderTexture displayTexture;

    void Init()
    {
        displayTexture = new RenderTexture(width, height, 0, defaultGFormat);
        displayTexture.enableRandomWrite = true;
        displayTexture.autoGenerateMips = false;
        displayTexture.Create();

        compute.SetTexture(0, "TargetTexture", displayTexture);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        GetComponent<MeshRenderer>().material.mainTexture = displayTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    void LateUpdate()
    {

    }
}
