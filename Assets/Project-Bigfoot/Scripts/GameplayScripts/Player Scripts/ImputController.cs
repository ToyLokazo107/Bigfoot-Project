using UnityEngine;
using System;

public class InputController : MonoBehaviour
{
    private InputSystem_Actions playerControls;
    public PlayerMovement playerMovement;

    [Header("Interaction Settings")]
    public Camera playerCamera;
    public float interactRange = 3.5f;
    public LayerMask interactableLayer;

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

        playerControls.Player.Interact.started += ctx => HandleInteraction();
        playerControls.Player.Attack.started += ctx => HandleUseItem();

        playerControls.Player.Drop.started += ctx => HandleDropItem();
        playerControls.Player.Slot1.started += ctx => HandleSwitchSlot(0); 
        playerControls.Player.Slot2.started += ctx => HandleSwitchSlot(1); 
        playerControls.Player.Slot3.started += ctx => HandleSwitchSlot(2);
    }

    private void HandleUseItem()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentStatus != GameStatus.EnCaceria)
            return;

        GameObject puntoMano = GameObject.Find("ManoDerecha");
        if (puntoMano != null && puntoMano.transform.childCount > 0)
        {
            ItemRecolectable itemEnMano = puntoMano.GetComponentInChildren<ItemRecolectable>();
            if (itemEnMano != null)
            {
                itemEnMano.AlternarUso();
            }
        }
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

    private void HandleInteraction()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentStatus != GameStatus.EnCaceria)
            return;

        if (playerCamera == null) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactableLayer, QueryTriggerInteraction.Collide))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    private void HandleDropItem()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentStatus != GameStatus.EnCaceria)
            return;

        PlayerInventory inventario = FindFirstObjectByType<PlayerInventory>();
        if (inventario != null)
        {
            inventario.SoltarObjetoActual();
        }
    }

    private void HandleSwitchSlot(int nuevoSlot)
    {
        if (GameManager.Instance != null && GameManager.Instance.currentStatus != GameStatus.EnCaceria)
            return;

        PlayerInventory inventario = FindFirstObjectByType<PlayerInventory>();
        if (inventario != null)
        {
            inventario.CambiarSlotActivo(nuevoSlot);
        }
    }

    private void OnDrawGizmos()
    {
        if (playerCamera == null) return;

        Gizmos.color = Color.cyan;
        Vector3 lookDirection = playerCamera.transform.forward;
        Gizmos.DrawRay(playerCamera.transform.position, lookDirection * interactRange);
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