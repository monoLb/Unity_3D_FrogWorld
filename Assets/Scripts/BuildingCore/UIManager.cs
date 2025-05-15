using System;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManagerment : MonoBehaviour
{
    [FormerlySerializedAs("database")] [SerializeField]
    private ObjectDatabaseSO objectDatabase;
    [SerializeField]
    private GameObject objectPrefab;
    [SerializeField]
    private Transform objectsParentGroup;
    private void Start()
    {
        GenerateUIButton();
    }

    private void GenerateUIButton()
    {
        foreach (var data in objectDatabase.ObjectDataList)
        {
            ObjectUI obj= Instantiate(objectPrefab, objectsParentGroup).GetComponent<ObjectUI>();
            
            obj.objectData = data;
        }
    }
}
