using UnityEngine;
using System.Collections;
using System;

public class FireExplosion : Ability
{
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        TimeToDestroy -= Time.deltaTime;
        if(TimeToDestroy <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public override void UpdateAction(int PrimaryStat)
    {
        base.UpdateAction(PrimaryStat);
        TimeToDestroy = 4;
        Cooldown = 5;
        AbilityCooldown = 3;
        Dmg = 1.5f * PrimaryStat;
        DmgType = DamageType.DOT;
        SecondDmgType = DamageType.SLOW;
        ActionPos = transform.position;
        IsAbility = true;
    }

    private void MinorUpdate()
    {
        DotTimeTick = 3;
        DotTimeCooldown = 1;
        SlowTimer = 3;
        AbilityEffectOneUsed = false;
        AbilityEffectTwoUsed = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" || other.tag == "Enemy")
        {
            ActionMan = GameObject.FindGameObjectWithTag("ActionManager");
            if (ActionMan != null)
            {
                MinorUpdate();
                ActionMan.GetComponent<ActionManager>().Attacked(other.gameObject, this);
            }
            else
            {
                Debug.Log("Something is wrong with ActionMan");
            }
        }
        
       
    }

    public override void Use(float _str, float _int, float _agi)
    {
        Debug.Log("Used Fire");
    }
}
