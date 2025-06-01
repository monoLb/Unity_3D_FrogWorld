using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public Transform buildingParent;
    public List<GameObject> buildingsList = new();
    private Stack<int> freeIndexes = new();

    public IntEventSO buildingCostEvent;

    public int PlaceBuilding(GameObject prefab, Vector3Int gridPos)
    {
        GameObject newObj = Instantiate(prefab, buildingParent);
        newObj.transform.position = gridPos;
        var objFBX = newObj.transform.GetChild(0);
        if (objFBX.GetComponent<UnityEngine.AI.NavMeshObstacle>() == null)
        {
            var obstacle= objFBX.gameObject.AddComponent<UnityEngine.AI.NavMeshObstacle>();
            Vector3 newV3 = obstacle.size;
            obstacle.size = newV3*0.4f;
            obstacle.carving = true;//挖去这一块
        }

        int returnIndex;
        if (freeIndexes.Count > 0)
        {
            int stackIndex = freeIndexes.Pop();
            buildingsList[stackIndex] = newObj;
            returnIndex = stackIndex;
        }
        else
        {
            buildingsList.Add(newObj);
            returnIndex = buildingsList.Count - 1;
        }

        return returnIndex;
    }

    public void RemoveBuildingAt(int gameObjectIndex)
    {
        if (gameObjectIndex < 0 || gameObjectIndex >= buildingsList.Count)
            return;

        Destroy(buildingsList[gameObjectIndex]);
        buildingsList[gameObjectIndex] = null;
        freeIndexes.Push(gameObjectIndex);
    }

    public void CostEvent(int value)
    {
        ResourceManager.Instance.ReduceMoney(value);
    }
}
