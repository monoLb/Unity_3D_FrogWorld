using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "DatabaseSO", menuName = "ScriptableObject/DatabaseSO")]
public class ObjectDatabaseSO : ScriptableObject
{
    public List<ObjectData> ObjectDataList;   
}

[Serializable]
public class ObjectData
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

}