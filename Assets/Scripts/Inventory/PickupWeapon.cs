using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PickupWeapon : MonoBehaviour, Interactable, SharedObject
{
	public WeaponController WeaponPrefab;

	public InventoryItem item;

	public int id;


	private void Awake()
	{

	}
    private void Start()
    {
        
    }
    public void Interact()
	{
		InventoryItem inventoryItem = WeaponController.Instance.GunStats;
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
		InventoryItem inventoryItem = WeaponController.Instance.GunStats;
		PlayerWeaponManager.Instance.RemoveWeapon(WeaponPrefab, inventoryItem);
		//UnityEngine.Object.Destroy(base.gameObject);
		ResourceManager.Instance.RemoveInteractItem(this.id);
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
