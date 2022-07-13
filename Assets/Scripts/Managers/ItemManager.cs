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
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.dropItem);
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		inventoryItem.Get(this.allItems[itemId], amount);
		Item component = gameObject.GetComponent<Item>();
		component.item = inventoryItem;
		gameObject.AddComponent<BoxCollider>();
		Vector3 position = GameManager.players[fromClient].transform.position;
		Transform transform = GameManager.players[fromClient].transform;
		Vector3 normalized = (transform.forward + Vector3.up * 0.35f).normalized;
		gameObject.transform.position = position;
		gameObject.GetComponent<Rigidbody>().AddForce(normalized * Inventory.throwForce);
		if (this.attatchDebug)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.debug, gameObject.transform);
			gameObject2.GetComponent<DebugObject>().text = string.Concat(objectID);
			gameObject2.transform.localPosition = Vector3.up * 1.25f;
		}
		gameObject.GetComponent<Item>().objectID = objectID;
		this.list.Add(objectID, gameObject);
	}
	public bool PickupItem(int objectID)
	{
		UnityEngine.Object.Destroy(this.list[objectID]);
		this.list.Remove(objectID);
		return true;
	}
}
