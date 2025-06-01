using System.Collections.Generic;
using UnityEngine;

public class BuildingDestroyer : MonoBehaviour
{
    public GameObject destroyIndicatorPrefab;
    
    private GameObject indicator;
    private Animator indicatorAnim;
    
    public void CreateAShovel()
    {
        indicator = Instantiate(destroyIndicatorPrefab);
        indicatorAnim = indicator.GetComponentInChildren<Animator>();
    }

    public void UpdatePosition(Vector3 pos)
    {
        if (indicator != null)
            indicator.transform.position = pos;
    }

    public void PlayAnimation()
    {
        if (indicatorAnim != null)
            indicatorAnim.SetTrigger("Destroy");
        else
            Debug.LogWarning("shovelAnimator is null — did you forget to call CreateAShovel()?");
    }
    
    public void QuiteRemovingState()
    {
        Destroy(indicator);   
    }
}
