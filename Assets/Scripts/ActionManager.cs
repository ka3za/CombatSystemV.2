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
            Debug.Log("LIST IS EMPTY");
        }
	}

    private void UpdateList()
    {
        for (int i = 0; i < info.Count; i++)
        {
            switch (info[i].ActionType.DmgType)
            {
                case Action.DamageType.KNOCKBACK:
                    info[i].Target.transform.GetComponent<Rigidbody2D>().AddForce((info[i].Target.transform.position - info[i].ActionType.AbilityPos).normalized * 50, ForceMode2D.Impulse);
                    break;
                case Action.DamageType.DOT:
                    if(info[i].Target.tag == "Player")
                    {
                        info[i].Target.GetComponent<Player>().CurrentClass.CurrentHealth -= info[i].ActionType.Dmg;
                        info[i].Target.GetComponent<Player>().UpdateStats();
                    }               
                    break;
                case Action.DamageType.SLOW:
                    if (info[i].Target.tag == "Player")
                    {
                        info[i].Target.GetComponent<Player>().CurrentClass.MovementSpeed = 50;
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
                        info[i].Target.transform.GetComponent<Rigidbody2D>().AddForce((info[i].Target.transform.position - info[i].ActionType.AbilityPos).normalized * 50, ForceMode2D.Impulse);
                        break;
                    case Action.DamageType.DOT:
                        if(info[i].Target.tag == "Player")
                        {
                            info[i].Target.GetComponent<Player>().CurrentClass.CurrentHealth -= info[i].ActionType.Dmg;
                            info[i].Target.GetComponent<Player>().UpdateStats();
                        }
                        break;
                    case Action.DamageType.SLOW:
                        if (info[i].Target.tag == "Player")
                        {
                            info[i].Target.GetComponent<Player>().CurrentClass.MovementSpeed = 50;
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
        Information tempInfo = new Information();
        //if((Ability)tempActionType.dmg == typeof(Ability))
        //{
            Ability tempAbility = new Ability();
            tempAbility = (Ability)tempActionType;
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
}
