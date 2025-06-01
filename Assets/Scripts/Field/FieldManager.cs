using System;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public static FieldManager Instance;
    public FieldType currentField;

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
}
