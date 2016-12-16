using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionManager : MonoBehaviour {


    private List<Information> info;

    private struct Information
    {
        private GameObject target;

        private Action actionType;

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
                // Debug.Log("LIST IS EMPTY");
            }
        }
        
	}
    /// <summary>
    /// This method is to update the info list abilities effects. It will go though the list and search after the ability's DmgType. Then it will run through a switch and depending what DmgType it is, there will happen something with the Target.
    /// </summary>
    private void UpdateList()
    {
        for (int i = 0; i < info.Count; i++)
        {
            switch (info[i].ActionType.DmgType)
            {
                case Action.DamageType.KNOCKBACK:
                    if (info[i].ActionType.Knockbacked == false)
                    {
                        info[i].Target.transform.GetComponent<Rigidbody2D>().AddForce((info[i].Target.transform.position - info[i].ActionType.AbilityPos).normalized * 75, ForceMode2D.Impulse);
                        info[i].ActionType.Knockbacked = true;
                    }

                    break;
                case Action.DamageType.DOT:
                    info[i].ActionType.DotTimeCooldown -= Time.deltaTime;
                    if (info[i].ActionType.DotTimeCooldown <= 0 && info[i].ActionType.DotTimeTick != 0)
                    {
                        info[i].ActionType.DotTimeCooldown = 1;
                        info[i].ActionType.DotTimeTick -= 1;
                        if (info[i].Target.tag == "Player")
                        {
                            info[i].Target.GetComponent<Player>().CurrentHealth -= info[i].ActionType.Dmg / 3;
                            info[i].Target.GetComponent<Player>().UpdateStats();
                        }
                    }

                    break;
                case Action.DamageType.SLOW:
                    if (info[i].ActionType.SlowAmount > 0)
                    {

                        info[i].ActionType.SlowAmount -= Time.deltaTime;
                        if (info[i].Target.tag == "Player")
                        {
                            info[i].Target.GetComponent<Player>().MovementSpeed = info[i].Target.GetComponent<Player>().MovementSpeed / 3;
                        }
                    }
                    else
                    {
                        if (info[i].Target.tag == "Player")
                        {
                            info[i].Target.GetComponent<Player>().MovementSpeed = info[i].Target.GetComponent<Player>().BaseMovementSpeed;
                        }
                    }

                    break;
                case Action.DamageType.STUN:
                    //info[i].Target.GetComponent<Player>().markasstunned?
                    break;
                default:
                    break;
            }

            if (info[i].ActionType.IsAbility == true)
            {
                Ability temp = (Ability)info[i].ActionType;
                switch (temp.SecondDmgType)
                {
                    case Action.DamageType.KNOCKBACK:
                        if (info[i].ActionType.Knockbacked == false)
                        {
                            info[i].Target.transform.GetComponent<Rigidbody2D>().AddForce((info[i].Target.transform.position - info[i].ActionType.AbilityPos).normalized * 50, ForceMode2D.Impulse);
                            info[i].ActionType.Knockbacked = true;
                        }
                        break;
                    case Action.DamageType.DOT:
                        info[i].ActionType.DotTimeCooldown -= Time.deltaTime;
                        if (info[i].ActionType.DotTimeCooldown <= 0 && info[i].ActionType.DotTimeTick != 0)
                        {
                            info[i].ActionType.DotTimeCooldown = 1;
                            info[i].ActionType.DotTimeTick -= 1;
                            if (info[i].Target.tag == "Player")
                            {
                                info[i].Target.GetComponent<Player>().CurrentHealth -= info[i].ActionType.Dmg / 3;
                                info[i].Target.GetComponent<Player>().UpdateStats();
                            }
                        }
                        break;
                    case Action.DamageType.SLOW:
                        if (info[i].ActionType.SlowAmount > 0)
                        {

                            info[i].ActionType.SlowAmount -= Time.deltaTime;
                            if (info[i].Target.tag == "Player")
                            {
                                info[i].Target.GetComponent<Player>().MovementSpeed = info[i].Target.GetComponent<Player>().MovementSpeed / 3;
                            }
                        }
                        else
                        {
                            if (info[i].Target.tag == "Player")
                            {
                                info[i].Target.GetComponent<Player>().MovementSpeed = info[i].Target.GetComponent<Player>().BaseMovementSpeed;
                            }
                        }
                        break;
                    case Action.DamageType.STUN:
                        //info[i].Target.GetComponent<Player>().markasstunned?
                        break;
                    default:
                        break;
                }
            }
        }
    }
    /// <summary>
    /// Updating the info list when there is a new turn in TurnBased combat mode
    /// </summary>
    private void TurnBasedUpdateList()
    {
        for (int i = 0; i < info.Count; i++)
        {
            switch (info[i].ActionType.DmgType)
            {
                case Action.DamageType.KNOCKBACK:
                    if (info[i].ActionType.Knockbacked == false)
                    {
                        info[i].Target.transform.GetComponent<Rigidbody2D>().AddForce((info[i].Target.transform.position - info[i].ActionType.AbilityPos).normalized * 75, ForceMode2D.Impulse);
                        info[i].ActionType.Knockbacked = true;
                    }else
                    {
                        //remove from list
                    }

                    break;
                case Action.DamageType.DOT:
                    if (info[i].ActionType.DotTimeTick != 0)
                    {
                        info[i].ActionType.DotTimeTick -= 1;

                        if (info[i].Target.tag == "Player")
                        {
                            info[i].Target.GetComponent<Player>().CurrentHealth -= info[i].ActionType.Dmg / 3;
                            info[i].Target.GetComponent<Player>().UpdateStats();
                        }
                    }else
                    {
                        //No more ticks remove it from list
                    }

                    break;
                case Action.DamageType.SLOW:
                    if (info[i].ActionType.SlowAmount != 0)
                    {

                        info[i].ActionType.SlowAmount -= 1;
                        if (info[i].Target.tag == "Player")
                        {
                            info[i].Target.GetComponent<Player>().MovementSpeed = info[i].Target.GetComponent<Player>().MovementSpeed / 3;
                        }
                    }
                    else
                    {
                        if (info[i].Target.tag == "Player")
                        {
                            info[i].Target.GetComponent<Player>().MovementSpeed = info[i].Target.GetComponent<Player>().BaseMovementSpeed;
                        }

                        //Remove itself from list
                    }

                    break;
                case Action.DamageType.STUN:
                    //info[i].Target.GetComponent<Player>().markasstunned?
                    break;
                default:
                    break;
            }

            if (info[i].ActionType.IsAbility == true)
            {
                Ability temp = (Ability)info[i].ActionType;
                switch (temp.SecondDmgType)
                {
                    case Action.DamageType.KNOCKBACK:
                        if (info[i].ActionType.Knockbacked == false)
                        {
                            info[i].Target.transform.GetComponent<Rigidbody2D>().AddForce((info[i].Target.transform.position - info[i].ActionType.AbilityPos).normalized * 75, ForceMode2D.Impulse);
                            info[i].ActionType.Knockbacked = true;
                        }
                        else
                        {
                            //remove from list
                        }

                        break;
                    case Action.DamageType.DOT:
                        if (info[i].ActionType.DotTimeTick != 0)
                        {
                            info[i].ActionType.DotTimeTick -= 1;

                            if (info[i].Target.tag == "Player")
                            {
                                info[i].Target.GetComponent<Player>().CurrentHealth -= info[i].ActionType.Dmg / 3;
                                info[i].Target.GetComponent<Player>().UpdateStats();
                            }
                        }
                        else
                        {
                            //No more ticks remove it from list
                        }

                        break;
                    case Action.DamageType.SLOW:
                        if (info[i].ActionType.SlowAmount != 0)
                        {

                            info[i].ActionType.SlowAmount -= 1;
                            if (info[i].Target.tag == "Player")
                            {
                                info[i].Target.GetComponent<Player>().MovementSpeed = info[i].Target.GetComponent<Player>().MovementSpeed / 3;
                            }
                        }
                        else
                        {
                            if (info[i].Target.tag == "Player")
                            {
                                info[i].Target.GetComponent<Player>().MovementSpeed = info[i].Target.GetComponent<Player>().BaseMovementSpeed;
                            }

                            //Remove itself from list
                        }

                        break;
                    case Action.DamageType.STUN:
                        //info[i].Target.GetComponent<Player>().markasstunned?
                        break;
                    default:
                        break;
                }
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
        bool allowed = true;

        for (int i = 0; i < info.Count; i++)
        {
            if(info[i].ActionType == tempActionType && info[i].Target == tempTarget)
            {
                allowed = false;
            }
        }

        if(allowed == true)
        {
            Information tempInfo = new Information();
            //if((Ability)tempActionType.dmg == typeof(Ability))
            //{
            Ability tempAbility = (Ability)tempActionType;
            Debug.Log("Check tempAbility : " + tempAbility.DmgType + "   " + tempAbility.SecondDmgType);
            tempInfo.ActionType = tempAbility;
            // }
            // else
            // {

            // }
            tempInfo.Target = tempTarget;
            //Check if ability is already on target
            info.Add(tempInfo);
        }

        Debug.Log("Is ability allowed : " + allowed);
        
    }
}
