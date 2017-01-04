using UnityEngine;
using System.Collections;
using System;

public class Bow : Weapon {

    void Start()
    {
        ActionMan = GameObject.FindGameObjectWithTag("ActionManager");
        DmgType = DamageType.SLOW;
        IsAbility = false;
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void Attack(int agility)
    {

    }

    private void MinorUpdate()
    {
        SlowTimer = 1;
    }

    public override void Use(float _str, float _int, float _agi)
    {
        foreach (GameObject item in enemies)
        {
            MinorUpdate();
            item.GetComponent<Enemy>().CurrentHealth -= _agi;
            ActionMan.GetComponent<ActionManager>().Attacked(item, this);
            Debug.Log("Target slowed : " + item.GetComponent<Enemy>().IsSlowed);

        }
        Debug.Log("Used Bow");
    }
}
