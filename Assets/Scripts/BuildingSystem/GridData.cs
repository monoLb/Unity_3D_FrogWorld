using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridData
{
    private Dictionary<Vector3Int, placementData> placedObjectDict = new();

    public void AddObjectAt(Vector3Int gridPos, Vector2Int gridSize,int id,int objIndex)
    {
        List<Vector3Int> occupiedList = CalculateOccupy(gridPos, gridSize);
        placementData data=new placementData(occupiedList,id,objIndex);
        foreach (var pos in occupiedList)
        {
            if(placedObjectDict.ContainsKey(pos))
                throw new Exception("Object already occupied");
            placedObjectDict[pos] = data;
        }
    }

    private List<Vector3Int> CalculateOccupy(Vector3Int gridPos, Vector2Int gridSize)
    {
        List<Vector3Int> calculateList = new ();
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                calculateList.Add(gridPos + new Vector3Int(x, 0, y));
            }
        }
        return calculateList;
    }

    public bool CheckIfOccupy(Vector3Int gridPos, Vector2Int gridSize)
    {
        List<Vector3Int> checkList = CalculateOccupy(gridPos, gridSize);
        foreach (var pos in checkList)
        {
            if (placedObjectDict.ContainsKey(pos))
                return false;
        }
        return true;
    }

    public int GetRepresentationIndex(Vector3Int gridPos)
    {
        if (placedObjectDict.ContainsKey(gridPos) == false)
            return -1;
        return placedObjectDict[gridPos].gridIndex;
    }

    public void RemoveObjectDictAt(Vector3Int gridPos)
    {
        foreach (var pos in placedObjectDict[gridPos].gridOne2AllList)
        {
            placedObjectDict.Remove(pos);
        }
    }
}

public class placementData
{
    public List<Vector3Int> gridOne2AllList = new();
    public int ID { get;private set; }
    public int gridIndex { get; private set; }

    public placementData(List<Vector3Int> list, int id, int gridIndex)
    {
        this.gridOne2AllList = list;
        this.ID = id;
        this.gridIndex = gridIndex;
    }
}