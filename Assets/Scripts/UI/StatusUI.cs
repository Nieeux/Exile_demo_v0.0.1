using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StatusUI : MonoBehaviour
{
    public static StatusUI Instance;
    [Header("Bar Status")]
    public Image HealthBar;
    public Image HurtBar;
    public Image stamina;
    public Image hunger;

    [Header("Number Status")]
    public GameObject NumberStatus;
    public TextMeshProUGUI HealthNumber;
    public TextMeshProUGUI StaminaNumber;
    public TextMeshProUGUI HungerNumber;


    private float HurtSpeed = 0.002f;
    public PlayerStats Player;

    public GameObject GameOver;
    public GameObject PauseStatus;
    public GameObject PauseMenu;
    public Camera ZoomUI;
    public float ViewZoom = 60;
    public float SpeedZoom = 20f;
    public bool IsShowStatus;
    public bool IsPause { get; set; }

    private void Awake()
    {
        FillBarStatus();
        StatusUI.Instance = this;

    }

    private void Start()
    {
        PauseMenu.SetActive(false);
        PauseStatus.SetActive(false);
        NumberStatus.SetActive(false);
        GameOver.SetActive(false);
    }
    private void Update()
    {
        ZoomUI.fieldOfView = Mathf.Lerp(ZoomUI.fieldOfView, ViewZoom, Time.deltaTime * SpeedZoom);
        stamina.fillAmount = Player.stamina / Player.maxStamina;
        hunger.fillAmount = Player.hunger / Player.maxHunger;

        if (Input.GetButtonDown("Status") && !PauseMenu.activeSelf)
        {
            SetStatusActivation(!PauseStatus.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !PauseStatus.activeSelf)
        {
            Pause(!PauseMenu.activeSelf);
        }

        //Load lai game khi Player chet
        if (Player.CurrentHealth <= 0)
        {
            ClosePauseMenu();        
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }

        //Update thanh mau khi HP Player khac MaxHP
        if (Player.CurrentHealth != Player.MaxHealth)
        {
            Debug.Log("UpdateHealth");
            FillBarStatus();
        }

        if (IsShowStatus == true)
        {
            StatsNumber();  
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
    }

    private void StatsNumber()
    {
        string text = "";
        float num = Player.CurrentHealth;
        float num2 = Player.MaxHealth;
        text += string.Format("Máu {0:0.} | {1:0.}", num, num2);
        this.HealthNumber.text = text;

        this.StaminaNumber.text = Player.stamina.ToString("Lực 00");
        this.HungerNumber.text = Player.hunger.ToString("Đói 00");
    }
    private void Healthnumber()
    {
      
        /*
        string text = "";
        float num = Player.CurrentHealth;
        float num2 = Player.MaxHealth;
        text += string.Format("Máu {0:0.} | {1:0.}", num, num2);
        this.HealthNumber.text = text;
        */
    }

    private void ClosePauseMenu()
    {
        GameOver.SetActive(true);
        SetStatusActivation(true);
    }
    private void SetStatusActivation(bool active)
    {
        NumberStatus.SetActive(active);
        PauseStatus.SetActive(active);
        IsShowStatus = active;

        if (PauseStatus.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerMovement.Instance.canMove = false;
            WeaponController.Instance.canFire = false;
            ViewZoom = 90;
            //WeaponUI.Instance.UpdateStatsWeapon();
            //AudioUtility.SetMasterVolume(VolumeWhenMenuOpen);
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            LockCursor();
        }

    }
    private void Pause(bool active)
    {
        PauseMenu.SetActive(active);
        IsPause = active;

        if (PauseMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerMovement.Instance.canMove = false;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerMovement.Instance.canMove = true;
            Time.timeScale = 1;
        }

    }
    private bool LockCursor()
    {
        Debug.Log("Turnoff");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerMovement.Instance.canMove = true;
        WeaponController.Instance.canFire = true;
        ViewZoom = 60;
        return this;
    }
}

