using UnityEngine;

public class Terrainchunk
{

	public Vector2 coord;

	GameObject meshObject;
	StructureGenerator zoneGenerator;
	PlaceObjects placeObjects;
	Vector2 position;
	Bounds bounds;
	Renderer renderer;

	float maxViewDst;
	Transform viewer;

	public Terrainchunk(Vector2 coord, int size, float maxviewDst, Transform parent, Transform viewer, Material material)
	{
		position = coord * size;
		this.viewer = viewer;
		maxViewDst = maxviewDst;
		bounds = new Bounds(position, Vector2.one * size);
		Vector3 positionV3 = new Vector3(position.x, 0, position.y);

		meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
		// gán layer
		meshObject.layer = LayerMask.NameToLayer("Ground");
		meshObject.transform.position = positionV3;
		meshObject.transform.localScale = Vector3.one * size / 10f;
		meshObject.transform.parent = parent;

		//meshObject.isStatic = true;

		renderer = meshObject.GetComponent<Renderer>();
		renderer.material = material;
		// gán code PlaceObjects
		this.zoneGenerator = this.meshObject.AddComponent<StructureGenerator>();
		this.placeObjects = this.meshObject.AddComponent<PlaceObjects>();

		// kích ho?t PlaceObjects
		this.zoneGenerator.Spawn = true;
		this.placeObjects.Spawn = true;

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
		meshObject.SetActive(visible);
	}

	public bool IsVisible()
	{
		return meshObject.activeSelf;
	}

}
