using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUIManager : MonoBehaviour
{
    public RectTransform WeaponPanel;
    public GameObject WeaponPanelPrefab;
    public List<WeaponUI> weaponUI = new List<WeaponUI>();
    void Start()
    {
        WeaponController activeWeapon = PlayerWeaponManager.Instance.GetActiveWeapon();
        if (activeWeapon)
        {
            AddWeapon(activeWeapon, PlayerWeaponManager.Instance.ActiveWeaponIndex);
            ChangeWeapon(activeWeapon);
        }

        PlayerWeaponManager.Instance.OnAddedWeapon += AddWeapon;
        PlayerWeaponManager.Instance.OnRemovedWeapon += RemoveWeapon;
        PlayerWeaponManager.Instance.OnSwitchedToWeapon += ChangeWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void AddWeapon(WeaponController newWeapon, int weaponIndex)
    {
        GameObject ammoCounterInstance = Instantiate(WeaponPanelPrefab, WeaponPanel);
        WeaponUI newAmmoCounter = ammoCounterInstance.GetComponent<WeaponUI>();

        newAmmoCounter.Initialize(newWeapon, weaponIndex);

        weaponUI.Add(newAmmoCounter);
    }

    void RemoveWeapon(WeaponController newWeapon, int weaponIndex)
    {
        int foundCounterIndex = -1;
        for (int i = 0; i < weaponUI.Count; i++)
        {
            if (weaponUI[i].WeaponUIIndex == weaponIndex)
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

    void ChangeWeapon(WeaponController weapon)
    {
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(WeaponPanel);
    }
}
