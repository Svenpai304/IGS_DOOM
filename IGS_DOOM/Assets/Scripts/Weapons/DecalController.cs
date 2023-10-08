using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalController : IPoolable
{
    public bool Active { get; set; }
    public GameObject decal;

    public void OnActivate()
    {
        if (decal != null)
        {
            decal.SetActive(true);
        }
    }

    public void OnDeactivate()
    {
        if (decal != null)
        {
            decal.SetActive(false);
        }
    }
}
