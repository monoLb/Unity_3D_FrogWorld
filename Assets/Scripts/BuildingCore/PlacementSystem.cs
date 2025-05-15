using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class PlacementSystem : MonoBehaviour
{
    

    // [SerializeField]
    // private GameObject BuildIndicator;
    
    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;
    
    //从UI点击获取数据
    // private ObjectData currentObjData;
    //数据传递，已由PlacemenState获取
   
    //放置的父物体位置
    [SerializeField] private Transform _placementTransParent;
    
    //不同种类 创建不同的类方便管理
    [field: SerializeField]
    public GridData floorGrid,furnitureGrid;
    //把创建的家具保存在列表里
    // private List<GameObject> objectList=new(); 新脚本代替
    
    //能否建造则显示不同的指示器颜色
    // public Renderer _buildIndicatorRenderer;
    //在其他脚本中调用
    [SerializeField]
    private PreviewSystem previewSystem;

    //优化：搭建后不检测原位置
    private Vector3Int lastPosition;
 
    //使用接口
    IBuildingState buildingState;//包含place和remove两部分
    
    [SerializeField] private ObjectPlacer placer;
    [SerializeField] private ObjectDestroyer destroyer;
    
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
        if(buildingState==null)
            return;
        
        var mousePos = inputManager.GetSelectedMapPosition();
        var gridPos = grid.WorldToCell(mousePos);
        
        if (buildingState!=null)
        {
            buildingState.UpdateState(gridPos);
        }
     
    }

    

    public void SetCurrentObjectData(ObjectData data)
    {
        BindInputActions();
        
        destroyer.QuiteRemovingState();
        buildingState = null;
        
        buildingState = new PlacementState(data.ID, grid, previewSystem, data, floorGrid, furnitureGrid, placer);
        lastPosition=Vector3Int.zero;
    }

    //使用按钮
    public void StartRemoving()
    {
        Debug.Log("StartRemoving");
        StopPlacement();
        
        buildingState = null;//消除其他状态
        
        buildingState = new RemovingState(grid, previewSystem, floorGrid, furnitureGrid, placer, destroyer);
        destroyer.CreateAShovel();
        
        BindInputActions();
    }

    public void StartPlacement()
    {
        BindInputActions();
    }

    private void StopPlacement()
    {
        if (buildingState==null)
            return;
        
        buildingState.EndState();
        buildingState = null;
        Debug.Log("Stop placement");

        previewSystem.StopShowingPlacementPreview();
        UnbindInputActions();
    }

    private void BindInputActions()
    {
        inputManager.OnClicked -= PlaceStucture;
        inputManager.OnEsc -= StopPlacement;
        inputManager.OnClicked += PlaceStucture;
        inputManager.OnEsc += StopPlacement;
    }

    private void UnbindInputActions()
    {
        inputManager.OnClicked -= PlaceStucture;
        inputManager.OnEsc -= StopPlacement;
    }


    // ReSharper disable Unity.PerformanceAnalysis
    private void PlaceStucture()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos=grid.WorldToCell(mousePos);
        
        
        buildingState.OnState(gridPos);
    }
    
}
