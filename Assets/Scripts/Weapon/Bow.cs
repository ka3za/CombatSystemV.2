using UnityEngine;
using System.Collections;
using System;

public class Bow : Weapon {

    void Start()
    {

    }

    // Update is called once per frame
    void Update () {
	
	}

    public void Attack(int agility)
    {

    }

    public override void Use(float _str, float _int, float _agi)
    {
        Debug.Log("Used Bow");
    }
}
