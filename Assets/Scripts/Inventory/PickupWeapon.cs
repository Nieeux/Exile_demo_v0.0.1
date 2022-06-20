using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour, Interactable, SharedObject
{
	public InventoryItem item;
	public WeaponController WeaponPrefab;

	public int amount;

	public int id;


	private void Awake()
	{

	}
	public void Interact()
	{
		if (PlayerWeaponManager.Instance.AddWeapon(WeaponPrefab))
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
		return "E | " + this.item.name;

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
