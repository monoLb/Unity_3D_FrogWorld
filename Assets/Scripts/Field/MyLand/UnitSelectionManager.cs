using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get;private set; }
    
    public List<GameObject> unitsAllList;
    public GameObject currentSelectedUnit;
    
    private PlayerController player;

    public LayerMask canSelectMask;
    public LayerMask groundMask;
    public GameObject flag;
    
    [FormerlySerializedAs("inputManagement")] public InputSystem inputSystem;
    public PlaceSystem placeSystem;
    
    private GameObject activeIndicaor;

    public RenderTexture unitRenderTexture;
    public RawImage _unitImage;
    public Button _unitLeftButton;
    public Button _unitRightButton;
    public Texture unitDefaultImage;
    
    public bool isBuilding = false;
    public GameObject BuildingGridIndicator_Prefab;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUnitCamera();
        inputSystem.OnEsc += StopBuild;
    }

    private void StopBuild()
    {
        isBuilding = false;
        if(player != null)
            player.SetBuildingUIInactive();
        inputSystem.gameObject.SetActive(false);
        placeSystem.gameObject.SetActive(false);
        placeSystem.QuitBuildingState();
    }

    private void Update()
    {
        if (!isBuilding)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, canSelectMask))
                {

                    SelectByClicking(hitInfo.collider.gameObject);

                }
                else if (!inputSystem.IsPointerOverUI())
                {
                    DeselectAll();
                }

            }

            
            //创建旗帜
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, groundMask))
                {
                    GameObject flagObj = Instantiate(flag, hitInfo.point, Quaternion.identity);
                    Destroy(flagObj, 1.2f);
                }
            }
        }
        
        if(isBuilding)
            BuildingGridIndicator_Prefab.SetActive(true);
        else
        {
            BuildingGridIndicator_Prefab.SetActive(false);
        }

    }   
    
    private void DeselectAll()
    {
        if (currentSelectedUnit != null)
        {
            TriggerSelectionIndicator(currentSelectedUnit,false);
            if (currentSelectedUnit.TryGetComponent(out  player))
            {
                player.SetBuildingUIInactive();
            }
        }
        _unitImage.texture = unitDefaultImage;
        currentSelectedUnit = null;
        player = null;
    }

    private void SelectByClicking(GameObject selectedUnit)
    {
        DeselectAll();
        currentSelectedUnit = selectedUnit;

        if (selectedUnit.TryGetComponent(out player))
        {
            player.SetBuildingUIActive();
            isBuilding = true;
            inputSystem.gameObject.SetActive(true);
            placeSystem.gameObject.SetActive(true);
        }

        UpdateUnitCamera();
        TriggerSelectionIndicator(selectedUnit, true);
    }
    

    private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
    {
        unit.transform.GetChild(1).gameObject.SetActive(isVisible);
    }

    private void UpdateUnitCamera()
    {
        if (currentSelectedUnit == null)
        {
            return;
        }

        UnitBase unit = currentSelectedUnit.GetComponent<UnitBase>();
        if (unit == null)
        {
            return;
        }

        unit.SetUnitCamera(null);
        unit.SetUnitCamera(unitRenderTexture);
        _unitImage.texture = unitRenderTexture;
    }
    
}
