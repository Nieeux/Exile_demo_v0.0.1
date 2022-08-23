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

    [Header("SFX")]
    public AudioSource Sfx;
    public AudioClip InteractSfx;

    private void Start()
    {
        this.ready = true;
        AllExecute();
        weapon = GetComponentInChildren<WeaponController>();
        base.Invoke("Getprice", 0.2f);
        weapon.GetComponent<Collider>().isTrigger = true;
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
        weapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.GetComponent<Collider>().isTrigger = false;
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
        return string.Format("{0} {1}\n<size=75%>E | {2}", this.price, language.GetLanguage(), this.weapon.GunStats.nameViet);
    }

    public ItemStats GetItem()
    {
        return this.weapon.GunStats;
    }
    private void Getprice()
    {
        int n = (int)weapon.GunStats.CurrentDurability;

        if (this.weapon.GunStats.Rare == ItemStats.ItemRare.upgrade)
        {
            n *= 2;
        }
        if (this.weapon.GunStats.Rare == ItemStats.ItemRare.advanced)
        {
            n *= 4;
        }
        if (this.weapon.GunStats.weaponType == ItemStats.WeaponType.AssaultRifles)
        {
            n *= 2;
        }

        if (this.weapon.Pullet.ammoType == Bullet.AmmoType.PiercingAmmo)
        {
            n *= 2;
        }
        if (this.weapon.Pullet.ammoType == Bullet.AmmoType.HighAmmo)
        {
            n *= 2;
        }
        if (this.weapon.Pullet.IsShotGunShell == true)
        {
            n *= 2;
        }
        n *= this.weapon.GunStats.buffs.Count;

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
