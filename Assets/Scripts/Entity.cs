using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

    [SerializeField]
    private BaseClass currentClass;

    public BaseClass CurrentClass
    {
        get { return currentClass; }
        set { currentClass = value; }
    }

    private float health;
    private float currentHealth;
    private int movementSpeed;

    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public int MovementSpeed
    {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void SetClass(string className)
    {
        switch (className)
        {
            case "Tank":
                currentClass = new Tank();
                break;
            case "Mage":
                currentClass = new Mage();
                break;
            case "Hunter":
                currentClass = new Hunter();
                break;
            default:
                break;
        }
    }

    public virtual void OnDeath()
    {

    }
}
