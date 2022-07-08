using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public GameObject MenuRoot;
    public GameObject StatusNumber;
    public Camera ZoomUI;
    private float ViewZoom = 60;
    public float SpeedZoom = 20f;
    public bool ShowMenu;

    public void Awake()
    {
        PauseMenu.Instance = this;
    }
    void Start()
    {
        MenuRoot.SetActive(false);
        //StatusNumber.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
        ZoomUI.fieldOfView = Mathf.Lerp(ZoomUI.fieldOfView, ViewZoom, Time.deltaTime * SpeedZoom); 

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
        StatusNumber.SetActive(active);
        MenuRoot.SetActive(active);
        ShowMenu = active;

        if (MenuRoot.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //Time.timeScale = 0f;
            PlayerMovement.Instance.canMove = false;
            WeaponController.Instance.canFire = false;
            ViewZoom = 90;
            //AudioUtility.SetMasterVolume(VolumeWhenMenuOpen);
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //Time.timeScale = 1f;
            PlayerMovement.Instance.canMove = true;
            WeaponController.Instance.canFire = true;
            ViewZoom = 60;
            //AudioUtility.SetMasterVolume(1);
        }
    }
}
