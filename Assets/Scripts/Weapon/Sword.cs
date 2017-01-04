using UnityEngine;
using System.Collections;
using System;

public class Sword : Weapon {

    void Start()
    {
        ActionMan = GameObject.FindGameObjectWithTag("ActionManager");
        DmgType = DamageType.KNOCKBACK;
        IsAbility = false;
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
            item.GetComponent<Enemy>().CurrentHealth -= _str;
            ActionPos = transform.position;
            ActionMan.GetComponent<ActionManager>().Attacked(item, this);

        }
        Debug.Log("Used Sword");
    }
}
