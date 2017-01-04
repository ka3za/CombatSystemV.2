using UnityEngine;
using System.Collections;
using System;

public abstract class Ability : Action {

    private DamageType secondDmgType;

    public DamageType SecondDmgType
    {
        get { return secondDmgType; }
        set { secondDmgType = value; }
    }

   


    public abstract override void Use(float _str, float _int, float _agi);
}