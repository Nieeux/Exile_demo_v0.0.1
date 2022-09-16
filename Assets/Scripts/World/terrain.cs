using UnityEngine;

public class terrain : MonoBehaviour
{
	public event System.Action<terrain, bool> onVisibilityChanged;
	public Vector2 coord;

	GameObject terrian;
	Vector2 position;
	Bounds bounds;

	float maxViewDst;
	Transform player;

	public terrain(Vector2 coord, int size, float maxviewDst, Transform parent, Transform player, GameObject ground, Material material)
	{
		position = coord * size;
		this.player = player;
		maxViewDst = maxviewDst;
		bounds = new Bounds(position, Vector2.one * size);
		Vector3 positionV3 = new Vector3(position.x, 0, position.y);

		terrian = new GameObject("Terrain");
		terrian.transform.position = positionV3;
		terrian.transform.parent = parent;
		Instantiate(ground, terrian.transform);

		terrian.AddComponent<StructureGenerator>();

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
