using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance;
    
    public Camera playerCamera;
    public Hands hands;
    public Transform holdPosition;
    public float interactionDistance = 10f;

    private IInteractable _currentInteractable;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInteraction();
        }

        if (Input.GetKeyDown(KeyCode.E) && _currentInteractable != null)
        {
            HandleStopInteraction();
        }
    }


    private void HandleInteraction()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, interactionDistance))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable) && _currentInteractable == null)
            {
                _currentInteractable = interactable;
                _currentInteractable.Interact(holdPosition);
                hands.PlayTakeItem();
            }
        }
    }

    public void HandleStopInteraction()
    {
        _currentInteractable.StopInteract();
        _currentInteractable = null;
    }
}