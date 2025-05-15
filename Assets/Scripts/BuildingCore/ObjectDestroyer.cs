using System;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    [SerializeField] private GameObject shovelPrefab;

    private GameObject shovel;
    private Animator shovelAnimator;

    public void CreateAShovel()
    {
        shovel = Instantiate(shovelPrefab);
        shovelAnimator = shovel.GetComponentInChildren<Animator>();

    }

    public void UpdatePosition(Vector3 pos)
    {
        if (shovel != null)
            shovel.transform.position = pos;
    }

    public void PlayAnimation()
    {
        if (shovelAnimator != null)
            shovelAnimator.SetTrigger("Destroy");
        else
            Debug.LogWarning("shovelAnimator is null — did you forget to call CreateAShovel()?");
    }
    
    public void QuiteRemovingState()
    {
        if(shovelAnimator != null) 
            Destroy(shovel);    
        Debug.Log("QuiteRemovingState");
    }
}