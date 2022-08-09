using UnityEngine;

public class Terrain
{
	public event System.Action<Terrain, bool> onVisibilityChanged;
	public Vector2 coord;

	GameObject meshObject;
	GameObject terrian;
	StructureGenerator zoneGenerator;
	GroundGenerator structure;
	PlaceObjects placeObjects;
	Vector2 position;
	Bounds bounds;


	float maxViewDst;
	Transform player;

	public Terrain(Vector2 coord, int size, float maxviewDst, Transform parent, Transform player, Material material)
	{
		position = coord * size;
		this.player = player;
		maxViewDst = maxviewDst;
		bounds = new Bounds(position, Vector2.one * size);
		Vector3 positionV3 = new Vector3(position.x, 0, position.y);

		terrian = new GameObject("Terrain");
		terrian.transform.position = positionV3;
		terrian.transform.parent = parent;
		// gán code PlaceObjects
		this.structure = this.terrian.AddComponent<GroundGenerator>();
		//this.placeObjects = this.terrian.AddComponent<PlaceObjects>();
		this.zoneGenerator = this.terrian.AddComponent<StructureGenerator>();

		// kích ho?t PlaceObjects
		this.structure.Spawn = true;
		//this.placeObjects.Spawn = true;
		this.zoneGenerator.Spawn = true;


		SetVisible(false);
	}

	Vector2 viewerPosition
	{
		get
		{
			return new Vector2(player.position.x, player.position.z);
		}
	}

	public void UpdateTerrainChunk()
	{
		bool wasVisible = IsVisible();

		float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
		bool visible = viewerDstFromNearestEdge <= maxViewDst;

		if (wasVisible != visible)
		{

			SetVisible(visible);
			if (onVisibilityChanged != null)
			{
				onVisibilityChanged(this, visible);
			}
		}
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
