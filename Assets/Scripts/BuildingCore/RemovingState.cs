using System;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    private Grid grid;
    private PreviewSystem previewSystem;
    private GridData floorData;
    private GridData furnitureData;
    private ObjectPlacer objectPlacer;
    private ObjectDestroyer objectDestroyer;

    public RemovingState(Grid grid,
                        PreviewSystem previewSystem,
                        GridData floorData,
                        GridData furnitureData,
                        ObjectPlacer objectPlacer,
                        ObjectDestroyer objectDestroyer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;
        this.objectDestroyer = objectDestroyer;
        
        previewSystem.StartShowingRemovingPreview();
    }
    
    public void EndState()
    {
        previewSystem.StopShowingPlacementPreview();
    }

    public void OnState(Vector3Int gridPos)
    {
        GridData selecedData = null;
        if (!furnitureData.CheckIfOccupy(gridPos, Vector2Int.one))
        {
            selecedData = furnitureData;
        }
        else if(!floorData.CheckIfOccupy(gridPos, Vector2Int.one))
        {
            selecedData = floorData;
        }

        if (selecedData == null)
        {
            //sound
            AudioManager.Instance.PlaySound(SoundType.falseSound);
        }
        else
        {
            gameObjectIndex = selecedData.GetRepresentationIndex(gridPos);
            if (gameObjectIndex == -1)
                return;
            //字典中删除
            selecedData.RemoveObjectDictAt(gridPos);
            //删除预制体
            objectPlacer.RemoveObjectAt(gameObjectIndex);
            
            AudioManager.Instance.PlaySound(SoundType.destroySound);
        }
        // Vector3 RemovePosition=grid.CellToWorld(gridPos);
        objectDestroyer.PlayAnimation();
        
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool removeValidity=CheckIfDestroyIsVaild(gridPos);
        objectDestroyer.UpdatePosition(gridPos);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPos),removeValidity);
    }

    private bool CheckIfDestroyIsVaild(Vector3Int gridPos)
    {
        return  !furnitureData.CheckIfOccupy(gridPos, Vector2Int.one)||
                !floorData.CheckIfOccupy(gridPos, Vector2Int.one);
    }

   
}
