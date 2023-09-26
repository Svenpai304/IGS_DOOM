using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    public delegate void Action();
    public static Action GlobalUpdate;

    private void Start()
    {       
        var a = new InputManager();
    }

    private void Update()
    {
        GlobalUpdate?.Invoke();
    }
}
