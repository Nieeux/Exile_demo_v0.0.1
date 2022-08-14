using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryCells : MonoBehaviour, IEventSystemHandler,IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public ItemStats currentItem;
    public RawImage Equip;
	public RawImage Select;
	public Image Durability;
	public Gradient GradientDurability;
	public Color idle;
    public Color hover;
	public Color EquipColor;
	public TextMeshProUGUI amount;
	public TextMeshProUGUI Name;
	public Image itemImage;
	public bool equipAble;

	public InventoryCells.CellType cellType;
	public enum CellType
	{
		Inventory,
		balo
	}

	public ItemStats spawnItem;

	private void Start()
	{
		if (this.spawnItem)
		{
			this.currentItem = ScriptableObject.CreateInstance<ItemStats>();
			this.currentItem.Getitem(this.spawnItem);
		}
		this.UpdateCell();
	}

	public void UpdateCell()
	{
		if (this.currentItem == null)
		{
			this.amount.text = "";
			this.Name.text = "";
			this.itemImage.sprite = null;
			this.itemImage.color = Color.clear;
			this.Durability.color = Color.clear;
		}
		else
		{
			this.amount.text = this.currentItem.GetAmount();
			this.Name.text = this.currentItem.GetName();
			this.itemImage.sprite = this.currentItem.sprite;
			this.itemImage.color = this.currentItem.colorIndex;

		}
		this.SetColor(this.idle);
	}

	public void UpdateDurability()
    {
		Durability.fillAmount = currentItem.CurrentDurability / currentItem.Durability;
		if (currentItem.Durability > 0)
		{
			this.Durability.color = GradientDurability.Evaluate(Durability.fillAmount);
		}
		else
		{
			this.Durability.color = Color.clear;
		}
	}

	public void Eat(int amount)
	{
		this.currentItem.amount -= amount;
		if (this.currentItem.amount <= 0)
		{
			this.RemoveItem();
		}
		this.UpdateCell();
	}

	public void SetColor(Color c)
	{
	}

	public void RemoveItem()
	{
		this.currentItem = null;
		this.UpdateCell();
	}
	public bool EquipItem()
	{
		return equipAble = true;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		this.SetColor(this.hover);
		if (this.currentItem)
		{
			string text = this.currentItem.nameViet + "\n<size=70%>" + this.currentItem.description;
			float Weight = this.currentItem.Weight;
			ItemInfo.Instance.SetWeight(Weight);
			ItemInfo.Instance.SetText(text);
			return;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.SetColor(this.idle);
		ItemInfo.Instance.OnDisable();
	}
}
