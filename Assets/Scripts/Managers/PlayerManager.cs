using System;
using TMPro;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class PlayerManager : MonoBehaviour, IComparable
{
	// Token: 0x1700003F RID: 63
	// (get) Token: 0x060005E6 RID: 1510 RVA: 0x0001F415 File Offset: 0x0001D615
	// (set) Token: 0x060005E7 RID: 1511 RVA: 0x0001F41D File Offset: 0x0001D61D
	public int graveId { get; set; }

	// Token: 0x060005E8 RID: 1512 RVA: 0x0001F426 File Offset: 0x0001D626
	private void Awake()
	{
		this.collide = base.GetComponent<Collider>();
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x0001F440 File Offset: 0x0001D640


	// Token: 0x060005EA RID: 1514 RVA: 0x0001F46B File Offset: 0x0001D66B


	// Token: 0x060005EB RID: 1515 RVA: 0x0001F481 File Offset: 0x0001D681
	public void RemoveGrave()
	{
		if (this.graveId == -1)
		{
			return;
		}
		ResourceManager.Instance.RemoveInteractItem(this.graveId);
		this.graveId = -1;
	}


	// Token: 0x060005ED RID: 1517 RVA: 0x0001F534 File Offset: 0x0001D734
	private void Start()
	{
	}


	public int CompareTo(object obj)
	{
		return 0;
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x0001F5E0 File Offset: 0x0001D7E0
	public Collider GetCollider()
	{
		return this.collide;
	}

	// Token: 0x0400053F RID: 1343
	public int id;

	// Token: 0x04000540 RID: 1344
	public string username;

	// Token: 0x04000541 RID: 1345
	public bool dead;

	// Token: 0x04000542 RID: 1346
	public Color color;

	// Token: 0x04000543 RID: 1347
	public Player player;

	// Token: 0x04000544 RID: 1348
	public int kills;

	// Token: 0x04000545 RID: 1349
	public int deaths;

	// Token: 0x04000546 RID: 1350
	public int ping;

	// Token: 0x04000547 RID: 1351
	public bool disconnected;

	// Token: 0x04000548 RID: 1352
	public bool loaded;

	// Token: 0x0400054A RID: 1354
	public TextMeshProUGUI nameText;


	// Token: 0x0400054C RID: 1356
	private Collider collide;

	// Token: 0x0400054D RID: 1357
	public Transform spectateOrbit;
}
