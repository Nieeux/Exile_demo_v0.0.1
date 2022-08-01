using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance;
    public PlayerStats PlayerHealth;
    public PlayerWeaponManager WeaponManager;
    bool m_FireInputWasHeld;

    private void Start()
    {
        PlayerInput.Instance = this;
        this.WeaponManager = base.GetComponent<PlayerWeaponManager>();
        this.PlayerHealth = base.GetComponent<PlayerStats>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (LockState())
        {
            if (Input.GetButtonDown("Pickup"))
            {
                Interactable currentInteractable = DetectItem.Instance.currentInteractable;
                Debug.Log("Pickup");
                // neu item ton tai va chua full do
                if (currentInteractable != null && !Inventory.Instance.IsInventoryFull())
                     //&& !Inventory.Instance.IsInventoryFull()
                {
                    currentInteractable.Interact();
                }
            }

        }
        if (!LockState())
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Da bam chuot phai");
                HotBar.Instance.DropItem();
            }
            if (Input.GetMouseButtonDown(0))
            {
                HotBar.Instance.Use();

            }
        }

        // Player ch?t load l?i game 
        if (PlayerHealth.CurrentHealth <= 0)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
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
