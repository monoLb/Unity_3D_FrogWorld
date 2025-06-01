using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public UnitBase targetToAttack;

    public bool isFinding, isAttacking, isDeath, isVictory, isFleeing;
    
    private UnitBase self;
    

    private void Start()
    {
        self = GetComponent<UnitBase>();
       
        if(FieldManager.Instance.currentField==FieldType.Battlefield)
            StartCoroutine(DelayedSearchNearEnemy());
    }

    IEnumerator DelayedSearchNearEnemy()
    {
        //等待时间之前可以展示 场景 trick
        yield return new WaitForSeconds(BattleManager.Instance.waitForStartTime);
        Debug.Log($"等待{BattleManager.Instance.waitForStartTime}秒后开始寻敌");
        targetToAttack= FindNearestEnemy(self,BattleManager.Instance.GetOpposedGroupList(self.groupType));
    }
    
    
    //从列表中寻找最近的敌人,移动到攻击范围内进行攻击
    //trick：优先攻击不同种类的敌人，血量，职业，dps，克制关系

    private void Update()
    {
        if (isDeath)
        {
            Debug.Log("Death");
        }
        else if(isVictory)
        {
            Debug.Log("Victory");
        }
    }

    public UnitBase FindNearestEnemy(UnitBase self,List<UnitBase> enemyList)
    {
        UnitBase closestTarget = null;
        float minDist = Mathf.Infinity;
        Vector3 selfPos = self.transform.position;
        foreach (var e in enemyList)
        {
            float dist = Vector3.Distance(selfPos, e.transform.position);
            Debug.Log(dist);
            
            if (dist < minDist)
            {
                minDist = dist;
                closestTarget = e;
            }
        }

        if (closestTarget != null)
        {
            isFinding = false;
            isAttacking = true;
        }
        
        return closestTarget;
    }
    
}
