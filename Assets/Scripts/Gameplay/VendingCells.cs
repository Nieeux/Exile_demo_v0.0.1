using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingCells : MonoBehaviour, Interact, SharedId
{
    private int id;

    public int price;

    private bool ready = true;

    private WeaponController weapon;

    public multiLanguage language;

    private void Start()
    {
        this.ready = true;
        AllExecute();
        weapon = GetComponentInChildren<WeaponController>();
        Getprice();
        weapon.GetComponent<Collider>().isTrigger = true;
    }

    public void Interact()
    {
        if (Inventory.Instance.GetMoney() < this.price)
        {
            return;
        }
        if (!this.ready)
        {
            return;
        }
        this.ready = false;
        Inventory.Instance.UseMoney(this.price);
        weapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.GetComponent<Collider>().isTrigger = false;
        weapon.transform.SetParent(null);
        RemoveObject();
    }

    public void AllExecute()
    {
        ItemStats RandomWeapon = ItemManager.Instance.GetRandomWeapons();
        Buff buff = ItemManager.Instance.GetBuff();
        Buff Debuff = ItemManager.Instance.GetDeBuff();
        ItemManager.Instance.DropWeaponAtVending(RandomWeapon.id, buff.id, Debuff.id, base.transform.position, base.transform.rotation, transform);

    }
    private void GetReady()
    {
        this.ready = true;
    }

    public string GetName()
    {
        return string.Format("{0} {1}\n<size=75%>E | {2}", this.price, language.VietNamese, this.weapon.GunStats.nameViet);
    }

    public ItemStats GetItem()
    {
        return this.weapon.GunStats;
    }
    private void Getprice()
    {
        int n = (int)this.weapon.GunStats.CurrentDurability;
        if(this.weapon.GunStats.Rare == ItemStats.ItemRare.original) 
        {
            n *= 2;
        }
        if (this.weapon.GunStats.Rare == ItemStats.ItemRare.upgrade)
        {
            n *= 4;
        }
        if (this.weapon.GunStats.Rare == ItemStats.ItemRare.advanced)
        {
            n *= 8;
        }
        if (this.weapon.GunStats.weaponType == ItemStats.WeaponType.AssaultRifles)
        {
            n *= 8;
        }


        price = n;
    }
    public bool IsStarted()
    {
        return false;
    }

    public void LocalExecute()
    {

    }

    public void RemoveObject()
    {
        Destroy(gameObject);
    }

    public void SetId(int id)
    {
        this.id = id;
    }



    public int GetId()
    {
        return this.id;
    }
}
