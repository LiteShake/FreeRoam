using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionHandler
{
    private PlayerInput playerInput;
    private InputAction interactAction;
    private GameObject nearbyInteractable;

    public InteractionHandler(MonoBehaviour parent)
    {
        playerInput = parent.GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
    }

    public void Enable()
    {
        interactAction.performed += HandleInteract;
    }

    public void Disable()
    {
        interactAction.performed -= HandleInteract;
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        if (nearbyInteractable != null)
        {
            var interactable = nearbyInteractable.GetComponent<IInteractable>();
            interactable?.Interact();
        }
    }

    public void SetNearbyInteractable(GameObject interactable)
    {
        nearbyInteractable = interactable;
    }

    public void ClearNearbyInteractable()
    {
        nearbyInteractable = null;
    }
}
