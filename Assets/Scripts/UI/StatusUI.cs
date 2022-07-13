using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StatusUI : MonoBehaviour
{
    //public Texture crosshairTexture;

    public Image HealthBar;
    public Image HurtBar;
    public Image stamina;
    public Image hunger;

    public GameObject GameOver;
    public TextMeshProUGUI HealthNumber;
    public TextMeshProUGUI StaminaNumber;
    public TextMeshProUGUI HungerNumber;
    public GameObject StatusNumber;

    private float HurtSpeed = 0.002f;
    public Transform playerCam;
    public PlayerStats Player;

    public static StatusUI Instance;
    public GameObject MenuRoot;
    public Camera ZoomUI;
    private float ViewZoom = 60;
    public float SpeedZoom = 20f;
    public bool ShowMenu;

    public void Awake()
    {
        StatusUI.Instance = this;

    }

    void Start()
    {
        MenuRoot.SetActive(false);
        StatusNumber.SetActive(false);
        GameOver.SetActive(false);
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
        if (PlayerStats.Instance.CurrentHealth <= 0)
        {
            ClosePauseMenu();        
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
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
        GameOver.SetActive(true);
        SetPauseMenuActivation(true);
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
}

