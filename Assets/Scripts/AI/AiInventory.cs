using UnityEngine;
using Random = System.Random;

public class AiInventory : MonoBehaviour
{
    public WeaponController CurrentWeapon;
    public ItemStats WeaponStats;
    public ItemStats currentArmor;
    WeaponIK weaponIK;
    AIController controller;
    private WeaponController brokeWeapon;
    public Transform WeaponContainer;
    private Random random;

    [Header("Rare")]
    public float Original;
    public float Upgrade;
    public float Advanced;


    private void Start()
    {
        this.random = new Random();
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
        GetRandomDrop(Original, Upgrade, Advanced);
    }

    private void GetRandomDrop(float Original, float Upgrade, float Advanced)
    {
        float num = Original + Upgrade + Advanced;
        float num2 = (float)random.NextDouble();
        if (num2 < Original / num)
        {
            DropHeal();
            return;
        }
        if (num2 < (Original + Upgrade) / num)
        {
            DropHeal();
            DropArmor();
            return;
        }
        DropWeapon();
        DropEquipMents();
        DropItems();
    }
    private void DropHeal()
    {
        ItemStats heal = ItemManager.Instance.GetHeal();
        ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
        inventoryItem.Getitem(heal);
        PickupItem pickup = Instantiate(inventoryItem.prefab, transform.position, transform.rotation).GetComponent<PickupItem>();
        //pickup.SetId(ItemManager.Instance.GetNewId());
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
    private void DropArmor()
    {
        if (currentArmor != null)
        {
            PickupItem pickup = Instantiate(currentArmor.prefab, transform.position, Quaternion.identity).GetComponent<PickupItem>();
            //pickup.SetId(ItemManager.Instance.GetNewId());
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
    private void DropWeapon()
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
    private void DropEquipMents()
    {
        ItemStats equip = ItemManager.Instance.GetRandomEquipments();
        ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
        inventoryItem.Getitem(equip);
        PickupItem pickup = Instantiate(inventoryItem.prefab, transform.position, transform.rotation).GetComponent<PickupItem>();
        //pickup.SetId(ItemManager.Instance.GetNewId());
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

    private void DropItems()
    {
        ItemStats equip = ItemManager.Instance.GetRandomItems();
        ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
        inventoryItem.Getitem(equip);
        PickupItem pickup = Instantiate(inventoryItem.prefab, transform.position, transform.rotation).GetComponent<PickupItem>();
        //pickup.SetId(ItemManager.Instance.GetNewId());
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

        ItemManager.Instance.getWeaponOriginal(RandomWeapon.id, WeaponContainer.transform.position, WeaponContainer.transform.rotation, WeaponContainer);
        CurrentWeapon = GetComponentInChildren<WeaponController>();

        WeaponStats = CurrentWeapon.GunStats;
        CurrentWeapon.coll.enabled = false;
        CurrentWeapon.rb.isKinematic = true;
        CurrentWeapon.canFire = false;
        //weaponIK.SetAimTransform(WeaponContainer);
    }
    public void StarterWeaponUpgrade()
    {
        ItemStats RandomWeapon = ItemManager.Instance.GetRandomWeapons();

        ItemManager.Instance.getWeaponUpgrade(RandomWeapon.id, WeaponContainer.transform.position, WeaponContainer.transform.rotation, WeaponContainer);
        CurrentWeapon = GetComponentInChildren<WeaponController>();

        WeaponStats = CurrentWeapon.GunStats;
        CurrentWeapon.coll.enabled = false;
        CurrentWeapon.rb.isKinematic = true;
        CurrentWeapon.canFire = false;
        //weaponIK.SetAimTransform(WeaponContainer);
    }
    public void StarterWeaponAdvanced()
    {
        ItemStats RandomWeapon = ItemManager.Instance.GetRandomWeapons();

        ItemManager.Instance.getweaponAdvanced(RandomWeapon.id, WeaponContainer.transform.position, WeaponContainer.transform.rotation, WeaponContainer);
        CurrentWeapon = GetComponentInChildren<WeaponController>();

        WeaponStats = CurrentWeapon.GunStats;
        CurrentWeapon.coll.enabled = false;
        CurrentWeapon.rb.isKinematic = true;
        CurrentWeapon.canFire = false;
        //weaponIK.SetAimTransform(WeaponContainer);
    }
}
