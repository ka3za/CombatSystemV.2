using UnityEngine;
using System.Collections;

public class Enemy : Entity {
    
    private GameObject player;
    [SerializeField]
    private BaseClass.ClassType pickEnemyClass;

    private Vector2 startPos;

    [SerializeField]
    private float combatRange = 2.5f;
    [SerializeField]
    private float meleeRange = 0.8f;
	// Use this for initialization
	void Start () {

        startPos = transform.position;

        switch (pickEnemyClass)
        {
            case BaseClass.ClassType.Hunter:
                CurrentClass = new Hunter();
                break;
            case BaseClass.ClassType.Mage:
                CurrentClass = new Mage();
                break;
            case BaseClass.ClassType.Tank:
                CurrentClass = new Tank();
                break;
            default:
                break;
        }
        player = GameObject.FindGameObjectWithTag("Player");
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (CheckDistanceToPlayer(combatRange))
        {
            Movement();
        }
        else if (Vector2.Distance(startPos, transform.position) >= 0.1f)
        {
            BackToSpawn();
        }
        
	}

    private bool CheckDistanceToPlayer(float distance)
    {
        return Vector2.Distance(player.transform.position, transform.position) <= distance;
    }

    private void Movement()
    {
        
        if(CheckDistanceToPlayer(meleeRange))
        {
            Attack();
        }else
        {
            GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position) * CurrentClass.MovementSpeed / 2);
        }
    }

    private void BackToSpawn()
    {
        Vector2 tempOwnPos = transform.position;
        GetComponent<Rigidbody2D>().AddForce((startPos - tempOwnPos) * CurrentClass.MovementSpeed / 2);
    }

    private void Attack()
    {
        //Debug.Log("PLAYER ATTACKED");
    }
}
