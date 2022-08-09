using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingCells : MonoBehaviour, Interact, SharedId
{
    private int id;

    public int price;

    private int basePrice;

    private bool ready = true;

    private bool opened;

    private WeaponController weapon;


    private void Start()
    {
        this.ready = true;
        this.basePrice = this.price;
        AllExecute();
        weapon = GetComponentInChildren<WeaponController>();
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
        weapon.transform.SetParent(null);
        RemoveObject();
    }

    public void AllExecute()
    {
        ItemStats RandomWeapon = ItemManager.Instance.GetRandomWeapons();
        ItemManager.Instance.DropWeaponAtVending(RandomWeapon.id, base.transform.position, base.transform.rotation, transform);

    }
    private void GetReady()
    {
        this.ready = true;
    }

    public string GetName()
    {
        this.price = (int)((float)this.basePrice);
        if (this.price < 1)
        {
            return "Open chest";
        }
        return string.Format("{0} Money\n<size=75%>open chest", this.price);
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
