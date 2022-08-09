using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;

public class WorldGenerator : MonoBehaviour {

	public const float maxViewDst = 160;
	//240
	public Transform player;
	public Material material;

	protected Random randomly;
	public float totalWeight { get; set; }

	public static Vector2 viewerPosition;
	private Vector2 viewedChunkCoord;
	int chunksVisibleInViewDst;
	int chunkSize = 40;
	public GameObject Ground;

	public int StructureAmount;

	public WeightTerrain terrain;
	public WeightTerrain[] Allstructure;
	public StructureGenerator.WeightedSpawn[] terrChoice;


	//[SerializeField]
	//private Vector3 terrainSize = new Vector3(1, 1, 1);
	//public Vector3 TerrainSize { get { return terrainSize; } }

	//[SerializeField]
	//private GameObject[] placeableObjects;
	//public GameObject[] PlaceableObjects { get { return placeableObjects; } }

	//[SerializeField]
	//private int minObjectsPerTile = 1, maxObjectsPerTile = 2;
	//public int MinObjectsPerTile { get { return minObjectsPerTile; } }
	//public int MaxObjectsPerTile { get { return maxObjectsPerTile; } }

	//[SerializeField]
	//private Vector3[] placeableObjectSizes;
	//public Vector3[] PlaceableObjectSizes { get { return placeableObjectSizes; } }

	Dictionary<Vector2, Terrain> terrainChunkDictionary = new Dictionary<Vector2, Terrain>();
	List<Terrain> terrainChunksVisibleLastUpdate = new List<Terrain>();

	void Start() 
	{
		this.CalculateWeight();
		this.randomly = new Random();
		StructureGenerator.WeightedSpawn[] terr = this.FindObjectToSpawn(this.Allstructure, this.totalWeight, this.randomly);
		terrChoice = terr;
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);

	}

	void FixedUpdate() {
		viewerPosition = new Vector2 (player.position.x, player.position.z);
		UpdateVisibleChunks ();
	}
		
	void UpdateVisibleChunks() {

		for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++) {
			terrainChunksVisibleLastUpdate [i].SetVisible (false);
		}
		terrainChunksVisibleLastUpdate.Clear ();
			
		int currentChunkCoordX = Mathf.RoundToInt (viewerPosition.x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt (viewerPosition.y / chunkSize);

		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++) {
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++) {
				viewedChunkCoord = new Vector2 (currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				if (terrainChunkDictionary.ContainsKey (viewedChunkCoord)) {
					terrainChunkDictionary [viewedChunkCoord].UpdateTerrainChunk ();
					if (terrainChunkDictionary [viewedChunkCoord].IsVisible ()) {
						terrainChunksVisibleLastUpdate.Add (terrainChunkDictionary [viewedChunkCoord]);
					}
				} else {

					Terrain newTerrain = new Terrain(viewedChunkCoord, chunkSize, maxViewDst, transform, player, material);
					terrainChunkDictionary.Add (viewedChunkCoord, newTerrain);
					newTerrain.onVisibilityChanged += OnTerrainChunkVisibilityChanged;

				}

			}
		}
	}
	void OnTerrainChunkVisibilityChanged(Terrain chunk, bool isVisible)
	{
		if (isVisible)
		{
			terrainChunksVisibleLastUpdate.Add(chunk);
		}
		else
		{
			terrainChunksVisibleLastUpdate.Remove(chunk);
		}
	}
	public void CalculateWeight()
	{
		this.totalWeight = 0f;
		foreach (WeightTerrain weightedSpawn in this.Allstructure)
		{
			this.totalWeight += weightedSpawn.weight;
		}
	}
	public StructureGenerator.WeightedSpawn[] FindObjectToSpawn(WeightTerrain[] structurePrefabs, float totalWeight, Random rand)
	{
		float num = (float)rand.NextDouble();
		float num2 = 0f;
		for (int i = 0; i < structurePrefabs.Length; i++)
		{
			num2 += structurePrefabs[i].weight;
			if (num < num2 / totalWeight)
			{
				return structurePrefabs[i].structure;
			}
		}
		return structurePrefabs[0].structure;
	}

	public Vector2 viewedChunk()
    {
		return viewedChunkCoord;

	}
}
[System.Serializable]
public class WeightTerrain
{
	public StructureGenerator.WeightedSpawn[] structure;
	public float weight;

}
