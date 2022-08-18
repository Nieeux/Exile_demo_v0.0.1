using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StatusUI : MonoBehaviour
{
    public static StatusUI Instance;

    [Header("Object Status")]
    public GameObject AllUI;
    public GameObject GameOver;
    public GameObject StatusMenu;
    public GameObject PauseMenu;

    [Header("Bar Status")]
    public Image HealthBar;
    public Image HurtBar;
    public Image stamina;
    public Image hunger;
    public Image sleepy;

    [Header("Number Status")]
    public TextMeshProUGUI HealthNumber;
    public TextMeshProUGUI StaminaNumber;
    public TextMeshProUGUI HungerNumber;
    public TextMeshProUGUI sleepyNumber;

    [Header("Stats Status")]
    public float HurtSpeed = 0.01f;
    public float damageFillTimeRemap;
    public PlayerStats Player;

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
        PauseMenu.SetActive(false);
        StatusMenu.SetActive(false);
        GameOver.SetActive(false);

    }
    private void Update()
    {
        ZoomUI.fieldOfView = Mathf.Lerp(ZoomUI.fieldOfView, ViewZoom, Time.deltaTime * SpeedZoom);

        this.stamina.fillAmount = Mathf.Lerp(this.stamina.fillAmount, (Player.stamina / Player.maxStamina), Time.deltaTime * 10f);
        this.hunger.fillAmount = Mathf.Lerp(this.hunger.fillAmount, (Player.hunger / Player.maxHunger), Time.deltaTime * 10f);
        this.sleepy.fillAmount = Mathf.Lerp(this.sleepy.fillAmount, (Player.sleepy / Player.maxSleepy), Time.deltaTime * 10f);

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
        HealthBar.fillAmount = Mathf.Lerp(this.HealthBar.fillAmount, (Player.GetHealthBar()), Time.deltaTime * 10f);
        if (HurtBar.fillAmount > HealthBar.fillAmount)
        {
            //yield return new WaitForSeconds(0.4f);
            //yield return AniamteFill();
            HurtBar.fillAmount -= HurtSpeed;
        }
        else
        {
            HurtBar.fillAmount = HealthBar.fillAmount;
        }   
    }
    /*
    private IEnumerator AniamteFill()
    {
        TakingDamage = false;
        float timer = 0.0f;
        float form = HurtBar.fillAmount;
        float to = HealthBar.fillAmount;

        float difference = form - to;
        float duration = difference / HurtSpeed;

        while (true)
        {
            if(HurtSpeed <= 0.0f)
            {
                timer = 1.0f;
            }
            else
            {
                timer += Time.deltaTime / duration;
            }
            float remappedTimer = 2;
            HurtBar.fillAmount = Mathf.Lerp(form, to, remappedTimer);

            if (timer >= 1.0f)
            {
                break;
            }
            yield return null;
        }

    }
    */


    private void StatsNumber()
    {
        string text = "";
        float num = Player.CurrentHealth;
        float num2 = Player.GetMaxHealth();
        text += string.Format("Máu {0:0.} | {1:0.}", num, num2);
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

