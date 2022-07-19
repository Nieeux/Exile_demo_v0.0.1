using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatsGenerator : MonoBehaviour
{
    public static ItemStatsGenerator Instance;

    public ItemStats item;

    public int Durability;

    public ItemStats itemChange;

    void Start()
    {
        ItemStatsGenerator.Instance = this;
        ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
        inventoryItem.Getweapon(this.item, this.Durability);
        itemChange = inventoryItem;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
