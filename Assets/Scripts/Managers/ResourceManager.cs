using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceManager : MonoBehaviour
{
	public static int generatorSeedOffset;

	public static ResourceManager Instance;

	public Dictionary<int, GameObject> list;
	public Dictionary<int, GameObject> builds;

	public static int globalId;

	public bool attatchDebug;

	private void Awake()
	{
		ResourceManager.Instance = this;
		this.list = new Dictionary<int, GameObject>();
		this.builds = new Dictionary<int, GameObject>();
		ResourceManager.generatorSeedOffset = 0;
		ResourceManager.globalId = 0;
	}

	public static int GetNextGenOffset()
	{
		int result = ResourceManager.generatorSeedOffset;
		ResourceManager.generatorSeedOffset++;
		return result;
	}

	public void AddResources(List<GameObject>[] trees)
	{
		for (int i = 0; i < trees.Length; i++)
		{
			for (int j = 0; j < trees[i].Count; j++)
			{
				GameObject gameObject = trees[i][j] = trees[i][j];
				int id = gameObject.GetComponentInChildren<SharedId>().GetId();
				this.AddObject(id, gameObject);
			}
		}
	}
	public bool RemoveInteractItem(int id)
	{
		if (!this.list.ContainsKey(id))
		{
			return false;
		}
		Interact componentInChildren = this.list[id].GetComponentInChildren<Interact>();
		this.list.Remove(id);
		componentInChildren.RemoveObject();
		return true;
	}
	public void AddObject(int key, GameObject o)
	{
		if (this.list.ContainsKey(key))
		{
			Debug.Log("Tried to add same key twice to resource manager, returning...");
			return;
		}
		this.list.Add(key, o);
		if (this.attatchDebug)
		{
		}
	}
	public int GetNextId()
	{
		return ResourceManager.globalId++;
	}
}

