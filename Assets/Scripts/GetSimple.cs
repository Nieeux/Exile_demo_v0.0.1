using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetSimple
{
	public static Vector2 xz(this Vector3 v)
	{
		return new Vector2(v.x, v.z);
	}
}
