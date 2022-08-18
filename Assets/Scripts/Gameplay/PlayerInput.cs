using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance;
    public PlayerStats PlayerHealth;
    public PlayerMovement playerMovement;
    public WeaponInventory WeaponManager;

    private void Start()
    {
        PlayerInput.Instance = this;
        this.WeaponManager = base.GetComponent<WeaponInventory>();
        this.PlayerHealth = base.GetComponent<PlayerStats>();
        this.playerMovement = base.GetComponent<PlayerMovement>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (LockState())
        {
            if (Input.GetButtonDown("Pickup"))
            {
                Interact currentInteractable = DetectItem.Instance.currentInteractable;

                if (currentInteractable != null)
                {
                    currentInteractable.Interact();
                }
            }

        }
        if (!LockState())
        {
            if (Input.GetMouseButtonDown(1))
            {
                Inventory.Instance.DropItem();
            }
            if (Input.GetMouseButtonDown(0))
            {
                Inventory.Instance.Use();

            }
        }

    }

    // Movement
    public bool GetCrouch()
    {
        if (LockState())
        {
            return Input.GetButtonDown("Crouch");
        }
        return false;
    }
    public bool GetRunning()
    {
        if (LockState())
        {
            return Input.GetKey(KeyCode.LeftShift);
        }
        return false;
    }
    public bool GetJump()
    {
        if (LockState())
        {
            return Input.GetButtonDown("Jump");
        }
        return false;
    }

    // Use Weapon
    public bool GetFireButtonDown()
    {
        if (LockState())
        {
            return Input.GetMouseButtonDown(0);
        }

        return false;
    }

    public bool GetFireButton()
    {
        if (LockState())
        {
            return Input.GetMouseButton(0);
        }

        return false;
    }

    public int GetSwitchWeapon()
    {
        if (LockState())
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                return -1;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                return 1;
        }
        return 0;
    }

    public bool GetDropWeapon()
    {
        if (LockState())
        {
            return Input.GetButtonDown("Drop Weapon");
        }
        return false;
    }

    // LockMouse
    public bool LockState()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }
}
