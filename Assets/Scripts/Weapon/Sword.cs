using UnityEngine;
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
        foreach (GameObject item in enemies)
        {
            item.GetComponent<Enemy>().Health -= 300;
        }
        Debug.Log("Used Sword");
    }
}
