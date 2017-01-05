using UnityEngine;
using System.Collections;
using System;

public class Staff : Weapon {

    void Start()
    {

        ActionMan = GameObject.FindGameObjectWithTag("ActionManager");
        DmgType = DamageType.DOT;
        IsAbility = false;
        Cooldown = 1;
    }
    
    // Update is called once per frame
    void Update ()
    {
        Cooldown -= Time.deltaTime;
	}

    public void Attack()
    {
        
    }

    private void MinorUpdate()
    {
        DotTimeTick = 3;
        DotTimeCooldown = 1;
        Dmg = 10;
        Cooldown = 1;
    }

    public override void Use(float _str, float _int, float _agi)
    {
        if(Cooldown <= 0)
        {
            MinorUpdate();
            foreach (GameObject item in enemies)
            {            
                item.GetComponent<Enemy>().CurrentHealth -= _str;
                ActionMan.GetComponent<ActionManager>().Attacked(item, this);
                Debug.Log("Staff used for real :p");
            }
        }
        
        Debug.Log("Used Staff");
    }
}
