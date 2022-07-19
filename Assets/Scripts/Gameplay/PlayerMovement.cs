using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController), typeof(PlayerStats), typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("Movement")]
    public float gravity = 20.0f;
    public Transform playerCamera;
    public Transform cameraCrouching;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 90f;
    public float walkingSpeed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2f;

    public float currentSpeed;
    public bool IsMoving;
    public bool isRunning;
    PlayerStats Stats;

    [Header("Crouch")]
    [Tooltip("Ratio (0-1) of the character height where the camera will be at")]
    public float CameraHeightRatio = 0f;
    [Tooltip("Height of character when crouching")]
    public float CapsuleHeightCrouching = 0.9f;
    [Tooltip("Height of character when standing")]
    public float CapsuleHeightStanding = 1.8f;
    [Tooltip("Speed of crouching transitions")]
    public float CrouchingSharpness = 10f;
    public float m_TargetCharacterHeight;

    [Header("HeadBob")]
    public AnimationCurve xCurve;
    public AnimationCurve yCurve;

    [HideInInspector]
    public bool canMove = true;

    PlayerInput InputHandler;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;
    public float rotationX = 0;


    public UnityAction<bool> OnStanceChanged;
    public bool IsGrounded { get; private set; }
    public bool IsCrouching { get; private set; }
    public bool IsDead { get; private set; }

    PlayerWeaponManager WeaponsManager;


    private void Awake()
    {
        PlayerMovement.Instance = this;

    }


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        WeaponsManager = GetComponent<PlayerWeaponManager>();
        InputHandler = GetComponent<PlayerInput>();
        Stats = GetComponent<PlayerStats>();
        Stats.OnDie += OnDie;
        rotation.y = transform.eulerAngles.y;

        // force the crouch state to false when starting
        SetCrouchingState(false, true);
        UpdateCharacterHeight(true);

    }

    private void Update()
    {

        //Input
        if (InputHandler.GetCrouchInputDown())
        {
            Debug.Log("Crouch");
            SetCrouchingState(!IsCrouching, false);

        }

        if (characterController)
        {
            
            isRunning = Input.GetKey(KeyCode.LeftShift) && IsMoving && Stats.CanRun() ;
            currentSpeed = isRunning && IsMoving ? runSpeed : walkingSpeed;
            currentSpeed = IsCrouching && IsMoving ? crouchSpeed : currentSpeed;
        }
        if (characterController.isGrounded)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            // di chuyen
            float curSpeedX = currentSpeed * Input.GetAxis("Vertical");
            float curSpeedY = currentSpeed * Input.GetAxis("Horizontal");
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
            var forwardsAmount = Vector3.Dot(transform.forward, moveDirection);

            if (Input.GetButtonDown("Jump") && canMove)
            {
                moveDirection.y = walkingSpeed;
            }


            IsMoving = moveDirection != Vector3.zero;
            //HeadBob
            if (IsMoving)
            {

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

        UpdateCharacterHeight(false);
    }
    void UpdateCharacterHeight(bool force)
    {
        // Update height instantly
        if (force)
        {
            characterController.height = m_TargetCharacterHeight;

        }
        // Update smooth height
        else if (characterController.height != m_TargetCharacterHeight)
        {
            characterController.height = Mathf.Lerp(characterController.height, m_TargetCharacterHeight,
                CrouchingSharpness * Time.deltaTime);
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
