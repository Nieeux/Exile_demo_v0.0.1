using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class WeaponUI : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
    public CanvasGroup CanvasGroup;
    public Image WeaponImage;
    WeaponController m_Weapon;
    public Gradient GradientDurability;
    public Image Durability;
    private bool ActiveWeapon;

    [Tooltip("Scale when weapon not selected")]
    public Vector3 UnselectedScale = Vector3.one * 0.8f;
    [Range(0, 1)]
    [Tooltip("Opacity when weapon not selected")]
    public float UnselectedOpacity = 0.5f;
    public float SpeedTransform;
    public GameObject ControlKeysRoot;
    public GameObject WeaponStatus;
    public TextMeshProUGUI WeaponName;
    public TextMeshProUGUI AmmoUI;

    public TextMeshProUGUI TextDamage;
    public TextMeshProUGUI TextFireRate;
    public TextMeshProUGUI TextCritical;
    public TextMeshProUGUI TextMagazine;
    public TextMeshProUGUI TextDurability;


    public int WeaponUIIndex { get; set; }
    private void Awake()
    {
       
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponStatus.SetActive(false);
        if (StatusUI.Instance.ShowMenu == true)
        {
            WeaponStatus.SetActive(true);
            ActiveWeapon = m_Weapon;
            UpdateStatsWeapon();
            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, ActiveWeapon ? 1f : UnselectedOpacity, Time.deltaTime * 10);
            transform.localScale = Vector3.Lerp(transform.localScale, ActiveWeapon ? Vector3.one : UnselectedScale, Time.deltaTime * 10);

        }
        else
        {
            bool isActiveWeapon = m_Weapon == PlayerWeaponManager.Instance.GetActiveWeapon();
            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, isActiveWeapon ? 1f : UnselectedOpacity, Time.deltaTime * 10);
            transform.localScale = Vector3.Lerp(transform.localScale, isActiveWeapon ? Vector3.one : UnselectedScale, Time.deltaTime * 10);
            ControlKeysRoot.SetActive(!isActiveWeapon);
            UpdateAmmo(m_Weapon);
            DurabilityBar(m_Weapon);
        }
    }

    private void LateUpdate()
    {


    }
    private void UpdateStatsWeapon()
    {
        Textdamage();
        TextfireRate();
        Textcritical();
        Textmagazine();
        Textdurability();
    }

    private void Textdamage()
    {
        string text = "";
        float num = m_Weapon.GunStats.GunDamage;
        text = string.Format("{0:0.}", num);
        this.TextDamage.text = text;
    }
    private void TextfireRate()
    {
        string text = "";
        float num = m_Weapon.GunStats.fireRate;
        text = string.Format("{0:0.0}", num);
        this.TextFireRate.text = text;
    }
    private void Textcritical()
    {
        string text = "";
        float num = m_Weapon.GunStats.Critical;
        text = string.Format("{0:0.0}", num);
        this.TextCritical.text = text;
    }
    private void Textmagazine()
    {
        string text = "";
        float num = m_Weapon.GunStats.Magazine;
        text = string.Format("{0:0.}", num);
        this.TextMagazine.text = text;
    }

    private void Textdurability()
    {
        string text = "";
        float num = m_Weapon.GunStats.CurrentDurability;
        text = string.Format("{0:0.}", num);
        this.TextDurability.text = text;
    }
    public void Initialize(WeaponController weapon, int weaponIndex)
    {
        m_Weapon = weapon;
        WeaponUIIndex = weaponIndex;
        WeaponImage.sprite = weapon.GunStats.sprite;
        WeaponName.text = weapon.GunStats.name;

    }
    public void UpdateAmmo(WeaponController weapon)
    {
        m_Weapon = weapon;
        AmmoUI.text = string.Concat(weapon.GunStats.CurrentMagazine);
    }
    public void DurabilityBar(WeaponController weapon)
    {
        m_Weapon = weapon;
        Durability.fillAmount = weapon.GunStats.CurrentDurability / weapon.GunStats.Durability;
        Durability.color = GradientDurability.Evaluate(Durability.fillAmount);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemInfo.Instance.SetText(m_Weapon.GunStats.name + "\n<size=70%>" + m_Weapon.GunStats.description, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemInfo.Instance.Fade(0f, 0.2f);
    }
}
