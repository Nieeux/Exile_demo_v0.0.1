using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Headbob01 : MonoBehaviour
{
	public static Headbob01 Instance;

	private float bobbingSpeed = 0.03f;

	private float bobbingRunSpeed = 0.03f;

	private float bobbingAmount = 0.03f;

	private float bobbingAmountRunning = 0.1f;

	private float timer;

    private void Awake()
    {
		Headbob01.Instance = this;

	}
    public void headBob()
    {
		float num = 0f;
		float axis = Input.GetAxis("Horizontal");
		float axis2 = Input.GetAxis("Vertical");
		float num2 = (!PlayerMovement.Instance.isRunning) ? this.bobbingSpeed : this.bobbingRunSpeed;
		Vector3 localPosition = base.transform.localPosition;
		if (Mathf.Abs(axis) == 0f && Mathf.Abs(axis2) == 0f)
		{
			this.timer = 0f;
		}
		else
		{
			num = Mathf.Sin(this.timer);
			this.timer += num2;
			if (this.timer > 6f)
			{
				this.timer -= 6f;
			}
		}
		if (num != 0f)
		{
			float num3 = num * ((!PlayerMovement.Instance.isRunning) ? this.bobbingAmount : this.bobbingAmountRunning);
			float num4 = Mathf.Abs(axis) + Mathf.Abs(axis2);
			num4 = Mathf.Clamp(num4, 0f, 1f);
			num3 = num4 * num3;
			localPosition.y = 0 + num3;
		}
		else
		{
			localPosition.y = 0;
		}
		base.transform.localPosition = localPosition;
	}


}
