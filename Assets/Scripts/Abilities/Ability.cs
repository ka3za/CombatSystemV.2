using UnityEngine;
using System.Collections;

public class Ability : Action {

    private DamageType secondDmgType;

    private Vector2 abilityPos;

    public DamageType SecondDmgType
    {
        get { return secondDmgType; }
        set { secondDmgType = value; }
    }
    public Vector2 AbilityPos
    {
        get { return abilityPos; }
        set { abilityPos = value; }
    }

    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {

    }
}