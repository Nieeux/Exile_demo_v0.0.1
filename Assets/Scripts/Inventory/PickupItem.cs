using System;
using UnityEngine;

public class PickupItem : MonoBehaviour, Interact, SharedId
{

	public ItemStats item;

	public int amount;

	public int id;


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

	public ItemStats GetItem()
    {
		return item;
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