using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class WeaponUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static WeaponUI Instance;
    public CanvasGroup CanvasGroup;

    [Header("Image")]
    public Image WeaponImage;
    public Image VienImage;
    public Image VienImage2;
    public Image Ammotype;
    public Image Durability;
    public Gradient GradientDurability;
    [Header("Text")]
    public TextMeshProUGUI WeaponName;
    public TextMeshProUGUI AmmoUI;
    public TextMeshProUGUI TextDamage;
    public TextMeshProUGUI TextFireRate;
    public TextMeshProUGUI TextCritical;
    public TextMeshProUGUI TextMagazine;
    public TextMeshProUGUI TextDurability;
    public TextMeshProUGUI TextWeaponRare;
    [Header("ColorUpdate")]
    public Color colorInceate;

    [Header("Buffs")]
    public GameObject pickupPrefab;
    public Transform pickupParent;

    public WeaponController m_Weapon;

    private bool ActiveWeapon;

    public Vector3 UnselectedScale = Vector3.one * 0.8f;

    [Range(0, 1)]

    public float UnselectedOpacity = 0.5f;
    public float SpeedTransform;
    public GameObject ControlKeysRoot;
    public GameObject WeaponStatus;

    Inventory player;
    MenuSetting Setting;

    public int WeaponUiId;

    private void Awake()
    {
        WeaponUI.Instance = this;
    }
    void Start()
    {

        Setting = FindObjectOfType<MenuSetting>();
        Setting.ChangeLanguage += UpdateVersionName;
        player = FindObjectOfType<Inventory>();
        player.UpdateUi += UpdateStatsWeaponincrease;
        WeaponStatus.SetActive(false);
        AddBuffs(m_Weapon);
        UpdateStatsWeapon();
        this.WeaponImage.color = m_Weapon.GunStats.colorIndex;
        this.VienImage.color = m_Weapon.GunStats.colorIndex;
        this.VienImage2.color = m_Weapon.GunStats.colorIndex;
        this.Ammotype.color = m_Weapon.WeaponAmmoType;

        this.TextWeaponRare.text = m_Weapon.GunStats.GetNameVersion(m_Weapon.GunStats.Rare);
        this.TextWeaponRare.color = m_Weapon.GunStats.colorIndex;

        UpdateStatsWeaponincrease();
        UpdateAmmo(m_Weapon);
        DurabilityBar(m_Weapon);
    }

    // Update is called once per frame
    void Update()
    {

        if (StatusUI.Instance.IsShowStatus == true)
        {
            WeaponStatus.SetActive(true);
            ActiveWeapon = m_Weapon;

            this.TextDurability.text = m_Weapon.GunStats.CurrentDurability.ToString("00");

            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, ActiveWeapon ? 1f : UnselectedOpacity, Time.deltaTime * 10);
            transform.localScale = Vector3.Lerp(transform.localScale, ActiveWeapon ? Vector3.one : UnselectedScale, Time.deltaTime * 10);

        }
        else 
        {
            WeaponStatus.SetActive(false);
            bool isActiveWeapon = m_Weapon == WeaponInventory.Instance.GetActiveWeapon();
            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, isActiveWeapon ? 1f : UnselectedOpacity, Time.deltaTime * 10);
            transform.localScale = Vector3.Lerp(transform.localScale, isActiveWeapon ? Vector3.one : UnselectedScale, Time.deltaTime * 10);
            ControlKeysRoot.SetActive(!isActiveWeapon);
            //Debug.Log("Update");
        }
    }

    public void UpdateStatsWeapon()
    {
        this.TextDamage.text = m_Weapon.GunStats.GunDamage.ToString("00");

        this.TextFireRate.text = m_Weapon.GunStats.fireRate.ToString("0.0");

        this.TextCritical.text = m_Weapon.GunStats.Weight.ToString("0.0 kg");

        this.TextMagazine.text = m_Weapon.GunStats.Magazine.ToString("00");

    }
    public void UpdateStatsWeaponincrease()
    {
        this.TextDamage.text = string.Format("{0:0.##}", m_Weapon.GunStats.GunDamage * DamageCalculations.Instance.GetDamageUI());
        this.TextDamage.color = colorInceate;
        if (DamageCalculations.Instance.GetDamageUI() == 1)
        {
            this.TextDamage.text = m_Weapon.GunStats.GunDamage.ToString("00");
            this.TextDamage.color = Color.white;
        }

    }

    public void UpdateUI()
    {
        UpdateAmmo(m_Weapon);
        DurabilityBar(m_Weapon);
    }

    public void Initialize(WeaponController weapon, int weaponIndex)
    {
        this.m_Weapon = weapon;
        this.WeaponUiId = weaponIndex;
        this.WeaponImage.sprite = weapon.GunStats.sprite;
        this.WeaponName.text = weapon.GunStats.nameViet;
    }

    private void UpdateVersionName()
    {
        TextWeaponRare.text = m_Weapon.GunStats.GetNameVersion(m_Weapon.GunStats.Rare);
    }

    private void UpdateAmmo(WeaponController weapon)
    {
        m_Weapon = weapon;
        AmmoUI.text = string.Concat(weapon.GunStats.CurrentMagazine);
    }
    private void DurabilityBar(WeaponController weapon)
    {
        m_Weapon = weapon;
        Durability.fillAmount = weapon.GunStats.CurrentDurability / weapon.GunStats.Durability;
        Durability.color = GradientDurability.Evaluate(Durability.fillAmount);
    }

    public void AddBuffs(WeaponController weapon)
    {
        for (int i = 0; i < weapon.GunStats.buffs.Count; i++)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.pickupPrefab, this.pickupParent);
            gameObject.GetComponent<BuffUI>().SetBuffs(weapon, i) ;
            gameObject.transform.SetSiblingIndex(0);
        }
 
    }
  
    public void OnPointerEnter(PointerEventData eventData)
    {
        //ItemInfo.Instance.SetText(m_Weapon.GunStats.nameViet + "\n<size=70%>" + m_Weapon.GunStats.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //ItemInfo.Instance.OnDisable();
    }

}
