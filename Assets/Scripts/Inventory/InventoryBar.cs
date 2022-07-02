using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryBar : MonoBehaviour, IEventSystemHandler,IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public InventoryItem currentItem;
    public RawImage slot;
    public Color idle;
    public Color hover;
    public TextMeshProUGUI amount;
    public Image itemImage;

	public InventoryBar.CellType cellType;
	public enum CellType
	{
		Inventory,
		balo
	}

	public InventoryItem spawnItem;

	private void Start()
	{
		if (this.spawnItem)
		{
			this.currentItem = ScriptableObject.CreateInstance<InventoryItem>();
			this.currentItem.Get(this.spawnItem, this.spawnItem.amount);
		}
		this.UpdateCell();
	}

	public void UpdateCell()
	{
		if (this.currentItem == null)
		{
			this.amount.text = "";
			this.itemImage.sprite = null;
			this.itemImage.color = Color.clear;
			Debug.Log("CellNull");
		}
		else
		{
			this.amount.text = this.currentItem.GetAmount();
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
