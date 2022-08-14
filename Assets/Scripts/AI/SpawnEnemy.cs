using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using UnityEngine.AI;

public class SpawnEnemy : MonoBehaviour
{
	public static SpawnEnemy Instance;

	public const float maxViewDst = 160;

	private WorldGenerator World;
	GameObject EnemyManager;

    [SerializeField]
	private int maxEnemy = 2;
	private Vector3[] enemyPos;

    [Header("Randomly")]
	public WeightedSpawn[] structurePrefabs;
	public float totalWeight { get; set; }
	protected Random randomly;

	public List<AIController> enemyList;

    private void Awake()
    {
		SpawnEnemy.Instance = this;

	}

	private void Start()
	{
		World = GetComponent<WorldGenerator>();
		EnemyManager = new GameObject("EnemyManager");
		//EnemyManager.transform.parent = transform;

		StartCoroutine(SpawnRoutine());
	}

	void Update()
	{

		
	}
	private IEnumerator SpawnRoutine()
	{
		WaitForSeconds wait = new WaitForSeconds(1f);

		while (true)
		{
			yield return wait;
			if (enemyList.Count < maxEnemy)
			{
				Spawn();
			}
		}
	}

	private void Spawn()
	{
		//this.Enemy = new List<GameObject>();
		this.randomly = new Random();
		this.enemyPos = new Vector3[this.maxEnemy];
		this.CalculateWeight();
		for (int i = 0; i < this.maxEnemy; i++)
		{

			Vector3 position = World.player.transform.position + new Vector3(UnityEngine.Random.Range(-10f, 10f) * 50f, 200f, UnityEngine.Random.Range(-10f, 10f) * 50f);
			Vector3 startPoint = RandomPointAboveTerrain();
			RaycastHit hit;
			Debug.DrawLine(position, position + Vector3.down * 500f, Color.red, 50f);

			if (Physics.Raycast(position, Vector3.down, out hit, 300f) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
			{
				this.enemyPos[i] = hit.point;
				//Quaternion orientation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
				AIController gameObject = this.FindEnemyToSpawn(this.structurePrefabs, this.totalWeight, this.randomly);
				AIController gameObject2 = Instantiate(gameObject, hit.point, gameObject.transform.rotation, EnemyManager.transform);
				//GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, hit.point, gameObject.transform.rotation, transform);
				this.enemyList.Add(gameObject2);

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

	private Vector3 RandomPointAboveTerrain()
	{
		return new Vector3(
			// random diem tao objects

			UnityEngine.Random.Range(World.player.transform.position.x - World.viewedChunk().x / 2, transform.position.x + 200 / 2), transform.position.y + World.viewedChunk().y * 2,
			UnityEngine.Random.Range(transform.position.z - 200 / 2, transform.position.z + 200 / 2));

	}

	public AIController FindEnemyToSpawn(WeightedSpawn[] structurePrefabs, float totalWeight, Random rand)
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


	[System.Serializable]
	public class WeightedSpawn
	{
		public AIController prefab;
		public float weight;
	}
}
