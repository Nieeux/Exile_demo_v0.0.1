using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;

public class ItemManager : MonoBehaviour
{
	[Header("Random Weight")]
	public float Original;

	public float Upgrade;

	public float Advanced;

	private float randomly;

	public static ItemManager Instance;
	private GameObject ItemDrop;
	public Dictionary<int, GameObject> list;
	private ItemStats item;

	private Dictionary<int, ItemStats> AllItems;
	private Dictionary<int, ItemStats> AllFoods;
	private Dictionary<int, ItemStats> AllEquipments;
	private Dictionary<int, ItemStats> AllWeapons;
	private Dictionary<int, ItemStats> AllArmors;
	private Dictionary<int, Buff> Allbuffs;
	private Dictionary<int, GameObject> AllBullets;
	private Dictionary<string, int> GetNameEquipments;

	public ItemStats heal;
	public ItemStats[] items;
	public ItemStats[] foods;
	public ItemStats[] equipments;
	public ItemStats[] weapons;
	public ItemStats[] armors;
	public Buff[] buffs;
	public GameObject[] bullets;

	public static int currentId;

	private Random random;

	private void Awake()
	{
		ItemManager.Instance = this;
		this.random = new Random();
		this.AllItems = new Dictionary<int, ItemStats>();
		this.AllFoods = new Dictionary<int, ItemStats>();
		this.AllEquipments = new Dictionary<int, ItemStats>();
		this.AllWeapons = new Dictionary<int, ItemStats>();
		this.AllArmors = new Dictionary<int, ItemStats>();
		this.Allbuffs = new Dictionary<int, Buff>();
		this.GetNameEquipments = new Dictionary<string, int>();
		this.GetAllItems();
		this.GetAllFoods();
		this.GetAllEquipments();
		this.GetAllWeapons();
		this.GetAllArmors();
		this.GetAllBuffs();

	}
	private void GetAllItems()
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			this.items[i].id = i;
			this.AllItems.Add(i, this.items[i]);
		}
	}
	private void GetAllFoods()
	{
		for (int i = 0; i < this.foods.Length; i++)
		{
			this.foods[i].id = i;
			this.AllFoods.Add(i, this.foods[i]);
		}
	}
	private void GetAllEquipments()
	{
		for (int i = 0; i < this.equipments.Length; i++)
		{
			this.equipments[i].id = i;
			this.AllEquipments.Add(i, this.equipments[i]);
		}
	}
	private void GetAllWeapons()
	{
		for (int i = 0; i < this.weapons.Length; i++)
		{
			this.weapons[i].id = i;
			this.AllWeapons.Add(i, this.weapons[i]);
		}
	}
	private void GetAllArmors()
	{
		for (int i = 0; i < this.armors.Length; i++)
		{
			this.armors[i].id = i;
			this.AllArmors.Add(i, this.armors[i]);
		}
	}
	private void GetAllBuffs()
	{
		for (int i = 0; i < this.buffs.Length; i++)
		{
			this.buffs[i].id = i;
			this.Allbuffs.Add(i, this.buffs[i]);
		}
	}

	public ItemStats GetEquipmentsByRare(ItemStats.ItemRare rare)
    {
        foreach (ItemStats inventoryItem in this.AllEquipments.Values)
        {
            if (inventoryItem.Rare == rare)
            {
                return inventoryItem;
            }
        }
        return null;
    }
	public ItemStats GetRandomEquipments(float Original, float Upgrade, float Advanced)
	{
		float num = Original + Upgrade + Advanced;
		float num2 = (float)random.NextDouble();
		if (num2 < Original / num)
		{
			return GetEquipmentsByRare(ItemStats.ItemRare.original);
		}
		if (num2 < (Original + Upgrade) / num)
		{
			return GetEquipmentsByRare(ItemStats.ItemRare.upgrade);

		}
		return GetEquipmentsByRare(ItemStats.ItemRare.advanced);
	}

	public float GetRandomNumber(float Original, float Upgrade, float Advanced)
	{
		float num = Original + Upgrade + Advanced;
		float num2 = (float)random.NextDouble();
		if (num2 < Original / num)
		{
			return randomly = 1;
		}
		if (num2 < (Original + Upgrade) / num)
		{
			return randomly = 2;

		}
		return randomly = 3;
	}
	public void DropWeaponAtPlace(int itemId, Vector3 pos)
	{
		GetRandomNumber(Original, Upgrade, Advanced);
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		if (randomly == 1)
		{
			inventoryItem.GetweaponOriginal(this.AllWeapons[itemId]);
		}
		if (randomly == 2)
		{
			inventoryItem.GetweaponUpgrade(this.AllWeapons[itemId]);
		}
		if (randomly == 3)
		{
			inventoryItem.GetweaponAdvanced(this.AllWeapons[itemId]);
		}

		item = inventoryItem;

		int randombuffamount = UnityEngine.Random.Range(1, 4);
		for (int i = 0; i < randombuffamount; i++)
		{
			item.buffs.Add(GetBuff());
		}

		item.bulletPrefab = GetBullets();

		PickupWeapon pickup = Instantiate(inventoryItem.prefab, pos, Quaternion.identity).GetComponent<PickupWeapon>();
		pickup.item = this.item;
		pickup.GetComponent<WeaponController>().GunStats = this.item;
		pickup.transform.position = pos;

		RaycastHit hit;
		if (Physics.Raycast(pickup.transform.position, Vector3.down, out hit, 10))
		{
			pickup.transform.SetParent(hit.collider.gameObject.transform.parent);
		}
	}

	public void DropWeaponAtVending(int itemId, Vector3 pos, Quaternion orientation, Transform transform)
	{
		GetRandomNumber(Original, Upgrade, Advanced);
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		if (randomly == 1)
		{
			inventoryItem.GetweaponOriginal(this.AllWeapons[itemId]);
		}
		if (randomly == 2)
		{
			inventoryItem.GetweaponUpgrade(this.AllWeapons[itemId]);
		}
		if (randomly == 3)
		{
			inventoryItem.GetweaponAdvanced(this.AllWeapons[itemId]);
		}
		item = inventoryItem;

		int randombuffamount = UnityEngine.Random.Range(1, 4);
		for (int i = 0; i < randombuffamount; i++)
		{
			item.buffs.Add(GetBuff());
		}

		item.bulletPrefab = GetBullets();

		PickupWeapon pickup = Instantiate(inventoryItem.prefab, pos, orientation, transform).GetComponent<PickupWeapon>();
		pickup.item = this.item;
		pickup.GetComponent<WeaponController>().GunStats = this.item;
		pickup.transform.position = pos;
		pickup.GetComponent<Rigidbody>().isKinematic = true;
	}

	public void getWeaponOriginal(int itemId, Vector3 pos, Quaternion orientation, Transform transform)
	{
		GetRandomNumber(Original, Upgrade, Advanced);
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		inventoryItem.GetweaponOriginal(this.AllWeapons[itemId]);
		item = inventoryItem;
		//Buff
		int randombuffamount = UnityEngine.Random.Range(1, 4);
		for (int i = 0; i < randombuffamount; i++)
		{
			item.buffs.Add(GetBuff());
		}
		item.bulletPrefab = GetBullets();

		PickupWeapon pickup = Instantiate(inventoryItem.prefab, pos, orientation, transform).GetComponent<PickupWeapon>();
		pickup.item = this.item;
		pickup.GetComponent<WeaponController>().GunStats = this.item;
		pickup.transform.position = pos;
	}
	public void getWeaponUpgrade(int itemId, Vector3 pos, Quaternion orientation, Transform transform)
	{
		GetRandomNumber(Original, Upgrade, Advanced);
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		inventoryItem.GetweaponUpgrade(this.AllWeapons[itemId]);
		item = inventoryItem;
		//Buff
		int randombuffamount = UnityEngine.Random.Range(1, 4);
		for (int i = 0; i < randombuffamount; i++)
		{
			item.buffs.Add(GetBuff());
		}
		item.bulletPrefab = GetBullets();

		PickupWeapon pickup = Instantiate(inventoryItem.prefab, pos, orientation, transform).GetComponent<PickupWeapon>();
		pickup.item = this.item;
		pickup.GetComponent<WeaponController>().GunStats = this.item;
		pickup.transform.position = pos;
	}
	public void getweaponAdvanced(int itemId, Vector3 pos, Quaternion orientation, Transform transform)
	{
		GetRandomNumber(Original, Upgrade, Advanced);
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		inventoryItem.GetweaponAdvanced(this.AllWeapons[itemId]);
		item = inventoryItem;
		//Buff
		int randombuffamount = UnityEngine.Random.Range(1, 4);
		for (int i = 0; i < randombuffamount; i++)
		{
			item.buffs.Add(GetBuff());
		}
		item.bulletPrefab = GetBullets();

		PickupWeapon pickup = Instantiate(inventoryItem.prefab, pos, orientation, transform).GetComponent<PickupWeapon>();
		pickup.item = this.item;
		pickup.GetComponent<WeaponController>().GunStats = this.item;
		pickup.transform.position = pos;
	}

	public void GetRandomStatsWeapon()
	{
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		if (randomly == 1)
		{
			inventoryItem.GetweaponOriginal(this.item);
		}
		if (randomly == 2)
		{
			inventoryItem.GetweaponUpgrade(this.item);
		}
		if (randomly == 3)
		{
			inventoryItem.GetweaponAdvanced(this.item);
		}
		item = inventoryItem;
	}

	public ItemStats GetHeal()
	{
		return this.heal;
	}
	public ItemStats GetRandomFoods()
	{
		return this.foods[UnityEngine.Random.Range(0, this.foods.Length)];
	}

	public ItemStats GetRandomEquipments()
	{
		return this.equipments[UnityEngine.Random.Range(0, this.equipments.Length)];
	}
	public ItemStats GetRandomWeapons()
	{
		return this.weapons[UnityEngine.Random.Range(0, this.weapons.Length)];
	}

	public ItemStats GetRandomArmor()
	{
		return this.armors[UnityEngine.Random.Range(0, this.armors.Length)];
	}

	public Buff GetBuff()
	{
		return this.buffs[UnityEngine.Random.Range(0, this.buffs.Length)];
	}
	public GameObject GetBullets()
	{
		return this.bullets[UnityEngine.Random.Range(0, this.bullets.Length)];
	}

}
