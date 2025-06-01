using System;
using UnityEngine;

public class PlaceState : IConstructState
{
    private int buildingIndex = -1;
    private int ID;
    private Grid grid;
    private PreviewManagement previewManagement;
    private BuildingData buildingData;
    private GridData gridData;
    private BuildingPlacer buildingPlacer;
    
    //实时检测能否被创建
    private bool buildValidity;

    public PlaceState(int id,
        Grid grid,
        PreviewManagement previewManagement,
        BuildingData buildingData,
        GridData gridData,
        BuildingPlacer buildingPlacer)
    {
        ID = id;
        this.grid = grid;
        this.previewManagement = previewManagement;
        this.buildingData = buildingData;
        this.gridData = gridData;
        this.buildingPlacer = buildingPlacer;

        if (ID > -1)
        {
            previewManagement.StopShowingPlacementPreview();
            previewManagement.StartShowingPlacementPreview(buildingData.prefab, this.buildingData.Size);
        }
        else
        {
            throw new SystemException($"No object with ID: {ID} was found.");
        }

    }

    public void EndState()
    {
        previewManagement.StopShowingPlacementPreview();
    }

    public void OnState(Vector3Int gridPos)
    {
        if (!buildValidity)
        {
            AudioManager.Instance.PlaySound(SoundType.falseSound);
            return;
        }
        
        if (buildingData.prefab)
        {
            //完成建造
            int placeIndex=buildingPlacer.PlaceBuilding(buildingData.prefab,gridPos);
               
            buildingPlacer.CostEvent(buildingData.requirements);
            AudioManager.Instance.PlaySound(SoundType.trueSound);
            //根据是否为地板保存在不同的GridData中
            gridData.AddObjectAt(gridPos,buildingData.Size,buildingData.ID,placeIndex);
            previewManagement.UpdatePosition(gridPos,buildValidity);
            
        }
        // else
        // {
        //     AudioManager.Instance.PlaySound(SoundType.falseSound);
        // }
    }

    private bool CheckBuildValidity(Vector3Int gridPos, Vector2Int gridSize)
    {
        if(ResourceManager.Instance.moneyAmount < buildingData.requirements) 
            return false;
        return gridData.CheckIfOccupy(gridPos, gridSize);
    }

    public void UpdateState(Vector3Int gridPos)
    {
        
        buildValidity=CheckBuildValidity(gridPos,buildingData.Size);
        previewManagement.UpdatePosition(grid.CellToWorld(gridPos),buildValidity);
    }
}
