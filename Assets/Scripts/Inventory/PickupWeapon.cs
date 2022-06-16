using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour, Interactable, SharedObject
{
	public InventoryItem item;

	public int amount;

	public int id;

	private Vector3 defaultScale;

	private Vector3 desiredScale;

	private void Awake()
	{
		this.defaultScale = base.transform.localScale;
		this.desiredScale = this.defaultScale;
	}
	public void Interact()
	{

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
		this.desiredScale = Vector3.Lerp(this.desiredScale, this.defaultScale, Time.deltaTime * 15f);
		this.desiredScale.y = this.defaultScale.y;
		base.transform.localScale = Vector3.Lerp(base.transform.localScale, this.desiredScale, Time.deltaTime * 15f);
	}

	public int GetId()
	{
		return this.id;
	}
}
