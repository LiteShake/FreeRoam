using UnityEngine;
using UnityEngine.InputSystem;

public class HumanController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    private CharacterController controller;
    private Vector3 movementInput;
    private float verticalVelocity;
    private bool isSprinting;
    private bool isGrounded;

    // Input System
    private PlayerInput playerInput;
    private InputAction moveAction, jumpAction, sprintAction;

    // Interaction
    private InteractionHandler interactionHandler;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        sprintAction = playerInput.actions["Sprint"];

        interactionHandler = new InteractionHandler(this);
    }

    private void OnEnable()
    {
        interactionHandler.Enable();
    }

    private void OnDisable()
    {
        interactionHandler.Disable();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        CheckGround();
    }

    private void HandleMovement()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        movementInput = new Vector3(input.x, 0, input.y);

        isSprinting = sprintAction.IsPressed();
        float speed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 move = transform.right * movementInput.x + transform.forward * movementInput.z;
        controller.Move(move * speed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        if (jumpAction.triggered && isGrounded)
            verticalVelocity = jumpForce;

        verticalVelocity += Physics.gravity.y * Time.deltaTime;
        controller.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundLayer);
    }

    public void EnableMovement(bool enable)
    {
        this.enabled = enable;
    }
}
