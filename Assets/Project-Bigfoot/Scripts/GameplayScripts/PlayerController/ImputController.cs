using UnityEngine;
using System;

public class InputController : MonoBehaviour
{
    private InputSystem_Actions playerControls;
    public PlayerMovement playerMovement;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public Action OnJumpStarted;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();

        playerControls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        playerControls.Player.Move.canceled += ctx => MoveInput = Vector2.zero;

        playerControls.Player.Look.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
        playerControls.Player.Look.canceled += ctx => LookInput = Vector2.zero;

        playerControls.Player.Jump.started += ctx => HandleJump();
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentStatus != GameStatus.EnCaceria)
            return;

        playerMovement.ProcessMovement(MoveInput);
        playerMovement.ProcessLook(LookInput);
    }

    private void HandleJump()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentStatus != GameStatus.EnCaceria)
            return;

        playerMovement.ProcessJump();
        OnJumpStarted?.Invoke();
    }

    public void EnableInput()
    {
        playerControls.Enable();
    }

    public void DisableInput()
    {
        playerControls.Disable();
    }

    private void OnEnable()
    {
        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }
}