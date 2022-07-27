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
        if (CanProcessInput())
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
        if (!CanProcessInput())
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
    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }
    public bool GetFireInputSingle()
    {
        return GetFireInputHeld() && !m_FireInputWasHeld;
    }

    public bool GetFireInputAuto()
    {
        return !GetFireInputHeld() && m_FireInputWasHeld;
    }
    public bool GetFireInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButton(0);
        }

        return false;
    }

    /*
    public int GetSelectWeaponInput()
    {
        if (CanProcessInput())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                return 1;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                return 2;
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                return 3;
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                return 4;
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                return 5;
            else
                return 0;
        }

        return 0;
    }
    */

    public int GetSwitchWeaponInput()
    {
        if (CanProcessInput())
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                return -1;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                return 1;
        }
        return 0;
    }

    public bool DropWeaponInput()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown("Drop Weapon");
            {

            }
        }
        return false;
    }
    public bool GetCrouchInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown("Crouch");
        }
        return false;
    }
    public bool GetRunningInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKey(KeyCode.LeftShift);
        }
        return false;
    }
}
