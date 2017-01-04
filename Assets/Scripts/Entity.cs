using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {


    private float health;
    private float currentHealth;
    private int movementSpeed;
    private int baseMovementSpeed;
    private bool isSlowed;
    private bool isStunned;

    [SerializeField]
    protected GameObject turnManager;

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

    public int BaseMovementSpeed
    {
        get { return baseMovementSpeed; }
        set { baseMovementSpeed = value; }
    }

    public bool IsSlowed
    {
        get { return isSlowed; }
        set { isSlowed = value; }
    }

    public bool IsStunned
    {
        get { return isStunned; }
        set { isStunned = value; }
    }




    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}
    

    public virtual void OnDeath()
    {
        gameObject.SetActive(false);
        //Destroy(gameObject,0.5f); 
    }
}
