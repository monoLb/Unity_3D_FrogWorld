using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator;

    // [SerializeField]
    // private GameObject BuildIndicator;
    
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
    public bool isPlacing;

    //不同种类 创建不同的类方便管理
    [field: SerializeField]
    public GridData floorGrid,furnitureGrid;
    //把创建的家具保存在列表里
    private List<GameObject> objectList=new();
    //能否建造则显示不同的指示器颜色
    // public Renderer _buildIndicatorRenderer;
    //在其他脚本中调用
    [SerializeField]
    private PreviewSystem previewSystem;
    
    //实时检测能否被创建
    private bool buildValidity;
    
    //优化
    private Vector3Int lastPosition;
    
    private void Start()
    {
        
        StartPlacement();
        floorGrid = new();
        furnitureGrid = new();
        //获取Renderer组件
        // _buildIndicatorRenderer= BuildIndicator.GetComponent<Renderer>();

    }
    
    private void Update()
    {
        var mousePos = inputManager.GetSelectedMapPosition();
        mouseIndicator.transform.position = mousePos;
        if(isPlacing)
        {
            var gridPos = grid.WorldToCell(mousePos);

            if (lastPosition != gridPos)
            {
                //实时监测能否被建造
                buildValidity=CheckBuildValidity(gridPos, currentObjData.Size);
                //改变颜色,位置
                previewSystem.UpdatePosition(gridPos,buildValidity);
            }
            
        }

    }

    private bool CheckBuildValidity(Vector3Int gridPos, Vector2Int gridSize)
    {
        GridData currentGrid = currentObjData.ID == 0 ? floorGrid: furnitureGrid;
        return currentGrid.CheckIfOccupy(gridPos, gridSize);
    }

    public void SetCurrentObjectData(ObjectData data)
    {
        currentObjData = data;
        mouseIndicator.SetActive(true);
        previewSystem.StopShowingPlacementPreview();
        lastPosition = Vector3Int.zero;
        previewSystem.StartShowingPlacementPreview(data.prefab,data.Size);
    }
    
    public void StartPlacement()
    {
        mouseIndicator.SetActive(false);
        // BuildIndicator.SetActive(false);
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

        if(lastPosition == gridPos)
            return;
        if (currentObjData != null)
        {
            
            //如果无法被建造则返回
            if (!buildValidity)
            {
                AudioManager.Instance.PlaySound(falsePlaceAudio,Camera.main.transform.position,1);
                return;
            }
            
            if (currentObjData.prefab)
            {
                // var objPos=grid.CellToWorld(gridPos); 
                //生成对象
                GameObject newObj=Instantiate(currentObjData.prefab,_placementTransParent);
                //修改位置
                newObj.transform.position = gridPos;
                //保存在表里
                objectList.Add(newObj);
                //根据是否为地板保存在不同的GridData中
                GridData selectedObj=currentObjData.ID==0?floorGrid:furnitureGrid;
                selectedObj.AddObjectAt(gridPos,currentObjData.Size,currentObjData.ID,objectList.Count-1);
                Debug.Log("ss");
                AudioManager.Instance.PlaySound(truePlaceAudio,Camera.main.transform.position,1);
                lastPosition=gridPos;
                
            }
        }
        else
        {
            AudioManager.Instance.PlaySound(falsePlaceAudio,Camera.main.transform.position,1);
        }
    
    }
    

    private void StopPlacement()
    {
        isPlacing = false;
        currentObjData = null;
        Debug.Log("Stop placement");
        previewSystem.StopShowingPlacementPreview();
        inputManager.OnClicked -= PlaceStucture;
        inputManager.OnEsc -= StopPlacement;
    }

    
}
