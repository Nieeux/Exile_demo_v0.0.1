using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class OpenSupply : MonoBehaviour, Interact
{
    public GameObject SupplyEmty;
    public multiLanguage supply;

    private Random random;

    public ItemStats item;

    public Interact currentInteractable { get; private set; }

    [Header("Rare")]
    public float Original;
    public float Upgrade;
    public float Advanced;

    private void Start()
    {
        this.random = new Random();
    }

    public void Interact()
    {
        GetRandomDrop(Original, Upgrade, Advanced);
        RemoveObject();
    }

    public ItemStats GetItem()
    {
        return null;
    }

    public string GetName()
    {

        return string.Format("{0}", supply.GetLanguage());
    }


    public bool IsStarted()
    {
        return true;
    }

    public void RemoveObject()
    {
        GameObject supply = Instantiate(SupplyEmty, transform.position, transform.rotation);

        supply.GetComponent<Rigidbody>().velocity = (-(PlayerMovement.Instance.transform.position - transform.position).normalized * 8) + new Vector3(0, 5, 0);
        Destroy(gameObject);
    }
    public void DropObject()
    {

    }
    public bool ActiveMenu()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }

    public void GetRandomDrop(float Original, float Upgrade, float Advanced)
    {
        float num = Original + Upgrade + Advanced;
        float num2 = (float)random.NextDouble();
        if (num2 < Original / num)
        {
            DropHeal();
            DropFood();
            return;
        }
        if (num2 < (Original + Upgrade) / num)
        {
            DropEquipMents();
            DropFood();
            DropHeal();
            return;
        }
        DropFood();
        DropWeapon();
        DropEquipMents();
    }
    private void DropFood()
    {
        ItemStats food = ItemManager.Instance.GetRandomFoods();
        ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
        inventoryItem.Getitem(food);
        PickupItem pickup = Instantiate(inventoryItem.prefab, transform.position, transform.rotation, transform).GetComponent<PickupItem>();
        pickup.item = inventoryItem;

        RaycastHit hit;
        if (Physics.Raycast(pickup.transform.position, Vector3.down, out hit, 10, LayerMask.GetMask("Ground")))
        {
            pickup.transform.SetParent(hit.collider.gameObject.transform.parent);
        }
        else
        {
            pickup.transform.SetParent(null);
        }
    }
    public void DropHeal()
    {
        ItemStats heal = ItemManager.Instance.GetHeal();
        ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
        inventoryItem.Getitem(heal);
        PickupItem pickup = Instantiate(inventoryItem.prefab, transform.position, transform.rotation).GetComponent<PickupItem>();
        pickup.item = inventoryItem;

        RaycastHit hit;
        if (Physics.Raycast(pickup.transform.position, Vector3.down, out hit, 10, LayerMask.GetMask("Ground")))
        {
            pickup.transform.SetParent(hit.collider.gameObject.transform.parent);
        }
        else
        {
            pickup.transform.SetParent(null);
        }
    }

    public void DropWeapon()
    {
        ItemStats RandomWeapon = ItemManager.Instance.GetRandomWeapons();
        ItemManager.Instance.getweaponAdvanced(RandomWeapon.id, base.transform.position, base.transform.rotation,null);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10, LayerMask.GetMask("Ground")))
        {
            transform.SetParent(hit.collider.gameObject.transform.parent);
        }
        else
        {
            transform.SetParent(null);
        }
    }

    public void DropArmor()
    {
        ItemStats Armor = ItemManager.Instance.GetRandomArmor();
        ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
        inventoryItem.Getitem(Armor);
        PickupItem pickup = Instantiate(inventoryItem.prefab, transform.position, transform.rotation).GetComponent<PickupItem>();
        pickup.item = inventoryItem;

        RaycastHit hit;
        if (Physics.Raycast(pickup.transform.position, Vector3.down, out hit, 10, LayerMask.GetMask("Ground")))
        {
            pickup.transform.SetParent(hit.collider.gameObject.transform.parent);
        }
        else
        {
            pickup.transform.SetParent(null);
        }
    }


    public void DropEquipMents()
    {
        ItemStats equip = ItemManager.Instance.GetRandomEquipments();
        ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
        inventoryItem.Getitem(equip);
        PickupItem pickup = Instantiate(inventoryItem.prefab, transform.position, transform.rotation).GetComponent<PickupItem>();
        pickup.item = inventoryItem;

        RaycastHit hit;
        if (Physics.Raycast(pickup.transform.position, Vector3.down, out hit, 10, LayerMask.GetMask("Ground")))
        {
            pickup.transform.SetParent(hit.collider.gameObject.transform.parent);
        }
        else
        {
            pickup.transform.SetParent(null);
        }
    }
}
