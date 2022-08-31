using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingCells : MonoBehaviour, Interact, SharedId
{
    private int id;

    public int price;

    private bool ready = true;

    public PickupWeapon weapon;

    public multiLanguage language;

    [Header("SFX")]
    public AudioSource Sfx;
    public AudioClip InteractSfx;

    private void Start()
    {
        this.ready = true;
        AllExecute();
        weapon = GetComponentInChildren<PickupWeapon>();
        base.Invoke("Getprice", 0.2f);
        weapon.coll.isTrigger = true;
    }

    public void Interact()
    {
        if (Inventory.Instance.GetMoney() < this.price)
        {
            MoneyUI.Instance.NotEnoughMoney();
            return;
        }
        if (!this.ready)
        {
            return;
        }
        this.ready = false;

        Sfx.clip = InteractSfx;
        this.Sfx.Play();

        Inventory.Instance.UseMoney(this.price);
        weapon.rb.isKinematic = false;
        weapon.coll.isTrigger = false;
        weapon.transform.SetParent(null);
        RemoveObject();
    }

    public void AllExecute()
    {
        ItemStats RandomWeapon = ItemManager.Instance.GetRandomWeapons();
        ItemManager.Instance.DropWeaponAtVending(RandomWeapon.id, base.transform.position, base.transform.rotation, transform);

    }

    public string GetName()
    {
        return string.Format("{0} {1}\n<size=75%>E | {2}", this.price, language.GetLanguage(), this.weapon.item.nameViet);
    }

    public ItemStats GetItem()
    {
        return this.weapon.item;
    }
    private void Getprice()
    {
        int n = (int)weapon.item.CurrentDurability;

        if (this.weapon.item.Rare == ItemStats.ItemRare.upgrade)
        {
            n *= 2;
        }
        if (this.weapon.item.Rare == ItemStats.ItemRare.advanced)
        {
            n *= 4;
        }
        if (this.weapon.item.weaponType == ItemStats.WeaponType.AssaultRifles)
        {
            n *= 2;
        }

        if (this.weapon.item.bulletType.ammoType == AmmoType.PiercingAmmo)
        {
            n *= 2;
        }
        if (this.weapon.item.bulletType.ammoType == AmmoType.HighAmmo)
        {
            n *= 2;
        }
        if (this.weapon.item.bulletType.IsShotGunShell == true)
        {
            n *= 2;
        }
        n *= this.weapon.item.buffs.Count;

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
    public void DropObject()
    {

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
