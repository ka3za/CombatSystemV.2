using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionManager : MonoBehaviour {


    private List<Information> info;

    private struct Information
    {
        private GameObject target;

        private Ability ability;

        public GameObject Target
        {
            get { return target; }
            set { target = value; }
        }

        public Ability Ability
        {
            get { return ability; }
            set { ability = value; }
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Attacked(GameObject tempTarget, Ability tempAbility)
    {

    }
}
