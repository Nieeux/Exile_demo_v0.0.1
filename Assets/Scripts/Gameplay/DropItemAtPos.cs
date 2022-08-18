using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DropItemAtPos : MonoBehaviour
{
    public enum ItemToDrop
    {
        Item,
        Food,
        Equipment,
        Weapon,
        Armor,
    }
    public ItemStats CurrentItem;

    public ItemToDrop ItemDrop;

    [Header("Rare")]
    public float Original;
    public float Upgrade;
    public float Advanced;

    void Start()
    {
        Dropitem();
    }
    private void Update()
    {
        if (CurrentItem != null)
        {
            //Destroy(gameObject);
        }
    }
    void Dropitem()
    {
        if (ItemDrop == ItemToDrop.Item)
        {

        }
        if (ItemDrop == ItemToDrop.Food)
        {
            ItemStats food = ItemManager.Instance.GetRandomFoods();
            ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
            inventoryItem.Getitem(food);
            PickupItem pickup = Instantiate(inventoryItem.prefab, transform.position, transform.rotation).GetComponent<PickupItem>();
            pickup.item = inventoryItem;
            CurrentItem = inventoryItem;
        }
        if (ItemDrop == ItemToDrop.Equipment)
        {
            ItemStats equip = ItemManager.Instance.GetRandomEquipments();
            ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
            inventoryItem.Getitem(equip);
            PickupItem pickup = Instantiate(inventoryItem.prefab, transform.position, transform.rotation).GetComponent<PickupItem>();
            pickup.item = inventoryItem;
            CurrentItem = inventoryItem;
        }
        if (ItemDrop == ItemToDrop.Weapon)
        {
            ItemStats RandomWeapon = ItemManager.Instance.GetRandomWeapons();
            ItemManager.Instance.DropWeaponAtPlace(RandomWeapon.id, base.transform.position);
            CurrentItem = RandomWeapon;

        }
        if (ItemDrop == ItemToDrop.Armor)
        {
            
            ItemStats armor = ItemManager.Instance.GetRandomArmor();
            ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
            inventoryItem.Getitem(armor);
            PickupItem pickup = Instantiate(inventoryItem.prefab, transform.position, transform.rotation).GetComponent<PickupItem>();
            pickup.item = inventoryItem;
            CurrentItem = inventoryItem;

        }

    }
}
