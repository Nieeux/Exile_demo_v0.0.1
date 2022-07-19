using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

	public Dictionary<int, GameObject> list;
	public Dictionary<int, ItemStats> allItems;

	public GameObject dropItem;
	public GameObject debug;
	public bool attatchDebug;

	public ItemStats[] allScriptableItems;

	public static int currentId;

	private void Awake()
	{
		ItemManager.Instance = this;
		this.list = new Dictionary<int, GameObject>();
		this.allItems = new Dictionary<int, ItemStats>();
		this.InitAllItems();
		this.InitAllDropTables();
	}
	private void InitAllItems()
	{
		for (int i = 0; i < this.allScriptableItems.Length; i++)
		{
			this.allScriptableItems[i].id = i;
			this.allItems.Add(i, this.allScriptableItems[i]);
		}
	}

	private void InitAllDropTables()
	{

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
        foreach (ItemStats inventoryItem in this.allItems.Values)
        {
            if (inventoryItem.name == name)
            {
                return inventoryItem;
            }
        }
        return null;
    }

	public void DropItem(int fromClient, int itemId, int amount, int objectID)
	{

	}
	public bool PickupItem(int objectID)
	{
		UnityEngine.Object.Destroy(this.list[objectID]);
		this.list.Remove(objectID);
		return true;
	}
}
