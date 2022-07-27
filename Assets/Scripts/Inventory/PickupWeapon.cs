using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PickupWeapon : MonoBehaviour, Interactable, SharedObject
{
	public WeaponController WeaponPrefab;

	public ItemStats item;

	public int id;

    public void Interact()
	{
		ItemStats inventoryItem = WeaponController.Instance.GunStats;
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
		ItemStats inventoryItem = WeaponController.Instance.GunStats;
		PlayerWeaponManager.Instance.RemoveWeapon(WeaponPrefab, inventoryItem);
		//UnityEngine.Object.Destroy(base.gameObject);
		ResourceManager.Instance.RemoveInteractItem(this.id);
	}

	public string GetName()
	{
		return "E | " + this.WeaponPrefab.GunStats.nameViet;

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

    private void OnTriggerEnter(Collider other)
    {
		Debug.Log("trigger Ai pickup");
		AIController AIPickup = other.GetComponent<AIController>();
		if(AIPickup != null)
        {
			AIPickup.AiEquip(WeaponPrefab);
			Destroy(gameObject);

		}
	}
}
