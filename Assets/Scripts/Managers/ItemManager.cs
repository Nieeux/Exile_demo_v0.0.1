using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

	public Dictionary<int, ItemStats> AllItems;
	public Dictionary<int, ItemStats> AllWeapons;
	public Dictionary<int, ItemStats> AllEquipments;
	public Dictionary<string, int> GetNameEquipments;

	public ItemStats[] Items;
	public ItemStats[] Weapons;
	public ItemStats[] Equipments;


	public static int currentId;

	private void Awake()
	{
		ItemManager.Instance = this;
		this.AllItems = new Dictionary<int, ItemStats>();
		this.AllWeapons = new Dictionary<int, ItemStats>();
		this.AllEquipments = new Dictionary<int, ItemStats>();
		this.GetNameEquipments = new Dictionary<string, int>();
		this.GetAllItems();
		this.GetAllEquipments();
		this.GetAllWeapon();

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
	void Start()
    {
        
    }
	public void Drop()
    {

	}
    void Update()
    {
        
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

}
