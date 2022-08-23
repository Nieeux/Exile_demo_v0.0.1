using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingFoods : MonoBehaviour, Interact, SharedId
{
    private int id;

    public int price;

    private bool ready = true;

    public ItemStats CurrentItem;

    public multiLanguage language;

    public multiLanguage languageBuy;

    public int GetId()
    {
        return this.id;
    }
    private void Start()
    {
        this.ready = true;
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
        GetFood();
        Inventory.Instance.UseMoney(this.price);
        price *= 2;
    }
    private void GetFood()
    {
        ItemStats food = ItemManager.Instance.GetRandomFoods();
        ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
        inventoryItem.Getitem(food);
        PickupItem pickup = Instantiate(inventoryItem.prefab, transform.position, transform.rotation, transform).GetComponent<PickupItem>();
        pickup.item = inventoryItem;
        CurrentItem = inventoryItem;
    }
    public void SetId(int id)
    {
        this.id = id;
    }
    public ItemStats GetItem()
    {
        return null;
    }

    public string GetName()
    {
        return string.Format("{0} {1}\n<size=75%>E | {2}", this.price, language.GetLanguage(), languageBuy.GetLanguage());
    }


    public bool IsStarted()
    {
        return false;
    }

    public void RemoveObject()
    {
        Destroy(gameObject);
    }
    public void DropObject()
    {

    }
}
