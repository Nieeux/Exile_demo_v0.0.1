using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StatusUI : MonoBehaviour
{
    //public Texture crosshairTexture;

    public Image HealthBar;
    public Image HurtBar;
    public Image stamina;
    public Image hunger;

    public TextMeshProUGUI HealthNumber;
    public TextMeshProUGUI StaminaNumber;
    public TextMeshProUGUI HungerNumber;
    public GameObject StatusNumber;

    private float HurtSpeed = 0.002f;
    public Transform playerCam;
    public PlayerStats Player;

    public static PauseMenu Instance;
    public GameObject MenuRoot;
    public Camera ZoomUI;
    private float ViewZoom = 60;
    public float SpeedZoom = 20f;
    public bool ShowMenu;

    void Start()
    {
        MenuRoot.SetActive(false);
        StatusNumber.SetActive(false);
    }
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

        FillBarStatus();

        if (ShowMenu == true)
        {
            Healthnumber();
            staminaNumber();
            hungerNumber();
        }
    }

    private void FillBarStatus()
    {
        HealthBar.fillAmount = Player.CurrentHealth / Player.MaxHealth;
        if (HurtBar.fillAmount > HealthBar.fillAmount)
        {
            HurtBar.fillAmount -= HurtSpeed;
        }
        else
        {
            HurtBar.fillAmount = HealthBar.fillAmount;
        }

        stamina.fillAmount = Player.stamina / Player.maxStamina;
        hunger.fillAmount = Player.hunger / Player.maxHunger;
    }
    private void Healthnumber()
    {
        string text = "";
        float num = Player.CurrentHealth;
        float num2 = Player.MaxHealth;
        text += string.Format("Máu {0:0.} | {1:0.}", num, num2);
        this.HealthNumber.text = text;
    }
    private void staminaNumber()
    {
        string text = "";
        float num = Player.stamina;
        text += string.Format("Lực {0:0.}", num);
        this.StaminaNumber.text = text;
    }
    private void hungerNumber()
    {
        string text = "";
        float num = Player.hunger;
        text += string.Format("Đói {0:0.}", num);
        this.HungerNumber.text = text;
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
            PlayerMovement.Instance.canMove = true;
            WeaponController.Instance.canFire = true;
            ViewZoom = 60;
            //AudioUtility.SetMasterVolume(1);
        }
    }
    void OnGUI()
    {
        GUI.color = Color.red;
        //GUI.DrawTexture(new Rect(Screen.width / 2 - 3, Screen.height / 2 - 3, 6, 6), crosshairTexture);

        if (Player.CurrentHealth <= 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 3, Screen.height / 2 - 3, 250, 25), "Game Over");
        }
        else
        {
            //GUI.DrawTexture(new Rect(Screen.width / 2 - 3, Screen.height / 2 - 3, 6, 6), crosshairTexture);
        }
    }
}

