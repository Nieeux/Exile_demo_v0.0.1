using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    public Image WeaponImage;
    WeaponController m_Weapon;

    [Tooltip("Scale when weapon not selected")]
    public Vector3 UnselectedScale = Vector3.one * 0.8f;
    [Range(0, 1)]
    [Tooltip("Opacity when weapon not selected")]
    public float UnselectedOpacity = 0.5f;
    public GameObject ControlKeysRoot;
    public TextMeshProUGUI WeaponName;
    public TextMeshProUGUI AmmoUI;

    public int WeaponUIIndex { get; set; }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isActiveWeapon = m_Weapon == PlayerWeaponManager.Instance.GetActiveWeapon();

        CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, isActiveWeapon ? 1f : UnselectedOpacity, Time.deltaTime * 10);
        transform.localScale = Vector3.Lerp(transform.localScale, isActiveWeapon ? Vector3.one : UnselectedScale, Time.deltaTime * 10);
        ControlKeysRoot.SetActive(!isActiveWeapon);
        UpdateAmmo(m_Weapon);
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

}
