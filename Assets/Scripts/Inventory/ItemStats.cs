using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System;

[CreateAssetMenu]
public class ItemStats : ScriptableObject
{
	[Header("Main")]
	public ItemStats.ItemType itemType;
	public int id;
	public new string name;
	public string nameViet;
	public string description;


	[Header("Details")]
	public bool stackable = true;
	public int amount;
	public int max;
	public float Weight;

	[Header("Food")]
	public float heal;
	public float hunger;
	public float stamina;

	[Header("Armor")]
	public ItemStats.ArmorType armorType;

	[Header("Gun")]
	public float GunDamage;
	public float fireRate;
	public float bulletsPerShot;
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
	public ItemStats.WeaponType weaponType;
	public ItemStats.ItemRare Rare;
	public List<Buff> buffs = new List<Buff>();
	public List<Buff> Debuffs = new List<Buff>();

	[Header("Weapon Recoil Animation")]
	public float punchStrenght = .2f;
	public int punchVibrato = 5;
	public float punchDuration = .3f;
	public float punchElasticity = .5f;
	public float punchDurationR = .3f;
	public Vector3 PunchR;

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
	public GameObject prefab;

	public string RareIndex;
	public string nameOriginal;
	public string nameUpgrade;
	public string nameAdvanced;

	public Color colorIndex;
	public Color colorOriginal;
	public Color colorUpgrade;
	public Color colorAdvanced;

