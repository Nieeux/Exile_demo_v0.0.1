using System;
using UnityEngine;

// Token: 0x0200005F RID: 95
public class Item : MonoBehaviour
{

	public InventoryItem item { get; set; }

	private void Awake()
	{
		this.outlineMat = base.GetComponent<MeshRenderer>().material;
		base.Invoke("ReadyToPickup", this.pickupDelay);

	}

	private void Start()
	{

	}

	private void FindOutlineColor()
	{

		if (this.item)
		{

		}
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0000DEEC File Offset: 0x0000C0EC
	private void OnTriggerStay(Collider other)
	{
		if (this.pickedUp || !this.readyToPickUp || Inventory.Instance.pickupCooldown)
		{
			return;
		}
		if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
		{
			return;
		}
		if (this.item && !Inventory.Instance.CanPickup(this.item))
		{
			return;
		}
		this.pickedUp = true;
		//ClientSend.PickupItem(this.objectID);
		Inventory.Instance.CheckInventoryAlmostFull();
	}

	private void ReadyToPickup()
	{
		this.readyToPickUp = true;
	}

	private void DespawnItem()
	{
		if (this.item != null && this.item.important)
		{
			return;
		}
		ItemManager.Instance.PickupItem(this.objectID);
		//ServerSend.PickupItem(-1, this.objectID);
	}

	// Token: 0x04000239 RID: 569
	public float pickupDelay = 0.85f;

	// Token: 0x0400023A RID: 570
	public int objectID;

	// Token: 0x0400023D RID: 573
	private bool pickedUp;

	// Token: 0x0400023E RID: 574
	private bool readyToPickUp;

	// Token: 0x0400023F RID: 575
	private Material outlineMat;

	// Token: 0x04000240 RID: 576
	public GameObject powerupParticles;
}
