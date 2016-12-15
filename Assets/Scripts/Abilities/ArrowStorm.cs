using UnityEngine;
using System.Collections;
using System;

public class ArrowStorm : Ability {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Attack(int agility, Vector2 mousePos)
    {

    }

    public override void Use()
    {
        Debug.Log("Used Arrow");
    }
}
