using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{   
	public float bobbingSpeed = 0.18f;

	public float bobbingRunSpeed = 0.18f;

	public float bobbingAmount = 0.2f;

	public float midpoint = 2f;

	private float timer;

	private void Update()
	{

		if (!PlayerInput.Instance.CanProcessInput())
		{
			return;
		}
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
			if (this.timer > 6.2831855f)
			{
				this.timer -= 6.2831855f;
			}
		}
		if (num != 0f)
		{
			float num3 = num * this.bobbingAmount;
			float num4 = Mathf.Abs(axis) + Mathf.Abs(axis2);
			num4 = Mathf.Clamp(num4, 0f, 1f);
			num3 = num4 * num3;
			localPosition.y = this.midpoint + num3;
		}
		else
		{
			localPosition.y = this.midpoint;
		}
		base.transform.localPosition = localPosition;
	}


}
