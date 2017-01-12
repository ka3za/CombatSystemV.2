using UnityEngine;
using System.Collections;
using System;

public class ArrowStorm : Ability {

    void Start()
    {
        ActionMan = GameObject.FindGameObjectWithTag("ActionManager");
        DmgType = DamageType.SLOW;
        SecondDmgType = DamageType.KNOCKBACK;
        IsAbility = true;
        Cooldown = 1;
    }

    // Update is called once per frame
    void Update () {
        Cooldown -= Time.deltaTime;
    }

    private void MinorUpdate()
    {
        SlowTimer = 3;
        AbilityEffectOneUsed = false;
        AbilityEffectTwoUsed = false;
    }

    public void Attack(int agility, Vector2 mousePos)
    {

    }

    public override void Use(float _str, float _int, float _agi)
    {
        Dmg = 1.5f * _agi;
        MinorUpdate();
        if (ActionMan.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Realtime)
        {
            if (Cooldown <= 0)
            {
                Cooldown = 1;
                foreach (GameObject item in enemies)
                {
                    item.GetComponent<Enemy>().CurrentHealth -= _agi;
                    ActionPos = transform.position;
                    ActionMan.GetComponent<ActionManager>().Attacked(item, this);

                }
            }
        }
        else
        {
            foreach (GameObject item in enemies)
            {
                item.GetComponent<Enemy>().CurrentHealth -= _agi;
                ActionPos = transform.position;
                ActionMan.GetComponent<ActionManager>().Attacked(item, this);

            }
        }
        Debug.Log("Used Arrow");
    }
}
