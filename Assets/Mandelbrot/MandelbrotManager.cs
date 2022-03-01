using UnityEngine;
using UnityEngine.InputSystem;

public class MandelbrotManager : MonoBehaviour
{
    [HeaderAttribute("Simulation Size")]
    public int width = 2560;
    public int height = 1440;

    [HeaderAttribute("Simulation Settings")]
    public ComputeShader compute;
    public float maxIterations = 256f;
    public float scalar = 1f; // for zooming in and stuff
    public Vector2 botleft = new Vector2(-2.00f, -1.12f);
    public Vector2 topright = new Vector2(0.47f, 1.12f);

    private int threadGroupX;
    private int threadGroupY;

    private RenderTexture result;

    // Initialize stuff
    void Init()
    {
        threadGroupX = Mathf.CeilToInt(width/8);
        threadGroupY = Mathf.CeilToInt(height/8);

        Helper.CreateRenderTexture(ref result, width, height);

        compute.SetFloat("scalar", scalar);
        compute.SetFloat("maxIterations", maxIterations);
        compute.SetVector("botleft", botleft);
        compute.SetVector("topright", topright);
        compute.SetVector("screenSize", new Vector2(width, height));
        compute.SetTexture(0, "result", result);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        GetComponent<MeshRenderer>().material.mainTexture = result;
    }

    // Update is called once per frame
    void Update()
    {
        compute.Dispatch(0, threadGroupX, threadGroupY, 1);
    }

    void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
    }

    void OnZoom(InputValue value)
    {
        
    }
}
