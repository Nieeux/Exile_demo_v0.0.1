using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
	public TextMeshProUGUI text;

	public RawImage image;

	public float padding;

	public static ItemInfo Instance;

	public GameObject Info;



	private void Awake()
	{
		ItemInfo.Instance = this;

	}
	private void Start()
    {
		Info.SetActive(false);

	}
    private void Update()
	{

	}

	public void OnDisable()
	{
		Info.SetActive(false);
	}
	private void OnEnable()
	{
		this.SetText("");
	}


	public void SetText(string t)
	{
		this.text.text = t;
		if (t == "")
		{
			Info.SetActive(false);
			//this.Fade(0f, 0.2f);
		}
		else
		{
			Info.SetActive(true);
			//this.Fade(1f, 0.2f);
		}
	}

	/*
	public void Fade(float opacity, float time = 0.2f)
	{
		this.text.CrossFadeAlpha(opacity, time, true);
		this.image.CrossFadeAlpha(opacity, time, true);
	}
	*/

}
