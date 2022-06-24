using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerAllStatus : MonoBehaviour
{
    public GameObject MenuRoot;

    void Start()
    {
        MenuRoot.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!MenuRoot.activeSelf && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetButtonDown("Pause Menu") || (MenuRoot.activeSelf) && Input.GetButtonDown("Cancel"))
        {
            SetPauseMenuActivation(!MenuRoot.activeSelf);

        }
    }
    public void ClosePauseMenu()
    {
        SetPauseMenuActivation(false);
    }
    void SetPauseMenuActivation(bool active)
    {
        MenuRoot.SetActive(active);

        if (MenuRoot.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            Player.Instance.canMove = false;
            //AudioUtility.SetMasterVolume(VolumeWhenMenuOpen);

            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            Player.Instance.canMove = true;
            //AudioUtility.SetMasterVolume(1);
        }

    }
}
