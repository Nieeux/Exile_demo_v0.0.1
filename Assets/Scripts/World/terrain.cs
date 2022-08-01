using UnityEngine;

public class terrain
{

	public Vector2 coord;

	GameObject meshObject;
	GameObject terrian;
	StructureGenerator zoneGenerator;
	Structure structure;
	PlaceObjects placeObjects;
	Vector2 position;
	Bounds bounds;


	float maxViewDst;
	Transform viewer;

	public terrain(Vector2 coord, int size, float maxviewDst, Transform parent, Transform viewer, Material material)
	{
		position = coord * size;
		this.viewer = viewer;
		maxViewDst = maxviewDst;
		bounds = new Bounds(position, Vector2.one * size);
		Vector3 positionV3 = new Vector3(position.x, 0, position.y);

		terrian = new GameObject("Terrain");
		terrian.transform.position = positionV3;
		terrian.transform.parent = parent;
		// gán code PlaceObjects
		this.structure = this.terrian.AddComponent<Structure>();
		this.placeObjects = this.terrian.AddComponent<PlaceObjects>();
		this.zoneGenerator = this.terrian.AddComponent<StructureGenerator>();

		// kích ho?t PlaceObjects
		this.structure.Spawn = true;
		this.placeObjects.Spawn = true;
		this.zoneGenerator.Spawn = true;


		SetVisible(false);
	}

	Vector2 viewerPosition
	{
		get
		{
			return new Vector2(viewer.position.x, viewer.position.z);
		}
	}

	public void UpdateTerrainChunk()
	{
		float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
		bool visible = viewerDstFromNearestEdge <= maxViewDst;
		SetVisible(visible);
	}



	public void SetVisible(bool visible)
	{
		terrian.SetActive(visible);
	}

	public bool IsVisible()
	{
		return terrian.activeSelf;
	}

}
