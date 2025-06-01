using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{

    [SerializeField] private IntEventSO levelEvent;
    public static GameManager Instance;

    public int cheifLevel;
    
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
        levelEvent.RaiseEvent(cheifLevel,this);
    }

    public void LevelUp()
    {
        cheifLevel++;
        levelEvent.RaiseEvent(cheifLevel,this);
    }
    
}
