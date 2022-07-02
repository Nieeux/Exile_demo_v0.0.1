using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour, Interactable, SharedObject
{
	public WeaponController WeaponPrefab;

	public InventoryItem item;

	public int Durability;

	public int id;


	private void Awake()
	{

	}
	public void Interact()
	{
		InventoryItem inventoryItem = ScriptableObject.CreateInstance<InventoryItem>();
		inventoryItem.GetWeapon(this.item, this.Durability);
		if (PlayerWeaponManager.Instance.AddWeapon(WeaponPrefab, inventoryItem))
		{
			// Handle auto-switching to weapon if no weapons currently
			if (PlayerWeaponManager.Instance.GetActiveWeapon() == null)
			{
				PlayerWeaponManager.Instance.SwitchWeapon(true);
			}
			Destroy(gameObject);
		}
	}

	public void LocalExecute()
	{

	}

	public void AllExecute()
	{

	}

	public void ServerExecute(int fromClient)
	{
	}

	public void RemoveObject()
	{

	}

	public string GetName()
	{
		return "E | " + this.WeaponPrefab.GunStats.name;

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
