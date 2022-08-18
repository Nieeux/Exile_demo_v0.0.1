using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickedupMoney : MonoBehaviour
{
    public TextMeshProUGUI number;

	private float fadeStart = 2f;

	private float fadeTime = 1f;


	private void Awake()
	{
		base.Invoke("StartFade", this.fadeStart);
	}

	private void StartFade()
	{
		this.number.CrossFadeAlpha(0f, this.fadeTime, true);
		base.Invoke("DestroySelf", this.fadeTime);
	}

	private void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
	public void SetMoney(float money)
    {
        this.number.text = money.ToString("+00");
    }
	public void removeMoney(float money)
	{
		this.number.text = money.ToString("-00");
	}
}
