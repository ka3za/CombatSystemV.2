using UnityEngine;
using System.Collections;

public class Ability : Action {

    private DamageType secondDmgType;

   

    public DamageType SecondDmgType
    {
        get { return secondDmgType; }
        set { secondDmgType = value; }
    }


    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {

    }
}