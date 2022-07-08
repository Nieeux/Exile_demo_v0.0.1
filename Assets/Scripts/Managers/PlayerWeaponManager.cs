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

    WeaponController[] WeaponSlots = new WeaponController[2];
    public Transform DefaultWeaponPosition;
    public Transform WeaponContainer;
    public Transform DownWeaponPosition;

    public int ActiveWeaponIndex { get; private set; }
    public bool IsAiming { get; private set; }

    public UnityAction<WeaponController> OnSwitchedToWeapon;
    public UnityAction<WeaponController, int> OnAddedWeapon;
    public UnityAction<WeaponController, int> OnRemovedWeapon;


    int m_WeaponSwitchNewWeaponIndex;
    float m_TimeStartedWeaponSwitch;
    WeaponSwitchState m_WeaponSwitchState;

    public float WeaponSwitchDelay = 1f;
    PlayerInput InputHandler;
    InventoryItem inventoryItem;


    Vector3 m_WeaponMainLocalPosition;

    [Header("Gun Recoil")]
    public Transform CameraRecoil;
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    public float dropForwardForce, dropUpwardForce;

    void Start()
    {
        //ActiveWeaponIndex = -1;
        m_WeaponSwitchState = WeaponSwitchState.Down;
        OnSwitchedToWeapon += OnWeaponSwitched;
        InputHandler = GetComponent<PlayerInput>();
    }
    private void Awake()
    {
        PlayerWeaponManager.Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        WeaponController activeWeapon = GetActiveWeapon();

        //Recoil sung
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        CameraRecoil.transform.localRotation = Quaternion.Euler(currentRotation);

        // weapon switch handling
        if ((activeWeapon == null || !activeWeapon.IsCharging) && (m_WeaponSwitchState == WeaponSwitchState.Up || m_WeaponSwitchState == WeaponSwitchState.Down))
        {
            int switchWeaponInput = InputHandler.GetSwitchWeaponInput();
            if (switchWeaponInput != 0)
            {
                bool switchUp = switchWeaponInput > 0;
                SwitchWeapon(switchUp);
            }

        }
        //Drop Weapon
        if (activeWeapon != null)
        {
            if (InputHandler.DropWeaponInput())
            {
                Debug.Log("DropWeapon");
                Interactable weaponIndex = activeWeapon.WeaponIndex;
                weaponIndex.RemoveObject();
            }
        }
        //Neu khong co vu khi trong inventory thi ActiveWeaponIndex = -1
        if (activeWeapon == null)
        {
            ActiveWeaponIndex = -1;
        }

    }
    public void RecoilFire()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

    private void LateUpdate()
    {
        UpdateWeaponSwitching();
        // Set final weapon socket position based on all the combined animation influences
        WeaponContainer.localPosition = m_WeaponMainLocalPosition;
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
            m_WeaponMainLocalPosition = Vector3.Lerp(DefaultWeaponPosition.localPosition,
                DownWeaponPosition.localPosition, switchingTimeFactor);
        }
        else if (m_WeaponSwitchState == WeaponSwitchState.PutUpNew)
        {
            m_WeaponMainLocalPosition = Vector3.Lerp(DownWeaponPosition.localPosition,
                DefaultWeaponPosition.localPosition, switchingTimeFactor);
        }
    }

    //Add v? khí vào tay
    public bool AddWeapon(WeaponController weaponPrefab, InventoryItem item)
    {
        // search our weapon slots for the first free one, assign the weapon to it, and return true if we found one. Return false otherwise
        for (int i = 0; i < WeaponSlots.Length; i++)
        {
            // only add the weapon if the slot is free
            if (WeaponSlots[i] == null)
            {
                // spawn the weapon prefab as child of the weapon socket

                WeaponController weaponInstance = Instantiate(weaponPrefab, WeaponContainer);
                weaponInstance.transform.localPosition = Vector3.zero;
                weaponInstance.transform.localRotation = Quaternion.identity;
                //Tat collider
                weaponInstance.coll.enabled = false;

                weaponInstance.rb.isKinematic = true;

                // tao thuoc tinh moi cho Weapon
                InventoryItem inventoryItem = weaponPrefab.GunStats;
                weaponInstance.GunStats = inventoryItem;

                // Set owner to this gameObject so the weapon can alter projectile/damage logic accordingly
                weaponInstance.Owner = gameObject;
                weaponInstance.SourcePrefab = weaponPrefab.gameObject;
                weaponInstance.ShowWeapon(false);

                WeaponSlots[i] = weaponInstance;

                if (OnAddedWeapon != null)
                {
                    OnAddedWeapon.Invoke(weaponInstance, i);
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
        if (index >= 0 &&
            index < WeaponSlots.Length)
        {
            return WeaponSlots[index];
        }

        // if we didn't find a valid active weapon in our weapon slots, return null
        return null;
    }
    // Iterate on all weapon slots to find the next valid weapon to switch to
    public void SwitchWeapon(bool ascendingOrder)
    {
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
                m_WeaponMainLocalPosition = DownWeaponPosition.localPosition;
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
    public bool RemoveWeapon(WeaponController weaponInstance, InventoryItem item)
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
                Destroy(weaponInstance.gameObject);
                WeaponController Instance = Instantiate(weaponInstance.GunStats.prefab, WeaponContainer.transform.position + (WeaponContainer.transform.forward), Quaternion.identity).GetComponent<WeaponController>();
                InventoryItem inventoryItem = weaponInstance.GunStats;
                Instance.GunStats = inventoryItem;

                Instance.rb.velocity = GetComponent<CharacterController>().velocity;
                Instance.rb.AddForce(transform.forward * dropForwardForce, ForceMode.Impulse);
                Instance.rb.AddForce(transform.up * dropUpwardForce, ForceMode.Impulse);
                //Add random rotation
                float random = Random.Range(-1f, 1f);
                Instance.rb.AddTorque(new Vector3(random, random, random) * 10);


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
}
