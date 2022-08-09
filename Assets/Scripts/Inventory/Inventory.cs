using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Inventory")]
	public int BarSelect;
	public Transform CamDrop;
	public ItemStats currentItem;
	public Transform inventoryBar;
	public List<InventoryCells> inventoryCells;
	//public InventoryCells[] inventoryCells;

	public int Moneys { get; set; }

	[Header("Equipments")]
	public List<ItemStats> Item;
	private float critcal = 0;
	private float speed = 0;
	public Dictionary<int, ItemStats> equipments;
	public Dictionary<string, int> GetNameEquipments;

    [Header("Tutorial")]
	public GameObject TipUseItem;

	public static Inventory Instance;

	private Vector2 randomDamageRange = new Vector2(0.7f, 1f);

    private void Awake()
    {
		Inventory.Instance = this;
	}

    private void Start()
    {
		FillCellList();
		//this.inventoryCells = inventoryBar.GetComponentsInChildren<InventoryCells>();
		TipUseItem.SetActive(false);
	}

    private void Update()
    {
		if (!ActiveMenu())
		{
			for (int i = 1; i <= 5; i++)
			{
				if (Input.GetButtonDown("Bar" + i))
				{
					this.BarSelect = i - 1;
					this.UpdateHotbar();
				}
			}
		}
		else
		{
			currentItem = null;
		}

		for (int i = 0; i < this.inventoryCells.Count; i++)
		{
			//dung o inventory da chon && dang select 
			if (i == this.BarSelect && currentItem != null)
			{
				this.inventoryCells[i].Select.color = this.inventoryCells[i].hover;
			}
			else
			{
				this.inventoryCells[i].Select.color = this.inventoryCells[i].idle;
			}
		}

		//Tutorial
		if (currentItem != null)
		{
			TipUseItem.SetActive(true);
		}
		else
		{
			TipUseItem.SetActive(false);
		}
	}

	private void UpdateHotbar()
	{

		if (this.inventoryCells[this.BarSelect].currentItem != this.currentItem)
		{
			this.currentItem = this.inventoryCells[this.BarSelect].currentItem;
		}

	}
	//Tim o inventory
	private void FillCellList()
	{
		this.inventoryCells = new List<InventoryCells>();
		foreach (InventoryCells item in this.inventoryBar.GetComponentsInChildren<InventoryCells>())
		{
			this.inventoryCells.Add(item);
		}
	}

	public void UpdateAllCells()
	{
		foreach (InventoryCells inventoryCell in this.inventoryCells)
		{
			inventoryCell.UpdateCell();
		}
	}
	public int AddItemToInventory(ItemStats item)
	{
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
		inventoryItem.Getitem(item, item.amount);
		InventoryCells inventoryCell = null;
		foreach (InventoryCells inventoryCell2 in this.inventoryCells)
		{
			if (inventoryCell2.currentItem == null)
			{
				if (!(inventoryCell != null))
				{
					inventoryCell = inventoryCell2;
				}
			}
			else if (inventoryCell2.currentItem.Compare(inventoryItem) && inventoryCell2.currentItem.stackable)
			{
				if (inventoryCell2.currentItem.amount + inventoryItem.amount <= inventoryCell2.currentItem.max)
				{
					inventoryCell2.currentItem.amount += inventoryItem.amount;
					inventoryCell2.UpdateCell();
					return 0;
				}
				int num = inventoryCell2.currentItem.max - inventoryCell2.currentItem.amount;
				inventoryCell2.currentItem.amount += num;
				inventoryItem.amount -= num;
				inventoryCell2.UpdateCell();
			}
		}
		if (inventoryCell)
		{
			inventoryCell.currentItem = inventoryItem;
			inventoryCell.UpdateCell();
			UIEvents.Instance.AddPickup(inventoryItem);
			return 0;
		}
		UIEvents.Instance.AddPickup(inventoryItem);
		return inventoryItem.amount;
	}


	public bool IsInventoryFull()
	{
		using (List<InventoryCells>.Enumerator enumerator = this.inventoryCells.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.currentItem == null)
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool HasItem(ItemStats requirement)
	{
		int num = 0;
		foreach (InventoryCells inventoryCell in this.inventoryCells)
		{
			if (!(inventoryCell.currentItem == null) && inventoryCell.currentItem.Compare(requirement))
			{
				num += inventoryCell.currentItem.amount;
				if (num >= requirement.amount)
				{
					break;
				}
			}
		}
		return num >= requirement.amount;
	}

	public void Use()
	{
		if (this.currentItem == null)
		{
			return;
		}
		if (this.currentItem.type == ItemStats.ItemType.Food)
		{
			UseFood(1);

			PlayerStats.Instance.Heal(50);

		}
		if (this.currentItem.type == ItemStats.ItemType.Equipment
			&& this.inventoryCells[this.BarSelect].equipAble == false)
		{
			EquipItemUI(currentItem);
			UpdateEquipmentsModified(currentItem);

		}
		if (this.currentItem.type == ItemStats.ItemType.Item)
		{

		}
	}

	public void EquipItemUI(ItemStats currentItem)
	{
		if (this.inventoryCells[this.BarSelect].currentItem == currentItem)
		{
			this.inventoryCells[BarSelect].Equip.color = this.inventoryCells[BarSelect].EquipColor;
			this.inventoryCells[BarSelect].EquipItem();

		}
	}

	public void DropItem()
	{
		if (this.currentItem == null)
		{
			return;
		}

		// Nem trang bi
		if (this.inventoryCells[this.BarSelect].equipAble == false)
		{
			PickupItem pickup = Instantiate(currentItem.prefab, CamDrop.transform.position, Quaternion.identity).GetComponent<PickupItem>();
			pickup.GetComponentInChildren<SharedId>().SetId(ResourceManager.Instance.GetNextId());
			this.inventoryCells[this.BarSelect].RemoveItem();
			this.UpdateHotbar();
		}
        // Huy trang bi
        else
        {
			this.inventoryCells[this.BarSelect].equipAble = false;
			this.inventoryCells[this.BarSelect].Equip.color = this.inventoryCells[this.BarSelect].idle;
			UpdateEquipmentsModified(currentItem);
		}
	}

	public void UseFood(int n)
	{
		this.currentItem.amount -= n;
		if (this.currentItem.amount <= 0)
		{
			this.inventoryCells[this.BarSelect].RemoveItem();
		}
		this.inventoryCells[this.BarSelect].UpdateCell();
	}

	private void UpdateEquipmentsModified(ItemStats Items)
	{
		if (Items.name == "khungtroluc" && this.inventoryCells[this.BarSelect].equipAble == true)
		//(Item.Find((x) => x.name == "khungtroluc"))
		{
			speed = 1;
		}
		else if (Items.name == "khungtroluc" && this.inventoryCells[this.BarSelect].equipAble == false)
		{
			speed = 0;
		}
		if (Items.name == "matkinh" && this.inventoryCells[this.BarSelect].equipAble == true)
		{
			critcal = 0.2f;
		}
		else if(Items.name == "matkinh" && this.inventoryCells[this.BarSelect].equipAble == false)
		{
			critcal = 0;
		}

		base.Invoke("UpdateUI", 0.1f);

	}
	public float GetCritical()
	{
		float n = critcal;
		return 0.1f + n;
	}
	public float GetSpeedUp()
	{
		float n = speed;
		return 0 + n;
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

	public DamageResult GetDamage()
	{
		float dmg = Random.Range(randomDamageRange.x, randomDamageRange.y);
		bool ItCrit = Random.value < GetCritical();

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
	private void UpdateUI()
    {
		UIPlayerStats.Instance.UpdateStatsPlayer();
	}
	public void Removeitem()
	{

		if (currentItem != null)
		{
			this.inventoryCells[this.BarSelect].RemoveItem();
			//this.UpdateHotbar();
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
		MoneyUI.Instance.Value = Moneys;
	}
	public void RewardMoney(int Money)
	{
		Moneys += Money;
		MoneyUI.Instance.Value = Moneys;
	}

	public bool ActiveMenu()
	{
		return Cursor.lockState == CursorLockMode.Locked;
	}
}
