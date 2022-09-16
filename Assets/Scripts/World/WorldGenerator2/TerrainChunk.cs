using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk : MonoBehaviour, PoolTerrain
{

	public int seed;

	public TerrainStats biome;

	public Mesh mesh;

	public MeshCollider meshCollider;

	public MeshFilter meshFilter;

	[HideInInspector]
	public List<TerrainStructure> props = new List<TerrainStructure>(32);

	private void Awake()
	{
		this.mesh = new Mesh();
		this.mesh.name = "TerrainChunkMesh";
		this.meshFilter.mesh = this.mesh;
	}

    public string GetPoolName()
	{
		return "terrain-chunk";
	}

	public Transform GetTransform()
	{
		return base.transform;
	}

	public void OnPoolGet()
	{
	}

	public void OnPoolAdd()
	{
		this.seed = 0;
	}

}
