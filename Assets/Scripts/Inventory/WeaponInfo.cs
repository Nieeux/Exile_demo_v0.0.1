using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponInfo : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public ItemStats powerup { get; set; }

	public void OnPointerEnter(PointerEventData eventData)
	{
		ItemInfo.Instance.SetText(this.powerup.name + "\n<size=50%><i>" + this.powerup.description);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		ItemInfo.Instance.OnDisable();
	}
}
