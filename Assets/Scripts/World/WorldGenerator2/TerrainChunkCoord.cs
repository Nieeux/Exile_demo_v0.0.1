using UnityEngine;

public struct TerrainChunkCoord
{

	public TerrainChunkCoord(int x, int z)
	{
		this.x = x;
		this.z = z;
	}

	public TerrainChunkCoord(Vector3 position)
	{
		this.x = Mathf.RoundToInt(position.x);
		this.z = Mathf.RoundToInt(position.z);
	}

	public override int GetHashCode()
	{
		return ((-2128831035 ^ this.x) * 16777619 ^ this.z) * 16777619;
	}

	public int x;

	public int z;
}
