using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] private float offset_Y = 0.1f;

    [SerializeField] private GameObject BuildIndicator;
    private GameObject previewObject;

    [SerializeField] private Material previewMaterialPrefab;
    private Material previewMaterialsInstance;
    
    //指示器的shader
    private Renderer buildIndicatorRenderer;
    void Start()
    {
        previewMaterialsInstance =new Material(previewMaterialPrefab);
        BuildIndicator.SetActive(false);
        buildIndicatorRenderer = BuildIndicator.GetComponent<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareIndicator(size);
        BuildIndicator.SetActive(true);
    }

    //修改指示器
    private void PrepareIndicator(Vector2Int size)
    {
        if (size.x > 0 && size.y > 0)
        {
            BuildIndicator.transform.localScale = new Vector3(size.x,size.y,1f);
        }
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
        BuildIndicator.SetActive(false);
        Destroy(previewObject);
    }
    
    //实时更新
    public void UpdatePosition(Vector3 pos, bool validity)
    {
        MovePreview(pos);
        MoveCursor(pos);
        ApplyFeedback(validity);
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        //BuildIndicator.GetComponent<Material>().color = c;
        buildIndicatorRenderer.material.color = c;
        previewMaterialsInstance.color = c;
        
    }

    private void MoveCursor(Vector3 pos)
    {
        BuildIndicator.transform.position = pos;
    }

    private void MovePreview(Vector3 pos)
    {
        previewObject.transform.position = new Vector3(pos.x, pos.y+offset_Y, pos.z);
    }
}
