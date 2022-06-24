using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Health m_PlayerHealth;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {

        if (Input.GetButtonDown("Pickup"))
        {
            Interactable currentInteractable = Player.Instance.currentInteractable;
            Debug.Log("Da bam E");
            // neu item ton tai va chua full do
            if (currentInteractable != null && !Inventory.Instance.IsInventoryFull())
            {
                currentInteractable.Interact();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Da bam chuot phai");
            HotBar.Instance.DropItem();
        }

        // Player ch?t load l?i game 
        if (m_PlayerHealth.CurrentHealth <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }
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
    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }
}
