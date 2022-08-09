using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;

public class EnemyGenerator: MonoBehaviour
{
	public const float maxViewDst = 160;
	//240
	private WorldGenerator World;

	public static Vector2 viewerPosition;
	int chunksVisibleInViewDst;
	int chunkSize = 240;

	public WeightedSpawn[] structurePrefabs;
	public float totalWeight { get; set; }
	protected Random randomGen;

	Dictionary<Vector2, enemySpawn> terrainChunkDictionary = new Dictionary<Vector2, enemySpawn>();
	List<enemySpawn> terrainChunksVisibleLastUpdate = new List<enemySpawn>();

	void Start()
	{
		World = GetComponent<WorldGenerator>();
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);

	}

	void Update()
	{
		viewerPosition = new Vector2(World.player.position.x, World.player.position.z);
		base.Invoke("UpdateVisibleChunks", 1);
	}

	void UpdateVisibleChunks()
	{

		for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
		{
			terrainChunksVisibleLastUpdate[i].SetVisible(false);
		}
		terrainChunksVisibleLastUpdate.Clear();

		int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
		{
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
			{
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
				{
					terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
					if (terrainChunkDictionary[viewedChunkCoord].IsVisible())
					{
						terrainChunksVisibleLastUpdate.Add(terrainChunkDictionary[viewedChunkCoord]);
					}
				}
				else
				{
					this.randomGen = new Random();
					GameObject gameObject = this.FindObjectToSpawn(this.structurePrefabs, this.totalWeight, this.randomGen);
					terrainChunkDictionary.Add(viewedChunkCoord, new enemySpawn(viewedChunkCoord, chunkSize, gameObject, World.player, maxViewDst));

				}

			}
		}
	}
	public void CalculateWeight()
	{
		this.totalWeight = 0f;
		foreach (WeightedSpawn weightedSpawn in this.structurePrefabs)
		{
			this.totalWeight += weightedSpawn.weight;
		}
	}
	public GameObject FindObjectToSpawn(WeightedSpawn[] structurePrefabs, float totalWeight, Random rand)
	{
		float num = (float)rand.NextDouble();
		float num2 = 0f;
		for (int i = 0; i < structurePrefabs.Length; i++)
		{
			num2 += structurePrefabs[i].weight;
			if (num < num2 / totalWeight)
			{
				return structurePrefabs[i].prefab;
			}
		}
		return structurePrefabs[0].prefab;
	}
}

public class enemySpawn
{
	GameObject enemy;
	Bounds bounds;
	Transform player;
	float maxViewDst;
	Vector2 position;

	public enemySpawn(Vector2 coord, int size, GameObject Enemy,Transform Player,float maxviewDst)
    {
		position = coord * size;
		this.maxViewDst = maxviewDst;
		this.player = Player;
		bounds = new Bounds(position, Vector2.one * size);
		Vector3 positionV3 = new Vector3(position.x, 0, position.y);
		enemy = Object.Instantiate<GameObject>(Enemy);
		enemy.transform.position = positionV3;

	}
	public void SetVisible(bool visible)
	{
		enemy.SetActive(visible);
		enemy.GetComponent<NavmeshGenerator>().Player.Agent.enabled = visible;
		enemy.GetComponent<NavmeshGenerator>().enabled = visible;
		enemy.GetComponent<AIController>().enabled = visible;
	}

	Vector2 viewerPosition
	{
		get
		{
			return new Vector2(player.position.x, player.position.z);
		}
	}

	public bool IsVisible()
	{
		return enemy.activeSelf;
	}
	public void UpdateTerrainChunk()
	{
		float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
		bool visible = viewerDstFromNearestEdge <= maxViewDst;
		SetVisible(visible);
	}
}

[System.Serializable]
public class WeightedSpawn
{
	public GameObject prefab;
	public float weight;
}
