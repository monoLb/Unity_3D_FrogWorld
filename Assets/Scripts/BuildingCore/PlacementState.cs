using System;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int objectIndex = -1;
    private int ID;
    private Grid grid;
    private PreviewSystem previewSystem;
    private ObjectData objectData;
    private GridData floorData;
    private GridData furnitureData;
    private ObjectPlacer objectPlacer;
    
    //实时检测能否被创建
    private bool buildValidity;
    

    public PlacementState(int id,
        Grid grid,
        PreviewSystem previewSystem,
        ObjectData objectData,
        GridData floorData,
        GridData furnitureData,
        ObjectPlacer objectPlacer)
    {
        ID = id;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.objectData = objectData;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        if (ID > -1)
        {
            previewSystem.StopShowingPlacementPreview();
            previewSystem.StartShowingPlacementPreview(objectData.prefab, objectData.Size);
        }
        else
        {
            throw new SystemException($"No object with ID: {ID} was found.");
        }
        
    }

    public void EndState()
    {
        previewSystem.StopShowingPlacementPreview();
    }

    public void OnState(Vector3Int gridPos)
    {
        
        if (!buildValidity)
        {
            AudioManager.Instance.PlaySound(SoundType.falseSound);
            return;
        }
        
        if (objectData.prefab)
        {
            //完成建造
            int placeIndex=objectPlacer.PlaceObject(objectData.prefab,gridPos);
               
            AudioManager.Instance.PlaySound(SoundType.trueSound);
            //根据是否为地板保存在不同的GridData中
            GridData selectedObj=objectData.ID==0?floorData:furnitureData;
            selectedObj.AddObjectAt(gridPos,objectData.Size,objectData.ID,placeIndex);
            previewSystem.UpdatePosition(gridPos,buildValidity);
            
            
        }
        else
        {
            AudioManager.Instance.PlaySound(SoundType.falseSound);
        }
    }
    
    private bool CheckBuildValidity(Vector3Int gridPos, Vector2Int gridSize)
    {
        GridData currentGrid = objectData.ID == 0 ? floorData: furnitureData;
        return currentGrid.CheckIfOccupy(gridPos, gridSize);
    }

    public void UpdateState(Vector3Int gridPos)
    {
        buildValidity=CheckBuildValidity(gridPos,objectData.Size);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPos),buildValidity);
    }
}
