using UnityEngine;

public class PreviewManagement : MonoBehaviour
{
    [SerializeField] private float offset_Y = 0.1f;

    private GameObject previewObject;

    [SerializeField] private Material previewMaterialPrefab;
    private Material previewMaterialsInstance;
    

    
    void Start()
    {
        previewMaterialsInstance =new Material(previewMaterialPrefab);
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
    }
    
    
    //预览模式：修改生成Object的材质
    private void PreparePreview(GameObject previewObj)
    {
        Renderer[] renders=previewObj.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renders)
        {
            Material[] materials = r.materials;//占位
            for (int m = 0; m < materials.Length; m++)
            {
                materials[m]=previewMaterialsInstance;
            }
            r.materials=materials;
        }
    }
    //结束预览模式：
    public void StopShowingPlacementPreview()
    {
        if(previewObject != null)
            Destroy(previewObject);
    }
    
    //实时更新
    public void UpdatePosition(Vector3 pos, bool validity)
    {
        if(previewObject != null)
            MovePreview(pos);
        ApplyFeedback(validity);
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        //BuildIndicator.GetComponent<Material>().color = c;
        if(previewObject != null)
            previewMaterialsInstance.color = c;
        
    }
    

    private void MovePreview(Vector3 pos)
    {
        previewObject.transform.position = new Vector3(pos.x, pos.y+offset_Y, pos.z);
    }
}
