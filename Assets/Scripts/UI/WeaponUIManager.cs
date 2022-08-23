using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUIManager : MonoBehaviour
{
    public static WeaponUIManager Instance;
    public RectTransform WeaponPanel;
    public GameObject WeaponPanelPrefab;
    public List<WeaponUI> weaponUI = new List<WeaponUI>();
    WeaponInventory inventory;

    private void Awake()
    {
        WeaponUIManager.Instance = this;
    }

    private void Start()
    {

        inventory = FindObjectOfType<WeaponInventory>();
        inventory.OnAddedWeapon += AddWeapon;
        inventory.OnRemovedWeapon += RemoveWeapon;
        inventory.OnSwitchedToWeapon += ChangeWeapon;

        WeaponController activeWeapon = inventory.GetActiveWeapon();
        if (activeWeapon)
        {
            AddWeapon(activeWeapon, inventory.ActiveWeaponIndex);
            ChangeWeapon(activeWeapon);
        }
    }

    public void updateUI(float weaponid)
    {
        WeaponController activeWeapon = WeaponInventory.Instance.GetActiveWeapon();
        if (activeWeapon)
        {
            for (int i = 0; i < weaponUI.Count; i++)
            {
                if (weaponUI[i].WeaponUiId == weaponid)
                {
                    weaponUI[i].UpdateUI();
                }
            }

        }
    }

    private void AddWeapon(WeaponController newWeapon, int weaponIndex)
    {
        GameObject ammoCounterInstance = Instantiate(WeaponPanelPrefab, WeaponPanel);
        WeaponUI newAmmoCounter = ammoCounterInstance.GetComponent<WeaponUI>();

        newAmmoCounter.Initialize(newWeapon, weaponIndex);

        weaponUI.Add(newAmmoCounter);
    }

    private void RemoveWeapon(WeaponController newWeapon, int weaponIndex)
    {
        int foundCounterIndex = -1;
        for (int i = 0; i < weaponUI.Count; i++)
        {
            if (weaponUI[i].WeaponUiId == weaponIndex)
            {
                foundCounterIndex = i;
                Destroy(weaponUI[i].gameObject);
            }
        }

        if (foundCounterIndex >= 0)
        {
            weaponUI.RemoveAt(foundCounterIndex);
        }
    }

    private void ChangeWeapon(WeaponController weapon)
    {
        //UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(WeaponPanel);
    }
}
