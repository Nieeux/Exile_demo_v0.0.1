using System.Collections.Generic;
using UnityEngine;

public class StructureSpawner : MonoBehaviour
{

		public float worldScale { get; set; } = 12f;
		public float totalWeight { get; set; }

		public StructureSpawner.WeightedSpawn[] structurePrefabs;
		private List<GameObject> structures;
		protected Randomly randomGen;
		public int nShrines = 50;
		private Vector3[] shrines;
		private int mapChunkSize = 240;

		private float worldEdgeBuffer = 0.6f;

		public LayerMask whatIsTerrain;

		public bool dontAddToResourceManager;

		public void CalculateWeight()
		{
			this.totalWeight = 0f;
			foreach (StructureSpawner.WeightedSpawn weightedSpawn in this.structurePrefabs)
			{
				this.totalWeight += weightedSpawn.weight;
			}
		}
		void Start()
		{
			this.structures = new List<GameObject>();
			this.randomGen = new Randomly();
			//this.randomGen = new Randomly(GameManager.GetSeed() + ResourceManager.GetNextGenOffset());
			this.shrines = new Vector3[this.nShrines];
			//this.mapChunkSize = TerrainGenerator.mapChunkSize;
			this.worldScale *= this.worldEdgeBuffer;
			this.CalculateWeight();
			int num = 0;
			for (int i = 0; i < this.nShrines; i++)
			{
				float x = (float)(this.randomGen.NextDouble() * 2.0 - 1.0) * (float)this.mapChunkSize / 2f;
				float z = (float)(this.randomGen.NextDouble() * 2.0 - 1.0) * (float)this.mapChunkSize / 2f;
				Vector3 vector = new Vector3(x, 0f, z) * this.worldScale;
				vector.y = 200f;
				Debug.DrawLine(vector, vector + Vector3.down * 500f, Color.cyan, 50f);
				RaycastHit hit;
				if (Physics.Raycast(vector, Vector3.down, out hit, 500f, this.whatIsTerrain))
				{
					this.shrines[i] = hit.point;
					num++;
					GameObject gameObject = this.FindObjectToSpawn(this.structurePrefabs, this.totalWeight, this.randomGen);
					GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, hit.point, gameObject.transform.rotation, transform);
					if (!this.dontAddToResourceManager)
					{
						//gameObject2.GetComponentInChildren<SharedObject>().SetId(ResourceManager.Instance.GetNextId());
					}
					this.structures.Add(gameObject2);
					this.Process(gameObject2, hit);
				}
			}
			if (!this.dontAddToResourceManager)
			{
				//ResourceManager.Instance.AddResources(this.structures);
			}
		}

		public virtual void Process(GameObject newStructure, RaycastHit hit)
		{
		}
		public GameObject FindObjectToSpawn(StructureSpawner.WeightedSpawn[] structurePrefabs, float totalWeight, Randomly rand)
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
			public GameObject prefab;
			public float weight;
		}
}

