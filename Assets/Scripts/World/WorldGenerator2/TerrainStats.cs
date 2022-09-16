using UnityEngine;
using System;

[CreateAssetMenu]
public class TerrainStats : ScriptableObject
{
    public string label = "Biome";

    public string id = "biome";

    public TerrainStats nextTerrain;

    public TerrainStats.NoiseOctave[] octaves;

    public PlaceList[] props;

    [Min(0f)]
    public int textureIndex;

    [ColorUsage(false, true)]
    public Color bounceLightColor = Color.black;

    public Color dustColor = Color.white;

    [Serializable]
    public class NoiseOctave
    {
        [Min(0f)]
        public float frequency = 1f;

        [Min(0f)]
        public float amplitude = 1f;

        public bool ridged;
    }
}
