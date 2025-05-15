using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private Transform objectParent;
    [SerializeField] private List<GameObject> ObjectList = new();

    private Stack<int> freeIndexes = new();

    public int PlaceObject(GameObject prefab, Vector3Int gridPos)
    {
        GameObject newObj = Instantiate(prefab, objectParent);
        newObj.transform.position = gridPos;

        int returnIndex;
        if (freeIndexes.Count > 0)
        {
            int stackIndex = freeIndexes.Pop();
            ObjectList[stackIndex] = newObj;
            returnIndex = stackIndex;
        }
        else
        {
            ObjectList.Add(newObj);
            returnIndex = ObjectList.Count - 1;
        }

        return returnIndex;
    }

    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (gameObjectIndex < 0 || gameObjectIndex >= ObjectList.Count)
            return;

        Destroy(ObjectList[gameObjectIndex]);
        ObjectList[gameObjectIndex] = null;
        freeIndexes.Push(gameObjectIndex);
    }
}