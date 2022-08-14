using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DebuffUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public WeaponUI weaponUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemInfo.Instance.SetText(weaponUI.m_Weapon.GunStats.Debuffs[0].nameViet + "\n<size=70%>" + weaponUI.m_Weapon.GunStats.Debuffs[0].description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemInfo.Instance.OnDisable();
    }
}
