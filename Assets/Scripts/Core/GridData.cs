using System;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    private Dictionary<Vector3Int, PlacementData> placedObjectDict = new();

    public void AddObjectAt(Vector3Int GridPos, Vector2Int objSize, int id,int placeObjIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePosition(GridPos, objSize);
        PlacementData data=new PlacementData(positionToOccupy,id,placeObjIndex);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjectDict.ContainsKey(pos))
                throw new Exception($"Dictionary already contains this cell position {pos}");
            placedObjectDict[pos]=data;
        }
    }

    private List<Vector3Int> CalculatePosition(Vector3Int gridPos, Vector2Int objSize)
    {
        List<Vector3Int> returnValue = new();
        for (int x = 0; x < objSize.x; x++)
        {
            for (int y = 0; y < objSize.y; y++)
            {
                returnValue.Add(gridPos+new Vector3Int(x,0,y));
            }
        }
        return returnValue;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPos, Vector2Int objSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPos, objSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjectDict.ContainsKey(pos))
                return false;
        }
        return true;
    }
    
}

public class PlacementData
{
    public List<Vector3Int> occupiedCells = new();
    public int ID { get;private set; }
    //记录创建的家具的编号，删除时方便。
    public int PlaceObjectIndex { get;private set; }

    public PlacementData(List<Vector3Int> occ, int id, int placeObjectIndex)
    {
        this.occupiedCells = occ;
        this.ID = id;
        this.PlaceObjectIndex = placeObjectIndex;
    }
}
