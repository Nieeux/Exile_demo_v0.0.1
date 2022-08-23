using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
	public TextMeshProUGUI text;

	public TextMeshProUGUI Weight;

	public RectTransform background;

	public Camera UiCamera;

	public static ItemInfo Instance;

	public GameObject Info;

	bool off;

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
        if (!ActiveMenu())
        {
			Vector2 Point;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, UiCamera, out Point);
			transform.localPosition = Point;
			off = true;
		}
        else
        {
			if(off == true)
            {
				Setoff();
			}
			
		}
	}

	private void Setoff()
    {
		Info.SetActive(false);
		off = false;
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
		//float textpadding = 4f;
		//Vector2 backgroundsize = new Vector2(text.preferredWidth + textpadding * 2f, text.preferredHeight + textpadding * 2f);
		//background.sizeDelta = backgroundsize;
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
	public void SetWeight(float weight)
    {
		if(weight > 0)
        {
			this.Weight.text = string.Format("{0:0.##} kg", weight);
		}
        else
        {
			this.Weight.text = "";
		}
		
	}

	public bool ActiveMenu()
	{
		return Cursor.lockState == CursorLockMode.Locked;
	}
	/*
	public void Fade(float opacity, float time = 0.2f)
	{
		this.text.CrossFadeAlpha(opacity, time, true);
		this.image.CrossFadeAlpha(opacity, time, true);
	}
	*/

}
