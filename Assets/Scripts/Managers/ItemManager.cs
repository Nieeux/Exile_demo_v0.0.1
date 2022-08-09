using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;

public class ItemManager : MonoBehaviour
{
	public GameObject ItemDrop;
	public Dictionary<int, GameObject> list;
	public ItemStats item;

	public static ItemManager Instance;

	public Dictionary<int, ItemStats> AllItems;
	public Dictionary<int, ItemStats> AllWeapons;
	public Dictionary<int, ItemStats> AllEquipments;
	public Dictionary<int, Buff> Allbuffs;
	public Dictionary<string, int> GetNameEquipments;

	public ItemStats[] Items;
	public ItemStats[] Weapons;
	public Buff[] buffs;
	public ItemStats[] Equipments;

	public static int currentId;

	private Random random;

	[Header("Random Weight")]
	public float Original;

	public float Upgrade;

	public float Advanced;

	private float randomly;

	private void Awake()
	{
		this.random = new Random();
		ItemManager.Instance = this;
		this.AllItems = new Dictionary<int, ItemStats>();
		this.AllWeapons = new Dictionary<int, ItemStats>();
		this.AllEquipments = new Dictionary<int, ItemStats>();
		this.Allbuffs = new Dictionary<int, Buff>();
		this.GetNameEquipments = new Dictionary<string, int>();
		this.GetAllItems();
		this.GetAllEquipments();
		this.GetAllWeapon();
		this.GetAllBuffs();

	}
	private void GetAllItems()
	{
		for (int i = 0; i < this.Items.Length; i++)
		{
			this.Items[i].id = i;
			this.AllItems.Add(i, this.Items[i]);
		}
	}

	private void GetAllWeapon()
	{
		for (int i = 0; i < this.Weapons.Length; i++)
		{
			this.Weapons[i].id = i;
			this.AllWeapons.Add(i, this.Weapons[i]);
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
	private void GetAllEquipments()
	{
		for (int i = 0; i < this.Equipments.Length; i++)
		{
			this.Equipments[i].id = i;
			this.AllEquipments.Add(i, this.Equipments[i]);

		}
	}
	private int AddEquipments(ItemStats[] Equipments, int id)
	{
		foreach (ItemStats Equipment in Equipments)
		{
			this.AllEquipments.Add(id, Equipment);
			this.GetNameEquipments.Add(Equipment.name, id);
			Equipment.id = id;
			id++;
		}
		return id;
	}

    public ItemStats GetItemByName(string name)
    {
        foreach (ItemStats inventoryItem in this.AllItems.Values)
        {
            if (inventoryItem.name == name)
            {
                return inventoryItem;
            }
        }
        return null;
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
	public void DropWeaponAtPlace(int itemId, int buffId, Vector3 pos)
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
		item.buffs.Add(Allbuffs[buffId]);
		PickupWeapon pickup = Instantiate(inventoryItem.prefab, pos, Quaternion.identity).GetComponent<PickupWeapon>();
		pickup.item = this.item;
		pickup.GetComponent<WeaponController>().GunStats = this.item;
		pickup.transform.position = pos;
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
		PickupWeapon pickup = Instantiate(inventoryItem.prefab, pos, orientation, transform).GetComponent<PickupWeapon>();
		pickup.item = this.item;
		pickup.GetComponent<WeaponController>().GunStats = this.item;
		pickup.transform.position = pos;
		pickup.GetComponent<Rigidbody>().isKinematic = true;
	}

	public void getWeaponOriginal(int itemId, int buffId, Vector3 pos, Quaternion orientation, Transform transform)
	{
		GetRandomNumber(Original, Upgrade, Advanced);
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		inventoryItem.GetweaponOriginal(this.AllWeapons[itemId]);
		item = inventoryItem;
		item.buffs.Add(Allbuffs[buffId]);
		PickupWeapon pickup = Instantiate(inventoryItem.prefab, pos, orientation, transform).GetComponent<PickupWeapon>();
		pickup.item = this.item;
		pickup.GetComponent<WeaponController>().GunStats = this.item;
		pickup.transform.position = pos;
	}
	public void getWeaponUpgrade(int itemId, int buffId, Vector3 pos, Quaternion orientation, Transform transform)
	{
		GetRandomNumber(Original, Upgrade, Advanced);
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		inventoryItem.GetweaponUpgrade(this.AllWeapons[itemId]);
		item = inventoryItem;
		item.buffs.Add(Allbuffs[buffId]);
		PickupWeapon pickup = Instantiate(inventoryItem.prefab, pos, orientation, transform).GetComponent<PickupWeapon>();
		pickup.item = this.item;
		pickup.GetComponent<WeaponController>().GunStats = this.item;
		pickup.transform.position = pos;
	}
	public void getweaponAdvanced(int itemId, int buffId, Vector3 pos, Quaternion orientation, Transform transform)
	{
		GetRandomNumber(Original, Upgrade, Advanced);
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		inventoryItem.GetweaponAdvanced(this.AllWeapons[itemId]);
		item = inventoryItem;
		item.buffs.Add(Allbuffs[buffId]);
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

	public ItemStats GetRandomWeapons()
	{
		return this.Weapons[UnityEngine.Random.Range(0, this.Weapons.Length)];
	}

	public Buff GetBuff()
	{
		return this.buffs[UnityEngine.Random.Range(0, this.buffs.Length)];
	}

	public ItemStats GetRandomWeapons(float whiteWeight, float blueWeight, float orangeWeight)
	{
		float num = whiteWeight + blueWeight + orangeWeight;
		float num2 = (float)random.NextDouble();
		if (num2 < whiteWeight / num)
		{
			return this.Weapons[UnityEngine.Random.Range(0, this.Weapons.Length)];
		}
		if (num2 < (whiteWeight + blueWeight) / num)
		{
			return this.Weapons[UnityEngine.Random.Range(0, this.Weapons.Length)];
		}
		return this.Weapons[UnityEngine.Random.Range(0, this.Weapons.Length)];
	}
}
