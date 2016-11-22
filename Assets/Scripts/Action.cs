using UnityEngine;
using System.Collections;

public class Action : MonoBehaviour {

    public enum DamageType { KNOCKBACK, DOT, SLOW, STUN}

    private DamageType dmgType;

    private float dmg;

    private float cooldown;

    private float modifier;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
