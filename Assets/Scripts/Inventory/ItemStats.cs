using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemStats : ScriptableObject
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

	[Header("Food")]

	public float heal;

	public float hunger;

	public float stamina;

	public ScriptableObject[] BuffFood;

	[Header("Gun")]

	public float GunDamage;

	public float fireRate;

	public float Critical;

	public float CurrentDurability;

	public float Durability;

	public int CurrentMagazine;

	public int Magazine;

	public bool singleFire;

	public float ReloadTime;

	public GameObject bulletPrefab;

	public AudioClip fireAudio;

	public AudioClip reloadAudio;

	public ItemStats.ItemRare Rare;

	//public ItemBuff[] buffs;

	//public Dictionary<StatType, float> stats = new Dictionary<StatType, float>();

	[Header("Visuals")]
	public Sprite sprite;

	public Material material;

	public Mesh mesh;

	public Vector3 rotationOffset;

	public Vector3 positionOffset;

	public float scale = 1f;

	public ItemStats.ItemType type;

	public GameObject prefab;

	[Header("Buff")]

	public ScriptableObject[] BuffWeapon;

	public void Getitem(ItemStats item, int amount)
	{
		this.id = item.id;
		this.name = item.name;
		this.description = item.description;

		this.stackable = item.stackable;
		this.amount = amount;
		this.max = item.max;

		this.sprite = item.sprite;
		this.material = item.material;
		this.mesh = item.mesh;
		this.type = item.type;
		this.prefab = item.prefab;
		this.rotationOffset = item.rotationOffset;
		this.positionOffset = item.positionOffset;
		this.scale = item.scale;
	}
	/*
	public void GetRandom(InventoryItem item)
    {
		buffs = new ItemBuff[item.buffs.Length];
		for (int i = 0; i < buffs.Length; i++)
		{
			item.buffs[i].value = Random.Range(item.buffs[i].min, item.buffs[i].max);
			stats.Add(item.buffs[i].stat, item.buffs[i].value);

		}
		this.stats = item.stats;
	}
	*/
	public void Getweapon(ItemStats item, float amount)
	{
		this.id = item.id;
		this.name = item.name;
		this.description = item.description;

		this.GunDamage = item.GunDamage;
		this.singleFire = item.singleFire;
		this.fireRate = item.fireRate;
		this.Critical = item.Critical;
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
		this.BuffWeapon = item.BuffWeapon;
	}

	public bool Compare(ItemStats other)
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
	public string GetName()
	{
		return this.name.ToString();
	}

	[System.Serializable]
	public enum ItemType
	{
		Item,
		Food,
		Equipment,
		Weapon,
	}
	[System.Serializable]
	public enum ItemRare
	{
		original,//Nguyên b?n
		upgrade, //Nâng c?p
		advanced,//Tân ti?n
	}
	[System.Serializable]
	public enum ItemTag
	{
		None,
		Food,
		Support
	}
	[System.Serializable]
	public enum StatType
	{
		GunDamage,
		fireRate,
		Durability,
	}

	[System.Serializable]
	public class ItemBuff
	{
		public StatType stat;
		public int value;

		public int min;

		public int max;

	}

}
