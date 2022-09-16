using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldManager : MonoBehaviour
{

    public TerrainStats Biome
    {
        get
        {
            return this.terrainGenerator.biome;
        }
    }
    public TerrainOverrider Overrider
    {
        get
        {
            return this.terrainGenerator.overrider;
        }
    }
    public bool TransferingBiome
    {
        get
        {
            return this.terrainGenerator.overrider != null;
        }
    }

    [SerializeField]
    private TerrainChunk chunkPrefab;

    public const float ViewDistance = 1024f;

    public const float ChunkSize = 128f;

    public const int ChunkResolution = 32;

    public const float ChunkArea = 16384f;

    private List<TerrainChunk> terrainChunkDictionary = new List<TerrainChunk>(1024);

    private Dictionary<TerrainChunkCoord, TerrainChunk> chunks = new Dictionary<TerrainChunkCoord, TerrainChunk>();

    private TerrainGenerator terrainGenerator;
    private ChunkGenerator chunkGenerator;
    private TerrainChunkLoader chunkLoader;
    private PlaceGenerator propSpawner;

    private void Awake()
    {
        this.terrainGenerator = new TerrainGenerator();
        this.chunkLoader = new TerrainChunkLoader(base.transform, 128f, Mathf.RoundToInt(16f));
        this.chunkGenerator = new ChunkGenerator(this.terrainGenerator, 128f, 32);
        this.propSpawner = new PlaceGenerator(this);
        TerrainChunkLoader terrainChunkLoader = this.chunkLoader;
        terrainChunkLoader.onLoad = (Action<TerrainChunkCoord, Vector3>)Delegate.Combine(terrainChunkLoader.onLoad, new Action<TerrainChunkCoord, Vector3>(this.OnChunkLoad));
        TerrainChunkLoader terrainChunkLoader2 = this.chunkLoader;
        terrainChunkLoader2.onUnload = (Action<TerrainChunkCoord, Vector3>)Delegate.Combine(terrainChunkLoader2.onUnload, new Action<TerrainChunkCoord, Vector3>(this.OnChunkUnload));
        //Global.terrain = this;
        //Shader.SetGlobalTexture(Uniform.TerrainAtlasAlbedoTexture, this.albedoAtlas);
        //Shader.SetGlobalTexture(Uniform.TerrainAtlasNormalTexture, this.normalAtlas);
        //Shader.SetGlobalTexture(Uniform.TerrainNoiseTexture, this.noise);
    }

    void Start()
    {
        this.propSpawner.Init();
    }

    private void Update()
    {
        if (!PlayerMovement.Instance.IsDead)
        {
            this.chunkLoader.SetObserverPosition(PlayerMovement.Instance.transform.position);
            this.SortChunks(PlayerMovement.Instance.transform.position);
        }
        this.UpdateChunks();
        this.UpdateOverriderClearing();
    }
    public void Init(Vector3 position, TerrainStats biome)
    {
        this.terrainGenerator.overrider = null;
        this.terrainGenerator.biome = biome;
        this.chunkLoader.SetObserverPosition(position);
        this.Regenerate();
    }

    public Vector3 Snap(Vector3 position)
    {
        TerrainGenerator.TerrainInfo terrainInfo = this.terrainGenerator.Get(position.xz());
        position.y = terrainInfo.height;
        return position;
    }

    public TerrainGenerator.TerrainInfo Get(Vector3 position)
    {
        return this.terrainGenerator.Get(position.xz());
    }

    public Vector3 GetNormal(Vector3 position)
    {
        Vector3 b = this.Snap(position);
        Vector3 a = this.Snap(position + Vector3.forward);
        Vector3 a2 = this.Snap(position + Vector3.right);
        Vector3 vector = a - b;
        Vector3 vector2 = a2 - b;
        return new Vector3(vector.y * vector2.z - vector.z * vector2.y, vector.z * vector2.x - vector.x * vector2.z, vector.x * vector2.y - vector.y * vector2.x).normalized;
    }

    public Color GetDustColor(Vector3 position)
    {
        TerrainGenerator.TerrainInfo terrainInfo = this.terrainGenerator.Get(position.xz());
        if (terrainInfo.biomeBlend == 0f)
        {
            return terrainInfo.biomeA.dustColor;
        }
        if (terrainInfo.biomeBlend == 1f)
        {
            return terrainInfo.biomeB.dustColor;
        }
        return Color.Lerp(terrainInfo.biomeA.dustColor, terrainInfo.biomeB.dustColor, terrainInfo.biomeBlend);
    }

    private void Generate(TerrainChunk chunk)
    {
        this.chunkGenerator.Generate(chunk);
        chunk.biome = this.GetBiome(chunk.transform.position);
        this.propSpawner.SpawnPropsAsync(chunk);
        chunk.gameObject.SetActive(true);
    }

    public void TransferToBiome(TerrainStats newBiome, Vector3 position)
    {
        if (this.terrainGenerator.overrider == null)
        {
            this.terrainGenerator.overrider = new TerrainOverrider(this.terrainGenerator.biome, position.xz(), 500f);
        }
        else
        {
            this.terrainGenerator.overrider.position = position.xz();
        }
        this.terrainGenerator.biome = newBiome;
        this.Regenerate();
    }

    private void UpdateChunks()
    {
        if (this.terrainChunkDictionary.Count == 0)
        {
            return;
        }
        bool flag = false;
        while (!flag)
        {
            if (this.terrainChunkDictionary.Count == 0)
            {
                return;
            }
            int index = this.terrainChunkDictionary.Count - 1;
            TerrainChunk terrainChunk = this.terrainChunkDictionary[index];
            this.terrainChunkDictionary.RemoveAt(index);
            if (terrainChunk)
            {
                this.Generate(terrainChunk);
                flag = true;
            }
        }
    }
    private void UpdateOverriderClearing()
    {
        if (this.terrainGenerator.overrider == null)
        {
            return;
        }
        if (!PlayerMovement.Instance.IsDead)
        {
            return;
        }
        float distance = this.terrainGenerator.overrider.radius + 300f;
        if (MathLib.DistanceCheck(this.terrainGenerator.overrider.position, PlayerMovement.Instance.transform.position.xz(), distance))
        {
            return;
        }
        this.terrainGenerator.overrider = null;
        this.Regenerate();
    }

    private void Regenerate()
    {
        foreach (TerrainChunk item in this.chunks.Values)
        {
            if (!this.terrainChunkDictionary.Contains(item))
            {
                this.terrainChunkDictionary.Add(item);
            }
        }
    }
    private void SortChunks(Vector3 observerPosition)
    {
        if (this.terrainChunkDictionary.Count < 2)
        {
            return;
        }
        Vector2 p = observerPosition.xz();
        this.terrainChunkDictionary.Sort(delegate (TerrainChunk a, TerrainChunk b)
        {
            if (!a)
            {
                return 0;
            }
            if (!b)
            {
                return 0;
            }
            float sqrMagnitude = (a.transform.position.xz() - p).sqrMagnitude;
            float sqrMagnitude2 = (b.transform.position.xz() - p).sqrMagnitude;
            if (sqrMagnitude == sqrMagnitude2)
            {
                return 0;
            }
            if (sqrMagnitude <= sqrMagnitude2)
            {
                return 1;
            }
            return -1;
        });
    }
    private void OnChunkLoad(TerrainChunkCoord coord, Vector3 position)
    {
        TerrainChunk terrainChunk = this.SpawnChunk(position);
        this.terrainChunkDictionary.Add(terrainChunk);
        this.chunks.Add(coord, terrainChunk);
    }

    private void OnChunkUnload(TerrainChunkCoord coord, Vector3 position)
    {
        TerrainChunk terrainChunk = this.chunks[coord];
        this.propSpawner.DespawnProps(terrainChunk);
        Pool.Add(terrainChunk);
        this.chunks.Remove(coord);
    }

    private TerrainChunk SpawnChunk(Vector3 position)
    {
        TerrainChunk terrainChunk = Pool.Get<TerrainChunk>(this.chunkPrefab.GetPoolName(), false);
        if (terrainChunk)
        {
            terrainChunk.transform.SetPositionAndRotation(position, Quaternion.identity);
            terrainChunk.transform.parent = base.transform;
        }
        else
        {
            terrainChunk = Instantiate<GameObject>(this.chunkPrefab.gameObject, position, Quaternion.identity, base.transform).GetComponent<TerrainChunk>();
            this.chunkGenerator.InitChunk(terrainChunk);
        }

        //Seed the gioi
        terrainChunk.seed = new TerrainChunkCoord(position).GetHashCode();
        terrainChunk.gameObject.SetActive(false);
        return terrainChunk;
    }

    public TerrainStats GetBiome(Vector3 position)
    {
        TerrainGenerator.TerrainInfo terrainInfo = this.terrainGenerator.Get(position.xz());
        if (terrainInfo.biomeBlend < 0.5f)
        {
            return terrainInfo.biomeA;
        }
        return terrainInfo.biomeB;
    }

    private void CullingTerrain()
    {
        Matrix4x4 P = Camera.main.projectionMatrix;
        Matrix4x4 V = Camera.main.transform.worldToLocalMatrix;
        Matrix4x4 VP = P * V;
    }
}
