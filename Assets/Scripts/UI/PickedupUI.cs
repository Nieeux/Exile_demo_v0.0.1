using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickedupUI : MonoBehaviour
{
	public Image icon;

	public TextMeshProUGUI item;

	private HorizontalLayoutGroup layout;

	private float desiredPad;

	private float fadeStart = 3f;

	private float fadeTime = 1f;

	private float padLeft;

	private void Awake()
	{

		this.layout = base.GetComponent<HorizontalLayoutGroup>();
		/*
		this.desiredPad = (float)this.layout.padding.left;
		this.layout.padding.left = -300;
		this.padLeft = (float)this.layout.padding.left;
		*/
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

	public void SetItem(InventoryItem i)
	{
		if (i.amount < 1)
		{
			this.icon.sprite = null;
			this.item.text = "Inventory full";
			return;
		}
		this.icon.sprite = i.sprite;
		this.item.text = string.Format("{0}x {1}", i.amount, i.name);
	}

	public void Update()
	{
		/*
		this.padLeft = Mathf.Lerp(this.padLeft, this.desiredPad, Time.deltaTime * 7f);
		RectOffset rectOffset = new RectOffset(this.layout.padding.left, this.layout.padding.right, this.layout.padding.top, this.layout.padding.bottom);
		rectOffset.left = (int)this.padLeft;
		this.layout.padding = rectOffset;
		this.layout.padding.left = (int)this.padLeft;
		*/
	}


}
