using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public delegate void Action();
    public Action GlobalUpdate;

    private void Awake()
    {
        if (instance != null) { instance = this; }
    }

    private void Update()
    {
        GlobalUpdate?.Invoke();
    }
}
