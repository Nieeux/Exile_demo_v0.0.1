using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingTrash : MonoBehaviour, Interact
{
    private WeaponController WeaponInHand;

    public multiLanguage Money;
    public multiLanguage Sell;
    public multiLanguage ThisPlaceSell;

    public ItemStats item;

    public Interact currentInteractable { get; private set; }


    private void Start()
    {

    }

    public void Interact()
    {
        if(ActiveMenu())
        {
            if(WeaponInHand != null){
                WeaponInventory.Instance.SellWeapon(WeaponInHand, item);
            }
            
        }
        else
        {
            Inventory.Instance.SellItem();
        }

    }

    public ItemStats GetItem()
    {
        return null;
    }

    public string GetName()
    {
        WeaponInHand = FindObjectOfType<WeaponInventory>().WeaponInActive;

        if (WeaponInHand != null)
        {
            item = WeaponInHand.GunStats;
            return string.Format("{0} {1} {2}\n<size=75%>E | {3}",this.WeaponInHand.GunStats.nameViet, WeaponInventory.Instance.WeaponPrice(WeaponInHand, item), Money.GetLanguage(), Sell.GetLanguage());
        }
        return string.Format("{0}", ThisPlaceSell.GetLanguage());

    }


    public bool IsStarted()
    {
        return true;
    }

    public void RemoveObject()
    {
       
    }
    public void DropObject()
    {

    }
    public bool ActiveMenu()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }
}
