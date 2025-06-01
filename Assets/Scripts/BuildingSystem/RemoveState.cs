using UnityEngine;

public class RemoveState : IConstructState
{
    private int gameObjectIndex = -1;
    private Grid grid;
    private PreviewManagement previewManagement;
    private GridData gridData;
    private BuildingPlacer buildingPlacer;
    private BuildingDestroyer buildingDestroyer;

    public RemoveState(Grid grid,
                        PreviewManagement previewManagement,
                        GridData gridData,
                        BuildingPlacer buildingPlacer,
                        BuildingDestroyer buildingDestroyer)
    {
        this.grid = grid;
        this.previewManagement = previewManagement;
        this.gridData = gridData;
        this.buildingPlacer = buildingPlacer;
        this.buildingDestroyer = buildingDestroyer;
        
    }
    
    public void EndState()
    {
        previewManagement.StopShowingPlacementPreview();
    }

    public void OnState(Vector3Int gridPos)
    {
        

        if (gridData == null)
        {
            //sound
            AudioManager.Instance.PlaySound(SoundType.falseSound);
        }
        else
        {
            gameObjectIndex = gridData.GetRepresentationIndex(gridPos);
            if (gameObjectIndex == -1)
                return;
            //字典中删除
            gridData.RemoveObjectDictAt(gridPos);
            //删除预制体
            buildingPlacer.RemoveBuildingAt(gameObjectIndex);
            
            AudioManager.Instance.PlaySound(SoundType.destroySound);
        }
        // Vector3 RemovePosition=grid.CellToWorld(gridPos);
        buildingDestroyer.PlayAnimation();
        
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool removeValidity=CheckIfDestroyIsVaild(gridPos);
        buildingDestroyer.UpdatePosition(gridPos);
        previewManagement.UpdatePosition(grid.CellToWorld(gridPos),removeValidity);
    }

    private bool CheckIfDestroyIsVaild(Vector3Int gridPos)
    {
        return !gridData.CheckIfOccupy(gridPos, Vector2Int.one);

    }

}
