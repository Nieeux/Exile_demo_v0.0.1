using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour {

	public const float maxViewDst = 160;
	//240
	public Transform viewer;
	public Material material;

	public static Vector2 viewerPosition;
	int chunksVisibleInViewDst;
	int chunkSize = 40;
	public GameObject Ground;

	public StructureGenerator.WeightedSpawn[] structurePrefabs;

	[SerializeField]
	private Vector3 terrainSize = new Vector3(1, 1, 1);
	public Vector3 TerrainSize { get { return terrainSize; } }
	[SerializeField]
	private GameObject[] placeableObjects;
	public GameObject[] PlaceableObjects { get { return placeableObjects; } }
	[SerializeField]
	private int minObjectsPerTile = 1, maxObjectsPerTile = 2;
	public int MinObjectsPerTile { get { return minObjectsPerTile; } }
	public int MaxObjectsPerTile { get { return maxObjectsPerTile; } }
	[SerializeField]
	private Vector3[] placeableObjectSizes;
	public Vector3[] PlaceableObjectSizes { get { return placeableObjectSizes; } }

	Dictionary<Vector2, terrain> terrainChunkDictionary = new Dictionary<Vector2, terrain>();
	List<terrain> terrainChunksVisibleLastUpdate = new List<terrain>();

	void Start() {
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);

	}

	void Update() {
		viewerPosition = new Vector2 (viewer.position.x, viewer.position.z);
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
				Vector2 viewedChunkCoord = new Vector2 (currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				if (terrainChunkDictionary.ContainsKey (viewedChunkCoord)) {
					terrainChunkDictionary [viewedChunkCoord].UpdateTerrainChunk ();
					if (terrainChunkDictionary [viewedChunkCoord].IsVisible ()) {
						terrainChunksVisibleLastUpdate.Add (terrainChunkDictionary [viewedChunkCoord]);
					}
				} else {

					terrainChunkDictionary.Add (viewedChunkCoord, new terrain (viewedChunkCoord, chunkSize, maxViewDst, transform, viewer, material));

				}

			}
		}
	}
}
