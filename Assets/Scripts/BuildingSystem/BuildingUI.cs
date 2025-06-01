using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour,IPointerDownHandler
{
    public Image buildingImage;
    public BuildingData buildingData;
    private PlaceSystem placeSystem;

    public bool isLocked;

    private void Start()
    {
        isLocked = CheckIsLocked(buildingData);
        Init(buildingData);
        placeSystem=GameObject.Find("PlaceSystem").GetComponent<PlaceSystem>();
    }

    private void Init(BuildingData data)
    {
        buildingData = data;
        buildingImage.sprite = buildingData.sprite;
        buildingImage.SetNativeSize();
        buildingImage.rectTransform.localScale = new Vector3(0.56f, 0.56f, 1f);
        buildingImage.color = isLocked ? new Color(0, 0, 0) : new Color(1, 1, 1);

    }

    private bool CheckIsLocked(BuildingData data)
    {
        if (GameManager.Instance.cheifLevel <= data.DependecyLevel ||  data.isLocked)
        {
            return true;
        }
        return false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLocked)
        {
            placeSystem.SetCurrentBuildingData(null);
            AudioManager.Instance.PlaySound(SoundType.falseSound);
            return;
        }
        placeSystem.SetCurrentBuildingData(buildingData);
    }
}
