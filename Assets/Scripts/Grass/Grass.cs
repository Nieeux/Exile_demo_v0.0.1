using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [Header("Debug")]
    public Vector3[] D_positions;
    public Vector3[] D_normals;
    public Vector2[] D_uvs;


    [Header("Components")]
    [SerializeField] private Mesh sourceMesh = default;
    [SerializeField] private Material material = default;
    [SerializeField] private ComputeShader computeShader = default;

    [Header("Grass")]
    public int GrassAmount;
    public Vector3 GroundSize;

    // length/width
    public float sizeWidth = 1f;
    public float sizeLength = 1f;

    // color settings
    public float rangeR, rangeG, rangeB;
    public Color AdjustedColor;

    [Range(1, 10)] public int allowedBladesPerVertex = 4;
    [Range(1, 5)] public int allowedSegmentsPerBlade = 5;

    // Blade
    [Header("Blade")]
    [Range(0, 1)] public float grassRandomHeight = 0.25f;
    [Range(0, 1)] public float bladeRadius = 0.2f;
    [Range(0, 1)] public float bladeForwardAmount = 0.38f;
    [Range(1, 5)] public float bladeCurveAmount = 2;

    [Range(0, 1)] public float bottomWidth = 0.1f;

    // Wind
    [Header("Wind")]
    public float windSpeed = 10;
    public float windStrength = 0.05f;
    // Interactor
    public float affectRadius = 1.5f;
    public float affectStrength = 1;
    // LOD
    [Header("LOD")]
    public float minFadeDistance = 40;
    public float maxFadeDistance = 60;
    public bool FadeInEditor;
    // Material
    [Header("Material")]
    public Color topTint = new Color(1, 1, 1);
    public Color bottomTint = new Color(0, 0, 1);
    // Other
    [Header("Other")]
    public UnityEngine.Rendering.ShadowCastingMode castShadow;
    private Camera Cam;

    ShaderInteractor[] interactors;

    // mesh lists
    [SerializeField]
    List<Vector3> positions = new List<Vector3>();
    [SerializeField]
    List<Color> colors = new List<Color>();
    [SerializeField]
    List<int> indicies = new List<int>();
    [SerializeField]
    List<Vector3> normals = new List<Vector3>();
    [SerializeField]
    List<Vector2> length = new List<Vector2>();
    int[] indi;
    public int I = 0;

    private struct SourceVertex
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector2 uv;
        public Vector3 color;
    }

    // A state variable to help keep track of whether compute buffers have been set up
    private bool m_Initialized;
    // A compute buffer to hold vertex data of the source mesh
    private ComputeBuffer m_SourceVertBuffer;
    // A compute buffer to hold vertex data of the generated mesh
    private ComputeBuffer m_DrawBuffer;
    // A compute buffer to hold indirect draw arguments
    private ComputeBuffer m_ArgsBuffer;
    // Instantiate the shaders so data belong to their unique compute buffers
    private ComputeShader m_InstantiatedComputeShader;
    [SerializeField] Material m_InstantiatedMaterial;
    // The id of the kernel in the grass compute shader
    private int m_IdGrassKernel;
    // The x dispatch size for the grass compute shader
    private int m_DispatchSize;
    // The local bounds of the generated mesh
    private Bounds m_LocalBounds;


    // The size of one entry in the various compute buffers
    private const int SOURCE_VERT_STRIDE = sizeof(float) * (3 + 3 + 2 + 3);
    private const int DRAW_STRIDE = sizeof(float) * (3 + (3 + 2 + 3) * 3);
    private const int INDIRECT_ARGS_STRIDE = sizeof(int) * 4;

    Bounds bounds;

    // The data to reset the args buffer with every frame
    // 0: vertex count per draw instance. We will only use one instance
    // 1: instance count. One
    // 2: start vertex location if using a Graphics Buffer
    // 3: and start instance location if using a Graphics Buffer
    private int[] argsBufferReset = new int[] { 0, 1, 0, 0 };

    void Start()
    {

        Cam = Camera.main;
        sourceMesh = GetComponent<MeshFilter>().sharedMesh;
        SetupMesh();
        /*
        for (int i = 0; i < GrassAmount; i++)
        {

            Vector3 startPoint = RandomPointAboveTerrain();
            startPoint.y = 200f;
            RaycastHit hit;

            if (Physics.Raycast(startPoint, Vector3.down, out hit, 500f) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {

                var grassPosition = hit.point;
                grassPosition -= this.transform.position;

                positions.Add((grassPosition));
                indicies.Add(I);
                length.Add(new Vector2(sizeWidth, sizeLength));                        
                colors.Add(new Color(AdjustedColor.r + (Random.Range(0, 1.0f) * rangeR), AdjustedColor.g + (Random.Range(0, 1.0f) * rangeG), AdjustedColor.b + (Random.Range(0, 1.0f) * rangeB), 1));
                normals.Add(hit.normal);
                I++;
            }
        }
        RebuildMesh();
        */
        InstantiateGrass();

        //base.Invoke("InstantiateGrass", 1f);

    }

    private Vector3 RandomPointAboveTerrain()
    {
        return new Vector3(
            // random diem tao objects
            Random.Range(transform.position.x - GroundSize.x / 2, transform.position.x + GroundSize.x / 2),
            transform.position.y + GroundSize.y * 2,
            Random.Range(transform.position.z - GroundSize.z / 2, transform.position.z + GroundSize.z / 2)
        );
    }
    void SetupMesh()
    {

        sourceMesh.GetVertices(positions);
        for (int i = 0; i < positions.Count; i++)
        {
            length.Add(new Vector2(sizeWidth, sizeLength));
        }

        I = positions.Count;

        sourceMesh.GetIndices(indicies, 0);
        indi = indicies.ToArray();
        sourceMesh.SetUVs(0, length);
        sourceMesh.GetColors(colors);
        sourceMesh.GetNormals(normals);

    }
    void RebuildMesh()
    {
        if (sourceMesh == null)
        {
            sourceMesh = new Mesh();
        }
        sourceMesh.Clear();
        sourceMesh.SetVertices(positions);
        indi = indicies.ToArray();
        sourceMesh.SetIndices(indi, MeshTopology.Points, 0);
        sourceMesh.SetUVs(0, length);
        sourceMesh.SetColors(colors);
        sourceMesh.SetNormals(normals);
        sourceMesh.RecalculateBounds();

    }

    private void InstantiateGrass()
    {
        
        m_InstantiatedComputeShader = Instantiate(computeShader);
        m_InstantiatedMaterial = Instantiate(material);

        Vector3[] positions = sourceMesh.vertices;
        Vector3[] normals = sourceMesh.normals;
        Vector2[] uvs = sourceMesh.uv;

        D_positions = positions;
        D_normals = normals;
        D_uvs = uvs;

        SourceVertex[] vertices = new SourceVertex[positions.Length];

        for (int i = 0; i < vertices.Length; i++)
        {

            vertices[i] = new SourceVertex()
            {
                position = positions[i],
                normal = normals[i],
                uv = uvs[i],
                color = new Vector3(AdjustedColor.r, AdjustedColor.g, AdjustedColor.b) // Color --> Vector3
            };
        }

        int numSourceVertices = vertices.Length;

        // Each segment has two points
        int maxBladesPerVertex = Mathf.Max(1, allowedBladesPerVertex);
        int maxSegmentsPerBlade = Mathf.Max(1, allowedSegmentsPerBlade);
        int maxBladeTriangles = maxBladesPerVertex * ((maxSegmentsPerBlade - 1) * 2 + 1);

        // Create compute buffers
        // The stride is the size, in bytes, each object in the buffer takes up
        m_SourceVertBuffer = new ComputeBuffer(vertices.Length, SOURCE_VERT_STRIDE,
            ComputeBufferType.Structured, ComputeBufferMode.Immutable);
        m_SourceVertBuffer.SetData(vertices);

        m_DrawBuffer = new ComputeBuffer(numSourceVertices * maxBladeTriangles, DRAW_STRIDE,
            ComputeBufferType.Append);
        m_DrawBuffer.SetCounterValue(0);

        m_ArgsBuffer =
            new ComputeBuffer(1, INDIRECT_ARGS_STRIDE, ComputeBufferType.IndirectArguments);

        // Cache the kernel IDs we will be dispatching
        m_IdGrassKernel = m_InstantiatedComputeShader.FindKernel("Main");

        // Set buffer data
        m_InstantiatedComputeShader.SetBuffer(m_IdGrassKernel, "_SourceVertices",
            m_SourceVertBuffer);
        m_InstantiatedComputeShader.SetBuffer(m_IdGrassKernel, "_DrawTriangles", m_DrawBuffer);
        m_InstantiatedComputeShader.SetBuffer(m_IdGrassKernel, "_IndirectArgsBuffer",
            m_ArgsBuffer);
        // Set vertex data
        m_InstantiatedComputeShader.SetInt("_NumSourceVertices", numSourceVertices);
        m_InstantiatedComputeShader.SetInt("_MaxBladesPerVertex", maxBladesPerVertex);
        m_InstantiatedComputeShader.SetInt("_MaxSegmentsPerBlade", maxSegmentsPerBlade);

        m_InstantiatedMaterial.SetBuffer("_DrawTriangles", m_DrawBuffer);
        
        // Calculate the number of threads to use. Get the thread size from the kernel
        // Then, divide the number of triangles by that size
        m_InstantiatedComputeShader.GetKernelThreadGroupSizes(m_IdGrassKernel,
            out uint threadGroupSize, out _, out _);
        m_DispatchSize = Mathf.CeilToInt((float)numSourceVertices / threadGroupSize);

        // Get the bounds of the source mesh and then expand by the maximum blade width and height
        m_LocalBounds = sourceMesh.bounds;
        m_LocalBounds.Expand(Mathf.Max(2, 2));

        // Transform the bounds to world space
        bounds = TransformBounds(m_LocalBounds);

        SetGrassDataBase();
        m_Initialized = true;

    }

    private void LateUpdate()
    {

        if (!m_Initialized)
        {
            // Initialization is not done, please check if there are null components
            // or just because there is not vertex being painted.
            return;
        }

        // Clear the draw and indirect args buffers of last frame's data
        m_DrawBuffer.SetCounterValue(0);
        m_ArgsBuffer.SetData(argsBufferReset);



        // Update the shader with frame specific data
        SetGrassDataUpdate();

        // Dispatch the grass shader. It will run on the GPU
        m_InstantiatedComputeShader.Dispatch(m_IdGrassKernel, m_DispatchSize, 1, 1);

        // DrawProceduralIndirect queues a draw call up for our generated mesh
        Graphics.DrawProceduralIndirect(m_InstantiatedMaterial, bounds, MeshTopology.Triangles, m_ArgsBuffer, 0, Cam, null, castShadow, true, gameObject.layer);

    }

    private void SetGrassDataBase()
    {
        interactors = (ShaderInteractor[])FindObjectsOfType(typeof(ShaderInteractor));

        // Send things to compute shader that dont need to be set every frame
        m_InstantiatedComputeShader.SetMatrix("_LocalToWorld", transform.localToWorldMatrix);
        m_InstantiatedComputeShader.SetFloat("_Time", Time.time);

        m_InstantiatedComputeShader.SetFloat("_GrassRandomHeight", grassRandomHeight);

        m_InstantiatedComputeShader.SetFloat("_WindSpeed", windSpeed);
        m_InstantiatedComputeShader.SetFloat("_WindStrength", windStrength);

        m_InstantiatedComputeShader.SetFloat("_InteractorRadius", affectRadius);
        m_InstantiatedComputeShader.SetFloat("_InteractorStrength", affectStrength);

        m_InstantiatedComputeShader.SetFloat("_BladeRadius", bladeRadius);
        m_InstantiatedComputeShader.SetFloat("_BladeForward", bladeForwardAmount);
        m_InstantiatedComputeShader.SetFloat("_BladeCurve", Mathf.Max(0, bladeCurveAmount));
        m_InstantiatedComputeShader.SetFloat("_BottomWidth", bottomWidth);

        if (FadeInEditor || Application.isPlaying == true)
        {


            m_InstantiatedComputeShader.SetFloat("_MinFadeDist", minFadeDistance);
            m_InstantiatedComputeShader.SetFloat("_MaxFadeDist", maxFadeDistance);
        }
        else
        {
            m_InstantiatedComputeShader.SetFloat("_MinFadeDist", 0);
            m_InstantiatedComputeShader.SetFloat("_MaxFadeDist", 999);
        }



        m_InstantiatedComputeShader.SetFloat("_OrthographicCamSize", Shader.GetGlobalFloat("_OrthographicCamSize"));
        m_InstantiatedComputeShader.SetVector("_OrthographicCamPos", Shader.GetGlobalVector("_OrthographicCamPos"));

        m_InstantiatedMaterial.SetColor("_TopTint", topTint);
        m_InstantiatedMaterial.SetColor("_BottomTint", bottomTint);

    }

    private void SetGrassDataUpdate()
    {
        // Compute Shader
        //  m_InstantiatedComputeShader.SetMatrix("_LocalToWorld", transform.localToWorldMatrix);
        m_InstantiatedComputeShader.SetFloat("_Time", Time.time);

        if (interactors.Length > 0)
        {
            Vector4[] positions = new Vector4[interactors.Length];
            for (int i = 0; i < interactors.Length; i++)
            {
                positions[i] = interactors[i].transform.position;

            }
            int shaderID = Shader.PropertyToID("_PositionsMoving");
            m_InstantiatedComputeShader.SetVectorArray(shaderID, positions);
            m_InstantiatedComputeShader.SetFloat("_InteractorsLength", interactors.Length);
        }
        if (Cam != null)
        {
            m_InstantiatedComputeShader.SetVector("_CameraPositionWS", Cam.transform.position);

        }
    }

    private Bounds TransformBounds(Bounds boundsOS)
    {
        var center = transform.TransformPoint(boundsOS.center);

        // transform the local extents' axes
        var extents = boundsOS.extents;
        var axisX = transform.TransformVector(extents.x, 0, 0);
        var axisY = transform.TransformVector(0, extents.y, 0);
        var axisZ = transform.TransformVector(0, 0, extents.z);

        // sum their absolute value to get the world extents
        extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
        extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
        extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

        return new Bounds { center = center, extents = extents };
    }
    private void OnDisable()
    {
        // Dispose of buffers and copied shaders here
        if (m_Initialized)
        {
            // If the application is not in play mode, we have to call DestroyImmediate
            if (Application.isPlaying)
            {
                Destroy(m_InstantiatedComputeShader);
                //  Destroy(m_InstantiatedMaterial);
            }
            else
            {
                DestroyImmediate(m_InstantiatedComputeShader);
                // DestroyImmediate(m_InstantiatedMaterial);
            }

            // Release each buffer
            m_SourceVertBuffer?.Release();
            m_DrawBuffer?.Release();
            m_ArgsBuffer?.Release();
        }

        m_Initialized = false;
    }
}
