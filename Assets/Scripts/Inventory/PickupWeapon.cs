using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PickupWeapon : MonoBehaviour, Interact
{
	public WeaponController WeaponPrefab;

	public ItemStats item;

	public multiLanguage language;

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
		UnityEngine.Object.Destroy(base.gameObject);
	}
	public void DropObject()
	{
		WeaponInventory.Instance.DropWeapon(WeaponPrefab, item);
	}
	public string GetName()
	{
		if (item == null)
		{
			return " " + language.GetLanguage();
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
}
