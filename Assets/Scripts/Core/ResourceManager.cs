using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public int unitAmount;
    public int moneyAmount;

    [Header("事件")] public IntEventSO moneyEvent;
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
    
    private void Start()
    {
        moneyEvent.RaiseEvent(moneyAmount,this);
    }

    public void AddMoney(int amount)
    {
        moneyAmount += Mathf.Max(0, amount);
        moneyEvent.RaiseEvent(moneyAmount,this);
    }

    public void ReduceMoney(int amount)
    {
        if(amount<0)
            Debug.LogWarning("Reducing money to negative");
        if (moneyAmount - amount < 0)
        {
            Debug.Log("You need more money");
        }
        else
        {
            moneyAmount -= amount;
            moneyEvent.RaiseEvent(moneyAmount,this);
        }
    }
}
