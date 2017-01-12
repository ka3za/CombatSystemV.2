using UnityEngine;
using System.Collections;
using System;

public class FireExplosion : Ability
{

    void Start()
    {
        ActionMan = GameObject.FindGameObjectWithTag("ActionManager");
        DmgType = DamageType.SLOW;
        SecondDmgType = DamageType.DOT;
        IsAbility = true;
        Cooldown = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Cooldown -= Time.deltaTime;
        //TimeToDestroy -= Time.deltaTime;
        //if(TimeToDestroy <= 0)
        //{
        //    gameObject.SetActive(false);
        //}
    }

    public override void UpdateAction(int PrimaryStat)
    {
        base.UpdateAction(PrimaryStat);
        TimeToDestroy = 4;
        Cooldown = 5;
        AbilityCooldown = 3;
        Dmg = 1.5f * PrimaryStat;
        ActionPos = transform.position;
    }

    private void MinorUpdate()
    {
        DotTimeTick = 3;
        DotTimeCooldown = 1;
        SlowTimer = 3;
        AbilityEffectOneUsed = false;
        AbilityEffectTwoUsed = false;
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if(other.tag == "Player" || other.tag == "Enemy")
    //    {
    //        ActionMan = GameObject.FindGameObjectWithTag("ActionManager");
    //        if (ActionMan != null)
    //        {
    //            MinorUpdate();
    //            ActionMan.GetComponent<ActionManager>().Attacked(other.gameObject, this);
    //        }
    //        else
    //        {
    //            Debug.Log("Something is wrong with ActionMan");
    //        }
    //    }
        
       
    //}

    public override void Use(float _str, float _int, float _agi)
    {
        Dmg = 1.5f * _str;
        MinorUpdate();
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

        Debug.Log("USED FireExplosion");
    }
}
