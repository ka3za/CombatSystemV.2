using UnityEngine;
using System.Collections;
using System;

public class Bow : Weapon {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Attack(int agility)
    {

    }

    public override void Use()
    {
        Debug.Log("Used Bow");
    }
}
