using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
	public static Inventory Instance;

	[Header("Inventory")]
	public int BarSelect;
	public Transform CamDrop;
	public ItemStats currentItem;
	public int Moneys;
	public float ItemWeight;

	public Transform inventoryBar;
	public List<InventoryCells> inventoryCells;

	[Header("Armor")]
	public int armorSlot;
	public ItemStats currentArmor;

	//private int Moneys { get; set; }

	PlayerStats playerStats;
	WeaponInventory Weapon;

	[Header("Equipments")]
	public List<ItemStats> Item;

	public Dictionary<int, ItemStats> equipments;
	public Dictionary<string, int> GetNameEquipments;

	public UnityAction<float> critcal;
	public UnityAction<float> speed;
	public UnityAction<float> damage;
	public UnityAction<float> weight;
	public UnityAction<float> maxHealth;

	public GameObject flashlight;
	public float NoiComDien;

	[Header("SFX")]
	public AudioSource Sfx;
	public AudioClip PickUpSfx;

	public UnityAction UpdateUi;

	private void Awake()
    {
		Inventory.Instance = this;
	}

    private void Start()
    {
		flashlight.SetActive(false);

		if (Sfx == null)
        {
			Sfx.GetComponent<AudioSource>();
		}
		Weapon = GetComponent<WeaponInventory>();
		playerStats = GetComponent<PlayerStats>();
		playerStats.OnDamaged += OnDamagedArmor;
		FillCellList();

	}

    private void Update()
    {
		if (!ActiveMenu() && !playerStats.PlayerDead())
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
	}

	#region Inventory
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
		inventoryItem.Getitem(item);
		//inventoryItem.Getitem(item, item.amount);
		EquipWeightModified(item);
		InventoryCells inventoryCell = null;

		//Sfx.clip = PickUpSfx;
		//this.Sfx.Play();
		this.Sfx.PlayOneShot(PickUpSfx);

		foreach (InventoryCells inventoryCell2 in this.inventoryCells)
		{
			if (inventoryCell2.currentItem == null)
			{
				if (!(inventoryCell != null))
				{
					inventoryCell = inventoryCell2;
				}
			}
			/*
			else if (inventoryCell2.currentItem.CheckId(inventoryItem) && inventoryCell2.currentItem.stackable)
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
			*/
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
			if (!(inventoryCell.currentItem == null) && inventoryCell.currentItem.CheckId(requirement))
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

	public bool HasEquipment(ItemStats requirement)
	{
		foreach (InventoryCells inventoryCell in this.inventoryCells)
		{
			if (!(inventoryCell.currentItem == null) && inventoryCell.equipAble && inventoryCell.currentItem.CheckEquipment(requirement))
			{
				return true;
			}
		}
		return false;
	}

	public void SellItem()
    {
		if (this.currentItem == null)
		{
			return;
		}
		if (this.inventoryCells[this.BarSelect].equipAble == false)
		{

			UnequipWeightModified(currentItem);
			PayMoney(currentItem);
			this.inventoryCells[this.BarSelect].RemoveItem();
			this.UpdateHotbar();		
		}
	}
	private void PayMoney(ItemStats item)
    {
		int Moneys = ItemPrice(item);
		RewardMoney(Moneys);
	}

	public int ItemPrice(ItemStats item)
	{
		int n = 0;
		if (item.itemType == ItemStats.ItemType.Food)
		{
			n = 100;
		}
		if (item.itemType == ItemStats.ItemType.Item)
		{
			n = 150;
		}
		if (item.itemType == ItemStats.ItemType.Equipment)
		{
			n = 300;
		}
		if (item.itemType == ItemStats.ItemType.Armor)
		{
			if(item.armorType == ItemStats.ArmorType.LightArmor)
            {
				n = 100;
			}
			if (item.armorType == ItemStats.ArmorType.NormalArmor)
			{
				n = 200;
			}
			if (item.armorType == ItemStats.ArmorType.HeavyArmor)
			{
				n = 300;
			}
		}
		if (item.Rare == ItemStats.ItemRare.upgrade)
		{
			n *= 2;
		}
		if (item.Rare == ItemStats.ItemRare.advanced)
		{
			n *= 4;
		}

		return n / 2;
	}
	#endregion

	#region UseEquipDrop
	public void Use()
	{
		if (this.currentItem == null)
		{
			return;
		}
		if (this.currentItem.itemType == ItemStats.ItemType.Item)
		{
			useItem(currentItem);
		}
		if (this.currentItem.itemType == ItemStats.ItemType.Food)
		{
			UseFood();
		}
		if (this.currentItem.itemType == ItemStats.ItemType.Equipment
			&& this.inventoryCells[this.BarSelect].equipAble == false)
		{
            if (HasEquipment(currentItem))
            {
				return;
            }
			EquipItemUI(currentItem);
			UpdateEquipmentsModified(currentItem);
		}
		if (this.currentItem.itemType == ItemStats.ItemType.Armor)
		{
			if (currentArmor != null)
			{
				return;
			}
			EquipItemUI(currentItem);
			UpdateArmorSlot(currentItem, this.BarSelect);
		}
		this.UpdateHotbar();
	}

	private void useItem(ItemStats item)
    {
		if (item.name == "callsupply")
		{
			OpenSupply pickup = Instantiate(item.prefab2, transform.position
				+ new Vector3(Random.Range(-10f, 10f) * 2f, 100f, Random.Range(-10f, 10f) * 2f), transform.rotation).GetComponent<OpenSupply>();

			this.inventoryCells[this.BarSelect].RemoveItem();
		}

		if (currentItem.heal > 0)
		{
			if (playerStats.Heal(currentItem))
				this.inventoryCells[this.BarSelect].RemoveItem();
		}
		if (currentItem.repair > 0)
		{
			if (Weapon.RepairWeapon(currentItem))
				this.inventoryCells[this.BarSelect].RemoveItem();
		}
	}

	private void UseFood()
	{
		if (currentItem.hunger > 0)
		{
			if (playerStats.Eat(currentItem.hunger *= (1 + NoiComDien)))
			this.inventoryCells[this.BarSelect].RemoveItem();
		}
		if (currentItem.sleep > 0)
		{
			if (playerStats.Coffee(currentItem))
			this.inventoryCells[this.BarSelect].RemoveItem();
		}

	}
	public void UseFoodAmount(int n)
	{

		this.currentItem.amount -= n;
		if (this.currentItem.amount <= 0)
		{
			this.inventoryCells[this.BarSelect].RemoveItem();
		}
		this.inventoryCells[this.BarSelect].RemoveItem();
	}


	public void UseMoney(int Money)
	{
		Moneys -= Money;
		if (Moneys <= 0)
		{
			Moneys = 0;
		}
		MoneyUI.Instance.Value = Moneys;
		MoneyUI.Instance.RemoveMoney(Money);
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

			UnequipWeightModified(currentItem);

			PickupItem pickup = Instantiate(currentItem.prefab, CamDrop.transform.position, Quaternion.identity).GetComponent<PickupItem>();
			pickup.item = currentItem;

			RaycastHit hit;
			if (Physics.Raycast(pickup.transform.position, Vector3.down, out hit, 10, LayerMask.GetMask("Ground")))
			{
				pickup.transform.SetParent(hit.collider.gameObject.transform.parent);
			}
			else
			{
				pickup.transform.SetParent(null);
			}
			this.inventoryCells[this.BarSelect].RemoveItem();
			this.UpdateHotbar();

		}
		// Huy trang bi
		else
		{
			this.inventoryCells[this.BarSelect].equipAble = false;
			this.inventoryCells[this.BarSelect].Equip.color = this.inventoryCells[this.BarSelect].idle;
			UpdateEquipmentsModified(currentItem);
			if (currentArmor != null)
			{
				UpdateArmorSlot(currentItem, this.BarSelect);
			}
		}
	}

	#endregion

	#region EquipModified
	private void UpdateEquipmentsModified(ItemStats Items)
	{
		if (Items.name == "khungtroluc" && this.inventoryCells[this.BarSelect].equipAble == true)
		//(Item.Find((x) => x.name == "khungtroluc"))
		{
			if (speed != null)
			{
				speed.Invoke(1);
			}
			if (weight != null)
			{
				weight.Invoke(10f);
			}
		}
		else if (Items.name == "khungtroluc" && this.inventoryCells[this.BarSelect].equipAble == false)
		{
			if (speed != null)
			{
				speed.Invoke(0);
			}
			if (weight != null)
			{
				weight.Invoke(0);
			}
		}
		if (Items.name == "matkinh" && this.inventoryCells[this.BarSelect].equipAble == true)
		{
			if (critcal != null)
			{
				critcal.Invoke(0.2f);
			}
		}
		else if (Items.name == "matkinh" && this.inventoryCells[this.BarSelect].equipAble == false)
		{
			if (critcal != null)
			{
				critcal.Invoke(0);
			}
		}
		if (Items.name == "maygiatoc" && this.inventoryCells[this.BarSelect].equipAble == true)
		{
			if (damage != null)
			{
				damage.Invoke(0.2f);
			}
		}
		else if (Items.name == "maygiatoc" && this.inventoryCells[this.BarSelect].equipAble == false)
		{
			if (damage != null)
			{
				damage.Invoke(0);
			}
		}
		if (Items.name == "noicomdien" && this.inventoryCells[this.BarSelect].equipAble == true)
		{
			NoiComDien = 0.5f;
		}
		else if (Items.name == "noicomdien" && this.inventoryCells[this.BarSelect].equipAble == false)
		{
			NoiComDien = 0;
		}

		if (Items.name == "vimachh" && this.inventoryCells[this.BarSelect].equipAble == true)
		{
			if (maxHealth != null)
			{
				maxHealth.Invoke(0.5f);
			}
		}
		else if (Items.name == "vimachh" && this.inventoryCells[this.BarSelect].equipAble == false)
		{
			if (maxHealth != null)
			{
				maxHealth.Invoke(0);
			}
		}
		if (Items.name == "denpin" && this.inventoryCells[this.BarSelect].equipAble == true)
		{
			flashlight.SetActive(true);
			//StartCoroutine(Flashlight(currentItem, this.BarSelect));

		}
		else if (Items.name == "denpin" && this.inventoryCells[this.BarSelect].equipAble == false)
		{
			
			flashlight.SetActive(false);
			//StopCoroutine(Flashlight(currentItem, this.BarSelect));
		}

		if (UpdateUi != null)
		{
			UpdateUi.Invoke();
		}

	}

	private void EquipWeightModified(ItemStats Item)
	{
		ItemWeight += Item.Weight;

		if (UpdateUi != null)
		{
			UpdateUi.Invoke();
		}
	}
	private void UnequipWeightModified(ItemStats Item)
	{
		ItemWeight -= Item.Weight;

		if(ItemWeight <= 0)
        {
			ItemWeight = 0;
		}

		if (UpdateUi != null)
		{
			UpdateUi.Invoke();
		}
	}

	private void UpdateArmorSlot(ItemStats Item, int armorslot)
	{

		if (Item.name == "LightArmor" && this.inventoryCells[this.BarSelect].equipAble == true)
		{
			currentArmor = Item;
			armorSlot = armorslot;
		}
		else if (Item.name == "LightArmor" && this.inventoryCells[this.BarSelect].equipAble == false)
		{
			currentArmor = null;
			armorSlot = 0;
		}
		if (Item.name == "NormalArmor" && this.inventoryCells[this.BarSelect].equipAble == true)
		{
			currentArmor = Item;
			armorSlot = armorslot;

		}
		else if (Item.name == "NormalArmor" && this.inventoryCells[this.BarSelect].equipAble == false)
		{
			currentArmor = null;
			armorSlot = 0;

		}
		if (Item.name == "HeavyArmor" && this.inventoryCells[this.BarSelect].equipAble == true)
		{
			currentArmor = Item;
			armorSlot = armorslot;

		}
		else if (Item.name == "HeavyArmor" && this.inventoryCells[this.BarSelect].equipAble == false)
		{
			currentArmor = null;
			armorSlot = 0;
		}
	}
	public void OnDamagedArmor(float damage)
	{
		if (currentArmor == null)
		{
			return;
		}

		currentArmor.CurrentDurability--;
		this.inventoryCells[armorSlot].UpdateDurability();

		if (currentArmor.CurrentDurability <= 0)
		{
			this.inventoryCells[armorSlot].equipAble = false;
			this.inventoryCells[armorSlot].Equip.color = this.inventoryCells[armorSlot].idle;
			this.inventoryCells[armorSlot].RemoveItem();
			currentArmor = null;
			this.UpdateHotbar();
		}

	}
	/*
	private IEnumerator Flashlight(ItemStats item, int BarSelect)
	{
		WaitForSeconds wait = new WaitForSeconds(5f);

		while (true)
		{
			yield return wait;
			item.CurrentDurability--;
			this.inventoryCells[BarSelect].UpdateDurability();
			Debug.Log("Flashlight");

            if (this.inventoryCells[BarSelect].equipAble == false)
            {
				break;
            }
			if (item.CurrentDurability <= 0)
			{
				this.inventoryCells[BarSelect].equipAble = false;
				this.inventoryCells[BarSelect].Equip.color = this.inventoryCells[armorSlot].idle;
				this.inventoryCells[BarSelect].RemoveItem();
				flashlight.SetActive(false);
				Debug.Log("DestroyFlashlight");
				this.UpdateHotbar();
				StopCoroutine(Flashlight(item, BarSelect));
				break;
			}
		}
	}
	*/
	#endregion

	#region GetData
	public bool ItemCurrentIsNull()
    {
		if(currentItem == null)
        {
			return true;
        }
		return false;
    }

	public bool ThisItemIsUnequip()
    {

		return this.inventoryCells[this.BarSelect].equipAble == false;

	}


	public float GetWeight()
	{
		return ItemWeight;

	}

	public ItemStats GetArmor()
	{
		return currentArmor;

	}
	public int GetMoney()
	{
		int num = 0;
		num += Moneys;
		return num;
	}

	public void RewardMoney(int Money)
	{
		Moneys += Money;
		MoneyUI.Instance.Value = Moneys;
		MoneyUI.Instance.AddMoney(Money);
	}
	#endregion

	#region UI
	public void EquipItemUI(ItemStats currentItem)
	{
		if (this.inventoryCells[this.BarSelect].currentItem == currentItem)
		{
			this.inventoryCells[BarSelect].Equip.color = this.inventoryCells[BarSelect].EquipColor;
			this.inventoryCells[BarSelect].EquipItem();

		}
	}

	public bool ActiveMenu()
	{
		return Cursor.lockState == CursorLockMode.Locked;
	}
	#endregion

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
}
