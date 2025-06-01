using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public float waitForStartTime=1f;
    public List<UnitBase> PlayerList,EnemyList;

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
    
    void Start()
    {
        GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject go in allUnits)
        {
            UnitBase unit = go.GetComponent<UnitBase>();
            if (unit != null)
            {
                AssignGroupByPosition(unit);
                BattleManager.Instance.GetGroupList(unit.groupType).Add(unit);
            }
        }
    }

    private void AssignGroupByPosition(UnitBase unit)
    {
        Vector3 pos = unit.transform.position;

        if (pos.x > pos.z)
            unit.groupType = GroupType.Enemy;
        else
            unit.groupType = GroupType.Friend;
    }

    
    public List<UnitBase> GetGroupList(GroupType groupType)
    {
        if (groupType == GroupType.Friend)
            return PlayerList;
        return EnemyList;
    }

    public List<UnitBase> GetOpposedGroupList(GroupType groupType)
    {
        if (groupType == GroupType.Friend)
            return EnemyList;
        return PlayerList;
    }
}
