using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlaceList : ScriptableObject
{
	public TerrainStructure prefab;

	[Min(0f)]
	public float minScale = 1f;

	[Min(0f)]
	public float maxScale = 1f;

	[Min(0f)]
	public float area = 10000f;

	public float heightOffset;

	public float terrainTextureScale = 1f;

	[Min(0f)]
	public float terrainBlending = 1f;

	[Range(0f, 1f)]
	public float terrainCoverStart;

	[Range(0f, 1f)]
	public float terrainCoverEnd = 1f;

	[Range(0f, 1f)]
	public float terrainSlopeTreshold = 1f;

	public PlaceList.Orientation orientation;

	public enum Orientation
	{
		Identity,
		RandomXYZ,
		RandomY,
		Terrain
	}
}
