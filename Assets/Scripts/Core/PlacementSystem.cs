using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator;

    [FormerlySerializedAs("cellIndicator")] [FormerlySerializedAs("buildIndicator")] [SerializeField]
    private GameObject BuildIndicator;
    
    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;
    
    //从UI点击获取数据
    private ObjectData currentObjData;
 

    [SerializeField]
    private AudioClip falsePlaceAudio,truePlaceAudio;


    //放置的父物体位置
    [SerializeField] private Transform _placementTransParent;

    //建造模式
    public bool isPlacing = false;
    
    private GridData floorData,furnitureData;
    private Renderer previewRenderer;
    //创建的对象
    private List<GameObject> placedGameObjectList = new();

    private void Start()
    {
        
        StartPlacement();
        floorData = new GridData();
        furnitureData = new GridData();
        previewRenderer = BuildIndicator.GetComponent<Renderer>();
    }
    
    private void Update()
    {
        var mousePos = inputManager.GetSelectedMapPosition();
        mouseIndicator.transform.position = mousePos;
        if(isPlacing)
        {
            var gridPos = grid.WorldToCell(mousePos);
            int index = currentObjData.ID;
            bool placementValidity = CheckPlacementValidity(gridPos, index);
            previewRenderer.material.color = placementValidity ? Color.white : Color.red;
            
            BuildIndicator.transform.position = grid.CellToWorld(gridPos);
        }

    }

    public void SetCurrentObjectData(ObjectData data)
    {
        currentObjData = data;
        mouseIndicator.SetActive(true);
        BuildIndicator.SetActive(true);
    }
    public void StartPlacement()
    {
        mouseIndicator.SetActive(false);
        BuildIndicator.SetActive(false);
        inputManager.OnClicked += PlaceStucture;
        inputManager.OnEsc += StopPlacement;
    }

    private void PlaceStucture()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos=grid.WorldToCell(mousePos);

        if (currentObjData != null)
        {
            bool placementValidity=CheckPlacementValidity(gridPos, currentObjData.ID);

            if (placementValidity == false)
            {
                AudioManager.Instance.PlaySound(falsePlaceAudio,Camera.main.transform.position,1);
                return;
            }
            if (currentObjData.prefab)
            {
                GameObject newObj = Instantiate(currentObjData.prefab,_placementTransParent);
                var objPos=grid.CellToWorld(gridPos); 
                newObj.transform.position = objPos;
                
                AudioManager.Instance.PlaySound(truePlaceAudio,Camera.main.transform.position,1);
                
                placedGameObjectList.Add(newObj);
                //保存的地板类型和家具类型在不同的GridData数据中
                GridData selectedData=currentObjData.ID==0?floorData:furnitureData;
                
                selectedData.AddObjectAt(
                    gridPos,
                    currentObjData.Size,
                    currentObjData.ID,
                    placedGameObjectList.Count-1);
            }
        }
        else
        {
            AudioManager.Instance.PlaySound(falsePlaceAudio,Camera.main.transform.position,1);
        }
    
    }

    private bool CheckPlacementValidity(Vector3Int gridPos, int id)
    {
        GridData selectedData=currentObjData.ID==0?floorData:furnitureData;

        return selectedData.CanPlaceObjectAt(gridPos, currentObjData.Size);
    }

    private void StopPlacement()
    {
        isPlacing = false;
        currentObjData = null;
        Debug.Log("Stop placement");
        BuildIndicator.SetActive(false);
    }

    
}
