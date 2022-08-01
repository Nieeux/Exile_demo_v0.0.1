using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System;

[CreateAssetMenu]
public class ItemStats : ScriptableObject
{
	[Header("Main")]
	public bool important;
	public int id;
	public new string name;
	public string nameViet;
	public string description;

	[Header("Details")]
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
	private GameObject IndexbulletPrefab;
	public GameObject bulletPrefab;
	public GameObject[] AllBulletPrefab;
	public AudioClip fireAudio;
	public AudioClip reloadAudio;

	public ItemStats.ItemRare Rare;

	[Header("Weapon Recoil Animation")]
	public float punchStrenght = .2f;
	public int punchVibrato = 5;
	public float punchDuration = .3f;
	public float punchDurationR = .3f;
	public float punchElasticity = .5f;

	[Header("Weapon Shake")]
	public float randomness = 70;
	public float ShakeDuration = 1;
	public float ShakeStrenght = 1;

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
	public Color colorIndex;
	public Color colorOriginal;
	public Color colorUpgrade;
	public Color colorAdvanced;

	[Header("Buff")]

	public ScriptableObject[] BuffWeapon;

	public void Getitem(ItemStats item, int amount)
	{
		this.id = item.id;
		this.name = item.name;
		this.nameViet = item.nameViet;
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
	public void GetAmmoType(ItemStats item)
	{
		

	}
	public void Getweapon(ItemStats item)
	{
		//Main
		this.id = item.id;
		this.name = item.name;
		this.nameViet = item.nameViet;
		this.description = item.description;

		//Weapon Stats
		this.GunDamage = item.GunDamage;
		this.singleFire = item.singleFire;
		this.fireRate = item.fireRate;
		this.Critical = item.Critical;
		this.CurrentDurability = item.CurrentDurability;
		this.Durability = item.Durability;
		this.Magazine = item.Magazine;
		this.CurrentMagazine = item.CurrentMagazine;
		this.ReloadTime = item.ReloadTime;
		this.bulletPrefab = item.bulletPrefab;
		this.AllBulletPrefab = item.AllBulletPrefab;

		//Weapon Recoil Animation
		this.punchStrenght = item.punchStrenght;
		this.punchVibrato = item.punchVibrato;
		this.punchDuration = item.punchDuration;
		this.punchDurationR = item.punchDurationR;
		this.punchElasticity = item.punchElasticity;

		//Weapon Shake
		this.randomness = item.randomness;
		this.ShakeDuration = item.ShakeDuration;
		this.ShakeStrenght = item.ShakeStrenght;

		//Visuals
		this.sprite = item.sprite;
		this.material = item.material;
		this.mesh = item.mesh;
		this.type = item.type;
		this.prefab = item.prefab;
		this.rotationOffset = item.rotationOffset;
		this.positionOffset = item.positionOffset;
		this.scale = item.scale;

		//Buff
		this.BuffWeapon = item.BuffWeapon;
	}
	public void GetweaponOriginal(ItemStats item)
	{

		this.id = item.id;
		this.name = item.name;
		this.nameViet = item.nameViet;
		this.description = item.description;

		this.GunDamage = item.GunDamage;
		this.singleFire = item.singleFire;
		this.fireRate = item.fireRate;
		this.Critical = item.Critical;
		this.CurrentDurability = item.CurrentDurability;
		this.Durability = item.Durability;
		this.Magazine = item.Magazine;
		this.CurrentMagazine = item.CurrentMagazine;
		this.ReloadTime = item.ReloadTime;
		this.AllBulletPrefab = item.AllBulletPrefab;

		//Weapon Recoil Animation
		this.punchStrenght = item.punchStrenght;
		this.punchVibrato = item.punchVibrato;
		this.punchDuration = item.punchDuration;
		this.punchDurationR = item.punchDurationR;
		this.punchElasticity = item.punchElasticity;

		//Weapon Shake
		this.randomness = item.randomness;
		this.ShakeDuration = item.ShakeDuration;
		this.ShakeStrenght = item.ShakeStrenght;

		//Random Bullet
		int index = UnityEngine.Random.Range(0, item.AllBulletPrefab.Length);
		item.IndexbulletPrefab = item.AllBulletPrefab[index];
		this.bulletPrefab = item.IndexbulletPrefab;

		if(bulletPrefab.GetComponent<Bullet>().ammoType == Bullet.AmmoType.HighAmmo)
        {

        }
		//this.ammoType = GetAmmoType<AmmoType>();
		//this.Rare = GetItemRare<ItemRare>(0.7f,0.2f,0.1f);

		this.sprite = item.sprite;
		this.material = item.material;
		this.mesh = item.mesh;
		this.type = item.type;
		this.prefab = item.prefab;

		this.colorOriginal = item.colorOriginal;
		this.colorUpgrade = item.colorUpgrade;
		this.colorAdvanced = item.colorAdvanced;
		this.colorIndex = item.colorOriginal;

		this.rotationOffset = item.rotationOffset;
		this.positionOffset = item.positionOffset;
		this.scale = item.scale;

		//Buff
		this.BuffWeapon = item.BuffWeapon;
	}
	public void GetweaponUpgrade(ItemStats item)
	{

		this.id = item.id;
		this.name = item.name;
		this.nameViet = item.nameViet;
		this.description = item.description;

		this.GunDamage = item.GunDamage * 1.5f;
		this.singleFire = item.singleFire;
		this.fireRate = item.fireRate;
		this.Critical = item.Critical;
		this.CurrentDurability = item.CurrentDurability;
		this.Durability = item.Durability;
		this.Magazine = item.Magazine;
		this.CurrentMagazine = item.CurrentMagazine;
		this.ReloadTime = item.ReloadTime;
		this.AllBulletPrefab = item.AllBulletPrefab;

		//Weapon Recoil Animation
		this.punchStrenght = item.punchStrenght;
		this.punchVibrato = item.punchVibrato;
		this.punchDuration = item.punchDuration;
		this.punchDurationR = item.punchDurationR;
		this.punchElasticity = item.punchElasticity;

		//Weapon Shake
		this.randomness = item.randomness;
		this.ShakeDuration = item.ShakeDuration;
		this.ShakeStrenght = item.ShakeStrenght;

		int index = UnityEngine.Random.Range(0, item.AllBulletPrefab.Length);
		item.IndexbulletPrefab = item.AllBulletPrefab[index];
		this.bulletPrefab = item.IndexbulletPrefab;

		//this.ammoType = GetAmmoType<AmmoType>();
		//this.Rare = GetItemRare<ItemRare>(0.7f,0.2f,0.1f);

		this.sprite = item.sprite;
		this.material = item.material;
		this.mesh = item.mesh;
		this.type = item.type;
		this.prefab = item.prefab;

		this.colorOriginal = item.colorOriginal;
		this.colorUpgrade = item.colorUpgrade;
		this.colorAdvanced = item.colorAdvanced;
		this.colorIndex = item.colorUpgrade;

		this.rotationOffset = item.rotationOffset;
		this.positionOffset = item.positionOffset;
		this.scale = item.scale;
		this.BuffWeapon = item.BuffWeapon;
	}
	public void GetweaponAdvanced(ItemStats item)
	{

		this.id = item.id;
		this.name = item.name;
		this.nameViet = item.nameViet;
		this.description = item.description;

		this.GunDamage = item.GunDamage * 2f;
		this.singleFire = item.singleFire;
		this.fireRate = item.fireRate;
		this.Critical = item.Critical;
		this.CurrentDurability = item.CurrentDurability;
		this.Durability = item.Durability;
		this.Magazine = item.Magazine;
		this.CurrentMagazine = item.CurrentMagazine;
		this.ReloadTime = item.ReloadTime;
		this.AllBulletPrefab = item.AllBulletPrefab;

		//Weapon Recoil Animation
		this.punchStrenght = item.punchStrenght;
		this.punchVibrato = item.punchVibrato;
		this.punchDuration = item.punchDuration;
		this.punchDurationR = item.punchDurationR;
		this.punchElasticity = item.punchElasticity;

		//Weapon Shake
		this.randomness = item.randomness;
		this.ShakeDuration = item.ShakeDuration;
		this.ShakeStrenght = item.ShakeStrenght;

		int index = UnityEngine.Random.Range(0, item.AllBulletPrefab.Length);
		item.IndexbulletPrefab = item.AllBulletPrefab[index];
		this.bulletPrefab = item.IndexbulletPrefab;

		//this.ammoType = GetAmmoType<AmmoType>();
		//this.Rare = GetItemRare<ItemRare>(0.7f,0.2f,0.1f);

		this.sprite = item.sprite;
		this.material = item.material;
		this.mesh = item.mesh;
		this.type = item.type;
		this.prefab = item.prefab;

		this.colorOriginal = item.colorOriginal;
		this.colorUpgrade = item.colorUpgrade;
		this.colorAdvanced = item.colorAdvanced;
		this.colorIndex = item.colorAdvanced;

		this.rotationOffset = item.rotationOffset;
		this.positionOffset = item.positionOffset;
		this.scale = item.scale;
		this.BuffWeapon = item.BuffWeapon;
	}
	/*
	private static Random random = new Random();
	public static ItemRare GetItemRare<ItemRare>(float whiteWeight, float blueWeight, float orangeWeight)
	{
		//string[] y = Enum.GetNames(typeof(ItemRare));
		Array y = Enum.GetNames(typeof(ItemRare));
		//ItemRare e = ;
		float num = whiteWeight + blueWeight + orangeWeight;
		float num2 = (float)random.NextDouble();
		if (num2 < whiteWeight / num)
		{
			//ItemRare f = (ItemRare)(Enum.GetValues(e.GetType())).GetValue(1);
			//return f;
		}
		if (num2 < (whiteWeight + blueWeight) / num)
		{
			//return (ItemRare)y.GetValue(UnityEngine.Random.Range(0, y.Length));
		}
		return (ItemRare)y.GetValue(UnityEngine.Random.Range(0, y.Length));

	}

	public static AmmoType GetAmmoType<AmmoType>()
	{
		Array A = Enum.GetValues(typeof(AmmoType));
		AmmoType V = (AmmoType)A.GetValue(UnityEngine.Random.Range(0, A.Length));
		return V;

	}
	*/


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
		return this.nameViet.ToString();
	}

	[Serializable]
	public enum ItemType
	{
		Item,
		Food,
		Equipment,
		Weapon,
	}
	[Serializable]
	public enum ItemRare
	{
		original,//Nguyên b?n
		upgrade, //Nâng c?p
		advanced,//Tân ti?n
	}
	[Serializable]
	public enum ItemTag
	{
		None,
		Food,
		Support
	}
	[Serializable]
	public enum StatType
	{
		GunDamage,
		fireRate,
		Durability,
	}

	[Serializable]
	public class ItemBuff
	{
		public StatType stat;
		public int value;

		public int min;

		public int max;

	}

}
