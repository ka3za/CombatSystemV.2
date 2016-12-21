using UnityEngine;
using System.Collections;
using System;

public class ArrowStorm : Ability {

    void Start()
    {

    }

    // Update is called once per frame
    void Update () {
	
	}

    public void Attack(int agility, Vector2 mousePos)
    {

    }

    public override void Use(float _str, float _int, float _agi)
    {
        Debug.Log("Used Arrow");
    }
}
