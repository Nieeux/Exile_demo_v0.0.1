using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController), typeof(PlayerStats), typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("Movement")]
    private float gravity = 20.0f;
    public Transform playerCamera;
    public Transform CameraPos;
    public Transform cameraCrouching;
    public float Sensitivity = 2.0f;
    public float lookXLimit = 90f;
    public float Speed { get; private set; }
    public float currentSpeed;
    public bool IsMoving;
    public bool isRunning;
    PlayerStats Stats;

    [Header("Crouch")]
    public float CameraHeightRatio = 0f;
    public float CapsuleHeightCrouching = 0.9f;
    public float CapsuleHeightStanding = 1.8f;
    public float CrouchingSharpness = 10f;
    public float m_TargetCharacterHeight;

    private bool canMove = true;

    PlayerInput InputHandler;
    CharacterController characterController;
    Inventory inventory;
    WeaponInventory weaponInventory;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;
    public float rotationX = 0;

    public bool IsGrounded { get; private set; }
    public bool IsCrouching { get; private set; }
    public bool IsDead { get; private set; }




    private void Awake()
    {
        PlayerMovement.Instance = this;

    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        weaponInventory = GetComponent<WeaponInventory>();
        InputHandler = GetComponent<PlayerInput>();
        Stats = GetComponent<PlayerStats>();
        inventory = GetComponent<Inventory>();
        Stats.OnSleep += Sleep;
        Stats.OnWakeUp += WakeUp;
        Stats.OnExhausted += OnExhausted;
        Stats.OnDie += OnDie;

        rotation.y = transform.eulerAngles.y;

        // force the crouch state to false when starting
        SetCrouchingState(false, true);
        UpdateCharacterHeight(true);
        GetSpeed();

    }

    private void Update()
    {
        if (characterController.isGrounded)
        {
            isRunning = InputHandler.GetRunning() && IsMoving && Stats.CanRun() && ActiveMenu() && IgnoreObstructions(false);
            if (isRunning)
            {
                SetCrouchingState(false, true);
            }

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            // di chuyen
            float curSpeedX = currentSpeed * Input.GetAxis("Vertical");
            float curSpeedY = currentSpeed * Input.GetAxis("Horizontal");
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
            var forwardsAmount = Vector3.Dot(transform.forward, moveDirection);

            if (InputHandler.GetJump())
            {
                moveDirection.y = currentSpeed;
            }

            IsMoving = moveDirection != Vector3.zero;

            if (IsMoving)
            {
                HeadBob.Instance.Headbob();
                WeaponAnimation.Instance.WeaponBob();
                UIBob.Instance.UIbob();
                GetSpeed();

            }

            if (InputHandler.GetCrouch() && !isRunning)
            {
                SetCrouchingState(!IsCrouching, false);

            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);

        // di chuyen camera
        if (ActiveMenu())
        {
            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * Sensitivity;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * Sensitivity, 0);
            }
        }

        UpdateCharacterHeight(false);
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
            IgnoreObstructions(ignoreObstructions);
        }

        IsCrouching = crouched;
        return true;
    }

    bool IgnoreObstructions(bool ignoreObstructions)
    {
        // Detect obstructions
        if (!ignoreObstructions)
        {
            Collider[] standingOverlaps = Physics.OverlapCapsule(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(CapsuleHeightStanding), characterController.radius, -1, QueryTriggerInteraction.Ignore);
            foreach (Collider c in standingOverlaps)
            {
                if (c != characterController)
                {
                    return false;
                }
            }
        }
        m_TargetCharacterHeight = CapsuleHeightStanding;
        return true;
    }

    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * characterController.radius);
    }
  
    Vector3 GetCapsuleTopHemisphere(float atHeight)
    {
        return transform.position + (transform.up * (1f - characterController.radius));
    }

    private bool ActiveMenu()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }

    public bool isCrouching()
    {
        return IsCrouching;
    }

    private void Sleep()
    {
        characterController.enabled = false;
        playerCamera.GetComponent<Rigidbody>().isKinematic = false;
        playerCamera.GetComponent<Rigidbody>().drag = 10;
        playerCamera.GetComponent<Collider>().enabled = true;
        weaponInventory.SwitchToWeaponIndex(-1, true);
        this.enabled = false;
        base.Invoke("CloseEye", 2f);

    }
    private void WakeUp()
    {

        playerCamera.transform.position = CameraPos.position;
        playerCamera.transform.rotation = CameraPos.rotation;
        playerCamera.GetComponent<Rigidbody>().isKinematic = true;
        playerCamera.GetComponent<Rigidbody>().drag = 0;
        playerCamera.GetComponent<Collider>().enabled = false;
        weaponInventory.SwitchToWeaponIndex(1, true);
        characterController.enabled = true;
        LoadingScenes.Instance.Show = false;
        this.enabled = true;
    }

    private void OnExhausted()
    {
        characterController.enabled = false;
        playerCamera.GetComponent<Rigidbody>().isKinematic = false;
        playerCamera.GetComponent<Collider>().enabled = true;
        weaponInventory.SwitchToWeaponIndex(-1, true);
        this.enabled = false;
        base.Invoke("CloseEye", 2f);
        
    }
    private void CloseEye()
    {
        LoadingScenes.Instance.Show = true;
    }

    private void OnDie()
    {
        Debug.Log("Player Die");
        IsDead = true;
        characterController.enabled = false;
        playerCamera.GetComponent<Rigidbody>().isKinematic = false;
        playerCamera.GetComponent<Collider>().enabled = true;
        weaponInventory.SwitchToWeaponIndex(-1, true);
        Destroy(this);
    }
}
