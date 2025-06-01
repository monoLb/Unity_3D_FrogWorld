using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    private Animator animator;
    
    public LayerMask canMoveLayer;

    public FieldType currentFieldType;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }


    public NavMeshAgent GetAgent()
    {
        return agent;
    }
}