	public void Getitem(ItemStats item)
	{
		this.itemType = item.itemType;
		this.id = item.id;
		this.name = item.name;
		this.nameViet = item.nameViet;
		this.description = item.description;

		this.stackable = item.stackable;
		this.amount = item.amount;
		this.max = item.max;
		this.Weight = item.Weight;
		this.Rare = item.Rare;
		this.CurrentDurability = item.CurrentDurability;
		this.Durability = item.Durability;
		this.armorType = item.armorType;

		this.sprite = item.sprite;
		this.colorIndex = item.colorIndex;
		this.material = item.material;
		this.mesh = item.mesh;
		this.prefab = item.prefab;
		this.rotationOffset = item.rotationOffset;
		this.positionOffset = item.positionOffset;
		this.scale = item.scale;
	}
	public void GetitemStack(ItemStats item, int amount)
	{
		this.itemType = item.itemType;
		this.id = item.id;
		this.name = item.name;
		this.nameViet = item.nameViet;
		this.description = item.description;

		this.stackable = item.stackable;
		this.amount = amount;
		this.max = item.max;
		this.Weight = item.Weight;
		this.Rare = item.Rare;
		this.CurrentDurability = item.CurrentDurability;
		this.Durability = item.Durability;
		this.armorType = item.armorType;

		this.sprite = item.sprite;
		this.colorIndex = item.colorIndex;
		this.material = item.material;
		this.mesh = item.mesh;
		this.prefab = item.prefab;
		this.rotationOffset = item.rotationOffset;
		this.positionOffset = item.positionOffset;
		this.scale = item.scale;
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
		this.fireRate = item.fireRate;
		this.bulletsPerShot = item.bulletsPerShot;
		this.singleFire = item.singleFire;
		this.Weight = item.Weight;
		this.CurrentDurability = item.CurrentDurability;
		this.Durability = item.Durability;
		this.Magazine = item.Magazine;
		this.CurrentMagazine = item.CurrentMagazine;
		this.ReloadTime = item.ReloadTime;
		this.bulletPrefab = item.bulletPrefab;
		this.AllBulletPrefab = item.AllBulletPrefab;
		item.buffs = new List<Buff>();

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
		this.itemType = item.itemType;
		this.prefab = item.prefab;
		this.rotationOffset = item.rotationOffset;
		this.positionOffset = item.positionOffset;
		this.scale = item.scale;
	}
	public void GetweaponOriginal(ItemStats item)
	{
		//Main
		this.id = item.id;
		this.name = item.name;
		this.nameViet = item.nameViet;
		this.description = item.description;

		//Weapon Stats
		this.GunDamage = item.GunDamage;
		this.fireRate = item.fireRate;
		this.bulletsPerShot = item.bulletsPerShot;
		this.singleFire = item.singleFire;
		this.Weight = item.Weight;
		this.CurrentDurability = item.CurrentDurability;
		this.Durability = item.Durability;
		this.Magazine = item.Magazine;
		this.CurrentMagazine = item.CurrentMagazine;
		this.ReloadTime = item.ReloadTime;
		this.AllBulletPrefab = item.AllBulletPrefab;
		this.weaponType = item.weaponType;
		this.Rare = item.Rare = ItemRare.original;

		//Buff
		item.buffs = new List<Buff>();
		item.Debuffs = new List<Buff>();

		//ItemRare name
		this.nameOriginal = item.nameOriginal;
		this.nameUpgrade = item.nameUpgrade;
		this.nameAdvanced = item.nameAdvanced;
		this.RareIndex = item.nameOriginal;

		//Weapon Recoil Animation
		this.punchStrenght = item.punchStrenght;
		this.punchVibrato = item.punchVibrato;
		this.punchDuration = item.punchDuration;
		this.punchElasticity = item.punchElasticity;
		this.punchDurationR = item.punchDurationR;
		this.PunchR = item.PunchR;

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
		this.itemType = item.itemType;
		this.prefab = item.prefab;

		this.colorOriginal = item.colorOriginal;
		this.colorUpgrade = item.colorUpgrade;
		this.colorAdvanced = item.colorAdvanced;
		this.colorIndex = item.colorOriginal;

		this.rotationOffset = item.rotationOffset;
		this.positionOffset = item.positionOffset;
		this.scale = item.scale;

	}
	public void GetweaponUpgrade(ItemStats item)
	{
		//Main
		this.id = item.id;
		this.name = item.name;
		this.nameViet = item.nameViet;
		this.description = item.description;

		//Weapon Stats
		this.GunDamage = item.GunDamage * 1.5f;
		this.fireRate = item.fireRate;
		this.bulletsPerShot = item.bulletsPerShot;
		this.singleFire = item.singleFire;
		this.Weight = item.Weight;
		this.CurrentDurability = item.CurrentDurability;
		this.Durability = item.Durability;
		this.Magazine = item.Magazine;
		this.CurrentMagazine = item.CurrentMagazine;
		this.ReloadTime = item.ReloadTime;
		this.AllBulletPrefab = item.AllBulletPrefab;
		this.weaponType = item.weaponType;
		this.Rare = item.Rare = ItemRare.upgrade;

		//Buff
		item.buffs = new List<Buff>();
		item.Debuffs = new List<Buff>();

		//ItemRare name
		this.nameOriginal = item.nameOriginal;
		this.nameUpgrade = item.nameUpgrade;
		this.nameAdvanced = item.nameAdvanced;
		this.RareIndex = item.nameUpgrade;

		//Weapon Recoil Animation
		this.punchStrenght = item.punchStrenght;
		this.punchVibrato = item.punchVibrato;
		this.punchDuration = item.punchDuration;
		this.punchElasticity = item.punchElasticity;
		this.punchDurationR = item.punchDurationR;
		this.PunchR = item.PunchR;

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
		this.itemType = item.itemType;
		this.prefab = item.prefab;

		this.colorOriginal = item.colorOriginal;
		this.colorUpgrade = item.colorUpgrade;
		this.colorAdvanced = item.colorAdvanced;
		this.colorIndex = item.colorUpgrade;

		this.rotationOffset = item.rotationOffset;
		this.positionOffset = item.positionOffset;
		this.scale = item.scale;
	}
	public void GetweaponAdvanced(ItemStats item)
	{
		//Main
		this.id = item.id;
		this.name = item.name;
		this.nameViet = item.nameViet;
		this.description = item.description;

		//Weapon Stats
		this.GunDamage = item.GunDamage * 2f;
		this.fireRate = item.fireRate;
		this.bulletsPerShot = item.bulletsPerShot;
		this.singleFire = item.singleFire;
		this.Weight = item.Weight;
		this.CurrentDurability = item.CurrentDurability * 2;
		this.Durability = item.Durability * 2;
		this.Magazine = item.Magazine * 2;
		this.CurrentMagazine = item.CurrentMagazine * 2;
		this.ReloadTime = item.ReloadTime;
		this.AllBulletPrefab = item.AllBulletPrefab;
		this.weaponType = item.weaponType;
		this.Rare = item.Rare = ItemRare.advanced;

		//Buff
		item.buffs = new List<Buff>();
		item.Debuffs = new List<Buff>();

		//ItemRare name
		this.nameOriginal = item.nameOriginal;
		this.nameUpgrade = item.nameUpgrade;
		this.nameAdvanced = item.nameAdvanced;
		this.RareIndex = item.nameAdvanced;

		//Weapon Recoil Animation
		this.punchStrenght = item.punchStrenght;
		this.punchVibrato = item.punchVibrato;
		this.punchDuration = item.punchDuration;
		this.punchElasticity = item.punchElasticity;
		this.punchDurationR = item.punchDurationR;
		this.PunchR = item.PunchR;

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
		this.itemType = item.itemType;
		this.prefab = item.prefab;

		this.colorOriginal = item.colorOriginal;
		this.colorUpgrade = item.colorUpgrade;
		this.colorAdvanced = item.colorAdvanced;
		this.colorIndex = item.colorAdvanced;

		this.rotationOffset = item.rotationOffset;
		this.positionOffset = item.positionOffset;
		this.scale = item.scale;
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
		Armor,
	}
	[Serializable]
	public enum ItemRare
	{
		original,//Nguyên b?n
		upgrade, //Nâng c?p
		advanced,//Tân ti?n
	}
	[Serializable]
	public enum WeaponType
	{
		Null,
		HandGuns,
		Sub_MachineGuns,
		AssaultRifles,
		ShotGuns,
		SniperRifles,
		MachineGuns,
	}
	[Serializable]
	public enum ArmorType
	{
		Null,
		LightArmor,
		NormalArmor,
		HeavyArmor,
	}
}
