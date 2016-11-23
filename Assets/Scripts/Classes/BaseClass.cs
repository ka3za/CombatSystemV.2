using UnityEngine;
using System.Collections;

public class BaseClass {

    public enum ClassType { Hunter, Mage, Tank}

    private ClassType theClassType;
    private int stamina;
    private int strength;
    private int intellect;
    private int agility;
    private int armor;
    private int movementSpeed;
    private float health;
    private float currentHealth;
    private int currentEnergy;
    private int maxEnergy;

    public ClassType TheClassType
    {
        get { return theClassType; }
        set { theClassType = value; }
    }

    public int Stamina
    {
        get { return stamina; }
        set { stamina = value; }
    }

    public int Strength
    {
        get { return strength; }
        set { strength = value; }
    }

    public int Intellect
    {
        get { return intellect; }
        set { intellect = value; }
    }

    public int Agility
    {
        get { return agility; }
        set { agility = value; }
    }

    public int Armor
    {
        get { return armor; }
        set { armor = value; }
    }

    public int MovementSpeed
    {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }
    public float Health
    {
        get { return Health; }
        set { Health = value; }
    }
    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }


    public int CurrentEnergy
    {
        get { return currentEnergy; }
        set { currentEnergy = value; }
    }

    public int MaxEnergy
    {
        get { return maxEnergy; }
        set { maxEnergy = value; }
    }
}
