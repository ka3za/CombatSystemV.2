using UnityEngine;
using System.Collections;
using System;

public class Weapon : Action {

    private Vector2 playerPos;

    private Vector2 mousePos;

    public Vector2 PlayerPos
    {
        set { playerPos = value; }
    }
    public Vector2 MousePos
    {
        set { mousePos = value; }
    }

    public void Block()
    {

    }

    public override void Use()
    {
        throw new NotImplementedException();
    }
}
