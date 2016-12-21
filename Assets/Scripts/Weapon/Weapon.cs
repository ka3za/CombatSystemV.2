using UnityEngine;
using System.Collections;
using System;

public abstract class Weapon : Action {

    public void Block()
    {

    }

    public abstract override void Use(float _str, float _int, float _agi);
}
