﻿using UnityEngine;
using System.Collections;
using System;

public class Sword : Weapon {

    void Start()
    {
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void Attack(int strength)
    {

    }

    public override void Use()
    {
        Debug.Log("Used Sword");
    }
}
