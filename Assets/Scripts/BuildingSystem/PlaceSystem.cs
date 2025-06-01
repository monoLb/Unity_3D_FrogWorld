using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlaceSystem : MonoBehaviour
{
    public InputManagement inputManagement;
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
        var mousePos = inputManagement.GetSelectedMapPosition();
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
        inputManagement.OnClicked -= PlaceStucture;
        inputManagement.OnEsc -= StopPlacement;
        inputManagement.OnClicked += PlaceStucture;
        inputManagement.OnEsc += StopPlacement;
    }

    private void UnbindInputActions()
    {
        inputManagement.OnClicked -= PlaceStucture;
        inputManagement.OnEsc -= StopPlacement;
    }  
    
    private void PlaceStucture()
    {
        if(constructState==null)
            return;
        if (inputManagement.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePos = inputManagement.GetSelectedMapPosition();
        Vector3Int gridPos=grid.WorldToCell(mousePos);
        
        
        constructState.OnState(gridPos);
    }
}
