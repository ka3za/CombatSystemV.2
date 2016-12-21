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

    public override void Use(float _str, float _int, float _agi)
    {
        foreach (GameObject item in enemies)
        {
            item.GetComponent<Enemy>().CurrentHealth -= 300;
        }
        Debug.Log("Used Sword");
    }
}
