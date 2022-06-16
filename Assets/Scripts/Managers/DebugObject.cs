using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class DebugObject : MonoBehaviour
{
	// Token: 0x0600012D RID: 301 RVA: 0x000080D0 File Offset: 0x000062D0
	private void Update()
	{
		if (!base.transform.parent)
		{
			return;
		}
		base.transform.rotation = Quaternion.identity;
		base.transform.position = base.transform.parent.position + this.offset;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00008128 File Offset: 0x00006328
	private void OnGUI()
	{
		if (!Player.Instance)
		{
			return;
		}
		if (!this.cam)
		{
			this.cam = Player.Instance.playerCamera.GetComponentInChildren<Camera>();
			return;
		}
		Vector3 vector = this.cam.WorldToViewportPoint(base.transform.position);
		if (vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f && vector.z > 0f)
		{
			Vector3 vector2 = Camera.main.WorldToScreenPoint(base.gameObject.transform.position);
			if (Vector3.Distance(Player.Instance.transform.position, base.transform.position) > 30f)
			{
				return;
			}
			Vector2 vector3 = GUI.skin.label.CalcSize(new GUIContent(this.text));
			GUI.Label(new Rect(vector2.x, (float)Screen.height - vector2.y, vector3.x, vector3.y), this.text);
		}
	}

	// Token: 0x04000120 RID: 288
	public string text;

	// Token: 0x04000121 RID: 289
	public Vector3 offset = new Vector3(0f, 1.5f, 0f);

	// Token: 0x04000122 RID: 290
	private Camera cam;
}
