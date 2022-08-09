using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class InventoryItem : MonoBehaviour
{
    public static InventoryItem Instance;

    public enum WeaponSwitchState
    {
        Up,
        Down,
        PutDownPrevious,
        PutUpNew,
    }
    public Item[] ItemSlots = new Item[5];
    public SortedDictionary<string, Item> sortedItem = new SortedDictionary<string, Item>();
    public Transform DefaultWeaponPosition;
    public Transform WeaponContainer;
    public Transform DownWeaponPosition;
    public Transform CamDrop;

    public UnityAction<Item> OnSwitchedToItem;
    public UnityAction<Item> OnAddedItem;
    public UnityAction<Item> OnRemovedItem;

    public int ActiveItemIndex { get; private set; }

    int m_ItemSwitchNewItemIndex;
    float m_TimeStartedWeaponSwitch;
    WeaponSwitchState m_WeaponSwitchState;

    public float WeaponSwitchDelay = 1f;
    PlayerInput InputHandler;

    Vector3 m_WeaponMainLocalPosition;

    public float dropForwardForce, dropUpwardForce;

    private void Awake()
    {
        InventoryItem.Instance = this;
    }
    public void Start()
    {
        m_WeaponSwitchState = WeaponSwitchState.Down;
        OnSwitchedToItem += OnWeaponSwitched;
        InputHandler = GetComponent<PlayerInput>();
    }
    void Update()
    {
        Item activeItem = GetActiveItem();

        // switch handling
        if (ActiveItemIndex != -1 && (activeItem == null || !activeItem.IsCharging) && (m_WeaponSwitchState == WeaponSwitchState.Up || m_WeaponSwitchState == WeaponSwitchState.Down))
        {
            /*
            int switchWeaponInput = InputHandler.GetSelectWeaponInput();
            if (switchWeaponInput != 0)
            {
                if (GetItemAtSlotIndex(switchWeaponInput - 1) != null)
                    SwitchToItemIndex(switchWeaponInput - 1);
            }
            */
        }
        //Drop Weapon
        if (activeItem != null)
        {
            if (InputHandler.GetDropWeapon())
            {
                Debug.Log("DropWeapon");
                Interact weaponIndex = activeItem.ItemIndex;
                weaponIndex.RemoveObject();
            }
        }
        //Neu khong co vu khi dang active thi ha het vu khi
        if (activeItem == null)
        {
            ActiveItemIndex = -1;
        }

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
                Item oldItem = GetItemAtSlotIndex(ActiveItemIndex);
                if (oldItem != null)
                {
                    oldItem.ShowItem(false);
                }

                ActiveItemIndex = m_ItemSwitchNewItemIndex;
                switchingTimeFactor = 0f;

                // Activate new weapon
                Item newItem = GetItemAtSlotIndex(ActiveItemIndex);
                if (OnSwitchedToItem != null)
                {
                    OnSwitchedToItem.Invoke(newItem);
                }

                if (newItem)
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

    public bool AddWeapon(Item weaponPrefab, ItemStats item)
    {
        // search our weapon slots for the first free one, assign the weapon to it, and return true if we found one. Return false otherwise
        for (int i = 0; i < ItemSlots.Length; i++)
        {
            // only add the weapon if the slot is free
            if (ItemSlots[i] == null)
            {
                // spawn the weapon prefab as child of the weapon socket

                Item weaponInstance = Instantiate(weaponPrefab, WeaponContainer);
                weaponInstance.transform.localPosition = Vector3.zero;
                weaponInstance.transform.localRotation = Quaternion.identity;
                //Tat collider
                weaponInstance.coll.enabled = false;

                weaponInstance.rb.isKinematic = true;

                // tao thuoc tinh moi cho Weapon
                ItemStats inventoryItem = weaponPrefab.ItemStats;
                weaponInstance.ItemStats = inventoryItem;

                // Set owner to this gameObject so the weapon can alter projectile/damage logic accordingly
                weaponInstance.Owner = gameObject;
                weaponInstance.SourcePrefab = weaponPrefab.gameObject;
                weaponInstance.ShowItem(false);

                ItemSlots[i] = weaponInstance;

                if (OnAddedItem != null)
                {
                    OnAddedItem.Invoke(weaponInstance);
                }

                return true;
            }
        }

        // Handle auto-switching to weapon if no weapons currently
        if (GetActiveItem() == null)
        {
            SwitchItem(true);
        }

        return false;
    }

    public Item GetActiveItem()
    {
        return GetItemAtSlotIndex(ActiveItemIndex);
    }

    public Item GetItemAtSlotIndex(int index)
    {
        // find the active weapon in our weapon slots based on our active weapon index
        if (index >= 0 && index < ItemSlots.Length)
        {
            return ItemSlots[index];
        }

        return null;
    }
    public void SwitchItem(bool ascendingOrder)
    {
        Debug.Log("SwitchItem");
        int newWeaponIndex = -1;
        int closestSlotDistance = ItemSlots.Length;
        for (int i = 0; i < ItemSlots.Length; i++)
        {
            // If the weapon at this slot is valid, calculate its "distance" from the active slot index (either in ascending or descending order)
            // and select it if it's the closest distance yet
            if (i != ActiveItemIndex && GetItemAtSlotIndex(i) != null)
            {
                int distanceToActiveIndex = GetDistanceBetweenItemSlots(ActiveItemIndex, i, ascendingOrder);

                if (distanceToActiveIndex < closestSlotDistance)
                {
                    closestSlotDistance = distanceToActiveIndex;
                    newWeaponIndex = i;
                }
            }
        }

        // Handle switching to the new weapon index
        SwitchToItemIndex(newWeaponIndex);
    }
    int GetDistanceBetweenItemSlots(int fromSlotIndex, int toSlotIndex, bool ascendingOrder)
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
            distanceBetweenSlots = ItemSlots.Length + distanceBetweenSlots;
        }

        return distanceBetweenSlots;
    }
    // Switches to the given weapon index in weapon slots if the new index is a valid weapon that is different from our current one
    public void SwitchToItemIndex(int newItemIndex, bool force = false)
    {
        if (force || (newItemIndex != ActiveItemIndex && newItemIndex >= 0))
        {
            // Store data related to weapon switching animation
            m_ItemSwitchNewItemIndex = newItemIndex;
            m_TimeStartedWeaponSwitch = Time.time;

            // Handle case of switching to a valid weapon for the first time (simply put it up without putting anything down first)
            if (GetActiveItem() == null)
            {
                m_WeaponMainLocalPosition = DownWeaponPosition.localPosition;
                m_WeaponSwitchState = WeaponSwitchState.PutUpNew;
                ActiveItemIndex = m_ItemSwitchNewItemIndex;

                Item newItem = GetItemAtSlotIndex(m_ItemSwitchNewItemIndex);
                if (OnSwitchedToItem != null)
                {
                    OnSwitchedToItem.Invoke(newItem);
                }
            }
            // otherwise, remember we are putting down our current weapon for switching to the next one
            else
            {
                m_WeaponSwitchState = WeaponSwitchState.PutDownPrevious;
            }
        }
    }
    void OnWeaponSwitched(Item newWeapon)
    {
        if (newWeapon != null)
        {
            newWeapon.ShowItem(true);
        }
    }

    public bool RemoveWeapon(Item weaponInstance, ItemStats item)
    {
        
        // Look through our slots for that weapon
        for (int i = 0; i < ItemSlots.Length; i++)
        {
            // when weapon found, remove it
            if (ItemSlots[i] == weaponInstance)
            {
                ItemSlots[i] = null;

                ItemSlots = ItemSlots.OrderBy(element => element == null).ToArray();
                if (OnRemovedItem != null)
                {
                    OnRemovedItem.Invoke(weaponInstance);
                }

                Destroy(weaponInstance.gameObject);
                Item Instance = Instantiate(weaponInstance.ItemStats.prefab, CamDrop.transform.position, Quaternion.identity).GetComponent<Item>();
                item = weaponInstance.ItemStats;
                Instance.ItemStats = item;

                Instance.rb.velocity = GetComponent<CharacterController>().velocity;
                Instance.rb.AddForce(transform.forward * dropForwardForce, ForceMode.Impulse);
                Instance.rb.AddForce(transform.up * dropUpwardForce, ForceMode.Impulse);

                //Add random rotation
                float random = Random.Range(-1f, 1f);
                Instance.rb.AddTorque(new Vector3(random, random, random) * 10);

                // Handle case of removing active weapon (switch to next weapon)
                if (i == ActiveItemIndex)
                {
                    SwitchItem(true);
                }

                return true;
            }
        }

        return false;
    }

}
