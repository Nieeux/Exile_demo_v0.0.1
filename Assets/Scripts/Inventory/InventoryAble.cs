using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAble : MonoBehaviour
{
	public int Moneys { get; set; }

	public static InventoryAble Instance;

	private float Critcal = 0;
	public List <ItemStats> Item;
	public Dictionary<int, ItemStats> equipments;
	public Dictionary<string, int> GetNameEquipments;

	private Vector2 randomDamageRange = new Vector2(0.7f, 1f);

	private void Start()
    {
		InventoryAble.Instance = this;

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

	public DamageResult GetDamage()
	{
		float dmg = Random.Range(randomDamageRange.x, randomDamageRange.y);
		bool ItCrit = Random.Range(0f, 1f) < Critical();

		if (ItCrit)
		{
			dmg *= 2f;
		}
		return new DamageResult(dmg, ItCrit);
	}

	public class DamageResult
	{
		public float damageMultiplier;
		public bool ItCrit;
		public float AmmoPiercing;

		public DamageResult(float damage, bool crit)
		{
			this.damageMultiplier = damage;
			this.ItCrit = crit;

			//this.AmmoPiercing = AmmoPiercing;
		}
	}
	public int GetMoney()
	{
		int num = 0;
		num += Moneys;
		return num;
	}
	public void UseMoney(int Money)
	{
		Moneys -= Money;
		if (Moneys <= 0)
		{
			Moneys = 0;
		}
	}
	public void RewardMoney(int Money)
	{
		Moneys += Money;
	}


}
