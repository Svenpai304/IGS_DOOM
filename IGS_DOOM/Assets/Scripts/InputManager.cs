using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class KeyCommand
{
    public KeyCode Key;
    public ICommand Command;
}

public class InputManager
{
    private List<KeyCommand> keyCommands;

    public InputManager()
    {
        GameManager.GlobalUpdate += Update;
    }

    public void BindInput(KeyCode _key, ICommand _command)
    {
        keyCommands.Add(new KeyCommand(){Key = _key, Command = _command});        
    }

    private void Update()
    {

    }
}
