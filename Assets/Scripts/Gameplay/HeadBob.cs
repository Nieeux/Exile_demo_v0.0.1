using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
	public static HeadBob Instance;

	public float walkingBobbingSpeed = 14f;

	public float RunningBobbingSpeed = 14f;

	public float WalkbobbingAmount = 0.05f;

	public float RunbobbingAmount = 0.05f;

	private float defaultPosY;

	private float timer;

	private void Awake()
	{
		HeadBob.Instance = this;

	}
	private void Start()
	{

		this.defaultPosY = base.transform.localPosition.y;
	}

	private void Update()
	{

	}
	public void Headbob()
    {
		if (PlayerMovement.Instance.isRunning)
		{
			this.timer += Time.deltaTime * this.RunningBobbingSpeed;
			transform.localPosition = new Vector3(base.transform.localPosition.x, this.defaultPosY + Mathf.Sin(this.timer) * this.RunbobbingAmount, base.transform.localPosition.z);
			return;
		}
		this.timer += Time.deltaTime * this.walkingBobbingSpeed;
		transform.localPosition = new Vector3(base.transform.localPosition.x, this.defaultPosY + Mathf.Sin(this.timer) * this.WalkbobbingAmount, base.transform.localPosition.z);
	}

}
