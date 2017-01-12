using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionManager : MonoBehaviour {


    private List<Information> info;

    private List<Information> tempInfoList;

    private List<Information> removeInfo;

    private int idCounter;

    private struct Information
    {
        private int id;

        private GameObject target;

        private Action actionType;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public GameObject Target
        {
            get { return target; }
            set { target = value; }
        }

        public Action ActionType
        {
            get { return actionType; }
            set { actionType = value; }
        }
    }

	// Use this for initialization
	void Start ()
    {
        info = new List<Information>();
        tempInfoList = new List<Information>();
        removeInfo = new List<Information>();
        idCounter = 0;
    }
	
	// Update is called once per frame
    /// <summary>
    /// UpdateList method will only run if there is something in the info list. 
    /// </summary>
	void Update ()
    {
        if(GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Realtime)
        {
            if (info.Count != 0)
            {
                UpdateList();
            }
            else
            {
                 //Debug.Log("LIST IS EMPTY");
            }
        }
        
	}
    /// <summary>
    /// This method is to update the info list abilities effects. It will go though the list and search after the ability's DmgType. Then it will run through a switch and depending what DmgType it is, there will happen something with the Target.
    /// </summary>
    private void UpdateList()
    {

        tempInfoList = info;
        for (int i = 0; i < tempInfoList.Count; i++)
        {       
            switch (tempInfoList[i].ActionType.DmgType)
            {
                case Action.DamageType.KNOCKBACK:
                    tempInfoList[i] = KnockBack(tempInfoList[i], true);
                    break;
                case Action.DamageType.DOT:
                    tempInfoList[i] = DOT(tempInfoList[i], true, false);
                    break;
                case Action.DamageType.SLOW:
                    tempInfoList[i] = Slow(tempInfoList[i], true, false);
                    break;
                case Action.DamageType.STUN:
                    tempInfoList[i] = Stun(tempInfoList[i], true, false);
                    break;
                default:
                    break;
            }

            if (tempInfoList[i].ActionType.IsAbility == true)
            {
                Ability temp = (Ability)tempInfoList[i].ActionType;
                switch (temp.SecondDmgType)
                {
                    case Action.DamageType.KNOCKBACK:
                        tempInfoList[i] = KnockBack(tempInfoList[i], false);
                        break;
                    case Action.DamageType.DOT:
                        tempInfoList[i] = DOT(tempInfoList[i], false, false);
                        break;
                    case Action.DamageType.SLOW:
                        tempInfoList[i] = Slow(tempInfoList[i], false, false);
                        break;
                    case Action.DamageType.STUN:
                        tempInfoList[i] = Stun(tempInfoList[i], false, false);
                        break;
                    default:
                        break;
                }
            }

            AddToCleanUp(tempInfoList[i]);   
        }
        CleanUp();      
    }
    /// <summary>
    /// Updating the info list when there is a new turn in TurnBased combat mode
    /// </summary>
    private void TurnBasedUpdateList()
    {
        tempInfoList = info;

        for (int i = 0; i < tempInfoList.Count; i++)
        {
            switch (tempInfoList[i].ActionType.DmgType)
            {
                case Action.DamageType.KNOCKBACK:
                    KnockBack(tempInfoList[i], true);
                    break;
                case Action.DamageType.DOT:
                    tempInfoList[i] = DOT(tempInfoList[i], true, true);
                    break;
                case Action.DamageType.SLOW:
                    tempInfoList[i] = Slow(tempInfoList[i], true, true);
                    break;
                case Action.DamageType.STUN:
                    tempInfoList[i] = Stun(tempInfoList[i], true, true);
                    break;
                default:
                    break;
            }

            if (tempInfoList[i].ActionType.IsAbility == true)
            {
                Ability temp = (Ability)tempInfoList[i].ActionType;
                switch (temp.SecondDmgType)
                {
                    case Action.DamageType.KNOCKBACK:
                        tempInfoList[i] = KnockBack(tempInfoList[i], false);
                        break;
                    case Action.DamageType.DOT:
                        tempInfoList[i] = DOT(tempInfoList[i], false, true);
                        break;
                    case Action.DamageType.SLOW:
                        tempInfoList[i] = Slow(tempInfoList[i], false, true);
                        break;
                    case Action.DamageType.STUN:
                        tempInfoList[i] = Stun(tempInfoList[i], false, true);
                        break;
                    default:
                        break;
                }
            }
            AddToCleanUp(tempInfoList[i]);
        }
        CleanUp();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tempInfo"></param>
    /// <param name="isLoopOne"></param>
    /// <returns></returns>
    private Information KnockBack(Information tempInfo, bool isLoopOne)
    {
        if(isLoopOne == true)
        {
            if (tempInfo.ActionType.IsAbility == false)
            {
                tempInfo.Target.transform.GetComponent<Rigidbody2D>().AddForce((tempInfo.Target.transform.position - tempInfo.ActionType.ActionPos).normalized * 75, ForceMode2D.Impulse);

                if (removeInfo.Contains(tempInfo) == false)
                {
                    removeInfo.Add(tempInfo);
                }
            }
            else
            {
                if (tempInfo.ActionType.AbilityEffectOneUsed == false)
                {
                    tempInfo.ActionType.AbilityEffectOneUsed = true;
                    tempInfo.Target.transform.GetComponent<Rigidbody2D>().AddForce((tempInfo.Target.transform.position - tempInfo.ActionType.ActionPos).normalized * 75, ForceMode2D.Impulse);
                }
            }
        }else
        {
            if (tempInfo.ActionType.AbilityEffectTwoUsed == false)
            {

                tempInfo.ActionType.AbilityEffectTwoUsed = true;
                tempInfo.Target.transform.GetComponent<Rigidbody2D>().AddForce((tempInfo.Target.transform.position - tempInfo.ActionType.ActionPos).normalized * 75, ForceMode2D.Impulse);
            }
        }
        return tempInfo;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tempInfo"></param>
    /// <param name="isLoopOne"></param>
    /// <param name="isTurnBased"></param>
    /// <returns></returns>
    private Information DOT(Information tempInfo, bool isLoopOne, bool isTurnBased)
    {

        if (isTurnBased == true)
        {
            tempInfo.ActionType.DotTimeTick -= 1;
            if (tempInfo.Target.tag == "Player")
            {
                tempInfo.Target.GetComponent<Player>().CurrentHealth -= tempInfo.ActionType.Dmg / 3;
                tempInfo.Target.GetComponent<Player>().UpdateStats();
            }
            if (tempInfo.Target.tag == "Enemy")
            {
                tempInfo.Target.GetComponent<Enemy>().CurrentHealth -= tempInfo.ActionType.Dmg / 3;
            }

            if (tempInfo.ActionType.DotTimeTick == 0)
            {
                if (tempInfo.ActionType.IsAbility == false)
                {
                    if (removeInfo.Contains(tempInfo) == false)
                    {
                        removeInfo.Add(tempInfo);
                    }
                }
                else
                {
                    if (isLoopOne == true)
                    {
                        tempInfo.ActionType.AbilityEffectOneUsed = true;
                    }
                    else
                    {
                        tempInfo.ActionType.AbilityEffectTwoUsed = true;
                    }

                }
            }
        }
        else
        {
            tempInfo.ActionType.DotTimeCooldown -= Time.deltaTime;
            if (tempInfo.ActionType.DotTimeCooldown <= 0)
            {
                tempInfo.ActionType.DotTimeCooldown = 1;
                tempInfo.ActionType.DotTimeTick -= 1;
                if (tempInfo.Target.tag == "Player")
                {
                    tempInfo.Target.GetComponent<Player>().CurrentHealth -= tempInfo.ActionType.Dmg / 3;
                    tempInfo.Target.GetComponent<Player>().UpdateStats();
                }
                if (tempInfo.Target.tag == "Enemy")
                {
                    tempInfo.Target.GetComponent<Enemy>().CurrentHealth -= tempInfo.ActionType.Dmg / 3;
                }
            }
            

            if (tempInfo.ActionType.DotTimeTick == 0)
            {
                if (tempInfo.ActionType.IsAbility == false)
                {
                    if (removeInfo.Contains(tempInfo) == false)
                    {
                        removeInfo.Add(tempInfo);
                    }
                }
                else
                {
                    if (isLoopOne == true)
                    {
                        tempInfo.ActionType.AbilityEffectOneUsed = true;
                    }
                    else
                    {
                        tempInfo.ActionType.AbilityEffectTwoUsed = true;
                    }

                }
            }
        }

        return tempInfo;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tempInfo"></param>
    /// <param name="isLoopOne"></param>
    /// <param name="isTurnBased"></param>
    /// <returns></returns>
    private Information Slow(Information tempInfo, bool isLoopOne, bool isTurnBased)
    {

        if (isTurnBased == true)
        {
            if (tempInfo.ActionType.SlowTimer != 0)
            {

                tempInfo.ActionType.SlowTimer -= 1;
                if (tempInfo.Target.tag == "Player")
                {
                    tempInfo.Target.GetComponent<Player>().MovementSpeed = tempInfo.Target.GetComponent<Player>().MovementSpeed / 3;
                }
                if (tempInfo.Target.tag == "Enemy")
                {
                    tempInfo.Target.GetComponent<Enemy>().MovementSpeed = tempInfo.Target.GetComponent<Enemy>().MovementSpeed / 3;
                }
            }
            else
            {
                if (tempInfo.Target.tag == "Player")
                {
                    tempInfo.Target.GetComponent<Player>().MovementSpeed = tempInfo.Target.GetComponent<Player>().BaseMovementSpeed;
                }
                if (tempInfo.Target.tag == "Enemy")
                {
                    tempInfo.Target.GetComponent<Enemy>().MovementSpeed = tempInfo.Target.GetComponent<Enemy>().BaseMovementSpeed;
                }

                if (tempInfo.ActionType.IsAbility == false)
                {
                    if (removeInfo.Contains(tempInfo) == false)
                    {
                        removeInfo.Add(tempInfo);
                    }
                }
                else
                {
                    if (isLoopOne == true)
                    {
                        tempInfo.ActionType.AbilityEffectOneUsed = true;
                    }
                    else
                    {
                        tempInfo.ActionType.AbilityEffectTwoUsed = true;
                    }
                }

            }
        }
        else
        {
            if (tempInfo.ActionType.SlowTimer > 0)
            {

                tempInfo.ActionType.SlowTimer -= Time.deltaTime;

                if (tempInfo.Target.tag == "Player")
                {
                    if (tempInfo.Target.GetComponent<Player>().IsSlowed == false)
                    {
                        tempInfo.Target.GetComponent<Player>().MovementSpeed = tempInfo.Target.GetComponent<Player>().MovementSpeed / 3;
                        tempInfo.Target.GetComponent<Player>().IsSlowed = true;
                    }
                }
                if (tempInfo.Target.tag == "Enemy")
                {
                    if (tempInfo.Target.GetComponent<Enemy>().IsSlowed == false)
                    {
                        tempInfo.Target.GetComponent<Enemy>().MovementSpeed = tempInfo.Target.GetComponent<Enemy>().MovementSpeed / 3;
                        tempInfo.Target.GetComponent<Enemy>().IsSlowed = true;
                    }
                }
            }
            else
            {
                if (tempInfo.Target.tag == "Player")
                {
                    tempInfo.Target.GetComponent<Player>().MovementSpeed = tempInfo.Target.GetComponent<Player>().BaseMovementSpeed;
                    tempInfo.Target.GetComponent<Player>().IsSlowed = false;
                }
                if (tempInfo.Target.tag == "Enemy")
                {
                    tempInfo.Target.GetComponent<Enemy>().MovementSpeed = tempInfo.Target.GetComponent<Enemy>().BaseMovementSpeed;
                    tempInfo.Target.GetComponent<Enemy>().IsSlowed = false;
                }

                if (tempInfo.ActionType.IsAbility == false)
                {
                    if (removeInfo.Contains(tempInfo) == false)
                    {
                        removeInfo.Add(tempInfo);
                    }
                }
                else
                {
                    if (isLoopOne == true)
                    {
                        tempInfo.ActionType.AbilityEffectOneUsed = true;
                    }
                    else
                    {
                        tempInfo.ActionType.AbilityEffectTwoUsed = true;
                    }
                }

            }
        }


            

        return tempInfo;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tempInfo"></param>
    /// <param name="isLoopOne"></param>
    /// <param name="isTurnBased"></param>
    /// <returns></returns>
    private Information Stun(Information tempInfo, bool isLoopOne, bool isTurnBased)
    {

        if(isTurnBased == true)
        {

        }
        else
        {
            if (tempInfo.ActionType.StunTimer > 0)
            {
                tempInfo.ActionType.StunTimer -= Time.deltaTime;

                if (tempInfo.Target.tag == "Player")
                {
                    if (tempInfo.Target.GetComponent<Player>().IsStunned == false)
                    {
                        tempInfo.Target.GetComponent<Player>().IsStunned = true;
                    }

                }
                if (tempInfo.Target.tag == "Enemy")
                {
                    if (tempInfo.Target.GetComponent<Enemy>().IsStunned == false)
                    {
                        tempInfo.Target.GetComponent<Enemy>().IsStunned = true;
                    }

                }

            }
            else
            {
                if (tempInfo.Target.tag == "Player")
                {
                    tempInfo.Target.GetComponent<Player>().IsStunned = false;
                }
                if (tempInfo.Target.tag == "Enemy")
                {
                    tempInfo.Target.GetComponent<Enemy>().IsStunned = false;
                }

                if (tempInfo.ActionType.IsAbility == false)
                {
                    if (removeInfo.Contains(tempInfo) == false)
                    {
                        removeInfo.Add(tempInfo);
                    }
                }
                else
                {
                    if(isLoopOne == true)
                    {
                        tempInfo.ActionType.AbilityEffectOneUsed = true;
                    }
                    else
                    {
                        tempInfo.ActionType.AbilityEffectTwoUsed = true;
                    }                                     
                }
            }
        }
        
        return tempInfo;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tempInfo"></param>
    private void AddToCleanUp(Information tempInfo)
    {
        if (tempInfo.ActionType.IsAbility == true)
        {
            if (tempInfo.ActionType.AbilityEffectOneUsed == true && tempInfo.ActionType.AbilityEffectTwoUsed == true)
            {
                if (removeInfo.Contains(tempInfo) == false)
                {
                    removeInfo.Add(tempInfo);
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void CleanUp()
    {
        if (removeInfo.Count != 0)
        {
            for (int i = 0; i < removeInfo.Count; i++)
            {
                info.Remove(removeInfo[i]);
            }

        }
    }


    public void NewTurn()
    {
        GetComponent<TurnManager>().SwitchTurn();
        GetComponent<TurnManager>().ReplenishActionPoints();
        TurnBasedUpdateList();
    }

    /// <summary>
    /// This method adds an ActionType(Ability or weapon) and the GameObject that got hit by the Action, to the info list. Is the ability is already on a target. This method will do nothing.
    /// </summary>
    /// <param name="tempTarget"></param>
    /// <param name="tempActionType"></param>
    public void Attacked(GameObject tempTarget, Action tempActionType)
    {
        Information tempInfo = new Information();
        tempInfo.ActionType = tempActionType;
        tempInfo.Target = tempTarget;
        tempInfo.Id = idCounter;
        idCounter++;
        info.Add(tempInfo);
    }
}
