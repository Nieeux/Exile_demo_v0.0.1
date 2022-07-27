using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class WeaponUI : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
    public static WeaponUI Instance;
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
        WeaponUI.Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (StatusUI.Instance.IsShowStatus == true)
        {
            WeaponStatus.SetActive(true);
            ActiveWeapon = m_Weapon;
            UpdateStatsWeapon();
            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, ActiveWeapon ? 1f : UnselectedOpacity, Time.deltaTime * 10);
            transform.localScale = Vector3.Lerp(transform.localScale, ActiveWeapon ? Vector3.one : UnselectedScale, Time.deltaTime * 10);

        }
        else
        {
            WeaponStatus.SetActive(false);
            bool isActiveWeapon = m_Weapon == PlayerWeaponManager.Instance.GetActiveWeapon();
            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, isActiveWeapon ? 1f : UnselectedOpacity, Time.deltaTime * 10);
            transform.localScale = Vector3.Lerp(transform.localScale, isActiveWeapon ? Vector3.one : UnselectedScale, Time.deltaTime * 10);
            ControlKeysRoot.SetActive(!isActiveWeapon);
            UpdateAmmo(m_Weapon);
            DurabilityBar(m_Weapon);
        }
    }

    public void UpdateStatsWeapon()
    {
        this.TextDamage.text = m_Weapon.GunStats.GunDamage.ToString("00");

        this.TextFireRate.text = m_Weapon.GunStats.fireRate.ToString("0.0");

        this.TextCritical.text = m_Weapon.GunStats.Critical.ToString("0.0");

        this.TextMagazine.text = m_Weapon.GunStats.Magazine.ToString("00");

        this.TextDurability.text = m_Weapon.GunStats.CurrentDurability.ToString("00");
    }

    public void Initialize(WeaponController weapon, int weaponIndex)
    {
        m_Weapon = weapon;
        WeaponUIIndex = weaponIndex;
        WeaponImage.sprite = weapon.GunStats.sprite;
        WeaponName.text = weapon.GunStats.nameViet;

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
    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemInfo.Instance.SetText(m_Weapon.GunStats.nameViet + "\n<size=70%>" + m_Weapon.GunStats.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemInfo.Instance.OnDisable();
    }
}
