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
			if (PlayerWeaponManager.Instance.AddWeapon(WeaponPrefab, item))
			{
				// Handle auto-switching to weapon if no weapons currently
				if (PlayerWeaponManager.Instance.GetActiveWeapon() == null)
				{
					PlayerWeaponManager.Instance.SwitchWeapon(true);
				}
				//Destroy(this.gameObject);
			}
		}

	}

	public void RemoveObject()
	{
		PlayerWeaponManager.Instance.DropWeapon(WeaponPrefab, item);
		//UnityEngine.Object.Destroy(base.gameObject);
		ResourceManager.Instance.RemoveInteractItem(this.id);
	}

	public string GetName()
	{
		if (item == null)
		{
			return " " + language.MultiLanguage;
		}
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
