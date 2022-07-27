using System;
using UnityEngine;

public class PickupItem : MonoBehaviour, Interactable, SharedObject
{
	public Item ItemPrefab;

	public ItemStats item;

	public int amount;

	public int id;


	private void Awake()
	{
	}

	public void Interact()
	{

		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		inventoryItem.Getitem(this.item, this.amount);
		Inventory.Instance.AddItemToInventory(inventoryItem);
		//ClientSend.PickupInteract(this.id);
		this.RemoveObject();

		/*
		ItemStats inventoryItem = Item.Instance.ItemStats;
		if (InventoryItem.Instance.AddWeapon(ItemPrefab, inventoryItem))
		{
			// Handle auto-switching to weapon if no weapons currently
			if (InventoryItem.Instance.GetActiveItem() == null)
			{
				InventoryItem.Instance.SwitchItem(true);
			}
			Destroy(gameObject);
		}
		*/
	}

	public void LocalExecute()
	{
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		inventoryItem.Getitem(this.item, this.amount);
		Inventory.Instance.AddItemToInventory(inventoryItem);
	}

	public void AllExecute()
	{
		this.RemoveObject();
	}

	public void ServerExecute(int fromClient)
	{
	}

	public void RemoveObject()
	{
		//ItemStats inventoryItem = Item.Instance.ItemStats;
		//InventoryItem.Instance.RemoveWeapon(ItemPrefab, inventoryItem);
		//UnityEngine.Object.Destroy(base.gameObject);
		ResourceManager.Instance.RemoveInteractItem(this.id);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public string GetName()
	{
		return "E | " + this.item.nameViet;

	}

	public bool IsStarted()
	{
		return false;
	}

	public void SetId(int id)
	{
		this.id = id;
	}

	private void Update()
	{

	}

	public int GetId()
	{
		return this.id;
	}
}