using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerStats : MonoBehaviour
{
    private VerticalLayoutGroup layout;
    public GameObject bar;
    public float padBottom = 0;
    [Header("Player Stats UI")]
    public TextMeshProUGUI DamageNumber;
    public TextMeshProUGUI SpeedNumber;
    public TextMeshProUGUI weightNumber;
    public TextMeshProUGUI DarkNumber;

    public void Awake()
    {
        this.layout = base.GetComponent<VerticalLayoutGroup>();
    }
    public void Start()
    {
        bar.SetActive(false);

        base.Invoke("UpdateStatsPlayer", 0.1f);

        //this.weightNumber.text = m_Weapon.GunStats.Critical.ToString("0.0");

        //this.DarkNumber.text = m_Weapon.GunStats.Magazine.ToString("00");
    }

    void Update()
    {
        if (StatusUI.Instance.IsShowStatus == true)
        {
            bar.SetActive(true);
            this.padBottom = Mathf.Lerp(this.padBottom, 70, Time.deltaTime * 20f);
            RectOffset rectOffset = new RectOffset(this.layout.padding.left, this.layout.padding.right, this.layout.padding.top, this.layout.padding.bottom);
            rectOffset.bottom = (int)this.padBottom;
            this.layout.padding = rectOffset;
            this.layout.padding.bottom = (int)this.padBottom;
        }
        else
        {
            //Khong cho update thuong xuyen
            if (padBottom > 0)
            {
                this.padBottom = (int)Mathf.Lerp(this.padBottom, 0, Time.deltaTime * 20f);
                RectOffset rectOffset = new RectOffset(this.layout.padding.left, this.layout.padding.right, this.layout.padding.top, this.layout.padding.bottom);
                rectOffset.bottom = (int)this.padBottom;
                this.layout.padding = rectOffset;
                this.layout.padding.bottom = (int)this.padBottom;
                bar.SetActive(false);
            }
        }

    }
    void UpdateStatsPlayer()
    {
        this.DamageNumber.text = PlayerStats.Instance.damage.ToString("00");

        this.SpeedNumber.text = PlayerMovement.Instance.walkingSpeed.ToString("00");
    }

}
