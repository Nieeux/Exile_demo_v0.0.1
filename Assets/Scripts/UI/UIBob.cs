using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBob : MonoBehaviour
{
	public static UIBob Instance;

	public float walkingBobbingSpeed = 14f;

	public float RunningBobbingSpeed = 14f;

	public float WalkbobbingAmount = 0.05f;

	public float RunbobbingAmount = 0.05f;

	private float defaultPosY;

	private float timer;

	[Space]
	[Header("Velocity based sway")]
	public float amount = 0.1f;

	public float maxAmount = 0.3f;

	public float smoothAmount = 6f;

	public float scale = 0.25f;

	public float Weight { get; set; }

	[Header("Step based sway")]
	public float stepStrenght = 5f;

	public float noiseStrenght = 1f;

	public float stepMultiplier = 1f;

	[Header("Rotation Step sway")]
	public float rotSmoothAmount = 6f;


	public float stepRotMultiplier = 1f;

	public Vector3 initPos;

	public Quaternion initRot;

	public Vector3 finalPosToMove;

	public Vector3 stepPos = Vector3.zero;

	public Quaternion stepRot = Quaternion.identity;

	private float ZAxis;

	PlayerStats player;
	bool playerDead = false;

	private void Awake()
	{
		UIBob.Instance = this;

	}
	private void Start()
	{
		player = FindObjectOfType<PlayerStats>();
		player.OnDie += PlayerDead;
		this.defaultPosY = base.transform.localPosition.y;

		this.Weight = 1f;
		this.initPos = base.transform.localPosition;
		this.initRot = base.transform.localRotation;
	}
	private void PlayerDead()
	{
		playerDead = true;
	}
	void Update()
	{
		if (playerDead)
		{
			return;
		}
		this.InputAxis();

		if (finalPosToMove != Vector3.zero)
		{
			this.stepPos = Vector3.Lerp(this.stepPos, Vector3.zero, Time.deltaTime * this.smoothAmount);
			Vector3 vector = this.finalPosToMove + this.stepPos;
			vector *= this.scale;
			vector *= this.Weight;
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, vector, Time.deltaTime * this.smoothAmount);

		}

	}
	public void UIbob()
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


	private void InputAxis()
	{
		this.ZAxis = -Input.GetAxis("Vertical") * this.amount;
		this.ZAxis = Mathf.Clamp(this.ZAxis, -this.maxAmount, this.maxAmount);
		this.finalPosToMove = new Vector3(0f, 0f, this.ZAxis) + this.initPos;
	}
}
