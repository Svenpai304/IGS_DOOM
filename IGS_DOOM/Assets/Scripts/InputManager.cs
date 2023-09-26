using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class InputManager : ICommand
{
    private ICommand inputCommand;

    public InputManager()
    {
        GameManager.GlobalUpdate += Update;
    }
    
    private void Update()
    {
        Debug.Log("TestLog");
    }
}
