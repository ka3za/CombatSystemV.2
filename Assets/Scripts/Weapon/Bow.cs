using UnityEngine;
using System.Collections;
using System;

public class Bow : Weapon {

    void Start()
    {
        ActionMan = GameObject.FindGameObjectWithTag("ActionManager");
        DmgType = DamageType.SLOW;
        IsAbility = false;
        Cooldown = 1;
    }

    // Update is called once per frame
    void Update ()
    {
        Cooldown -= Time.deltaTime;
    }

    private void MinorUpdate()
    {
        SlowTimer = 1;
        Cooldown = 1;
    }

    public override void Use(float _str, float _int, float _agi)
    {
        if (Cooldown <= 0)
        {
            MinorUpdate();
            foreach (GameObject item in enemies)
            {
                item.GetComponent<Enemy>().CurrentHealth -= _agi;
                ActionMan.GetComponent<ActionManager>().Attacked(item, this);
                Debug.Log("Target slowed : " + item.GetComponent<Enemy>().IsSlowed);

            }
        }
        Debug.Log("Used Bow");
    }
}
