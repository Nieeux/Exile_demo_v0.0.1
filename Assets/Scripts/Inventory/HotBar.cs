using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBar : MonoBehaviour
{
	public static HotBar Instance;
	public GameObject TipUseItem;

	public int currentActive;
	public bool IsItem;

	public Inventory inventory;
	private InventoryCells[] inventoryCells;

	public ItemStats currentItem;
	public Transform CamDrop;


	public Vector3 UnselectedScale = Vector3.one * 0.8f;

	private void Start()
	{

		HotBar.Instance = this;
		this.inventoryCells = this.inventory.inventoryParent.GetComponentsInChildren<InventoryCells>();
		//this.Bars[this.currentActive].slot.color = this.Bars[this.currentActive].hover;
		//base.Invoke("UpdateHotbar", 1f);
		this.UpdateHotbar();
		TipUseItem.SetActive(false);
	}

	private void Update()
	{
        if (!ActiveMenu())
        {
			for (int i = 1; i <= 5; i++)
			{
				if (Input.GetButtonDown("Hotbar" + i))
				{
					this.currentActive = i - 1;
					this.UpdateHotbar();
				}
			}
			if (currentItem == null)
			{
				IsItem = false;
			}
			else
			{
				IsItem = true;
				
			}
		}
		for (int i = 0; i < this.inventoryCells.Length; i++)
		{
			//dung o inventory da chon && dang select 
			if (i == this.currentActive && IsItem == true)
			{
				this.inventoryCells[i].Select.color = this.inventoryCells[i].hover;
			}
			else
			{
				this.inventoryCells[i].Select.color = this.inventoryCells[i].idle;
			}
		}
        if (ActiveMenu())
        {
			IsItem = false;
		}

		if (IsItem == true)
		{
			TipUseItem.SetActive(true);
		}
        else
        {
			TipUseItem.SetActive(false);
		}
	}

	private void UpdateHotbar()
	{
		if (this.inventoryCells[this.currentActive].currentItem != this.currentItem)
		{
			this.currentItem = this.inventoryCells[this.currentActive].currentItem;
		}
		/*
		for (int j = 0; j < this.Bars.Length; j++)
		{
			this.Bars[j].itemImage.sprite = this.inventoryCells[j].itemImage.sprite;
			this.Bars[j].itemImage.color = this.inventoryCells[j].itemImage.color;
			this.Bars[j].amount.text = this.inventoryCells[j].amount.text;
		}
		*/
	}
	public void EquipItem(ItemStats currentItem)
    {
		for (int i = 0; i < this.inventoryCells.Length; i++)
		{
			if (i == this.currentActive && this.inventoryCells[this.currentActive].currentItem == currentItem)
			{
				this.inventoryCells[i].Equip.color = this.inventoryCells[i].EquipColor;
				this.inventoryCells[i].EquipAble();
				//this.Bars[i].itemImage.color = this.Bars[i].EquipColor;
				//Bars[i].transform.localScale = Vector3.Lerp(transform.localScale, UnselectedScale, Time.deltaTime * 10);

			}

		}
	}
	private void StopUse()
	{
		base.CancelInvoke();
	}

	public void Use()
	{
		if (this.currentItem == null)
		{
			return;
		}
		Debug.Log("UsedItem");
		if (this.currentItem.type == ItemStats.ItemType.Food)
		{
			UseItem(1);
			PlayerStats.Instance.Heal(50);
		}
		if (this.currentItem.type == ItemStats.ItemType.Equipment)
		{
			EquipItem(currentItem);

		}
		if (this.currentItem.type == ItemStats.ItemType.Item)
		{

		}
	}
	public void DropItem()
    {
		// Item phai ton tai va item ko dc trang bi
		if (currentItem != null && this.inventoryCells[this.currentActive].equipAble == false)
        {
			//PickupItem pickup = Instantiate(currentItem.prefab, playerToDrop.DetectCamera.transform.position + (playerToDrop.DetectCamera.transform.forward), Quaternion.identity).GetComponent<PickupItem>();
			PickupItem pickup = Instantiate(currentItem.prefab, CamDrop.transform.position, Quaternion.identity).GetComponent<PickupItem>();
			pickup.GetComponentInChildren<SharedObject>().SetId(ResourceManager.Instance.GetNextId());
			this.inventoryCells[this.currentActive].RemoveItem();
			this.UpdateHotbar();
		}
		// Huy trang bi
        if (this.inventoryCells[this.currentActive].EquipAble())
        {
			this.inventoryCells[this.currentActive].equipAble = false;
			this.inventoryCells[this.currentActive].Equip.color = this.inventoryCells[this.currentActive].idle;
		}

	}
	public void Removeitem()
	{

		if (currentItem != null)
		{
			this.inventoryCells[this.currentActive].RemoveItem();
			this.UpdateHotbar();
		}
	}
	public void UseItem(int n)
	{
		this.currentItem.amount -= n;
		if (this.currentItem.amount <= 0)
		{
			this.inventoryCells[this.currentActive].RemoveItem();
		}
		this.inventoryCells[this.currentActive].UpdateCell();
		this.UpdateHotbar();
	}
	public bool ActiveMenu()
	{
		return Cursor.lockState == CursorLockMode.Locked;
	}
}
