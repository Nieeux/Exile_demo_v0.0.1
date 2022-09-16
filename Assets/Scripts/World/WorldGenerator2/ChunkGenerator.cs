using UnityEngine;
using UnityEngine.Rendering;

public class ChunkGenerator
{
	private const MeshUpdateFlags meshUpdateFlags = MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers;

	private TerrainGenerator terrainGenerator;

	private float chunkSize;

	private Vector2[] verticesCache;

	private int[] trianglesCache;

	private ChunkGenerator.VertexData[] buffer;

	public struct VertexData
	{
		public static VertexAttributeDescriptor[] layout = new VertexAttributeDescriptor[]
		{
				new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, 0),
				new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3, 0),
				new VertexAttributeDescriptor(VertexAttribute.TexCoord1, VertexAttributeFormat.Float32, 4, 0)
		};

		public Vector3 position;

		public Vector3 normal;

		public float road;

		public float biomeA;

		public float biomeB;

		public float biomeBlend;
	}

	public ChunkGenerator(TerrainGenerator terrainGenerator, float chunkSize, int meshResolution)
	{
		this.terrainGenerator = terrainGenerator;
		this.chunkSize = chunkSize;
		this.GenerateCache(meshResolution);
		this.buffer = new ChunkGenerator.VertexData[this.verticesCache.Length];
	}

	private void GenerateCache(int resolution)
	{
		int num = resolution + 1;
		this.verticesCache = new Vector2[num * num];
		float num2 = this.chunkSize / (float)resolution;
		float num3 = this.chunkSize * 0.5f;
		int num4 = 0;
		for (int i = 0; i < num; i++)
		{
			int j = 0;
			while (j < num)
			{
				this.verticesCache[num4] = new Vector2((float)j * num2 - num3, (float)i * num2 - num3);
				j++;
				num4++;
			}
		}
		this.trianglesCache = new int[resolution * resolution * 6];
		int num5 = 0;
		int num6 = 0;
		int k = 0;
		while (k < resolution)
		{
			int l = 0;
			while (l < resolution)
			{
				this.trianglesCache[num5] = num6;
				this.trianglesCache[num5 + 3] = (this.trianglesCache[num5 + 2] = num6 + 1);
				this.trianglesCache[num5 + 4] = (this.trianglesCache[num5 + 1] = num6 + resolution + 1);
				this.trianglesCache[num5 + 5] = num6 + resolution + 2;
				l++;
				num5 += 6;
				num6++;
			}
			k++;
			num6++;
		}
	}

	public void InitChunk(TerrainChunk chunk)
	{
		chunk.mesh.SetVertexBufferParams(this.verticesCache.Length, ChunkGenerator.VertexData.layout);
		chunk.mesh.SetVertexBufferData<ChunkGenerator.VertexData>(this.buffer, 0, 0, this.buffer.Length, 0, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers);
		chunk.mesh.triangles = this.trianglesCache;
	}

	public void Generate(TerrainChunk chunk)
	{
		this.GenerateMeshGeometry(chunk);
		this.UpdateMesh(chunk);
	}

	private void GenerateMeshGeometry(TerrainChunk chunk)
	{
		int num = this.verticesCache.Length;
		Transform transform = chunk.transform;
		for (int i = 0; i < num; i++)
		{
			Vector2 vector = this.verticesCache[i];
			Vector3 position;
			position.x = vector.x;
			position.y = 0f;
			position.z = vector.y;
			Vector3 v = transform.TransformPoint(position);
			TerrainGenerator.TerrainInfo terrainInfo = this.terrainGenerator.Get(v.xz());
			position.y = terrainInfo.height;
			this.buffer[i].position = position;
			this.buffer[i].road = terrainInfo.road;
			this.buffer[i].biomeA = (float)terrainInfo.biomeA.textureIndex;
			this.buffer[i].biomeB = (float)(terrainInfo.biomeB ? terrainInfo.biomeB.textureIndex : 0);
			this.buffer[i].biomeBlend = terrainInfo.biomeBlend;
		}
	}

	public void UpdateMesh(TerrainChunk chunk)
	{
		chunk.mesh.SetVertexBufferData<ChunkGenerator.VertexData>(this.buffer, 0, 0, this.buffer.Length, 0, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers);
		chunk.mesh.RecalculateNormals();
		chunk.mesh.RecalculateBounds();
		chunk.meshCollider.sharedMesh = chunk.mesh;
	}

}

