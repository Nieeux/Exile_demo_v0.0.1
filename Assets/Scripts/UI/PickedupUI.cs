using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickedupUI : MonoBehaviour
{
	public Image icon;

	public TextMeshProUGUI item;

	private float fadeStart = 3f;

	private float fadeTime = 1f;


	private void Awake()
	{
		base.Invoke("StartFade", this.fadeStart);
	}

	private void StartFade()
	{
		this.icon.CrossFadeAlpha(0f, this.fadeTime, true);
		this.item.CrossFadeAlpha(0f, this.fadeTime, true);
		base.Invoke("DestroySelf", this.fadeTime);
	}

	private void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void SetItem(ItemStats i)
	{
		this.icon.sprite = i.sprite;
		this.icon.color = i.colorIndex;
		this.item.text = string.Format("{0}",i.GetName());
	}
}
