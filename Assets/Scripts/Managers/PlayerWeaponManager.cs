using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance;

    public enum WeaponSwitchState
    {
        Up,
        Down,
        PutDownPrevious,
        PutUpNew,
    }

    public WeaponController[] WeaponSlots = new WeaponController[2];
    public Transform WeaponContainer;

    public int ActiveWeaponIndex { get; private set; }
    public int index;
    public bool IsAiming { get; private set; }

    public UnityAction<WeaponController> OnSwitchedToWeapon;
    public UnityAction<WeaponController, int> OnAddedWeapon;
    public UnityAction<WeaponController, int> OnRemovedWeapon;


    int m_WeaponSwitchNewWeaponIndex;
    float m_TimeStartedWeaponSwitch;
    WeaponSwitchState m_WeaponSwitchState;

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
        PlayerWeaponManager.Instance = this;
    }

    void Start()
    {
        //ActiveWeaponIndex = -1;
        m_WeaponSwitchState = WeaponSwitchState.Down;
        OnSwitchedToWeapon += OnWeaponSwitched;
        InputHandler = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        WeaponController activeWeapon = GetActiveWeapon();
        index = ActiveWeaponIndex;

        currentReturnSpeed = playerMovement.isCrouching() ? CrouchreturnSpeed : returnSpeed;
        currentrecoilY = playerMovement.isCrouching() ? CrouchrecoilY : recoilY;
        //Recoil sung

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, currentReturnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        CameraRecoil.transform.localRotation = Quaternion.Euler(currentRotation);

        // weapon switch handling
        if (ActiveWeaponIndex != -1 && (activeWeapon == null || !activeWeapon.IsCharging) && (m_WeaponSwitchState == WeaponSwitchState.Up || m_WeaponSwitchState == WeaponSwitchState.Down))
        {
            int switchWeaponInput = InputHandler.GetSwitchWeapon();
            if (switchWeaponInput != 0)
            {
                bool switchUp = switchWeaponInput > 0;
                SwitchWeapon(switchUp);
            }

        }
        //Drop Weapon
        if (activeWeapon != null)
        {
            if (InputHandler.GetDropWeapon())
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
    public void RecoilFire()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-currentrecoilY, currentrecoilY), Random.Range(-recoilZ, recoilZ));
    }

    private void LateUpdate()
    {
        UpdateWeaponSwitching();
        // Set final weapon socket position based on all the combined animation influences
       // m_WeaponMainLocalPosition = WeaponContainer.localPosition;
    }

    void UpdateWeaponSwitching()
    {
        // Calculate the time ratio (0 to 1) since weapon switch was triggered
        float switchingTimeFactor = 0f;

        if (WeaponSwitchDelay == 0f)
        {
            switchingTimeFactor = 1f;
        }
        else
        {
            switchingTimeFactor = Mathf.Clamp01((Time.time - m_TimeStartedWeaponSwitch) / WeaponSwitchDelay);
        }

        // Handle transiting to new switch state
        if (switchingTimeFactor >= 1f)
        {
            if (m_WeaponSwitchState == WeaponSwitchState.PutDownPrevious)
            {
                // Deactivate old weapon
                WeaponController oldWeapon = GetWeaponAtSlotIndex(ActiveWeaponIndex);
                if (oldWeapon != null)
                {
                    oldWeapon.ShowWeapon(false);
                }

                ActiveWeaponIndex = m_WeaponSwitchNewWeaponIndex;
                switchingTimeFactor = 0f;

                // Activate new weapon
                WeaponController newWeapon = GetWeaponAtSlotIndex(ActiveWeaponIndex);
                if (OnSwitchedToWeapon != null)
                {
                    OnSwitchedToWeapon.Invoke(newWeapon);
                }

                if (newWeapon)
                {
                    m_TimeStartedWeaponSwitch = Time.time;
                    m_WeaponSwitchState = WeaponSwitchState.PutUpNew;
                }
                else
                {
                    // if new weapon is null, don't follow through with putting weapon back up
                    m_WeaponSwitchState = WeaponSwitchState.Down;
                }
            }
            else if (m_WeaponSwitchState == WeaponSwitchState.PutUpNew)
            {
                m_WeaponSwitchState = WeaponSwitchState.Up;
            }
        }

        // Handle moving the weapon socket position for the animated weapon switching
        if (m_WeaponSwitchState == WeaponSwitchState.PutDownPrevious)
        {
            Rot = Vector3.Slerp(Rot, DownRotation, switchingTimeFactor);
            WeaponContainer.localRotation = Quaternion.Euler(Rot);
            WeaponContainer.localPosition = Vector3.Lerp(defuPosition, DownPosition, switchingTimeFactor);
        }
        else if (m_WeaponSwitchState == WeaponSwitchState.PutUpNew)
        {
            Rot = Vector3.Slerp(Rot, defuRotation, switchingTimeFactor);
            WeaponContainer.localRotation = Quaternion.Euler(Rot);
            WeaponContainer.localPosition = Vector3.Lerp(DownPosition, defuPosition, switchingTimeFactor);
        }
    }
    public void CloseWeapon()
    {
        if (GetActiveWeapon() != null)
        {
            SwitchToWeaponIndex(-1, true);
        }
    }
    public void ShowWeapon()
    {
        if (ActiveWeaponIndex == -1)
        {
            SwitchWeapon(true);
        }
    }
    //Add v? kh� v�o tay
    public bool AddWeapon(WeaponController weaponPrefab, ItemStats item)
    {
        // search our weapon slots for the first free one, assign the weapon to it, and return true if we found one. Return false otherwise
        for (int i = 0; i < WeaponSlots.Length; i++)
        {
            // only add the weapon if the slot is free
            if (WeaponSlots[i] == null)
            {
                // spawn the weapon prefab as child of the weapon socket

                //weaponPrefab.transform.localPosition = WeaponContainer.localPosition;
                weaponPrefab.transform.SetParent(WeaponContainer);
                //WeaponController weaponInstance = Instantiate(weaponPrefab, WeaponContainer);
                weaponPrefab.transform.localPosition = Vector3.zero;
                weaponPrefab.transform.localRotation = Quaternion.identity;
                //Tat collider
                weaponPrefab.coll.enabled = false;

                weaponPrefab.rb.isKinematic = true;

                // tao thuoc tinh moi cho Weapon
                //weaponInstance.GunStats = item;
                //weaponInstance.GetComponent<PickupWeapon>().item = item;

                // Add Inventory Able
                //inventory.Equipments(item);

                weaponPrefab.ShowWeapon(false);
                WeaponSlots[i] = weaponPrefab;

                if (OnAddedWeapon != null)
                {
                    OnAddedWeapon.Invoke(weaponPrefab, i);
                }

                return true;
            }
        }

        // Handle auto-switching to weapon if no weapons currently
        if (GetActiveWeapon() == null)
        {
            SwitchWeapon(true);
        }

        return false;
    }
    public WeaponController GetActiveWeapon()
    {
        return GetWeaponAtSlotIndex(ActiveWeaponIndex);
    }
    public WeaponController GetWeaponAtSlotIndex(int index)
    {
        // find the active weapon in our weapon slots based on our active weapon index
        if (index >= 0 && index < WeaponSlots.Length)
        {
            return WeaponSlots[index];
        }

        return null;
    }

    // Iterate on all weapon slots to find the next valid weapon to switch to
    public void SwitchWeapon(bool ascendingOrder)
    {
        Debug.Log("SwitchWeapon");
        int newWeaponIndex = -1;
        int closestSlotDistance = WeaponSlots.Length;
        for (int i = 0; i < WeaponSlots.Length; i++)
        {
            // If the weapon at this slot is valid, calculate its "distance" from the active slot index (either in ascending or descending order)
            // and select it if it's the closest distance yet
            if (i != ActiveWeaponIndex && GetWeaponAtSlotIndex(i) != null)
            {
                int distanceToActiveIndex = GetDistanceBetweenWeaponSlots(ActiveWeaponIndex, i, ascendingOrder);

                if (distanceToActiveIndex < closestSlotDistance)
                {
                    closestSlotDistance = distanceToActiveIndex;
                    newWeaponIndex = i;
                }
            }
        }

        // Handle switching to the new weapon index
        SwitchToWeaponIndex(newWeaponIndex);
    }
    // Calculates the "distance" between two weapon slot indexes
    // For example: if we had 5 weapon slots, the distance between slots #2 and #4 would be 2 in ascending order, and 3 in descending order
    int GetDistanceBetweenWeaponSlots(int fromSlotIndex, int toSlotIndex, bool ascendingOrder)
    {
        int distanceBetweenSlots = 0;

        if (ascendingOrder)
        {
            distanceBetweenSlots = toSlotIndex - fromSlotIndex;
        }
        else
        {
            distanceBetweenSlots = -1 * (toSlotIndex - fromSlotIndex);
        }

        if (distanceBetweenSlots < 0)
        {
            distanceBetweenSlots = WeaponSlots.Length + distanceBetweenSlots;
        }

        return distanceBetweenSlots;
    }
    // Switches to the given weapon index in weapon slots if the new index is a valid weapon that is different from our current one
    public void SwitchToWeaponIndex(int newWeaponIndex, bool force = false)
    {
        if (force || (newWeaponIndex != ActiveWeaponIndex && newWeaponIndex >= 0))
        {
            // Store data related to weapon switching animation
            m_WeaponSwitchNewWeaponIndex = newWeaponIndex;
            m_TimeStartedWeaponSwitch = Time.time;

            // Handle case of switching to a valid weapon for the first time (simply put it up without putting anything down first)
            if (GetActiveWeapon() == null)
            {
                m_WeaponSwitchState = WeaponSwitchState.PutUpNew;
                ActiveWeaponIndex = m_WeaponSwitchNewWeaponIndex;

                WeaponController newWeapon = GetWeaponAtSlotIndex(m_WeaponSwitchNewWeaponIndex);
                if (OnSwitchedToWeapon != null)
                {
                    OnSwitchedToWeapon.Invoke(newWeapon);
                }
            }
            // otherwise, remember we are putting down our current weapon for switching to the next one
            else
            {
                m_WeaponSwitchState = WeaponSwitchState.PutDownPrevious;
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
        // Look through our slots for that weapon
        for (int i = 0; i < WeaponSlots.Length; i++)
        {
            // when weapon found, remove it
            if (WeaponSlots[i] == weaponInstance)
            {
                WeaponSlots[i] = null;

                if (OnRemovedWeapon != null)
                {
                    OnRemovedWeapon.Invoke(weaponInstance, i);
                }

                //Destroy(weaponInstance.gameObject);
                //WeaponController Instance = Instantiate(weaponInstance.GunStats.prefab, CamDrop.transform.position, Quaternion.identity).GetComponent<WeaponController>();
                //Instance.GunStats = item;
                //Instance.GetComponent<PickupWeapon>().item = item;

                weaponInstance.canFire = false;
                weaponInstance.rb.isKinematic = false;
                weaponInstance.GetComponent<Collider>().enabled = true;
                weaponInstance.rb.velocity = GetComponent<CharacterController>().velocity;
                weaponInstance.rb.AddForce(WeaponContainer.transform.forward * dropForwardForce, ForceMode.Impulse);
                weaponInstance.rb.AddForce(WeaponContainer.transform.up * dropUpwardForce, ForceMode.Impulse);

                //inventory.Unequipments(item);
                //Add random rotation
                float random = Random.Range(-1f, 1f);
                weaponInstance.rb.AddTorque(new Vector3(random, random, random) * 10);

                weaponInstance.transform.SetParent(null);

                //OnHit(weaponInstance.GetComponent<Collider>());
                // Handle case of removing active weapon (switch to next weapon)
                if (i == ActiveWeaponIndex)
                {
                    SwitchWeapon(true);
                }

                return true;
            }
        }

        return false;
    }
    public void OnHit(Collider collider)
    {
        HitAble damageable = collider.GetComponent<HitAble>();
        if (damageable)
        {
            Inventory.DamageResult damageMultiplier = Inventory.Instance.GetDamage();
            float damageMultiplier2 = damageMultiplier.damageMultiplier;
            bool crit = damageMultiplier.ItCrit;
            float Damage = (int)(playerStats.damage * damageMultiplier2);
            HitEffect hitEffect = HitEffect.AmmoNormal;
            if (crit)
            {
                hitEffect = HitEffect.Crit;
            }
            Vector3 pos = collider.transform.position;

            damageable.Damage(Damage, (int)hitEffect, pos);
        }
    }
    public bool BrokeWeapon(WeaponController weaponInstance, ItemStats item)
    {
        // Look through our slots for that weapon
        for (int i = 0; i < WeaponSlots.Length; i++)
        {
            // when weapon found, remove it
            if (WeaponSlots[i] == weaponInstance)
            {
                WeaponSlots[i] = null;

                if (OnRemovedWeapon != null)
                {
                    OnRemovedWeapon.Invoke(weaponInstance, i);
                }
                //Destroy(weaponInstance.gameObject);
                //WeaponController Instance = Instantiate(weaponInstance.GunStats.prefab, CamDrop.transform.position, Quaternion.identity).GetComponent<WeaponController>();
                weaponInstance.GunStats = null;
                weaponInstance.GetComponent<PickupWeapon>().item = null;
                brokeWeapon = weaponInstance;
                base.Invoke("StartFade", 5);

                weaponInstance.canFire = false;
                weaponInstance.rb.isKinematic = false;
                weaponInstance.GetComponent<Collider>().enabled = true;

                weaponInstance.transform.SetParent(null);
                // Handle case of removing active weapon (switch to next weapon)
                if (i == ActiveWeaponIndex)
                {
                    SwitchWeapon(true);
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
