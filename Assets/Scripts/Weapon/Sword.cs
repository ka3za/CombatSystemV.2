using UnityEngine;
using System.Collections;
using System;

public class Sword : Weapon
{

    void Start()
    {
        ActionMan = GameObject.FindGameObjectWithTag("ActionManager");
        DmgType = DamageType.KNOCKBACK;
        IsAbility = false;
        Cooldown = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Cooldown -= Time.deltaTime;
    }

    public void Attack(int strength)
    {

    }

    public override void Use(float _str, float _int, float _agi)
    {
        if (ActionMan.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Realtime)
        {
            if (Cooldown <= 0)
            {
                Cooldown = 1;
                foreach (GameObject item in enemies)
                {
                    item.GetComponent<Enemy>().CurrentHealth -= _str;
                    ActionPos = transform.position;
                    ActionMan.GetComponent<ActionManager>().Attacked(item, this);

                }
            }
            Debug.Log("Used Sword");
        }
        else
        {
            foreach (GameObject item in enemies)
            {
                item.GetComponent<Enemy>().CurrentHealth -= _str;
                ActionPos = transform.position;
                ActionMan.GetComponent<ActionManager>().Attacked(item, this);

            }
        }
    }
}
