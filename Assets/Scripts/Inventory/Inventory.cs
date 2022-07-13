using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public static Inventory Instance;

	public List<InventoryBar> cells;
	public Transform hotkeysTransform;
	public Transform inventoryParent;

	public static readonly float throwForce = 700f;

	private void Awake()
	{
		Inventory.Instance = this;
	}

	private void Start()
	{
		this.FillCellList();
	}

	//Tim o inventory
	private void FillCellList()
	{
		this.cells = new List<InventoryBar>();
		foreach (InventoryBar item in this.inventoryParent.GetComponentsInChildren<InventoryBar>())
		{
			this.cells.Add(item);
		}
	}
	public bool CanPickup(ItemStats i)
	{
		if (i == null)
		{
			return false;
		}
		int num = i.amount;
		if (this.IsInventoryFull())
		{
			using (List<InventoryBar>.Enumerator enumerator = this.cells.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InventoryBar inventoryCell = enumerator.Current;
					if (inventoryCell != null && inventoryCell.currentItem.id == i.id)
					{
						num -= inventoryCell.currentItem.max - inventoryCell.currentItem.amount;
						if (num <= 0)
						{
							return true;
						}
					}
				}
				return false;
			}
		}
		return true;
	}


	public void UpdateAllCells()
	{
		foreach (InventoryBar inventoryCell in this.cells)
		{
			inventoryCell.UpdateCell();
		}
	}

	public bool pickupCooldown { get; set; }
	public void CooldownPickup()
	{
		this.pickupCooldown = true; ;
	}

	public void CheckInventoryAlmostFull()
	{
		int num = 0;
		using (List<InventoryBar>.Enumerator enumerator = this.cells.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.currentItem == null)
				{
					num++;
					if (num > 2)
					{
						return;
					}
				}
			}
		}
		if (num == 1)
		{
			this.CooldownPickup();
		}
	}

	public bool IsInventoryFull()
	{
		Debug.Log("check full");
		using (List<InventoryBar>.Enumerator enumerator = this.cells.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.currentItem == null)
				{
					return false;
				}
			}
		}
		return true;
	}


	public int AddItemToInventory(ItemStats item)
	{
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		inventoryItem.Get(item, item.amount);
		InventoryBar inventoryCell = null;
		foreach (InventoryBar inventoryCell2 in this.cells)
		{
			if (inventoryCell2.currentItem == null)
			{
				if (!(inventoryCell != null))
				{
					inventoryCell = inventoryCell2;
				}
			}
			else if (inventoryCell2.currentItem.Compare(inventoryItem) && inventoryCell2.currentItem.stackable)
			{
				if (inventoryCell2.currentItem.amount + inventoryItem.amount <= inventoryCell2.currentItem.max)
				{
					inventoryCell2.currentItem.amount += inventoryItem.amount;
					inventoryCell2.UpdateCell();
					return 0;
				}
				int num = inventoryCell2.currentItem.max - inventoryCell2.currentItem.amount;
				inventoryCell2.currentItem.amount += num;
				inventoryItem.amount -= num;
				inventoryCell2.UpdateCell();
			}
		}
		if (inventoryCell)
		{
			inventoryCell.currentItem = inventoryItem;
			inventoryCell.UpdateCell();
			MonoBehaviour.print("added to available cell");
			UIEvents.Instance.AddPickup(inventoryItem);
			return 0;
		}
		UIEvents.Instance.AddPickup(inventoryItem);
		return inventoryItem.amount;
	}


	public void DropItemIntoWorld(ItemStats item)
	{
		if (item == null)
		{
			return;
		}
	}

	public int GetMoney()
	{
		int num = 0;
		foreach (InventoryBar inventoryCell in this.cells)
		{
			if (!(inventoryCell.currentItem == null) && inventoryCell.currentItem.name == "Money")
			{
				num += inventoryCell.currentItem.amount;
			}
		}
		return num;
	}

	public void UseMoney(int amount)
	{
		int num = 0;
		ItemStats itemByName = ItemManager.Instance.GetItemByName("Money");
		foreach (InventoryBar inventoryCell in this.cells)
		{
			if (!(inventoryCell.currentItem == null) && inventoryCell.currentItem.Compare(itemByName))
			{
				if (inventoryCell.currentItem.amount > amount)
				{
					int num2 = amount - num;
					inventoryCell.currentItem.amount -= num2;
					inventoryCell.UpdateCell();
					MonoBehaviour.print("taking money");
					break;
				}
				num += inventoryCell.currentItem.amount;
				MonoBehaviour.print("removing money");
				inventoryCell.RemoveItem();
			}
		}
	}

	public bool HasItem(ItemStats requirement)
	{
		int num = 0;
		foreach (InventoryBar inventoryCell in this.cells)
		{
			if (!(inventoryCell.currentItem == null) && inventoryCell.currentItem.Compare(requirement))
			{
				num += inventoryCell.currentItem.amount;
				if (num >= requirement.amount)
				{
					break;
				}
			}
		}
		return num >= requirement.amount;
	}

	public void RemoveItem(ItemStats requirement)
	{
		int num = 0;
		foreach (InventoryBar inventoryCell in this.cells)
		{
			if (!(inventoryCell.currentItem == null) && inventoryCell.currentItem.Compare(requirement))
			{
				if (inventoryCell.currentItem.amount > requirement.amount)
				{
					int num2 = requirement.amount - num;
					inventoryCell.currentItem.amount -= num2;
					inventoryCell.UpdateCell();
					break;
				}
				Debug.Log("remove");
				num += inventoryCell.currentItem.amount;
				inventoryCell.RemoveItem();
			}
		}
	}
}
