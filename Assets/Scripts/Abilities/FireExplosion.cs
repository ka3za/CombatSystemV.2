using UnityEngine;
using System.Collections;

public class FireExplosion : Ability
{
    // Use this for initialization
    void Start()
    {
        ActionMan = GameObject.FindGameObjectWithTag("ActionManager");
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public override void UpdateAction(int PrimaryStat)
    {
        base.UpdateAction(PrimaryStat);
        TimeToDestroy = 4;
        Cooldown = 5;
        AbilityCooldown = 3;
        Dmg = 1.5f * PrimaryStat;
        DmgType = DamageType.DOT;
        DotTimeTick = 3;
        DotTimeCooldown = 1;
        SecondDmgType = DamageType.SLOW;
        Knockbacked = false;
        SlowAmount = 3;
        AbilityPos = transform.position;
        IsAbility = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(ActionMan != null)
        {
            ActionMan.GetComponent<ActionManager>().Attacked(other.gameObject, this);
        }else
        {
            Debug.Log("Something is wrong with ActionMan");
        }
       
    }


}
