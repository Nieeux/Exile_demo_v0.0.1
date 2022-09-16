using System;
using UnityEngine;

public class TerrainStructure : MonoBehaviour, PoolTerrain
{
	public string GetPoolName()
	{
		return this.pool;
	}

	public Transform GetTransform()
	{
		return this.t;
	}

	public void OnPoolAdd()
	{
		if (this.rb)
		{
			this.rb.Sleep();
		}
	}

	public void OnPoolGet()
	{
		if (this.rb)
		{
			this.rb.Sleep();
		}
	}

	public Transform t;

	public Rigidbody rb;

	public Transform[] snappers;

	public MeshRenderer[] renderers;

	public string pool = "prop";

	public bool canBeDisabled = true;

	public Action onSpawn;

	public Action onDespawn;
}
