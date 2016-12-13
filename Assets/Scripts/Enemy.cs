using UnityEngine;
using System.Collections;

public class Enemy : Entity {
    
    private GameObject player;

    private Vector2 startPos;

    [SerializeField]
    private float combatRange = 2.5f;
    [SerializeField]
    private float meleeRange = 0.8f;
	// Use this for initialization
	void Start () {

        MovementSpeed = 150;

        startPos = transform.position;

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

    /// <summary>
    /// This will return false or true depending on how far away the Player is from the Enemy(itself)
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    private bool CheckDistanceToPlayer(float distance)
    {
        return Vector2.Distance(player.transform.position, transform.position) <= distance;
    }

    /// <summary>
    /// Enemy movement.
    /// </summary>
    private void Movement()
    {
        
        if(CheckDistanceToPlayer(meleeRange))
        {
            Attack();
        }else
        {
            GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position) * MovementSpeed / 2);
        }
    }

    /// <summary>
    /// If the Enemy is away from its original spawn position and far away from the Player, this method allow it to return to its spawn location.
    /// </summary>
    private void BackToSpawn()
    {
        Vector2 tempOwnPos = transform.position;
        GetComponent<Rigidbody2D>().AddForce((startPos - tempOwnPos) * MovementSpeed / 2);
    }

    /// <summary>
    /// Method to make the Enemy attack
    /// </summary>
    private void Attack()
    {
        //Debug.Log("PLAYER ATTACKED");
    }
}
