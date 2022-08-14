using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiInventory : MonoBehaviour
{
    public WeaponController CurrentWeapon;
    public ItemStats WeaponStats;
    public ItemStats currentArmor;
    WeaponIK weaponIK;
    AIController controller;
    private WeaponController brokeWeapon;

    public Transform WeaponContainer;

    private void Start()
    {
        controller = GetComponent<AIController>();
        weaponIK = GetComponent<WeaponIK>();
        StarterArmor();
    }

    public void AiEquip(WeaponController weapon, ItemStats item)
    {
        WeaponController Weapon = Instantiate(weapon, WeaponContainer);
        Weapon.GunStats = item;
        Weapon.GetComponent<PickupWeapon>().item = item;

        CurrentWeapon = Weapon;

        Weapon.transform.localPosition = Vector3.zero;
        Weapon.transform.localRotation = Quaternion.identity;

        Weapon.coll.enabled = false;
        Weapon.rb.isKinematic = true;
        Weapon.canFire = false;

        WeaponStats = Weapon.GunStats;

        weaponIK.SetAimTransform(WeaponContainer);
    }

    public void DropItem()
    {
        if (currentArmor != null)
        {
            PickupItem pickup = Instantiate(currentArmor.prefab, transform.position, Quaternion.identity).GetComponent<PickupItem>();
            pickup.item = currentArmor;

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
    public void DropWeapon()
    {
        if (WeaponStats != null)
        {
            WeaponController Weapon = Instantiate(WeaponStats.prefab, transform.position, Quaternion.identity).GetComponent<WeaponController>();
            Weapon.GunStats = WeaponStats;
            Weapon.GetComponent<PickupWeapon>().item = WeaponStats;

            RaycastHit hit;
            if (Physics.Raycast(Weapon.transform.position, Vector3.down, out hit, 10, LayerMask.GetMask("Ground")))
            {
                Weapon.transform.SetParent(hit.collider.gameObject.transform.parent);
            }
            else
            {
                Weapon.transform.SetParent(null);
            }
        }

    }
    public void BrokeWeapon(ItemStats item)
    {
        if (WeaponStats.CurrentDurability <= 0 && WeaponStats != null)
        {
            WeaponController Weapon = Instantiate(item.prefab, transform.position, Quaternion.identity).GetComponent<WeaponController>();
            Weapon.GunStats = null;
            Weapon.GetComponent<PickupWeapon>().item = null;
            brokeWeapon = Weapon;

            Destroy(CurrentWeapon.WeaponRoot);
            this.CurrentWeapon = null;
            this.WeaponStats = null;
            base.Invoke("StartFade", 5);
        }

    }
    private void StartFade()
    {
        brokeWeapon.rb.drag = 5;
        brokeWeapon.coll.enabled = false;
        Destroy(brokeWeapon.WeaponRoot, 2);
    }

    public void StarterArmor()
    {
        ItemStats Armor = ItemManager.Instance.GetRandomArmor();
        ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
        inventoryItem.Getitem(Armor);
        currentArmor = inventoryItem;
    }
    public void StarterWeaponOriginal()
    {
        ItemStats RandomWeapon = ItemManager.Instance.GetRandomWeapons();

        Buff buff = ItemManager.Instance.GetBuff();
        Buff Debuff = ItemManager.Instance.GetDeBuff();

        ItemManager.Instance.getWeaponOriginal(RandomWeapon.id, buff.id, Debuff.id, WeaponContainer.transform.position, WeaponContainer.transform.rotation, WeaponContainer);
        CurrentWeapon = GetComponentInChildren<WeaponController>();

        WeaponStats = CurrentWeapon.GunStats;
        CurrentWeapon.coll.enabled = false;
        CurrentWeapon.rb.isKinematic = true;
        CurrentWeapon.canFire = false;
        weaponIK.SetAimTransform(WeaponContainer);
    }
    public void StarterWeaponUpgrade()
    {
        ItemStats RandomWeapon = ItemManager.Instance.GetRandomWeapons();

        Buff buff = ItemManager.Instance.GetBuff();
        Buff Debuff = ItemManager.Instance.GetDeBuff();

        ItemManager.Instance.getWeaponUpgrade(RandomWeapon.id, buff.id, Debuff.id, WeaponContainer.transform.position, WeaponContainer.transform.rotation, WeaponContainer);
        CurrentWeapon = GetComponentInChildren<WeaponController>();

        WeaponStats = CurrentWeapon.GunStats;
        CurrentWeapon.coll.enabled = false;
        CurrentWeapon.rb.isKinematic = true;
        CurrentWeapon.canFire = false;
        weaponIK.SetAimTransform(WeaponContainer);
    }
    public void StarterWeaponAdvanced()
    {
        ItemStats RandomWeapon = ItemManager.Instance.GetRandomWeapons();

        Buff buff = ItemManager.Instance.GetBuff();
        Buff Debuff = ItemManager.Instance.GetDeBuff();

        ItemManager.Instance.getweaponAdvanced(RandomWeapon.id, buff.id, Debuff.id, WeaponContainer.transform.position, WeaponContainer.transform.rotation, WeaponContainer);
        CurrentWeapon = GetComponentInChildren<WeaponController>();

        WeaponStats = CurrentWeapon.GunStats;
        CurrentWeapon.coll.enabled = false;
        CurrentWeapon.rb.isKinematic = true;
        CurrentWeapon.canFire = false;
        weaponIK.SetAimTransform(WeaponContainer);
    }
}
