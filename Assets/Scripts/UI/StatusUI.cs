using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusUI : MonoBehaviour
{
	public TextMeshProUGUI money;

	private void Awake()
	{
		base.InvokeRepeating("SlowUpdate", 0f, 1f);
	}

	private void SlowUpdate()
	{
		this.UpdateMoney();
	}

	private void UpdateMoney()
	{
		this.money.text = string.Concat(Inventory.Instance.GetMoney());
	}
}
