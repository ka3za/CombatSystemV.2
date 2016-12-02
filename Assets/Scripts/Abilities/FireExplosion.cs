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
        Dmg = 3 * PrimaryStat;
        DmgType = DamageType.DOT;
        SecondDmgType = DamageType.KNOCKBACK;
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
