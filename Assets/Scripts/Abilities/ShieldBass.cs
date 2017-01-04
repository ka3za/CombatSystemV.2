using UnityEngine;
using System.Collections;
using System;

public class ShieldBass : Ability {

    void Start()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        TimeToDestroy -= Time.deltaTime;
        if (TimeToDestroy <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public override void UpdateAction(int PrimaryStat)
    {
        base.UpdateAction(PrimaryStat);
        TimeToDestroy = 1;
        Cooldown = 5;
        AbilityCooldown = 3;
        Dmg = 1.5f * PrimaryStat;
        DmgType = DamageType.KNOCKBACK;
        SecondDmgType = DamageType.STUN;
        ActionPos = transform.position;
        IsAbility = true;
    }

    private void MinorUpdate()
    {
        StunTimer = 2;
        AbilityEffectOneUsed = false;
        AbilityEffectTwoUsed = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
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

    public void Attack(int stamina, Vector2 playerPos, Vector2 mousePos)
    {

    }

    public override void Use(float _str, float _int, float _agi)
    {
        Debug.Log("Used Shield");
    }
}
