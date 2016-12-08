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
	void Update ()
    {
        if(info.Count != 0)
        {
            UpdateList();
        }
        else
        {
           // Debug.Log("LIST IS EMPTY");
        }
	}

    private void UpdateList()
    {
        for (int i = 0; i < info.Count; i++)
        {
            switch (info[i].ActionType.DmgType)
            {
                case Action.DamageType.KNOCKBACK:
                    if(info[i].ActionType.Knockbacked == false)
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
