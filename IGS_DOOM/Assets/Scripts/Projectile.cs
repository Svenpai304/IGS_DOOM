using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile
{
    public Projectile()
    {
        GameManager.instance.GlobalUpdate += Update;
    }

    ~Projectile() 
    {
        GameManager.instance.GlobalUpdate -= Update;
    }

    private void Update()
    {

    }
}
