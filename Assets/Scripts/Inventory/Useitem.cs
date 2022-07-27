using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Useitem : MonoBehaviour
{
    public static Useitem Instance;

    public ItemStats currentItem;

    public bool Select;

    private void Awake()
    {

        Useitem.Instance = this;
        this.SelectItem(null);
    }

    public void SelectItem(ItemStats item)
    {

        this.StopUse();
        this.currentItem = item;
        if (item == null)
        {
            return;
        }

    }



    private void StopUse()
    {
        base.CancelInvoke();
    }

    public void Use()
    {
        if (this.currentItem == null)
        {
            return;
        }
        if (this.currentItem.type == ItemStats.ItemType.Food)
        {
            HotBar.Instance.UseItem(1);
            PlayerStats.Instance.Heal(50);
        }
        if (this.currentItem.type == ItemStats.ItemType.Equipment)
        {

        }
        if (this.currentItem.type == ItemStats.ItemType.Item)
        {

        }
    }
}
