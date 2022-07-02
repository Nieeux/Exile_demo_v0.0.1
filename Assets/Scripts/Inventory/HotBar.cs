using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBar : MonoBehaviour
{
	public static HotBar Instance;

	private int currentActive;

	public Inventory inventory;
	private InventoryBar[] Bars;
	private InventoryBar[] inventoryBars;
	public InventoryItem currentItem;
	public DetectItem playerToDrop;


	private void Start()
	{
		HotBar.Instance = this;
		this.inventoryBars = this.inventory.hotkeysTransform.GetComponentsInChildren<InventoryBar>();
		this.Bars = base.GetComponentsInChildren<InventoryBar>();
		this.Bars[this.currentActive].slot.color = this.Bars[this.currentActive].hover;
		this.UpdateHotbar();
		base.Invoke("UpdateHotbar", 1f);
	}

	private void Update()
	{
		for (int i = 1; i <= 5; i++)
		{
			if (Input.GetButtonDown("Hotbar" + i))
			{
				this.currentActive = i - 1;
				Debug.Log("da bam");
				this.UpdateHotbar();
				
			}
		}
		
	}
	private IEnumerator TurnOffBar()
    {
		yield return new WaitForSeconds(1f);
		this.Bars[this.currentActive].slot.color = this.Bars[this.currentActive].idle;
		currentItem = null;
		yield break;

	}
	public void UpdateHotbar()
	{
		//StartCoroutine(TurnOffBar());
		if (this.inventoryBars[this.currentActive].currentItem != this.currentItem)
		{
			this.currentItem = this.inventoryBars[this.currentActive].currentItem;

			if (UseBar.Instance)
			{
				//UseBar.Instance.SetWeapon(this.currentItem);
			}

		}
		for (int i = 0; i < this.Bars.Length; i++)
		{
			if (i == this.currentActive)
			{
				this.Bars[i].slot.color = this.Bars[i].hover;
				
			}
			else
			{
				this.Bars[i].slot.color = this.Bars[i].idle;
			}
		}
		for (int j = 0; j < this.Bars.Length; j++)
		{
			this.Bars[j].itemImage.sprite = this.inventoryBars[j].itemImage.sprite;
			this.Bars[j].itemImage.color = this.inventoryBars[j].itemImage.color;
			this.Bars[j].amount.text = this.inventoryBars[j].amount.text;
		}
	}

	public void DropItem()
    {

		if (currentItem != null)
        {
			PickupItem pickup = Instantiate(currentItem.prefab, playerToDrop.DetectCamera.transform.position + (playerToDrop.DetectCamera.transform.forward), Quaternion.identity).GetComponent<PickupItem>();
			pickup.GetComponentInChildren<SharedObject>().SetId(ResourceManager.Instance.GetNextId());
			this.inventoryBars[this.currentActive].RemoveItem();
			this.UpdateHotbar();
		}

	}

	public void UseItem(int n)
	{
		this.currentItem.amount -= n;
		if (this.currentItem.amount <= 0)
		{
			this.inventoryBars[this.currentActive].RemoveItem();
		}
		this.inventoryBars[this.currentActive].UpdateCell();
		this.UpdateHotbar();
	}
}
