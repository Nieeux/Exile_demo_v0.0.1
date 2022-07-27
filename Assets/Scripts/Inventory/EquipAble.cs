using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipAble : MonoBehaviour
{
	public static EquipAble Instance;
	private int[] increased;
	private float Critcal = 0;
	public List <ItemStats> Item;
	public Dictionary<int, ItemStats> equipments;
	public Dictionary<string, int> GetNameEquipments;
	private void Start()
    {
		EquipAble.Instance = this;

	}
    private void Update()
    {

	}
    public void Equipments(ItemStats Items)
	{
		//this.equipments.Add(Items.id, Items);
		//this.GetNameEquipments.Add(Items.name, Items.id);
		this.Item.Add(Items);
		UpdateEquipmentsModified();
	}
	public void Unequipments(ItemStats Items)
	{
		this.Item.Remove(Items);
		UpdateEquipmentsModified();
	}
	private void UpdateEquipmentsModified()
    {
		Debug.Log("UpdateEquip");
		if (Item.Find((x) => x.name == "khungtroluc"))
		{
			Critcal = 0.5f;
		}
		else
		{
			Critcal = 0;
		}
	}
	public ItemStats GetItemByName(string name)
	{
		foreach (ItemStats inventoryItem in this.Item)
		{
			if (inventoryItem.name == name)
			{
				Debug.Log("Tim ten");
				return inventoryItem;
			}

		}
		return null;
	}

	public float Critical()
	{
		float n = Critcal;
		return 0.1f + n;
	}

}
