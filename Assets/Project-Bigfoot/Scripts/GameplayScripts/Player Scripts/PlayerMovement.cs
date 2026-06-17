using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 15f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundLayer;
    public Transform playerCamera;

    private CharacterController controller;
    private StaminaPlayer staminaPlayer;

    private Vector3 velocity;
    private bool isGrounded;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        staminaPlayer = GetComponent<StaminaPlayer>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void ProcessMovement(Vector2 moveInput, bool isSprinting)
    {
        float currentSpeed = moveSpeed;

        if (isSprinting && moveInput.magnitude > 0.1f && staminaPlayer.CanSprint())
        {
            currentSpeed = sprintSpeed;
            staminaPlayer.DecreaseStamina();
        }
        else
        {
            staminaPlayer.IncreaseStamina();
        }

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    public void ProcessLook(Vector2 lookInput)
    {
        yRotation += lookInput.x * mouseSensitivity * Time.deltaTime;
        xRotation -= lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    public void ProcessJump()
    {
        if (isGrounded)
        {
            velocity.y = jumpForce;
        }
    }
}