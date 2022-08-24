using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Inventory))]
public class WeaponInventory : MonoBehaviour
{
    public static WeaponInventory Instance;

    public enum WeaponState
    {
        PutUp,
        PutDown,
        Switch,
        PutUpNew,

    }

    public WeaponController[] WeaponSlots = new WeaponController[2];
    public WeaponController WeaponInActive;
    public List<Buff> buffsInActive;

    public Transform WeaponContainer;

    public int ActiveWeaponIndex { get; private set; }
    public int index;

    public UnityAction<WeaponController> OnSwitchedToWeapon;
    public UnityAction<WeaponController, int> OnAddedWeapon;
    public UnityAction<WeaponController, int> OnRemovedWeapon;

 

    int m_WeaponSwitchNewWeaponIndex;
    float m_TimeStartedWeaponSwitch;
    public WeaponState WeaponSwitchState;

    public float WeaponSwitchDelay = 1f;
    Inventory inventory;
    PlayerInput InputHandler;

    public Vector3 DownPosition = new Vector3(0.124f, -0.12f, 0);
    public Vector3 DownRotation = new Vector3(20, -40, 40);
    public Vector3 defuRotation = new Vector3(0, 0, 0);
    public Vector3 defuPosition = new Vector3(0, 0, 0);
    Vector3 Rot;

    private WeaponController brokeWeapon;

    public float weaponWeight;
    [Header("Buff")]
    public UnityAction<float, bool> critcal;
    public UnityAction<float, bool> heal;
    public UnityAction<float, bool> bioTech;
    public UnityAction<float, bool> speed;

    [Header("SFX")]
    public AudioSource Sfx;
    public AudioClip InteractSfx;

    private void Awake()
    {
        WeaponInventory.Instance = this;
    }

    void Start()
    {
        if (Sfx == null)
        {
            Sfx.GetComponent<AudioSource>();
        }
        //ActiveWeaponIndex = -1;
        //WeaponSwitchState = WeaponState.PutDown;
        OnSwitchedToWeapon += OnWeaponSwitched;
        inventory = GetComponent<Inventory>();
        InputHandler = GetComponent<PlayerInput>();

    }

    // Update is called once per frame
    void Update()
    {
        WeaponController activeWeapon = GetActiveWeapon();

        // weapon switch handling
        if (ActiveWeaponIndex != -1 && (activeWeapon == null || !activeWeapon.IsCharging) && (WeaponSwitchState == WeaponState.PutUp || WeaponSwitchState == WeaponState.PutDown))
        {
            int switchWeaponInput = InputHandler.GetSwitchWeapon();
            if (switchWeaponInput != 0)
            {
                SwitchWeapon();
            }

        }
        //Drop Weapon
        if (activeWeapon != null)
        {
            if (InputHandler.GetDropWeapon() && activeWeapon.canDrop)
            {
                Debug.Log("DropWeapon");
                Interact weaponIndex = activeWeapon.WeaponIndex;
                weaponIndex.DropObject();
            }
        }
        //Neu khong co vu khi dang active thi ha het vu khi
        if (activeWeapon == null)
        {
            ActiveWeaponIndex = -1;
        }

    }


    private void LateUpdate()
    {
        UpdateWeaponSwitching();
    }

