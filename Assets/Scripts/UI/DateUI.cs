using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DateUI : MonoBehaviour
{
    [Header("Big Date UI")]
    public GameObject Day;
    public static DateUI Instance;
    public TextMeshProUGUI dayNumber;
    public Image Vienbig;


    [Header("Small Date UI")]
    public Gradient DayBarColor;
    public TextMeshProUGUI dayNumberInUI;
    public Image DayBar;
    public Image Vien;

    public multiLanguage language;

    private void Awake()
    {
        DateUI.Instance = this;
    }

    void Start()
    {
        Day.gameObject.SetActive(false);
    }

    public void ShowDay(int day)
    {
        if(this == null)
        {
            return;
        }
        this.dayNumber.text = string.Format("{0} {1}", language.GetLanguage(), day);
        Day.gameObject.SetActive(true);
        UpdateDay(day);
        base.Invoke("fade", 2f);

    }
    private void UpdateDay(int day)
    {
        this.dayNumberInUI.text = day.ToString("00");
    }

    void fade()
    {
        this.dayNumber.CrossFadeAlpha(0f, 2f, true);
        this.Vienbig.CrossFadeAlpha(0f, 2f, true);
        base.Invoke("TurnOff", 2f);
    }

    void TurnOff()
    {
        Day.gameObject.SetActive(false);
    }

    void Update()
    {

        this.DayBar.fillAmount = DayNightCycle.Instance.GetcurrentTimeOfDay() / 1;
        this.DayBar.color = DayBarColor.Evaluate(DayBar.fillAmount);
        this.dayNumberInUI.color = DayBarColor.Evaluate(DayBar.fillAmount);
        this.Vien.color = DayBarColor.Evaluate(DayBar.fillAmount);
    }
}
