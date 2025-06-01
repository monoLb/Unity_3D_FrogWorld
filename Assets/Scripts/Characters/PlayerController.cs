using UnityEngine;

public class PlayerController : UnitBase
{
    public GameObject buildingUI;

    public void SetBuildingUIActive()
    {
        buildingUI.SetActive(true);
    }

    public void SetBuildingUIInactive()
    {
        buildingUI.SetActive(false);
    }
    
    
}
