using UnityEngine;
using System.Collections;

public class Enemy : Entity {
    
    private GameObject player;

    private Vector2 startPos;

    [SerializeField]
    private float combatRange = 2.5f;
    [SerializeField]
    private float meleeRange = 0.8f;

    private bool activated;

    private float currentMovePoints;

    private bool isMoving = false;

    private int movePoints = 10;

    public bool Activated
    {
        get { return activated; }

        set { activated = value; }
    }
    // Use this for initialization
    void Start () {

        MovementSpeed = 150;

        startPos = transform.position;

        currentMovePoints = movePoints;

        player = GameObject.FindGameObjectWithTag("Player");
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (CheckDistanceToPlayer(combatRange))
        {
            Movement();
        }
        else if (Vector2.Distance(startPos, transform.position) >= 0.1f && turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Realtime)
        {
            BackToSpawn();
        }
        else if (CheckDistanceToPlayer(combatRange) && turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Turnbased)
        {
            turnManager.GetComponent<TurnManager>().DeactivateEnemy();
        }

        if (turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Turnbased && currentMovePoints <= 0)
        {
            turnManager.GetComponent<TurnManager>().DeactivateEnemy();
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

        if (CheckDistanceToPlayer(meleeRange))
        {
            Attack();
        }
        else if (turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Realtime)
        {
            GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position) * MovementSpeed / 2);
        }
        else if (turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Turnbased)
        {
            if (activated && currentMovePoints != 0 && !isMoving)
            {
                float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

                if (Vector2.Distance(player.transform.position, transform.position) >= 2f)
                {
                    currentMovePoints = 0;
                }
                else if (Vector2.Distance(player.transform.position, transform.position) >= 1.5f)
                {
                    currentMovePoints -= 6;
                }
                else if (Vector2.Distance(player.transform.position, transform.position) >= 1f)
                {
                    currentMovePoints -= 4;
                }
                else if (Vector2.Distance(player.transform.position, transform.position) >= 0.5f)
                {
                    currentMovePoints -= 2;
                }

                isMoving = true;

                Debug.Log(distanceToPlayer);
            }
            else if (activated && isMoving)
            {
                Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, player.transform.position.y), meleeRange);
                if (Vector2.Distance(transform.position, player.transform.position) >= 0.2f)
                {
                    isMoving = false;
                }

            }
            else if (activated && currentMovePoints == 0 && !isMoving)
            {
                turnManager.GetComponent<TurnManager>().DeactivateEnemy();
            }
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
        if (turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Realtime)
        {
            //Debug.Log("PLAYER ATTACKED");
        }
        else
        {
            currentMovePoints -= 2;
        }
    }
}
