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

    public bool SelectItem(ItemStats item)
    {

        this.StopUse();
        this.currentItem = item;
        if (item == null)
        {
            return false;
        }



        //PlayerWeaponManager.Instance.CloseWeapon();
        /*
        
        ItemContainer.SetActive(true);

        GameObject Item = Instantiate(currentItem.prefab, ItemContainer.transform);
        Item.GetComponent<Rigidbody>().isKinematic = true;
        ItemContainer.transform.localRotation = Quaternion.Euler(item.rotationOffset);
        ItemContainer.transform.localPosition = item.positionOffset;
        */


        return true;

    }
    private void Update()
    {
        if (currentItem == null)
        {
            //PlayerWeaponManager.Instance.ShowWeapon();


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
        Debug.Log("UsedItem");
        if (this.currentItem.type == ItemStats.ItemType.Food)
        {
            HotBar.Instance.UseItem(1);
            PlayerStats.Instance.Heal(50);
        }
        if (this.currentItem.type == ItemStats.ItemType.Equipment)
        {
            HotBar.Instance.EquipItem(currentItem);

        }
        if (this.currentItem.type == ItemStats.ItemType.Item)
        {

        }
    }
}
