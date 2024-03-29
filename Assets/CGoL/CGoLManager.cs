using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class CGoLManager : MonoBehaviour
{
    private const GraphicsFormat defaultGFormat = GraphicsFormat.R16G16B16A16_SFloat;

    [HeaderAttribute("Simulation Size")]
    public int width = 2560;
    public int height = 1440;

    [HeaderAttribute("Simulation Settings")]
    public ComputeShader compute;
    public int rangeStart = 2;
    public int rangeEnd = 3;
    public int spawn = 3;
    public float cutoff;

    RenderTexture displayTexture;
    RenderTexture processTexture;

    private int threadGroupX;
    private int threadGroupY;

    void Init()
    {
        threadGroupX = Mathf.CeilToInt(width/8);
        threadGroupY = Mathf.CeilToInt(height/8);

        CreateRenderTexture(ref displayTexture, width, height);
        CreateRenderTexture(ref processTexture, width, height);

        compute.SetTexture(0, "displayTexture", displayTexture);
        compute.SetTexture(0, "processTexture", processTexture);

        compute.SetTexture(1, "displayTexture", displayTexture);
        compute.SetTexture(1, "processTexture", processTexture);

        compute.SetTexture(2, "displayTexture", displayTexture);

        compute.SetInt("rangeStart", rangeStart);
        compute.SetInt("rangeEnd", rangeEnd);
        compute.SetInt("spawn", spawn);
        compute.SetFloat("cutoff", cutoff);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        GetComponent<MeshRenderer>().material.mainTexture = displayTexture;
        compute.Dispatch(2, threadGroupX, threadGroupY, 1);
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

    void OnMouseDown()
    {
        compute.SetFloat("time", Time.fixedTime);
        //compute.SetFloat("deltaTime", Time.fixedDeltaTime);

        compute.Dispatch(0, threadGroupX, threadGroupY, 1);
        compute.Dispatch(1, threadGroupX, threadGroupY, 1);
    }

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
