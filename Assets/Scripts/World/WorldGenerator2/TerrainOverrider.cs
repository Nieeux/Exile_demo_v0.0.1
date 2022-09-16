using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainOverrider
{
	public TerrainOverrider(TerrainStats biome, Vector2 position, float radius)
	{
		this.biome = biome;
		this.position = position;
		this.radius = radius;
	}

	public TerrainStats biome;
	public Vector2 position;
	public float radius;
}