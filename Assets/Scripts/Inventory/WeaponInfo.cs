using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponInfo : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public InventoryItem powerup { get; set; }

	public void OnPointerEnter(PointerEventData eventData)
	{
		ItemInfo.Instance.SetText(this.powerup.name + "\n<size=50%><i>" + this.powerup.description, true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		ItemInfo.Instance.Fade(0f, 0.2f);
	}
}
