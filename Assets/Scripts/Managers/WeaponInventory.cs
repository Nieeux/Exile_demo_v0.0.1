using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

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
    PlayerInput InputHandler;
    PlayerMovement playerMovement;
    PlayerStats playerStats;

    public Vector3 DownPosition = new Vector3(0.124f, -0.12f, 0);
    public Vector3 DownRotation = new Vector3(20, -40, 40);
    public Vector3 defuRotation = new Vector3(0, 0, 0);
    public Vector3 defuPosition = new Vector3(0, 0, 0);
    Vector3 Rot;

    private WeaponController brokeWeapon;

    public float weaponWeight;
    [Header("Buff")]
    public float critcal;
    public float heal;
    public float bioTech;
    public float speed;

   [Header("Gun Recoil")]
    public Transform CameraRecoil;
    private Vector3 currentRotation;
    private Vector3 targetRotation;


    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    [SerializeField] private float CrouchrecoilY;
    [SerializeField] private float CrouchreturnSpeed;

    [SerializeField] private float currentReturnSpeed;
    [SerializeField] private float currentrecoilY;

    public float dropForwardForce, dropUpwardForce;

    private void Awake()
    {
        WeaponInventory.Instance = this;
    }

    void Start()
    {
        //ActiveWeaponIndex = -1;
        //WeaponSwitchState = WeaponState.PutDown;
        OnSwitchedToWeapon += OnWeaponSwitched;
        InputHandler = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();

    }

    // Update is called once per frame
    void Update()
    {
        WeaponController activeWeapon = GetActiveWeapon();

        //currentReturnSpeed = playerMovement.isCrouching() ? CrouchreturnSpeed : returnSpeed;
        //currentrecoilY = playerMovement.isCrouching() ? CrouchrecoilY : recoilY;
        //Recoil sung

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, currentReturnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        CameraRecoil.transform.localRotation = Quaternion.Euler(currentRotation);

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
                weaponIndex.RemoveObject();
            }
        }
        //Neu khong co vu khi dang active thi ha het vu khi
        if (activeWeapon == null)
        {
            ActiveWeaponIndex = -1;
        }

    }
    //public void RecoilFire()
    //{
       // targetRotation += new Vector3(recoilX, Random.Range(-currentrecoilY, currentrecoilY), Random.Range(-recoilZ, recoilZ));
    //}

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
                critcal += 0.1f;

            }

            if (buffsInActive[i].name == "HealTech")
            {
                heal += 5f;
            }

            if (buffsInActive[i].name == "BioTech")
            {
                bioTech += 1.2f;
                speed += 0.5f;
            }

        }

        base.Invoke("UpdateUI", 0.2f);
    }
    private void RefreshBuffsModified()
    {
        critcal = 0;
        heal = 0;
        bioTech = 0;
        speed = 0;

    }
    public float GetBuffCritical()
    {
        float n = critcal;
        return n;
    }
    public float GetBuffHealTech()
    {
        float n = heal;
        return n;
    }
    public float GetBuffSpeed()
    {
        float n = speed;
        return n;
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

    public float GetWeight()
    {
        float n = bioTech;
        if (n == 0)
        {
            return weaponWeight;
        }
        return weaponWeight / bioTech;
    }

    private void UpdateUI()
    {
        UIPlayerStats.Instance.UpdateStatsPlayer();
    }
    private bool inventoryFull()
    {
        for (int i = 0; i < WeaponSlots.Length; i++)
        {
            if (WeaponSlots[i] == null)
            {
                return false;
            }
        }
        return true;
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

                if (WeaponInActive == null)
                {
                    this.WeaponInActive = weaponPrefab;

                    UpdateBuffsModified(weaponPrefab);
                }

                EquipWeightModified(item);

                weaponPrefab.ShowWeapon(false);
                WeaponSlots[i] = weaponPrefab;

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
