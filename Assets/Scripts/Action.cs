﻿using UnityEngine;
using System.Collections;

public class Action : MonoBehaviour {

    public enum DamageType { KNOCKBACK, DOT, SLOW, STUN}

    private DamageType dmgType;

    private GameObject actionMan;

    private float dmg;

    private float cooldown;

    private float abilityCooldown;

    private float timeToDestroy;

    private float modifier;

    private Vector3 abilityPos;

    private bool isAbility;

    private bool knockbacked;

    private float dotTimeTick;
    private float dotTimeCooldown;

    private float slowAmount;

    public float SlowAmount
    {
        get { return slowAmount; }
        set { slowAmount = value; }
    }

    public bool Knockbacked
    {
        get { return knockbacked; }
        set { knockbacked = value; }
    }

    public float DotTimeTick
    {
        get { return dotTimeTick; }
        set { dotTimeTick = value; }
    }

    public float DotTimeCooldown
    {
        get { return dotTimeCooldown; }
        set { dotTimeCooldown = value; }
    }

    public DamageType DmgType
    {
        get { return dmgType; }
        set { dmgType = value; }
    }

    public GameObject ActionMan
    {
        get { return actionMan; }
        set { actionMan = value; }
    }

    public float Dmg
    {
        get { return dmg; }
        set { dmg = value; }
    }

    public float Cooldown
    {
        get { return cooldown; }
        set { cooldown = value; }
    }
    public float AbilityCooldown
    {
        get { return abilityCooldown; }
        set { abilityCooldown = value; }
    }

    public float Modifier
    {
        get { return modifier; }
        set { modifier = value; }
    }
    public float TimeToDestroy
    {
        get { return timeToDestroy; }
        set { timeToDestroy = value; }
    }

    public Vector3 AbilityPos
    {
        get { return abilityPos; }
        set { abilityPos = value; }
    }

    public bool IsAbility
    {
        get { return isAbility; }
        set { isAbility = value; }
    }

    public virtual void UpdateAction(int PrimaryStat)
    {

    }

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //public void AttachToParrent(GameObject newParrent)
    //{
    //    transform.parent = newParrent.transform;
    //    Debug.Log(newParrent.name + " is now parrent of " + name);
    //}

    //public void DetachFromParrent(GameObject exParrent)
    //{
    //    transform.parent = null;
    //    Debug.Log(exParrent.name + " is no longer the parrent of " + name);
    //}
}
