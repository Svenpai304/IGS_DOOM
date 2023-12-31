using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    public bool Active { get; set; }
    public void OnActivate();
    public void OnDeactivate();
}
