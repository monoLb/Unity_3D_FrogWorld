using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingDataSO", menuName = "ScriptableObjects/BuildingDataSO")]
public class BuildingDataSO : ScriptableObject
{
    public List<BuildingData> buildingList;
    
}

[Serializable]
public class BuildingData
{
    [field:SerializeField]
    public string name { get;private set; }
    [field:SerializeField]
    public int ID{ get; private set;}
    [field:SerializeField]
    public Sprite sprite{ get; private set;}
    [field:SerializeField]
    public Vector2Int Size{ get; private set;}
    [field:SerializeField]
    public GameObject prefab{ get;private set; }
    [field:SerializeField]
    [TextArea(3, 10)]
    public string description;
    [field: SerializeField] 
    public int requirements;
    [field:SerializeField]
    public int DependecyLevel;
    [field:SerializeField]
    public bool isLocked;

}