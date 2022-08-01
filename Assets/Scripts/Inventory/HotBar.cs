using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBar : MonoBehaviour
{
	public static HotBar Instance;
	public GameObject TipUseItem;

	public int BarSelect;

	public InventoryCells[] inventoryCells;

	public ItemStats currentItem;
	public Transform CamDrop;


	public Vector3 UnselectedScale = Vector3.one * 0.8f;
    private void Awake()
    {
		HotBar.Instance = this;
	}
    private void Start()
	{
		this.inventoryCells = GetComponentsInChildren<InventoryCells>();
		TipUseItem.SetActive(false);
	}

	private void Update()
	{
        if (!ActiveMenu())
        {
			for (int i = 1; i <= 5; i++)
			{
				if (Input.GetButtonDown("Bar" + i))
				{
					this.BarSelect = i - 1;
					this.UpdateHotbar();
				}
			}
		}
        else
        {
			currentItem = null;
		}

		for (int i = 0; i < this.inventoryCells.Length; i++)
		{
			//dung o inventory da chon && dang select 
			if (i == this.BarSelect && currentItem != null)
			{
				this.inventoryCells[i].Select.color = this.inventoryCells[i].hover;
			}
			else
			{
				this.inventoryCells[i].Select.color = this.inventoryCells[i].idle;
			}
		}

		//Tutorial
		if (currentItem != null)
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
		
		if (this.inventoryCells[this.BarSelect].currentItem != this.currentItem)
		{
			this.currentItem = this.inventoryCells[this.BarSelect].currentItem;
		}

	}
	public void EquipItemUI(ItemStats currentItem)
    {
		if (this.inventoryCells[this.BarSelect].currentItem == currentItem)
		{
			this.inventoryCells[BarSelect].Equip.color = this.inventoryCells[BarSelect].EquipColor;
			this.inventoryCells[BarSelect].EquipItem();

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
		if (this.currentItem.type == ItemStats.ItemType.Food)
		{
			UseItem(1);
			Debug.Log("Eat");
			PlayerStats.Instance.Heal(50);

		}
		if (this.currentItem.type == ItemStats.ItemType.Equipment 
			&& this.inventoryCells[this.BarSelect].equipAble == false)
		{
			InventoryAble.Instance.Equipments(currentItem);
			EquipItemUI(currentItem);

		}
		if (this.currentItem.type == ItemStats.ItemType.Item)
		{

		}
	}

	public void DropItem()
    {
		// Item phai ton tai va item ko dc trang bi
		if (currentItem != null && this.inventoryCells[this.BarSelect].equipAble == false)
        {
			PickupItem pickup = Instantiate(currentItem.prefab, CamDrop.transform.position, Quaternion.identity).GetComponent<PickupItem>();
			pickup.GetComponentInChildren<SharedObject>().SetId(ResourceManager.Instance.GetNextId());
			this.inventoryCells[this.BarSelect].RemoveItem();
			this.UpdateHotbar();
		}
		// Huy trang bi
        if (this.inventoryCells[this.BarSelect].EquipItem())
        {
			InventoryAble.Instance.Unequipments(currentItem);
			this.inventoryCells[this.BarSelect].equipAble = false;
			this.inventoryCells[this.BarSelect].Equip.color = this.inventoryCells[this.BarSelect].idle;
		}

	}
	public void Removeitem()
	{

		if (currentItem != null)
		{
			this.inventoryCells[this.BarSelect].RemoveItem();
			//this.UpdateHotbar();
		}
	}
	public void UseItem(int n)
	{
		this.currentItem.amount -= n;
		if (this.currentItem.amount <= 0)
		{
			this.inventoryCells[this.BarSelect].RemoveItem();
		}
		this.inventoryCells[this.BarSelect].UpdateCell();
	}

	public bool ActiveMenu()
	{
		return Cursor.lockState == CursorLockMode.Locked;
	}
}
