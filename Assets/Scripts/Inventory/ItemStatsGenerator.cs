using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatsGenerator : MonoBehaviour
{
    public static ItemStatsGenerator Instance;

    public InventoryItem item;

    public int Durability;

    public InventoryItem itemChange;

    void Start()
    {
        ItemStatsGenerator.Instance = this;
        InventoryItem inventoryItem = ScriptableObject.CreateInstance<InventoryItem>();
        inventoryItem.GetWeapon(this.item, this.Durability);
        itemChange = inventoryItem;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
