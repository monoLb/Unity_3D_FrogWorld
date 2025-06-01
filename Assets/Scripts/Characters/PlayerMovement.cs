using System;
using UnityEngine;

public class PlayerMovement : UnitMovement
{
    private void Update()
    {
        if (currentFieldType == FieldType.Land)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, canMoveLayer))
                {
                    agent.destination = hitInfo.point;
                    //创建特效
                }
            }
        }
    }
}
