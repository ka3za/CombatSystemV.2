using UnityEngine;
using System.Collections;
using System;

public class Weapon : Action {

    public void Block()
    {

    }

    public override void Use()
    {
        Debug.Log("Use on Weapon");
    }
}
