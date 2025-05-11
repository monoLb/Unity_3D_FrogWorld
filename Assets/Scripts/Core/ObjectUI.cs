using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ObjectUI : MonoBehaviour,IPointerDownHandler
{
    [SerializeField]
    private Image objectImage;
    [SerializeField]    
    private TextMeshProUGUI name;
    
    public ObjectData objectData;
    
    private PlacementSystem placementSystem;
 
    private void Start()
    {
        Init(objectData);
        placementSystem=GameObject.Find("PlacementSystem").GetComponent<PlacementSystem>();
    }
    
    private void Init(ObjectData data)
    {
        objectData = data;
        name.text = objectData.name;
        objectImage.sprite = objectData.sprite;
        objectImage.SetNativeSize();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        placementSystem.isPlacing = true;
        placementSystem.SetCurrentObjectData(objectData);
    }
}
