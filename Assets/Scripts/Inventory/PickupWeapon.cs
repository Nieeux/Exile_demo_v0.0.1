using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PickupWeapon : MonoBehaviour, Interact, SharedId
{
	public WeaponController WeaponPrefab;

	public ItemStats item;

	public multiLanguage language;

	public int id;

    public void Interact()
	{
        if(item != null)
		{
			if (WeaponInventory.Instance.AddWeapon(WeaponPrefab, item))
			{

				if (WeaponInventory.Instance.GetActiveWeapon() == null)
				{
					WeaponInventory.Instance.SwitchWeapon();
				}

			}
		}

	}

	public void RemoveObject()
	{
		WeaponInventory.Instance.DropWeapon(WeaponPrefab, item);
	}

	public string GetName()
	{
		if (item == null)
		{
			return " " + language.VietNamese;
		}
		return "E | " + this.WeaponPrefab.GunStats.nameViet;

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
	/*
    private void OnTriggerEnter(Collider other)
    {
		AIController AIPickup = other.GetComponent<AIController>();
		if(AIPickup != null)
        {
			AIPickup.AiEquip(WeaponPrefab, item);
			Destroy(gameObject);

		}
	}
	*/
}
