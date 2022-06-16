using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerStats m_PlayerHealth;
    void Start()
    {

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
}
