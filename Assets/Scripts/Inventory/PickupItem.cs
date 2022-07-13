using System;
using UnityEngine;

public class PickupItem : MonoBehaviour, Interactable, SharedObject
{
	public ItemStats item;

	public int amount;

	public int id;


	private void Awake()
	{
	}

	public void Interact()
	{
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		inventoryItem.Get(this.item, this.amount);
		Inventory.Instance.AddItemToInventory(inventoryItem);
		//ClientSend.PickupInteract(this.id);
		this.RemoveObject();
	}

	public void LocalExecute()
	{
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		inventoryItem.Get(this.item, this.amount);
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
		UnityEngine.Object.Destroy(base.gameObject);
		ResourceManager.Instance.RemoveInteractItem(this.id);
	}

	public string GetName()
	{
		return "E| " + this.item.name;

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