using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class InputSystem : MonoBehaviour
{
    [SerializeField]
    private Camera _screenCamera;
    [SerializeField]
    private LayerMask groundLayerMask;

    [SerializeField] 
    private Vector3 lastPosition;
    public event Action OnClicked, OnEsc;

    private void Update()
    {
    
        if(Input.GetMouseButtonDown(0))
            OnClicked?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape))
            OnEsc?.Invoke();
        
    }
    

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();
    
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos=Input.mousePosition;
        mousePos.z = _screenCamera.nearClipPlane;
        Ray ray = _screenCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,100f,groundLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}
