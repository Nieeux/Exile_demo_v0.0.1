using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StatusUI : MonoBehaviour
{
    public static StatusUI Instance;

    [Header("Object Status")]
    public GameObject AllUI;
    public GameObject StatusMenu;
    public GameObject PauseMenu;
    public GameObject SettingsMenu;
    public GameObject GameOver;
    public TextMeshProUGUI GameOverShowSource;

    [Header("Bar Status")]
    public Image HealthBar;
    public Image HurtBar;

    public CanvasGroup HurtCanvasGroup;
    public float CriticaHealthVignetteMaxAlpha = .8f;
    public float PulsatingVignetteFrequency = 4f;
    public Color HurtColor;
    public Color NormalColor;
    public Color lightColor;
    public Image stamina;
    public Image hunger;
    public Image sleepy;

    [Header("Vien/LowWarning")]
    public Image VienHealth;
    public Image VienSleep;
    public Image VienHunger;
    public Image IconSleep;
    public Image IconHunger;
    private bool IschangeHealthColor = false;
    private bool IschangeHungerColor = false;
    private bool IschangeSleepColor = false;

    [Header("Number Status")]
    public TextMeshProUGUI HealthNumber;
    public TextMeshProUGUI StaminaNumber;
    public TextMeshProUGUI HungerNumber;
    public TextMeshProUGUI sleepyNumber;

    [Header("Stats Status")]
    public float HurtSpeed = 0.01f;
    public float damageFillTimeRemap;
    public PlayerStats Player;

    [Header("Multi-Language")]
    public multiLanguage Source;
    public multiLanguage multiLanguage;
    public Camera ZoomUI;
    public float ViewZoom = 60;
    public float SpeedZoom = 20f;
    public bool IsShowStatus;
    public bool IsPause;


    private void Awake()
    {
        FillBarStatus();
        StatusUI.Instance = this;
    }

    private void Start()
    {
        SettingsMenu.SetActive(false);
        PauseMenu.SetActive(false);
        StatusMenu.SetActive(false);
        GameOver.SetActive(false);

    }
    private void Update()
    {
        ZoomUI.fieldOfView = Mathf.Lerp(ZoomUI.fieldOfView, ViewZoom, Time.deltaTime * SpeedZoom);

        this.stamina.fillAmount = Mathf.Lerp(this.stamina.fillAmount, (Player.stamina / Player.maxStamina), Time.deltaTime * 10f);
        this.hunger.fillAmount = Mathf.Lerp(this.hunger.fillAmount, (Player.GetHungerRatio()), Time.deltaTime * 10f);
        if (Player.GetHungerRatio() <= 0.3)
        {
            float vignetteAlpha = (1 - (Player.GetHungerRatio() / 0.3f)) * CriticaHealthVignetteMaxAlpha;
            hunger.color = Color.Lerp(lightColor, HurtColor, ((Mathf.Sin(Time.time * PulsatingVignetteFrequency) / 2) + 0.5f) * vignetteAlpha);
            VienHunger.color = Color.Lerp(lightColor, HurtColor, ((Mathf.Sin(Time.time * PulsatingVignetteFrequency) / 2) + 0.5f) * vignetteAlpha);
            IconHunger.color = Color.Lerp(lightColor, HurtColor, ((Mathf.Sin(Time.time * PulsatingVignetteFrequency) / 2) + 0.5f) * vignetteAlpha);
            IschangeHungerColor = true;
        }
        else
        {
            if(IschangeHungerColor == true)
            {
                hunger.color = lightColor;
                VienHunger.color = lightColor;
                IconHunger.color = lightColor;
                Debug.Log("ChangeColorHunger");
                IschangeHungerColor = false;
            }

        }

        this.sleepy.fillAmount = Mathf.Lerp(this.sleepy.fillAmount, (Player.GetSleepyRatio()), Time.deltaTime * 10f);
        if (Player.GetSleepyRatio() <= 0.3)
        {
            float vignetteAlpha = (1 - (Player.GetSleepyRatio() / 0.3f)) * CriticaHealthVignetteMaxAlpha;
            sleepy.color = Color.Lerp(lightColor, HurtColor, ((Mathf.Sin(Time.time * PulsatingVignetteFrequency) / 2) + 0.5f) * vignetteAlpha);
            VienSleep.color = Color.Lerp(lightColor, HurtColor, ((Mathf.Sin(Time.time * PulsatingVignetteFrequency) / 2) + 0.5f) * vignetteAlpha);
            IconSleep.color = Color.Lerp(lightColor, HurtColor, ((Mathf.Sin(Time.time * PulsatingVignetteFrequency) / 2) + 0.5f) * vignetteAlpha);
            IschangeSleepColor = true;
        }
        else
        {
            if (IschangeSleepColor == true)
            {
                sleepy.color = lightColor;
                VienSleep.color = lightColor;
                IconSleep.color = lightColor;
                Debug.Log("ChangeColorSpleey");
                IschangeSleepColor = false;
            }

        }

        if (Input.GetButtonDown("Status") && !PauseMenu.activeSelf)
        {
            SetStatusActivation(!StatusMenu.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !StatusMenu.activeSelf)
        {
            Pause(!PauseMenu.activeSelf);
        }

        //Load lai game khi Player chet
        if (Player.CurrentHealth <= 0)
        {
            Gameover();        

        }

        //Update thanh mau khi HP Player khac MaxHP
        if (Player.CurrentHealth != 100)
        {
        }
        FillBarStatus();

        if (IsShowStatus == true)
        {
            StatsNumber();  
        }

    }

    private void FillBarStatus()
    {
        HealthBar.fillAmount = Mathf.Lerp(this.HealthBar.fillAmount, (Player.GetHealthRatio()), Time.deltaTime * 10f);
        HurtBar.fillAmount = HealthBar.fillAmount;
        if (Player.GetHealthRatio() <= 0.4)
        {
            float vignetteAlpha = (1 - (Player.GetHealthRatio() / 0.4f)) * CriticaHealthVignetteMaxAlpha;
            HealthBar.color = Color.Lerp(NormalColor, HurtColor, ((Mathf.Sin(Time.time * PulsatingVignetteFrequency) / 2) + 0.5f) * vignetteAlpha);
            VienHealth.color = Color.Lerp(NormalColor, HurtColor, ((Mathf.Sin(Time.time * PulsatingVignetteFrequency) / 2) + 0.5f) * vignetteAlpha);
            IschangeHealthColor = true;
        }
        else
        {
            if (IschangeHealthColor == true)
            {
                HealthBar.color = NormalColor;
                VienHealth.color = NormalColor;
                Debug.Log("ChangeColorHealth");
                IschangeHealthColor = false;
            }
        }

    }

    private void StatsNumber()
    {
        string text = "";
        float num = Player.CurrentHealth;
        float num2 = Player.GetMaxHealth();
        text += string.Format("{0:0.} | {1:0.}", num, num2);
        this.HealthNumber.text = text;

        this.StaminaNumber.text = Player.stamina.ToString("00");
        this.HungerNumber.text = Player.hunger.ToString("00");
        this.sleepyNumber.text = Player.sleepy.ToString("00");
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

    private void Gameover()
    {
        GameOver.SetActive(true);
        GameOverShowSource.text = string.Format("{0} <color=#00FFFF>{1}</color>",Source.GetLanguage() ,1+(int)DayNightCycle.Instance.GetCurrentTime());
        SetStatusActivation(true);
    }
    private void SetStatusActivation(bool active)
    {
        StatusMenu.SetActive(active);
        IsShowStatus = active;

        if (StatusMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            ViewZoom = 90;
            //WeaponUI.Instance.UpdateStatsWeapon();
            //AudioUtility.SetMasterVolume(VolumeWhenMenuOpen);
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ViewZoom = 60;
        }

    }
    private void Pause(bool active)
    {
        PauseMenu.SetActive(active);
        IsPause = active;
        if (SettingsMenu.activeSelf)
        {
            SettingsMenu.SetActive(active);
        }

        if (PauseMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }

    }
}