    void UpdateWeaponSwitching()
    {

        float switchingTimeFactor = 0f;

        if (WeaponSwitchDelay == 0f)
        {
            switchingTimeFactor = 1f;
        }
        else
        {
            switchingTimeFactor = Mathf.Clamp01((Time.time - m_TimeStartedWeaponSwitch) / WeaponSwitchDelay);
        }

        if (switchingTimeFactor >= 1f)
        {
            if (WeaponSwitchState == WeaponState.Switch)
            {
                // Deactivate old weapon
                WeaponController oldWeapon = GetWeaponAtSlotIndex(ActiveWeaponIndex);
                if (oldWeapon != null)
                {
                    oldWeapon.ShowWeapon(false);
                    this.WeaponInActive = oldWeapon;
                   
                }

                ActiveWeaponIndex = m_WeaponSwitchNewWeaponIndex;
                switchingTimeFactor = 0f;

                // Activate new weapon
                WeaponController newWeapon = GetWeaponAtSlotIndex(ActiveWeaponIndex);
                if (OnSwitchedToWeapon != null)
                {
                    OnSwitchedToWeapon.Invoke(newWeapon);
                    this.WeaponInActive = newWeapon;
                }

                if (newWeapon)
                {
                    m_TimeStartedWeaponSwitch = Time.time;
                    WeaponSwitchState = WeaponState.PutUpNew;
                    RefreshBuffsModified();
                    UpdateBuffsModified(WeaponInActive);
                }
                else
                {
                    // if new weapon is null, don't follow through with putting weapon back up
                    WeaponSwitchState = WeaponState.PutDown;
                }
            }
            else if (WeaponSwitchState == WeaponState.PutUpNew)
            {
                WeaponSwitchState = WeaponState.PutUp;
            }
        }

        // Handle transiting to new switch state


        // Handle moving the weapon socket position for the animated weapon switching
        if (WeaponSwitchState == WeaponState.Switch)
        {
            Rot = Vector3.Slerp(Rot, DownRotation, switchingTimeFactor);
            WeaponContainer.localRotation = Quaternion.Euler(Rot);
            WeaponContainer.localPosition = Vector3.Lerp(defuPosition, DownPosition, switchingTimeFactor);
        }
        if (WeaponSwitchState == WeaponState.PutUpNew)
        {
            Rot = Vector3.Slerp(Rot, defuRotation, switchingTimeFactor);
            WeaponContainer.localRotation = Quaternion.Euler(Rot);
            WeaponContainer.localPosition = Vector3.Lerp(DownPosition, defuPosition, switchingTimeFactor);
        }
        if (WeaponSwitchState == WeaponState.PutDown)
        {
            Rot = Vector3.Slerp(Rot, DownRotation, 5f * Time.deltaTime);
            WeaponContainer.localRotation = Quaternion.Euler(Rot);
            WeaponContainer.localPosition = Vector3.Lerp(WeaponContainer.localPosition, DownPosition, switchingTimeFactor);
        }
    }

    private void UpdateBuffsModified(WeaponController WeaponActive)
    {
        
        if (WeaponActive == null)
        {
            return;
        }
        this.buffsInActive = WeaponActive.GunStats.buffs;

        for (int i = 0; i < this.buffsInActive.Count; i++)
        {

            if (buffsInActive[i].name == "Critical")
            {
                if (critcal != null)
                {
                    critcal.Invoke(0.1f,false);
                }
            }

            if (buffsInActive[i].name == "HealTech")
            {
                if (heal != null)
                {
                    heal.Invoke(5f, false);
                }
            }

            if (buffsInActive[i].name == "BioTech")
            {
                if (bioTech != null)
                {
                    bioTech.Invoke(0.2f, false);
                }
                if (speed != null)
                {
                    speed.Invoke(0.5f, false);
                }
            }

        }

        base.Invoke("UpdateUI", 0.2f);
    }
    private void RefreshBuffsModified()
    {
        if (critcal != null)
        {
            critcal.Invoke(0, true);
        }
        if (heal != null)
        {
            heal.Invoke(0, true);
        }
        if (bioTech != null)
        {
            bioTech.Invoke(0, true);
        }
        if (speed != null)
        {
            speed.Invoke(0, true);
        }
    }

    public float GetWeight()
    {
        return weaponWeight;
    }

    private void EquipWeightModified(ItemStats Item)
    {
        weaponWeight += Item.Weight;

        base.Invoke("UpdateUI", 0.2f);
    }
    private void UnequipWeightModified(ItemStats Item)
    {
        weaponWeight -= Item.Weight;

        base.Invoke("UpdateUI", 0.2f);
    }


    private void UpdateUI()
    {
        UIPlayerStats.Instance.UpdateStatsPlayer();
    }

