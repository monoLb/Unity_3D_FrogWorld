using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingUICreator : MonoBehaviour
{
    [Header("建造")]
    public BuildingDataSO buildingData;
    public GameObject buildingUIPrefab;
    public Transform _buildingParent;

    private List<GameObject> BuildingUIList = new List<GameObject>();

    private void GenerateBuildingUI()
    {
        foreach (var data in buildingData.buildingList)
        {
            BuildingUI buildingUI = Instantiate(buildingUIPrefab, _buildingParent).GetComponent<BuildingUI>();
            buildingUI.buildingData = data;
            BuildingUIList.Add(buildingUI.gameObject);
        }
    }

    public void UpdateBuildingUI()
    {
        foreach (var obj in BuildingUIList)
        {
            Destroy(obj);
        }
        BuildingUIList.Clear();
        
        GenerateBuildingUI();
        
    }
}


