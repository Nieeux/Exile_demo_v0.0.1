using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UseBar : MonoBehaviour
{
    public GameObject NoticeBoard;
    public static UseBar Instance;
    public ItemStats currentItem;
    public bool Select;

    private void Awake()
    {
        NoticeBoard.SetActive(false);
        UseBar.Instance = this;
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

        PlayerWeaponManager.Instance.CloseWeapon();
        NoticeBoard.SetActive(true);
        /*
        GameObject Item = Instantiate(currentItem.prefab,transform);
        Item.GetComponent<Rigidbody>().isKinematic = true;
        transform.localRotation = Quaternion.Euler(item.rotationOffset);
        transform.localPosition = item.positionOffset;
        */
        return true;

    }
    private void Update()
    {
        if (currentItem == null)
        {
            PlayerWeaponManager.Instance.ShowWeapon();
            NoticeBoard.SetActive(false);
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
            HotBar.Instance.Removeitem();
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
