using UnityEngine;

public class PlayerItemInspection : MonoBehaviour
{
    public static PlayerItemInspection Instance;
    
    public Camera playerCamera;
    public float inspectRotationSpeed;
    public UVLighter uvLighter;
    
    private GameObject _currentHeldItem;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Update()
    {
        HandleInspection();
    }

    public void BeginInspection(GameObject currentHeldItem)
    {
        _currentHeldItem = currentHeldItem;
    }

    public void EndInspection()
    {
        _currentHeldItem = null;
        uvLighter.ToggleLighterOff();
    }
    
    private void HandleInspection()
    {
        if (_currentHeldItem == null)
            return;
        
        if (Input.GetMouseButton(0))
        {
            Vector2 mouseDelta = Input.mousePositionDelta;

            _currentHeldItem.transform.Rotate(playerCamera.transform.up, -mouseDelta.x * inspectRotationSpeed * Time.deltaTime, Space.World);
            _currentHeldItem.transform.Rotate(playerCamera.transform.right, mouseDelta.y * inspectRotationSpeed * Time.deltaTime, Space.World);
        }
    }
}