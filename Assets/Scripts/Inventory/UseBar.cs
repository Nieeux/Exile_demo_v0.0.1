using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseBar : MonoBehaviour
{
    public static UseBar Instance;
    private InventoryItem currentItem;
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public Transform renderTransform;
    private InventoryBar[] inventoryCells;
    private InventoryBar[] cells;
    private int currentActive;

    private void Awake()
    {
        UseBar.Instance = this;
        this.SetWeapon(null);
    }

    public void SetWeapon(InventoryItem item)
    {
        this.StopUse();
        this.currentItem = item;
        if (item == null)
        {
            this.meshRenderer.material = null;
            this.meshFilter.mesh = null;
            return;
        }
        this.renderTransform.localRotation = Quaternion.Euler(item.rotationOffset);
        this.renderTransform.localScale = Vector3.one * item.scale;
        this.renderTransform.localPosition = item.positionOffset;
    }

    private void StopUse()
    {
        base.CancelInvoke();
    }

    public void DropItem()
    {
        if (this.currentItem != null)
        {
            //InventoryUI.Instance.DropItem();
        }
    }

    public void UpdateHotbar()
    {
        if (this.inventoryCells[this.currentActive].currentItem != this.currentItem)
        {
            this.currentItem = this.inventoryCells[this.currentActive].currentItem;
            if (UseBar.Instance)
            {
                UseBar.Instance.SetWeapon(this.currentItem);
            }
        }
        for (int i = 0; i < this.cells.Length; i++)
        {
            if (i == this.currentActive)
            {
                this.cells[i].slot.color = this.cells[i].hover;
            }
            else
            {
                this.cells[i].slot.color = this.cells[i].idle;
            }
        }
        for (int j = 0; j < this.cells.Length; j++)
        {
            this.cells[j].itemImage.sprite = this.inventoryCells[j].itemImage.sprite;
            this.cells[j].itemImage.color = this.inventoryCells[j].itemImage.color;
            this.cells[j].amount.text = this.inventoryCells[j].amount.text;
        }
    }
}
