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
			this.currentItem.Getitem(this.spawnItem, this.spawnItem.amount);
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
			Debug.Log("CellNull");
		}
		else
		{
			this.amount.text = this.currentItem.GetAmount();
			this.Name.text = this.currentItem.GetName();
			this.itemImage.sprite = this.currentItem.sprite;
			this.itemImage.color = Color.white;
		}
		this.SetColor(this.idle);
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
	public bool EquipAble()
	{
		return equipAble = true;
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.SetColor(this.hover);
		if (this.currentItem)
		{
			string text = this.currentItem.name + "\n<size=70%>" + this.currentItem.description;
			ItemInfo.Instance.SetText(text, false);
			return;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.SetColor(this.idle);
		ItemInfo.Instance.Fade(0f, 0.2f);
	}
}
