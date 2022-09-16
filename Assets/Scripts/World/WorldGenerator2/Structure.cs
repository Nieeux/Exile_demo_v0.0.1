using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Structure : MonoBehaviour
{
	public float totalRare { get; set; }

	private List<GameObject> structures;
	protected Random randomly;
	private Vector3[] shrines;
	public int StructureAmount;

	GameObject terrian;
	private Vector3 terrainSize = new Vector3(64, 1, 64);
	public Vector3 TerrainSize { get { return terrainSize; } }

	public Structure.WeightedSpawn[] Allstructure;

	public bool Spawn;

	void Start()
	{
		Place();
	}

	public void CalculateRare()
	{
		this.totalRare = 0f;
		foreach (WeightedSpawn weightedSpawn in this.Allstructure)
		{
			this.totalRare += weightedSpawn.Rare;
		}
	}
	public void Place()
	{

		this.structures = new List<GameObject>();
		this.randomly = new Random();
		this.shrines = new Vector3[this.StructureAmount];
		this.CalculateRare();
		int num = 0;
		for (int i = 0; i < this.StructureAmount; i++)
		{

            Vector3 startPoint = RandomPointAboveTerrain();
			startPoint.y = 200f;
			//Debug.DrawLine(startPoint, startPoint + Vector3.down * 500f, Color.red, 50f);
			RaycastHit hit;

			if (Physics.Raycast(startPoint, Vector3.down, out hit, 500f) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
			{
				this.shrines[i] = hit.point;
				num++;
				GameObject gameObject = this.FindStructureToSpawn(this.Allstructure, this.totalRare, this.randomly);

				Quaternion orientation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
				Quaternion orientation2 = gameObject.transform.rotation;
				Quaternion orientation3 = orientation;

				GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, hit.point, orientation3, transform);
				//GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, hit.point, gameObject.transform.rotation, transform);
				this.structures.Add(gameObject2);

			}
		}
		Destroy(this);
		//MeshCombine();
	}
	public GameObject FindStructureToSpawn(WeightedSpawn[] structurePrefabs, float totalRare, Random rand)
	{
		float num = (float)rand.NextDouble();
		float num2 = 0f;
		for (int i = 0; i < structurePrefabs.Length; i++)
		{
			num2 += structurePrefabs[i].Rare;
			if (num < num2 / totalRare)
			{
				return structurePrefabs[i].prefab;
			}
		}
		return structurePrefabs[0].prefab;
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

	private void MeshCombine()
    {
		MeshFilter[] meshFilters = terrian.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];

		int i = 0;
		while (i < meshFilters.Length)
		{
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			meshFilters[i].gameObject.SetActive(false);

			i++;
		}
		var meshfilter = terrian.GetComponent<MeshFilter>();
		meshfilter.mesh = new Mesh();
		meshfilter.mesh.CombineMeshes(combine);
		terrian.GetComponent<MeshCollider>().sharedMesh = meshfilter.mesh;
		//transform.GetComponent<MeshFilter>().mesh = new Mesh();
		//transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		terrian.gameObject.SetActive(true);
		//transform.rotation = Quaternion.identity;
		//transform.position = Vector3.zero;

		foreach (Transform child in terrian.transform)
		{
			Destroy(child.gameObject);
		}
	}

	[System.Serializable]
	public class WeightedSpawn
	{
		public GameObject prefab;
		public float Rare;
		public bool Secret;
	}
}

