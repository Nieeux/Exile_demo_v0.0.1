using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator
{
    public TerrainStats biome;

	public TerrainOverrider overrider;

	private FastNoise noise = new FastNoise(1992);

	public TerrainGenerator.TerrainInfo Get(Vector2 position)
	{
		TerrainGenerator.TerrainInfo result = default(TerrainGenerator.TerrainInfo);
		this.GetBiome(position, ref result);
		this.GetRoad(position, ref result);
		this.GetHeight(position, ref result);
		return result;
	}

	private void GetBiome(Vector2 position, ref TerrainGenerator.TerrainInfo info)
	{
		info.biomeA = this.biome;
		if (this.overrider == null)
		{
			info.biomeBlend = 0f;
			return;
		}
		info.biomeB = this.overrider.biome;
		info.biomeBlend = Vector3.Distance(position, this.overrider.position);
		info.biomeBlend /= this.overrider.radius;
		info.biomeBlend = Mathf.Min(1f, info.biomeBlend);
		info.biomeBlend = 1f - Mathf.Pow(info.biomeBlend, 8f);
	}

	private void GetHeight(Vector2 position, ref TerrainGenerator.TerrainInfo info)
	{
		info.height = this.GetHeight(position, this.biome);
		if (this.overrider != null && info.biomeBlend > 0f)
		{
			float height = this.GetHeight(position, this.overrider.biome);
			info.height = Mathf.Lerp(info.height, height, info.biomeBlend);
		}
		info.height += Mathf.InverseLerp(0.975f, 0.99f, info.road) * info.roadHeight;
	}

	private float GetHeight(Vector2 position, TerrainStats biome)
	{
		float num = 0f;
		for (int i = 0; i < biome.octaves.Length; i++)
		{
			num += this.GetNoiseOctave(position, biome.octaves[i]);
		}
		return num;
	}

	private void GetRoad(Vector2 position, ref TerrainGenerator.TerrainInfo info)
	{
		position.x += 333.33f;
		info.roadHeight = -this.GetNoiseOctave(position, 1f, 2f, true);
		position.x += 333.33f;
		position.x += this.GetNoiseOctave(position, 1f, 100f, false);
		position.x += this.GetNoiseOctave(position, 10f, 3f, false);
		info.road = this.GetNoiseOctave(position, 0.1f, 1f, true);
	}

	private float GetNoiseOctave(Vector2 position, TerrainStats.NoiseOctave octave)
	{
		return this.GetNoiseOctave(position, octave.frequency, octave.amplitude, octave.ridged);
	}


	private float GetNoiseOctave(Vector2 position, float frequency, float amplitude, bool ridged)
	{
		float num = this.noise.GetPerlin(position.x * frequency, position.y * frequency);
		if (ridged)
		{
			num = 1f - Mathf.Abs(num);
		}
		return num * amplitude;
	}

	public struct TerrainInfo
    {

        public TerrainStats biomeA;

        public TerrainStats biomeB;

        public float biomeBlend;

        public float height;

        public float road;

        public float roadHeight;
    }
}
