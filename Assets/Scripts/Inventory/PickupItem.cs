using System;
using UnityEngine;

public class PickupItem : MonoBehaviour, Interact
{

	public ItemStats item;

	private void Awake()
	{
	}

	public void Interact()
	{
		if (!Inventory.Instance.IsInventoryFull())
        {
			ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
			inventoryItem.Getitem(this.item);
			Inventory.Instance.AddItemToInventory(inventoryItem);

			//ClientSend.PickupInteract(this.id);
			this.RemoveObject();
		}
        else
        {

        }
	}

	public void RemoveObject()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
	public void DropObject()
	{

	}
	public string GetName()
	{
		return "E | " + this.item.GetName();
	}

	public ItemStats GetItem()
    {
		return item;
    }

	public bool IsStarted()
	{
		return false;
	}

}