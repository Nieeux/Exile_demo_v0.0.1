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

	public float fireRate;

	public int CurrentDurability;

	public float Durability;

	public int CurrentMagazine;

	public int Magazine;

	public bool singleFire;

	public float ReloadTime;

	public GameObject bulletPrefab;

	public AudioClip fireAudio;

	public AudioClip reloadAudio;

	[Header("Visuals")]
	public Sprite sprite;

	public Material material;

	public Mesh mesh;

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
		this.CurrentDurability = item.CurrentDurability;
		this.Durability = item.Durability;
		this.Magazine = item.Magazine;
		this.CurrentMagazine = item.CurrentMagazine;
		this.ReloadTime = item.ReloadTime;
		this.bulletPrefab = item.bulletPrefab;

		this.sprite = item.sprite;
		this.material = item.material;
		this.mesh = item.mesh;
		this.type = item.type;
		this.prefab = item.prefab;
		this.rotationOffset = item.rotationOffset;
		this.positionOffset = item.positionOffset;
		this.scale = item.scale;
	}
	public void GetWeapon(InventoryItem item, int amount)
	{
		this.id = item.id;
		this.name = item.name;
		this.description = item.description;

		this.stackable = item.stackable;
		this.amount = item.amount;
		this.max = item.max;

		this.singleFire = item.singleFire;
		this.fireRate = item.fireRate;
		this.CurrentDurability = amount;
		this.Durability = item.Durability;
		this.Magazine = item.Magazine;
		this.CurrentMagazine = item.CurrentMagazine;
		this.ReloadTime = item.ReloadTime;
		this.bulletPrefab = item.bulletPrefab;

		this.sprite = item.sprite;
		this.material = item.material;
		this.mesh = item.mesh;
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
