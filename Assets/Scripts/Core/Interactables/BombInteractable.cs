using UnityEngine;

public class BombInteractable : MonoBehaviour, IInteractable
{
    public void Interact(Transform holdPosition)
    {
        GameManager.Instance.BadEnd();
        Destroy(gameObject);
    }

    public void StopInteract()
    {
    }
}