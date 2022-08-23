using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image BuffsSprite;
    public Buff buff;
    public WeaponUI weaponUI;


    public void SetBuffs(WeaponController weapon, int number)
    {
        buff = weapon.GunStats.buffs[number];
        this.BuffsSprite.sprite = buff.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemInfo.Instance.SetText(buff.GetDescription());
        ItemInfo.Instance.SetWeight(0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemInfo.Instance.OnDisable();
    }
}
