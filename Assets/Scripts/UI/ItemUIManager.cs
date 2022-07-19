using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIManager : MonoBehaviour
{
    public RectTransform ItemPanel;
    public GameObject ItemPanelPrefab;
    public List<ItemUI> ItemUI = new List<ItemUI>();
    private void Start()
    {
        Item activeWeapon = InventoryItem.Instance.GetActiveItem();
        if (activeWeapon)
        {
            AddWeapon(activeWeapon, InventoryItem.Instance.ActiveItemIndex);
            ChangeWeapon(activeWeapon);
        }

        InventoryItem.Instance.OnAddedItem += AddWeapon;
        InventoryItem.Instance.OnRemovedItem += RemoveWeapon;
        InventoryItem.Instance.OnSwitchedToItem += ChangeWeapon;
    }
    void AddWeapon(Item newWeapon, int weaponIndex)
    {
        GameObject ammoCounterInstance = Instantiate(ItemPanelPrefab, ItemPanel);
        ItemUI newAmmoCounter = ammoCounterInstance.GetComponent<ItemUI>();

        newAmmoCounter.Initialize(newWeapon, weaponIndex);

        ItemUI.Add(newAmmoCounter);
    }

    void RemoveWeapon(Item newWeapon, int weaponIndex)
    {
        int foundCounterIndex = -1;
        for (int i = 0; i < ItemUI.Count; i++)
        {
            if (ItemUI[i].WeaponUIIndex == weaponIndex)
            {
                foundCounterIndex = i;
                Destroy(ItemUI[i].gameObject);
            }
        }

        if (foundCounterIndex >= 0)
        {
            ItemUI.RemoveAt(foundCounterIndex);
        }
    }

    void ChangeWeapon(Item weapon)
    {
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(ItemPanel);
    }

}
