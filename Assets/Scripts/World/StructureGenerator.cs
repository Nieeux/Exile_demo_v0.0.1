using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class StructureGenerator : MonoBehaviour
{
	public float totalWeight { get; set; }

	private List<GameObject> structures;
	protected Random randomly;
	private Vector3[] shrines;

	public bool dontAddToResourceManager;

	private Vector3 terrainSize = new Vector3(40, 1, 40);
	public Vector3 TerrainSize { get { return terrainSize; } }

	public WorldGenerator TerrainController;
	//public Randomly randomGen;
	public bool Spawn;

	void Start()
	{
		TerrainController = FindObjectOfType<WorldGenerator>();

		if (this.Spawn)
		{
			Place();
		}
		//Destroy(this);
	}

	public void CalculateWeight()
	{
		this.totalWeight = 0f;
		foreach (WeightedSpawn weightedSpawn in this.TerrainController.terrChoice)
		{
			this.totalWeight += weightedSpawn.weight;
		}
	}
	public void Place()
	{
		this.structures = new List<GameObject>();
		this.randomly = new Random();
		this.shrines = new Vector3[this.TerrainController.StructureAmount];
		this.CalculateWeight();
		int num = 0;
		for (int i = 0; i < this.TerrainController.StructureAmount; i++)
		{

            Vector3 startPoint = RandomPointAboveTerrain();
			startPoint.y = 200f;
			//Debug.DrawLine(startPoint, startPoint + Vector3.down * 500f, Color.red, 50f);
			RaycastHit hit;

			if (Physics.Raycast(startPoint, Vector3.down, out hit, 500f) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
			{
				this.shrines[i] = hit.point;
				num++;
				Quaternion orientation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
				GameObject gameObject = this.FindObjectToSpawn(this.TerrainController.terrChoice, this.totalWeight, this.randomly);
				GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, hit.point, orientation, transform);
				//GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, hit.point, gameObject.transform.rotation, transform);
				this.structures.Add(gameObject2);
				this.Process(gameObject2, hit);
			}
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
	public virtual void Process(GameObject newStructure, RaycastHit hit)
	{
	}

	private Vector3 RandomPointAboveTerrain()
	{
		return new Vector3(
			// random diem tao objects
			UnityEngine.Random.Range(transform.position.x - TerrainSize.x / 2, transform.position.x + TerrainSize.x / 2),
			transform.position.y + TerrainSize.y * 2,
			UnityEngine.Random.Range(transform.position.z - TerrainSize.z / 2, transform.position.z + TerrainSize.z / 2)
		);
	}

	[System.Serializable]
	public class WeightedSpawn
	{
		public GameObject prefab;
		public float weight;
	}
}
