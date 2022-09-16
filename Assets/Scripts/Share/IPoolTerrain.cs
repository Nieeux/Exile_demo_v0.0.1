using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PoolTerrain
{
	string GetPoolName();

	Transform GetTransform();

	void OnPoolGet();

	void OnPoolAdd();
}