    public bool AddWeapon(WeaponController weaponPrefab, ItemStats item)
    {

        for (int i = 0; i < WeaponSlots.Length; i++)
        {

            if (WeaponSlots[i] == null)
            {
                weaponPrefab.transform.SetParent(WeaponContainer);
                weaponPrefab.transform.localPosition = Vector3.zero;
                weaponPrefab.transform.localRotation = Quaternion.identity;
                weaponPrefab.coll.enabled = false;
                weaponPrefab.rb.isKinematic = true;

                Sfx.clip = InteractSfx;
                this.Sfx.Play();

                if (WeaponInActive == null)
                {
                    this.WeaponInActive = weaponPrefab;

                    UpdateBuffsModified(weaponPrefab);
                }

                EquipWeightModified(item);

                weaponPrefab.ShowWeapon(false);
                WeaponSlots[i] = weaponPrefab;
                weaponPrefab.WeaponID = i;

                if (OnAddedWeapon != null)
                {
                    OnAddedWeapon.Invoke(weaponPrefab, i);
                }
                UIEvents.Instance.AddPickup(item);
                return true;
            }
        }

        if (GetActiveWeapon() == null)
        {
            SwitchWeapon();
        }

        return false;
    }

    public WeaponController GetActiveWeapon()
    {
        return GetWeaponAtSlotIndex(ActiveWeaponIndex);
    }
    public WeaponController GetWeaponAtSlotIndex(int index)
    {

        if (index >= 0 && index < WeaponSlots.Length)
        {
            return WeaponSlots[index];
        }

        return null;
    }


    public void SwitchWeapon()
    {
        int newWeaponIndex = -1;
        for (int i = 0; i < WeaponSlots.Length; i++)
        {

            if (i != ActiveWeaponIndex && GetWeaponAtSlotIndex(i) != null)
            {
                newWeaponIndex = i;
            }
        }
        SwitchToWeaponIndex(newWeaponIndex);
    }

    public void SwitchToWeaponIndex(int newWeaponIndex, bool force = false)
    {
        if (force || (newWeaponIndex != ActiveWeaponIndex && newWeaponIndex >= 0))
        {

            m_WeaponSwitchNewWeaponIndex = newWeaponIndex;
            m_TimeStartedWeaponSwitch = Time.time;

            if (GetActiveWeapon() == null)
            {
                WeaponSwitchState = WeaponState.PutUpNew;
                ActiveWeaponIndex = m_WeaponSwitchNewWeaponIndex;

                WeaponController newWeapon = GetWeaponAtSlotIndex(m_WeaponSwitchNewWeaponIndex);
                this.WeaponInActive = newWeapon;
                RefreshBuffsModified();
                UpdateBuffsModified(WeaponInActive);

                if (OnSwitchedToWeapon != null)
                {
                    OnSwitchedToWeapon.Invoke(newWeapon);
                }
            }
            else
            {
                WeaponSwitchState = WeaponState.Switch;
            }
        }
    }

    void OnWeaponSwitched(WeaponController newWeapon)
    {
        if (newWeapon != null)
        {
            newWeapon.ShowWeapon(true);
        }
    }

    public bool RepairWeapon(ItemStats item)
    {
        if(WeaponInActive == null)
        {
            return false;
        }
        if (WeaponInActive.GunStats.CurrentDurability >= WeaponInActive.GunStats.Durability)
        {
            return false;
        }

        this.WeaponInActive.GunStats.CurrentDurability += item.repair;
        this.WeaponInActive.GunStats.CurrentDurability = Mathf.Clamp(WeaponInActive.GunStats.CurrentDurability, 0f, WeaponInActive.GunStats.Durability);
        WeaponUIManager.Instance.updateUI(ActiveWeaponIndex);
        return true;
    }

