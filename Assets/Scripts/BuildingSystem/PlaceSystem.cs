using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlaceSystem : MonoBehaviour
{
    [FormerlySerializedAs("inputManagement")] public InputSystem inputSystem;
    public Grid grid;
    
    public GridData gridData;
    
    public PreviewManagement previewManagement;

    private IConstructState constructState;
    
    public BuildingPlacer placer;
    public BuildingDestroyer destroyer;

    private void Start()
    {
        StartPlacement();
        gridData = new();
    }


    private void Update()
    {
     
        if(constructState == null)
            return;
        var mousePos = inputSystem.GetSelectedMapPosition();
        var gridPos = grid.WorldToCell(mousePos);

        if (constructState != null)
        {
            constructState.UpdateState(gridPos);
        }
        
        
    }

    

    public void SetCurrentBuildingData(BuildingData data)
    {
        if(data == null)
            return;
        BindInputActions();
        QuitBuildingState();
        
        constructState = null;

        constructState = new PlaceState(data.ID, grid, previewManagement, data, gridData, placer);
        
    }

    public void QuitBuildingState()
    {
        destroyer.QuiteRemovingState();
    }

    public void StartRemoving()
    {
        StopPlacement();

        constructState = null;
        constructState=new RemoveState(grid,previewManagement,gridData,placer,destroyer);
        destroyer.CreateAShovel();
        
        BindInputActions();
    }

    private void StartPlacement()
    {
        BindInputActions();
        
    }
    
  
    
    private void StopPlacement()
    {
        if (constructState==null)
            return;
        
        constructState.EndState();
        constructState = null;
        Debug.Log("Stop placement");
        
        previewManagement.StopShowingPlacementPreview();
        UnbindInputActions();
    }
    
    private void BindInputActions()
    {
        inputSystem.OnClicked -= PlaceStucture;
        inputSystem.OnEsc -= StopPlacement;
        inputSystem.OnClicked += PlaceStucture;
        inputSystem.OnEsc += StopPlacement;
    }

    private void UnbindInputActions()
    {
        inputSystem.OnClicked -= PlaceStucture;
        inputSystem.OnEsc -= StopPlacement;
    }  
    
    private void PlaceStucture()
    {
        if(constructState==null)
            return;
        if (inputSystem.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePos = inputSystem.GetSelectedMapPosition();
        Vector3Int gridPos=grid.WorldToCell(mousePos);
        
        
        constructState.OnState(gridPos);
    }
}
