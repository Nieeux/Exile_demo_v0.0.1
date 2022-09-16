using System.Collections.Generic;
using UnityEngine;

public static class Pool
{
	public static void Add(PoolTerrain o)
	{
		if (!Pool.folder)
		{
			Pool.folder = new GameObject("Pool").transform;
			UnityEngine.Object.DontDestroyOnLoad(Pool.folder.gameObject);
		}
		string poolName = o.GetPoolName();
		Queue<PoolTerrain> queue;
		if (!Pool.pools.TryGetValue(poolName, out queue))
		{
			queue = new Queue<PoolTerrain>();
			Pool.pools.Add(poolName, queue);
		}
		Transform transform = o.GetTransform();
		transform.parent = Pool.folder;
		transform.gameObject.SetActive(false);
		queue.Enqueue(o);
		o.OnPoolAdd();
	}
	public static T Get<T>(string poolName, bool activate = true) where T : PoolTerrain
	{
		Queue<PoolTerrain> queue;
		if (!Pool.pools.TryGetValue(poolName, out queue))
		{
			return default(T);
		}
		if (queue.Count == 0)
		{
			return default(T);
		}
		T result = (T)((object)queue.Dequeue());
		Transform transform = result.GetTransform();
		transform.parent = null;
		if (activate)
		{
			transform.gameObject.SetActive(true);
		}
		result.OnPoolGet();
		return result;
	}

	private static Dictionary<string, Queue<PoolTerrain>> pools = new Dictionary<string, Queue<PoolTerrain>>();

	private static Transform folder;
}
