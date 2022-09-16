using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RgMovement : MonoBehaviour
{
    public static RgMovement Instance;
    public float Sensitivity = 2.0f;
    private bool canMove = true;

    public Transform playerCamera;
	public Transform orientation;
	public float lookXLimit = 90f;
    public float rotationX = 0;

	float Vertical;
	float Horizontal;
	Vector3 moveDirection = Vector3.zero;
	public float Speed { get; private set; }
	public float currentSpeed;
	public bool IsMoving;
	public bool isRunning;
	public bool IsCrouching;

	public float delay = 5f;
	public float groundCancel;
	public bool cancellingGrounded;
	public bool IsGrounded;

	private float maxSlopeAngle = 35f;

	public LayerMask whatIsGround;

	PlayerStats Stats;
	Rigidbody rb;
    private Collider playerCollider;
	PlayerInput InputHandler;

	private void Awake()
    {
		RgMovement.Instance = this;
        this.rb = base.GetComponent<Rigidbody>();
		Stats = GetComponent<PlayerStats>();
		InputHandler = GetComponent<PlayerInput>();
	}
    void Start()
    {
        this.playerCollider = base.GetComponent<Collider>();

	}

    
    private void Update()
    {
		if (ActiveMenu())
		{
			IsMoving = moveDirection != Vector3.zero;
			isRunning = InputHandler.GetRunning() && IsMoving && Stats.CanRun();
			if (canMove)
			{
				rotationX += -Input.GetAxis("Mouse Y") * Sensitivity;
				rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
				playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
				transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * Sensitivity, 0);
			}
			if (InputHandler.GetJump())
			{
				Jump();
			}
			Vertical = Input.GetAxis("Vertical");
			Horizontal = Input.GetAxis("Horizontal");
			moveDirection = transform.forward * Vertical + transform.right * Horizontal;
			GetSpeed();
			UpdateCollisionChecks();

		}

    }
    private void FixedUpdate()
    {
		rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
	}


	private void GetSpeed()
	{
		Speed = Stats.CurrentSpeed();
		float run = Speed * 2;
		float crouch = Speed / 2;
		float current = isRunning && IsMoving ? run : Speed;
		current = IsCrouching && IsMoving ? crouch : current;
		currentSpeed = current;
	}

	private void Jump()
	{
		// reset y velocity
		rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

		rb.AddForce(transform.up * (currentSpeed * 2f), ForceMode.Impulse);
	}
	private void OnCollisionStay(Collision other)
	{
		int layer = other.gameObject.layer;
		if (this.whatIsGround != (this.whatIsGround | 1 << layer))
		{
			return;
		}
		for (int i = 0; i < other.contactCount; i++)
		{
			Vector3 normal = other.contacts[i].normal;
			normal = new Vector3(normal.x, Mathf.Abs(normal.y), normal.z);
			if (this.IsGround(normal))
			{
				this.IsGrounded = true;
				this.cancellingGrounded = false;
				this.groundCancel = 0;
			}
		}
	}
	private void UpdateCollisionChecks()
	{
		if (!this.cancellingGrounded)
		{
			this.cancellingGrounded = true;
		}
		else
		{
			this.groundCancel++;
			if ((float)this.groundCancel > this.delay)
			{
				this.StopGrounded();
			}
		}
	}
	private void StopGrounded()
	{
		this.IsGrounded = false;
	}

	private bool IsGround(Vector3 v)
	{
		return Vector3.Angle(Vector3.up, v) < this.maxSlopeAngle;
	}
	private bool ActiveMenu()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }
}
