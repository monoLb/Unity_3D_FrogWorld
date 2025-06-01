using System;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitBase : MonoBehaviour
{
    public UnitType unitType;
    public GroupType groupType;
    //属性面板
    public float attackDis=1f;
    
    //组件
    private Camera unitCamera;

    private void Start()
    {
        FieldManager.Instance.currentField = FieldManager.Instance.currentField;
        
        unitCamera=this.gameObject.transform.GetChild(2).gameObject.GetComponent<Camera>();
        
        if(FieldManager.Instance.currentField==FieldType.Land)
        //开局进入队列
            UnitSelectionManager.Instance.unitsAllList.Add(gameObject); 
    }

    private void OnDestroy()
    {
        if(FieldManager.Instance.currentField==FieldType.Land)
            UnitSelectionManager.Instance.unitsAllList.Remove(gameObject);
    }

    public void SetUnitCamera(RenderTexture texture)
    {
        this.unitCamera.targetTexture=texture;
        unitCamera.enabled = (texture != null); 
    }
    
    
}
