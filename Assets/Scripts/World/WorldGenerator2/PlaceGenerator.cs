using System;
using System.Collections;
using UnityEngine;

public class PlaceGenerator
{
	private WorldManager terrain;

	private Randomizer randomizer = new Randomizer();

	private UniqueQueue<TerrainChunk> queue = new UniqueQueue<TerrainChunk>(256);

	public PlaceGenerator(WorldManager terrain)
	{
		this.terrain = terrain;
	}

	public void Init()
	{
		CoroutineManager.Start(this.Update());
	}

	private IEnumerator Update()
	{
		for (; ; )
		{
			yield return null;
			if (this.queue.Count != 0)
			{
				TerrainChunk terrainChunk = this.queue.Dequeue();
				if (terrainChunk.isActiveAndEnabled)
				{
					yield return CoroutineManager.Start(this.SpawnProps(terrainChunk));
				}
			}
		}
		yield break;
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00033292 File Offset: 0x00031492
	public void SpawnPropsAsync(TerrainChunk chunk)
	{
		this.queue.Enqueue(chunk);
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x000332A0 File Offset: 0x000314A0
	private IEnumerator SpawnProps(TerrainChunk chunk)
	{
		this.DespawnProps(chunk);
		this.randomizer.SetSeed(chunk.seed);
		Vector3 position = chunk.transform.position;
		PlaceList[] definitions = chunk.biome.props;
		int definitionsCount = definitions.Length;
		int num;
		for (int di = 0; di < definitionsCount; di = num + 1)
		{
			PlaceList definition = definitions[di];
			float propsCount = 16384f / definition.area;
			if (propsCount < 1f)
			{
				propsCount = (float)((this.randomizer.GetFloat() <= propsCount) ? 1 : 0);
			}
			else
			{
				propsCount = Mathf.Round(propsCount);
			}
			int pi = 0;
			while ((float)pi < propsCount && chunk.isActiveAndEnabled)
			{
				this.SpawnProp(chunk, definition, position);
				yield return null;
				num = pi;
				pi = num + 1;
			}
			num = di;
		}
		yield break;
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x000332B8 File Offset: 0x000314B8
	public void DespawnProps(TerrainChunk chunk)
	{
		int count = chunk.props.Count;
		if (count == 0)
		{
			return;
		}
		for (int i = 0; i < count; i++)
		{
			TerrainStructure prop = chunk.props[i];
			Action onDespawn = prop.onDespawn;
			if (onDespawn != null)
			{
				onDespawn();
			}
			Pool.Add(prop);
		}
		chunk.props.Clear();
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x00033310 File Offset: 0x00031510
	private void SpawnProp(TerrainChunk chunk, PlaceList definition, Vector3 position)
	{
		float num = 64f;
		position.x += this.randomizer.GetFloat(-num, num);
		position.z += this.randomizer.GetFloat(-num, num);
		TerrainGenerator.TerrainInfo terrainInfo = this.terrain.Get(position);
		if (definition.terrainSlopeTreshold < 1f && Vector3.Angle(Vector3.up, this.terrain.GetNormal(position)) > definition.terrainSlopeTreshold * 90f)
		{
			return;
		}
		position.y = terrainInfo.height;
		Quaternion propRotation = this.GetPropRotation(chunk, definition, position);
		position.y += definition.heightOffset;
		TerrainStructure prop = Pool.Get<TerrainStructure>(definition.prefab.pool, true);
		if (prop)
		{
			prop.t.SetPositionAndRotation(position, propRotation);
			prop.t.parent = chunk.transform;
		}
		else
		{
			prop = UnityEngine.Object.Instantiate<GameObject>(definition.prefab.gameObject, position, propRotation, chunk.transform).GetComponent<TerrainStructure>();
		}
		//prop.t.localScale = Vector3.one * this.GetScale(definition);
		Action onSpawn = prop.onSpawn;
		if (onSpawn != null)
		{
			onSpawn();
		}
		//TerrainBlender.Apply(prop.renderers, position, definition.terrainBlending, definition.terrainCoverStart, definition.terrainCoverEnd, definition.terrainTextureScale);
		if (prop.rb)
		{
			prop.rb.Sleep();
		}
		chunk.props.Add(prop);
		if (prop.snappers.Length == 0)
		{
			return;
		}
		int num2 = prop.snappers.Length;
		for (int i = 0; i < num2; i++)
		{
			Transform transform = prop.snappers[i];
			//transform.position = Global.terrain.Snap(transform.position);
		}
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x000334D4 File Offset: 0x000316D4
	private float GetScale(PlaceList definition)
	{
		float num = Mathf.Pow(this.randomizer.GetFloat(), 2f);
		return definition.minScale + num * (definition.maxScale - definition.minScale);
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x00033510 File Offset: 0x00031710
	private Quaternion GetPropRotation(TerrainChunk chunk, PlaceList prop, Vector3 position)
	{
		if (prop.orientation == PlaceList.Orientation.RandomXYZ)
		{
			return this.randomizer.GetRotation();
		}
		if (prop.orientation == PlaceList.Orientation.RandomY)
		{
			return Quaternion.AngleAxis(this.randomizer.GetFloat(0f, 360f), Vector3.up);
		}
		if (prop.orientation == PlaceList.Orientation.Terrain)
		{
			Vector3 normal = this.terrain.GetNormal(position);
			return Quaternion.LookRotation(Vector3.Cross(this.randomizer.GetPositionOnSphere(), normal), normal);
		}
		return Quaternion.identity;
	}
}
