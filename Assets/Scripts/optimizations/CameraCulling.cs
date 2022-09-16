using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class CameraCulling : MonoBehaviour
{
    public const int NUMBER_OF_UNITY_LAYERS = 32;
    public float DefaultCullDistance;
    public bool UseSphericalCulling;
    Camera cam;
    public LayerCullInfo[] LayerCullInfoArray = new LayerCullInfo[32];
    public float DistanceMultiplier = 1f;

    void Start()
    {
        cam = GetComponent<Camera>();
        ApplyCurrentSettings();
        UpdateLayerNames();
    }

    void Update()
    {
        if (transform.hasChanged)
        {
            //cam.cullingMatrix = Matrix4x4.Ortho(-boxScale * cam.aspect, boxScale * cam.aspect, -boxScale, boxScale, cam.nearClipPlane, cam.farClipPlane) * cam.worldToCameraMatrix;
            cam.cullingMatrix = cam.projectionMatrix * cam.worldToCameraMatrix;
            //cam.cullingMatrix = Matrix4x4.Perspective(cam.fieldOfView, cam.aspect, cam.nearClipPlane * 0.5f, cam.farClipPlane) * cam.worldToCameraMatrix;
        }
    }
    public void UpdateLayerNames()
    {
        for (int index = 0; index < NUMBER_OF_UNITY_LAYERS; index++)
        {
            if (LayerCullInfoArray[index] != null)
            {
                if (!string.IsNullOrEmpty(LayerMask.LayerToName(index)))
                {
                    LayerCullInfoArray[index].LayerName = LayerMask.LayerToName(index);
                }
                else
                {
                    LayerCullInfoArray[index].LayerName = "Layer Not Defined";
                }
            }
            else
            {
                Debug.LogError("Null layer in LayerCullInfoArray. Call ValidateLayerCullInfoArray() first, or make sure there are no null layers.");
            }
        }
    }

    public void ApplyCurrentSettings()
    {
        foreach (LayerCullInfo cullInfo in LayerCullInfoArray)
        {
            if (cullInfo.UseDefaultCullDistance)
            {
                cullInfo.CullDistance = DefaultCullDistance;
            }
        }

        float[] cullFloatArray = new float[NUMBER_OF_UNITY_LAYERS];
        for (int index = 0; index < NUMBER_OF_UNITY_LAYERS; index++)
        {
            cullFloatArray[index] = LayerCullInfoArray[index].CullDistance * DistanceMultiplier;
        }

        cam.layerCullDistances = cullFloatArray;

        cam.layerCullSpherical = UseSphericalCulling;
    }

    [System.Serializable]
    public class LayerCullInfo
    {
        public string LayerName;
        public bool UseDefaultCullDistance = true;
        public float CullDistance;
    }
}