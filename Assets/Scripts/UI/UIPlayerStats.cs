using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerStats : MonoBehaviour
{
    public static UIPlayerStats Instance;
    private VerticalLayoutGroup layout;
    public GameObject bar;
    public GameObject SleepButton;
    public float padBottom = 0;
    [Header("Player Stats UI")]
    public TextMeshProUGUI DamageNumber;
    public TextMeshProUGUI SpeedNumber;
    public TextMeshProUGUI CritNumber;
    public TextMeshProUGUI weightNumber;
    public TextMeshProUGUI DarkNumber;

    public void Awake()
    {
        UIPlayerStats.Instance = this;
        this.layout = base.GetComponent<VerticalLayoutGroup>();
    }
    public void Start()
    {
        bar.SetActive(false);
        SleepButton.SetActive(false);
        base.Invoke("UpdateStatsPlayer", 0.1f);

        //this.weightNumber.text = m_Weapon.GunStats.Critical.ToString("0.0");

        //this.DarkNumber.text = m_Weapon.GunStats.Magazine.ToString("00");
    }

    private void Update()
    {
        if (StatusUI.Instance.IsShowStatus == true)
        {
            bar.SetActive(true);
            if (PlayerStats.Instance.IsSleep())
            {
                SleepButton.SetActive(true);
            }
            this.padBottom = Mathf.Lerp(this.padBottom, 70, Time.deltaTime * 20f);
            RectOffset rectOffset = new RectOffset(this.layout.padding.left, this.layout.padding.right, this.layout.padding.top, this.layout.padding.bottom);
            rectOffset.bottom = (int)this.padBottom;
            this.layout.padding = rectOffset;
            this.layout.padding.bottom = (int)this.padBottom;
        }
        else if (padBottom > 0)
        {
            //Khong cho update thuong xuyen
            this.padBottom = (int)Mathf.Lerp(this.padBottom, 0, Time.deltaTime * 20f);
            RectOffset rectOffset = new RectOffset(this.layout.padding.left, this.layout.padding.right, this.layout.padding.top, this.layout.padding.bottom);
            rectOffset.bottom = (int)this.padBottom;
            this.layout.padding = rectOffset;
            this.layout.padding.bottom = (int)this.padBottom;
            bar.SetActive(false);
        }

    }
    public void UpdateStatsPlayer()
    {
        Debug.Log("UpdateStatsUI");
        this.DamageNumber.text = PlayerStats.Instance.damage.ToString("00");
        this.SpeedNumber.text = PlayerStats.Instance.CurrentSpeed().ToString("0.0");
        this.CritNumber.text = string.Format("{0:}", Inventory.Instance.GetCritical().ToString("#0.##" + '%'));
        this.weightNumber.text = string.Format("{0:0.##}/ {1} kg", PlayerStats.Instance.Weight(), PlayerStats.Instance.maxWeight);

    }

}
