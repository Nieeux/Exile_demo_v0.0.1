using UnityEngine;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("PlayerStats")]
    public float gravity = 20.0f;
    public Transform playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 90f;
    public float Speed = 5f;
    Health health;

    [HideInInspector]
    public bool canMove = true;
    public LineRenderer line;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;
    public float rotationX = 0;

    [Header("DetectItem")]
    public Transform DetectCamera;
    public LayerMask whatIsInteractable;
    public GameObject interactUI;
    private Transform interactUi;
    private TextMeshProUGUI interactText;
    private Collider currentCollider;

    public static Player Instance { get; private set; }
    public Interactable currentInteractable { get; private set; }
    public bool IsDead { get; private set; }

    PlayerWeaponManager WeaponsManager;


    private void Awake()
    {
        Player.Instance = this;
        this.interactUi = Object.Instantiate<GameObject>(this.interactUI).transform;
        this.interactText = this.interactUi.GetComponentInChildren<TextMeshProUGUI>();
        this.interactUi.gameObject.SetActive(false);
    }


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        WeaponsManager = GetComponent<PlayerWeaponManager>();
        health = GetComponent<Health>();
        health.OnDie += OnDie;
        rotation.y = transform.eulerAngles.y;

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

        
    }
    void LateUpdate()
    {
        DetectItem();
    }

    void OnDie()
    {
        IsDead = true;
        canMove = false;
        WeaponsManager.SwitchToWeaponIndex(-1, true);
    }
    private void DetectItem()
    {
        // Detect Item
        RaycastHit Hit;
        if (Physics.Raycast(DetectCamera.transform.position, DetectCamera.transform.forward, out Hit, 2) && Hit.transform.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            line.enabled = true;
            line.SetPosition(0, DetectCamera.transform.position);
            line.SetPosition(1, Hit.point);
            this.currentInteractable = Hit.collider.gameObject.GetComponent<Interactable>();
            if (this.currentInteractable == null)
            {
                return;
            }

            if (this.currentInteractable != null)
            {
                this.currentCollider = Hit.collider;
            }
            this.interactUi.gameObject.SetActive(true);
            this.interactText.text = (this.currentInteractable.GetName() ?? "");
            this.interactUi.transform.position = Hit.collider.gameObject.transform.position + Vector3.up * Hit.collider.bounds.extents.y;
            this.interactText.CrossFadeAlpha(1f, 0.1f, false);
            return;
            // di vao trigger hien ten item
            if (Hit.collider.isTrigger)
            {

            }
        }
        else
        {
            line.enabled = false;
            this.currentCollider = null;
            this.currentInteractable = null;
            this.interactText.CrossFadeAlpha(0f, 0.1f, false);
        }
    }
}