    public int WeaponPrice(WeaponController weaponInstance, ItemStats item)
    {
        int n = (int)weaponInstance.GunStats.CurrentDurability;

        if (item.Rare == ItemStats.ItemRare.upgrade)
        {
            n *= 2;
        }
        if (item.Rare == ItemStats.ItemRare.advanced)
        {
            n *= 4;
        }
        if (item.weaponType == ItemStats.WeaponType.AssaultRifles)
        {
            n *= 2;
        }
        
        if (weaponInstance.Pullet.ammoType == Bullet.AmmoType.PiercingAmmo)
        {
            n *= 2;
        }
        if (weaponInstance.Pullet.ammoType == Bullet.AmmoType.HighAmmo)
        {
            n *= 2;
        }
        if (weaponInstance.Pullet.IsShotGunShell == true)
        {
            n *= 2;
        }
        n *= item.buffs.Count;

        return n / 2;
    }
    private void PayMoney(WeaponController weaponInstance, ItemStats item)
    {
        int Moneys = WeaponPrice(weaponInstance, item);

        inventory.RewardMoney(Moneys);
    }
    public bool SellWeapon(WeaponController weaponInstance, ItemStats item)
    {

        for (int i = 0; i < WeaponSlots.Length; i++)
        {
            if (WeaponSlots[i] == weaponInstance)
            {
                WeaponSlots[i] = null;

                if (OnRemovedWeapon != null)
                {
                    OnRemovedWeapon.Invoke(weaponInstance, i);
                }
                this.WeaponInActive = null;
                RefreshBuffsModified();
                UnequipWeightModified(item);
                Destroy(weaponInstance.WeaponRoot);
                PayMoney(weaponInstance, item);

                if (i == ActiveWeaponIndex)
                {
                    SwitchWeapon();
                }

                return true;
            }
        }

        return false;
    }

    public bool DropWeapon(WeaponController weaponInstance, ItemStats item)
    {

        for (int i = 0; i < WeaponSlots.Length; i++)
        {
            if (WeaponSlots[i] == weaponInstance)
            {
                WeaponSlots[i] = null;

                if (OnRemovedWeapon != null)
                {
                    OnRemovedWeapon.Invoke(weaponInstance, i);
                }

                this.WeaponInActive = null;
                RefreshBuffsModified();

                weaponInstance.canFire = false;
                weaponInstance.GetComponent<Collider>().enabled = true;
                weaponInstance.rb.isKinematic = false;

                UnequipWeightModified(item);

                RaycastHit hit;
                if (Physics.Raycast(weaponInstance.transform.position,Vector3.down,out hit, 10, LayerMask.GetMask("Ground")))
                {
                    weaponInstance.transform.SetParent(hit.collider.gameObject.transform.parent);
                }
                else
                {
                    weaponInstance.transform.SetParent(null);
                }

                if (i == ActiveWeaponIndex)
                {
                    SwitchWeapon();
                }

                return true;
            }
        }

        return false;
    }

    public bool BrokeWeapon(WeaponController weaponInstance, ItemStats item)
    {
       
        for (int i = 0; i < WeaponSlots.Length; i++)
        {
            if (WeaponSlots[i] == weaponInstance)
            {
                WeaponSlots[i] = null;

                if (OnRemovedWeapon != null)
                {
                    OnRemovedWeapon.Invoke(weaponInstance, i);
                }
                this.WeaponInActive = null;
                RefreshBuffsModified();
                UnequipWeightModified(item);

                weaponInstance.GunStats = null;
                weaponInstance.GetComponent<PickupWeapon>().item = null;
                brokeWeapon = weaponInstance;
                base.Invoke("StartFade", 5);

                weaponInstance.canFire = false;
                weaponInstance.rb.isKinematic = false;
                weaponInstance.GetComponent<Collider>().enabled = true;

                weaponInstance.transform.SetParent(null);

                if (i == ActiveWeaponIndex)
                {
                    SwitchWeapon();
                }

                return true;
            }
        }

        return false;
    }
    private void StartFade()
    {
        brokeWeapon.rb.drag = 5;
        brokeWeapon.coll.enabled = false;
        Destroy(brokeWeapon.WeaponRoot, 2);
    }

}
