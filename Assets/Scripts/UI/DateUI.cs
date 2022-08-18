using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DateUI : MonoBehaviour
{
    public static DateUI Instance;
    public GameObject dateUI;
    public Image DayBar;
    public Image Vien;
    public Gradient DayBarColor;
    public TextMeshProUGUI dayNumberInUI;
    public TextMeshProUGUI dayNumber;
    public CanvasGroup canvasGroup;
    public float pad = 0;
    public bool Show;
    public multiLanguage language;

    private void Awake()
    {
        DateUI.Instance = this;
        this.canvasGroup.alpha = 0f;
        this.dateUI.gameObject.SetActive(false);

    }

    void Start()
    {
        
    }
    public void ShowDay(int day)
    {
        if(this == null)
        {
            return;
        }
        this.dayNumber.text = string.Format("{0} {1}", language.VietNamese, day);
        Show = true;
        UpdateDay(day);
        base.Invoke("fade", 3f);

    }
    private void UpdateDay(int day)
    {
        this.dayNumberInUI.text = day.ToString("00");
    }
    void fade()
    {
        Show = false;
    }
    void Update()
    {

        this.DayBar.fillAmount = DayNightCycle.Instance.GetcurrentTimeOfDay() / 1;
        this.DayBar.color = DayBarColor.Evaluate(DayBar.fillAmount);
        this.dayNumberInUI.color = DayBarColor.Evaluate(DayBar.fillAmount);
        this.Vien.color = DayBarColor.Evaluate(DayBar.fillAmount);

        if (Show == true)
        {
            
            this.pad = Mathf.Lerp(this.pad, 1, Time.deltaTime * 5f);
            this.canvasGroup.alpha = pad;
            this.dateUI.gameObject.SetActive(true);

        }
        else
        {
            this.pad = Mathf.Lerp(this.pad, 0, Time.deltaTime * 5f);
            this.canvasGroup.alpha = pad;
            if (pad == 0)
            {
                this.dateUI.gameObject.SetActive(false);
            }

        }
    }
}
