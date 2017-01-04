using UnityEngine;
using System.Collections;
using System;

public class Staff : Weapon {

    void Start()
    {

        ActionMan = GameObject.FindGameObjectWithTag("ActionManager");
        DmgType = DamageType.DOT;
        IsAbility = false;
    }
    
    // Update is called once per frame
    void Update ()
    {
	
	}

    public void Attack()
    {
        
    }

    private void MinorUpdate()
    {
        DotTimeTick = 3;
        DotTimeCooldown = 1;
        Dmg = 10;
    }

    public override void Use(float _str, float _int, float _agi)
    {
        foreach (GameObject item in enemies)
        {

            MinorUpdate();
            item.GetComponent<Enemy>().CurrentHealth -= _str;
            ActionMan.GetComponent<ActionManager>().Attacked(item, this);

        }
        Debug.Log("Used Staff");
    }
}
