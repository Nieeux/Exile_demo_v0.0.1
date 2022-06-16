using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryItem : ScriptableObject
{
	[Header("Main")]

	public bool important;

	public int id;

	public new string name;

	public string description;


	[Header("Inventory details")]
	public bool stackable = true;

	public int amount;

	public int max;

	[Header("Gun")]

	public float GunDamage;

	public bool singleFire;

	public float fireRate;

	public int CurrentMagazine;

	public int Magazine;

	public float ReloadTime;

	public GameObject bulletPrefab;

	[Header("Visuals")]
	public Sprite sprite;


	public Vector3 rotationOffset;

	public Vector3 positionOffset;

	public float scale = 1f;

	public InventoryItem.ItemType type;

	public GameObject prefab;

	public enum ItemType
	{
		Item,
		Food,
		Equipment,
		Gun,
	}
	public void Get(InventoryItem item, int amount)
	{
		this.id = item.id;
		this.name = item.name;
		this.description = item.description;

		this.stackable = item.stackable;
		this.amount = amount;
		this.max = item.max;

		this.singleFire = item.singleFire;
		this.fireRate = item.fireRate;
		this.Magazine = item.Magazine;
		this.ReloadTime = item.ReloadTime;
		this.bulletPrefab = item.bulletPrefab;

		this.sprite = item.sprite;
		this.type = item.type;
		this.prefab = item.prefab;
		this.rotationOffset = item.rotationOffset;
		this.positionOffset = item.positionOffset;
		this.scale = item.scale;
	}
	[System.Serializable]
	public enum ItemTag
	{
		None,
		Food,
		Support
	}
	public bool Compare(InventoryItem other)
	{
		return !(other == null) && this.id == other.id;
	}
	public string GetAmount()
	{
		if (!this.stackable || this.amount == 1)
		{
			return "";
		}
		return this.amount.ToString();
	}
}
