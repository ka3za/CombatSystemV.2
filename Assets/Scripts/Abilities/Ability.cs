using UnityEngine;
using System.Collections;
using System;

public class Ability : Action {

    private DamageType secondDmgType;

   

    public DamageType SecondDmgType
    {
        get { return secondDmgType; }
        set { secondDmgType = value; }
    }

    public override void Use()
    {
        throw new NotImplementedException();
    }
}