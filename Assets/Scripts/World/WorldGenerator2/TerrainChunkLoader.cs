using System.Collections.Generic;
using UnityEngine;
using System;

public class TerrainChunkLoader
{

	public float ChunkSize { get; private set; }

	public TerrainChunkLoader(Transform transform, float chunkSize, int viewDistance)
	{
		this.transform = transform;
		this.viewDistance = viewDistance;
		this.ChunkSize = chunkSize;
		this.GenerateMatrix();
	}

	private void GenerateMatrix()
	{
		int num = this.viewDistance;
		int num2 = num * 2 + 1;
		int num3 = num2 * num2;
		this.matrix = new Vector2Int[num3];
		int num4 = 0;
		for (int i = -num; i <= num; i++)
		{
			for (int j = -num; j <= num; j++)
			{
				this.matrix[num4++] = new Vector2Int(i, j);
			}
		}
	}

	public void SetObserverPosition(Vector3 observerPosition)
	{
		Matrix4x4 P = Camera.main.projectionMatrix;
		Matrix4x4 V = Camera.main.transform.worldToLocalMatrix;
		Matrix4x4 VP = P * V;

		observerPosition = this.transform.InverseTransformPoint(observerPosition) / this.ChunkSize;
		int num = Mathf.RoundToInt(observerPosition.x);
		int num2 = Mathf.RoundToInt(observerPosition.z);
		foreach (TerrainChunkCoord item in this.chunks)
		{
			this.chunksToUnload.Add(item);
		}
		int num3 = this.matrix.Length;
		for (int i = 0; i < num3; i++)
		{
			TerrainChunkCoord terrainChunkCoord;
			terrainChunkCoord.x = num + this.matrix[i].x;
			terrainChunkCoord.z = num2 + this.matrix[i].y;
			if (!this.chunks.Contains(terrainChunkCoord))
			{
				this.LoadChunk(terrainChunkCoord);
			}
			this.chunksToUnload.Remove(terrainChunkCoord);
		}
		foreach (TerrainChunkCoord c in this.chunksToUnload)
		{
			this.UnloadChunk(c);
		}
		this.chunksToUnload.Clear();
	}

	private void LoadChunk(TerrainChunkCoord c)
	{
		this.chunks.Add(c);
		Action<TerrainChunkCoord, Vector3> action = this.onLoad;
		if (action == null)
		{
			return;
		}
		action(c, new Vector3((float)c.x * this.ChunkSize, 0f, (float)c.z * this.ChunkSize));
	}

	private void UnloadChunk(TerrainChunkCoord c)
	{
		this.chunks.Remove(c);
		Action<TerrainChunkCoord, Vector3> action = this.onUnload;
		if (action == null)
		{
			return;
		}
		action(c, new Vector3((float)c.x * this.ChunkSize, 0f, (float)c.z * this.ChunkSize));
	}

	public Action<TerrainChunkCoord, Vector3> onLoad;

	public Action<TerrainChunkCoord, Vector3> onUnload;

	private Transform transform;

	private Vector2Int[] matrix;

	private HashSet<TerrainChunkCoord> chunks = new HashSet<TerrainChunkCoord>();

	private HashSet<TerrainChunkCoord> chunksToUnload = new HashSet<TerrainChunkCoord>();

	private int viewDistance;
}
