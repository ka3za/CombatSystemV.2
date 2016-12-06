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
            Debug.Log(info[i].ActionType);
            //cooldowns + aftereffects
            if(info[i].ActionType.GetType() == typeof(Ability))
            {
                Ability temp = (Ability)info[i].ActionType;
                switch (temp.SecondDmgType)
                {
                    case Action.DamageType.KNOCKBACK:
                        Debug.Log("KNOCKBACK : " + info[i].Target.tag);
                        break;
                    case Action.DamageType.DOT:
                        Debug.Log("DOT : " + info[i].Target.tag);
                        break;
                    case Action.DamageType.SLOW:
                        Debug.Log("SLOW : " + info[i].Target.tag);
                        break;
                    case Action.DamageType.STUN:
                        Debug.Log("STUN : " + info[i].Target.tag);
                        break;
                    default:
                        break;
                }
            }
        }
    }


    public void Attacked(GameObject tempTarget, Action tempActionType)
    {
        switch (tempActionType.DmgType)
        {
            case Action.DamageType.KNOCKBACK:
                //Debug.Log("KNOCKBACK : " + tempTarget.tag);
                break;
            case Action.DamageType.DOT:
                //Debug.Log("DOT : " + tempTarget.tag);
                break;
            case Action.DamageType.SLOW:
                //Debug.Log("SLOW : " + tempTarget.tag);
                break;
            case Action.DamageType.STUN:
                //Debug.Log("STUN : " + tempTarget.tag);
                break;
            default:
                break;
        }
        Information tempInfo = new Information();
        tempInfo.ActionType = tempActionType;
        tempInfo.Target = tempTarget;
        Debug.Log("Attacked is working : " + tempActionType.Dmg);
        info.Add(tempInfo);
        Debug.Log("info object size : " + info.Count);
    }
}
