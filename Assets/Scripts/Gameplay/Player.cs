﻿using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Player Stats")]
    public float gravity = 20.0f;
    public Transform playerCamera;
    public Transform cameraCrouching;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 90f;
    public float Speed = 5f;
    Health health;

    [Header("Movement")]
    [Tooltip("Max movement speed when grounded (when not sprinting)")]
    public float MaxSpeedOnGround = 10f;
    [Tooltip("Multiplicator for the sprint speed (based on grounded speed)")]
    public float SprintSpeedModifier = 2f;

    [HideInInspector]
    public bool canMove = true;

    PlayerInput InputHandler;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;
    public float rotationX = 0;

    [Header("Stance")]
    [Tooltip("Ratio (0-1) of the character height where the camera will be at")]
    public float CameraHeightRatio = 0.9f;
    [Tooltip("Height of character when crouching")]
    public float CapsuleHeightCrouching = 0.9f;
    [Tooltip("Height of character when standing")]
    public float CapsuleHeightStanding = 1.8f;

    [Tooltip("Speed of crouching transitions")]
    public float CrouchingSharpness = 10f;

    float m_TargetCharacterHeight;

    public UnityAction<bool> OnStanceChanged;
    public bool IsCrouching { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsGrounded { get; private set; }

    PlayerWeaponManager WeaponsManager;


    private void Awake()
    {
        Player.Instance = this;

    }


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        WeaponsManager = GetComponent<PlayerWeaponManager>();
        InputHandler = GetComponent<PlayerInput>();
        health = GetComponent<Health>();
        health.OnDie += OnDie;
        rotation.y = transform.eulerAngles.y;

        // force the crouch state to false when starting
        SetCrouchingState(false, true);
        UpdateCharacterHeight(true);

    }

    void Update()
    {

        if (characterController.isGrounded)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            // di chuyen
            float curSpeedX = canMove ? (Speed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (Speed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
            var forwardsAmount = Vector3.Dot(transform.forward, moveDirection);

            if (Input.GetButtonDown("Jump") && canMove)
            {
                moveDirection.y = Speed;
            }
        }

        // gravity
        moveDirection.y -= gravity * Time.deltaTime;
        // di chuyen character
        characterController.Move(moveDirection * Time.deltaTime);

        // di chuyen camera
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        }
        // crouching
        if (InputHandler.GetCrouchInputDown())
        {
            Debug.Log("Crouch");
            SetCrouchingState(!IsCrouching, false);
        }
        UpdateCharacterHeight(false);
    }
    void UpdateCharacterHeight(bool force)
    {
        // Update height instantly
        if (force)
        {
            characterController.height = m_TargetCharacterHeight;
            characterController.center = Vector3.up * characterController.height * 0.5f;
            cameraCrouching.transform.localPosition = Vector3.up * m_TargetCharacterHeight * CameraHeightRatio;
            //m_Actor.AimPoint.transform.localPosition = characterController.center;
        }
        // Update smooth height
        else if (characterController.height != m_TargetCharacterHeight)
        {
            // resize the capsule and adjust camera position
            characterController.height = Mathf.Lerp(characterController.height, m_TargetCharacterHeight,
                CrouchingSharpness * Time.deltaTime);
            characterController.center = Vector3.up * characterController.height * 0.5f;
            cameraCrouching.transform.localPosition = Vector3.Lerp(cameraCrouching.transform.localPosition,
                Vector3.up * m_TargetCharacterHeight * CameraHeightRatio, CrouchingSharpness * Time.deltaTime);
            //m_Actor.AimPoint.transform.localPosition = characterController.center;
        }
    }

    bool SetCrouchingState(bool crouched, bool ignoreObstructions)
    {
        // set appropriate heights
        if (crouched)
        {
            m_TargetCharacterHeight = CapsuleHeightCrouching;
        }
        else
        {
            // Detect obstructions
            if (!ignoreObstructions)
            {
                Collider[] standingOverlaps = Physics.OverlapCapsule(
                    GetCapsuleBottomHemisphere(),
                    GetCapsuleTopHemisphere(CapsuleHeightStanding),
                    characterController.radius,
                    -1,
                    QueryTriggerInteraction.Ignore);
                foreach (Collider c in standingOverlaps)
                {
                    if (c != characterController)
                    {
                        return false;
                    }
                }
            }

            m_TargetCharacterHeight = CapsuleHeightStanding;
        }

        if (OnStanceChanged != null)
        {
            OnStanceChanged.Invoke(crouched);
        }

        IsCrouching = crouched;
        return true;
    }
    // Gets the center point of the bottom hemisphere of the character controller capsule    
    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * characterController.radius);
    }
    // Gets the center point of the top hemisphere of the character controller capsule    
    Vector3 GetCapsuleTopHemisphere(float atHeight)
    {
        return transform.position + (transform.up * (atHeight - characterController.radius));
    }
    void OnDie()
    {
        IsDead = true;
        canMove = false;
        WeaponsManager.SwitchToWeaponIndex(-1, true);
    }
}
