using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyUI : MonoBehaviour
{
	public static MoneyUI Instance;
	public TextMeshProUGUI money;
	public CanvasGroup canvasWarning;

	public float DamageFlashMaxAlpha = 1f;

	public float FlashDuration;

	public bool ActiveWarning;

	float LastTimeFlashStarted = Mathf.NegativeInfinity;

	public int CountFPS = 30;
	public float MoneyAniDuration = 1f;
	public string NumberFormat = "N0";
	private int _value;
	private Coroutine CountingCoroutine;

	public GameObject pickupPrefab;

	public Transform pickupParent;

	private void Awake()
	{
		MoneyUI.Instance = this;
	}
    private void Start()
    {

	}
    private void Update()
    {
		if (ActiveWarning)
		{
			float normalizedTime = (Time.time - LastTimeFlashStarted) / FlashDuration;

			if (normalizedTime < 1f)
			{
				float flashAmount = DamageFlashMaxAlpha * (1f - normalizedTime);
				canvasWarning.alpha = flashAmount;
			}
			else
			{
				canvasWarning.gameObject.SetActive(false);
				ActiveWarning = false;
			}
		}
	}
    public void AddMoney(float money)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.pickupPrefab, this.pickupParent);
		gameObject.GetComponent<PickedupMoney>().SetMoney(money);
		gameObject.transform.SetSiblingIndex(0);
	}
	public void RemoveMoney(float money)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.pickupPrefab, this.pickupParent);
		gameObject.GetComponent<PickedupMoney>().removeMoney(money);
		gameObject.transform.SetSiblingIndex(0);
	}
	public void NotEnoughMoney()
    {
		LastTimeFlashStarted = Time.time;
		ActiveWarning = true;
		canvasWarning.alpha = 0f;
		canvasWarning.gameObject.SetActive(true);
		Debug.Log("NotEnoughMoney");

	}
	public int Value
	{
		get
		{
			return _value;
		}
		set
		{
			UpdateText(value);
			_value = value;
		}
	}
	private void UpdateText(int newValue)
	{
		if (CountingCoroutine != null)
		{
			StopCoroutine(CountingCoroutine);
		}

		CountingCoroutine = StartCoroutine(CountText(newValue));
	}

	private IEnumerator CountText(int newValue)
	{
		WaitForSeconds Wait = new WaitForSeconds(1f / CountFPS);
		int previousValue = _value;
		int stepAmount;

		if (newValue - previousValue < 0)
		{
			stepAmount = Mathf.FloorToInt((newValue - previousValue) / (CountFPS * MoneyAniDuration)); // newValue = -20, previousValue = 0. CountFPS = 30, and Duration = 1; (-20- 0) / (30*1) // -0.66667 (ceiltoint)-> 0
		}
		else
		{
			stepAmount = Mathf.CeilToInt((newValue - previousValue) / (CountFPS * MoneyAniDuration)); // newValue = 20, previousValue = 0. CountFPS = 30, and Duration = 1; (20- 0) / (30*1) // 0.66667 (floortoint)-> 0
		}

		if (previousValue < newValue)
		{
			while (previousValue < newValue)
			{
				previousValue += stepAmount;
				if (previousValue > newValue)
				{
					previousValue = newValue;
				}

				money.SetText(previousValue.ToString(NumberFormat));

				yield return Wait;
			}
		}
		else
		{
			while (previousValue > newValue)
			{
				previousValue += stepAmount; // (-20 - 0) / (30 * 1) = -0.66667 -> -1              0 + -1 = -1
				if (previousValue < newValue)
				{
					previousValue = newValue;
				}

				money.SetText(previousValue.ToString(NumberFormat));

				yield return Wait;
			}
		}
	}
}
