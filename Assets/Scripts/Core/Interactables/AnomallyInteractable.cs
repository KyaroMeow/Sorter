using UnityEngine;

public class AnomallyInteractable : MonoBehaviour, IInteractable
{
    public void Interact(Transform holdPosition)
    {
        GameManager.Instance.StartAnomally();
        PlayerInteraction.Instance.HandleStopInteraction();
    }

    public void StopInteract()
    {
        Destroy(gameObject);
    }
}