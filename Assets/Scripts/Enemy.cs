using UnityEngine;
using System.Collections;

public class Enemy : Entity
{

    private GameObject player;

    private Vector2 startPos;

    [SerializeField]
    private float combatRange = 2.5f;
    [SerializeField]
    private float meleeRange = 0.8f;

    private bool activated = false;

    private float currentMovePoints;

    private bool isMoving = false;

    private int movePoints = 10;

    private float meleeCD;
    private float meleeCDTimer;

    public bool Activated
    {
        get { return activated; }

        set { activated = value; }
    }

    public float CurrentMovePoints
    {
        get
        {
            return currentMovePoints;
        }

        set
        {
            currentMovePoints = value;
        }
    }

    public int MovePoints
    {
        get
        {
            return movePoints;
        }

        set
        {
            movePoints = value;
        }
    }

    // Use this for initialization
    void Start()
    {

        MovementSpeed = 160;

        BaseMovementSpeed = 160;

        CurrentHealth = 180;

        startPos = transform.position;

        CurrentMovePoints = MovePoints;

        player = GameObject.FindGameObjectWithTag("Player");

        IsSlowed = false;

        IsStunned = false;

        meleeCD = 1.5f;

    }

    // Update is called once per frame
    void Update()
    {
        if (IsStunned == false)
        {
            if (CheckDistanceToPlayer(combatRange))
            {
                Movement();
            }
            else if (Vector2.Distance(startPos, transform.position) >= 0.1f && turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Realtime)
            {
                BackToSpawn();
            }
            else if (CheckDistanceToPlayer(combatRange) == false && turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Turnbased && activated)
            {
                Debug.Log(1);
                turnManager.GetComponent<TurnManager>().DeactivateEnemy();
                CurrentMovePoints = MovePoints;
                activated = false;
            }

            if (turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Turnbased && CurrentMovePoints <= 0 && !isMoving && activated)
            {
                Debug.Log(2);
                turnManager.GetComponent<TurnManager>().DeactivateEnemy();
                CurrentMovePoints = MovePoints;
                activated = false;
            }

            Debug.Log(CurrentHealth);
        }


        if (CurrentHealth <= 0)
        {
            //Et hack fix til at tage enemy ud af enemies listen
            transform.position = new Vector3(50000, 0);
            OnDeath();
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
        meleeCDTimer -= Time.deltaTime;
        if (CheckDistanceToPlayer(meleeRange))
        {
            Attack();
        }
        else if (turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Realtime)
        {
            GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * MovementSpeed);
        }
        else if (turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Turnbased)
        {
            if (!activated && GetComponent<Rigidbody2D>().velocity != new Vector2(0, 0))
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().angularVelocity = 0f;

            }

            if (activated && CurrentMovePoints != 0 && !isMoving)
            {
                Debug.Log("Blargh1");
                float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

                if (Vector2.Distance(player.transform.position, transform.position) >= 2f)
                {
                    CurrentMovePoints = 0;
                }
                else if (Vector2.Distance(player.transform.position, transform.position) >= 1.5f)
                {
                    CurrentMovePoints -= 6;
                }
                else if (Vector2.Distance(player.transform.position, transform.position) >= 1f)
                {
                    CurrentMovePoints -= 4;
                }
                else if (Vector2.Distance(player.transform.position, transform.position) >= 0.5f)
                {
                    CurrentMovePoints -= 2;
                }
                isMoving = true;

                Debug.Log(distanceToPlayer);
            }
            else if (activated && isMoving)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 0.5f * Time.deltaTime);
                Debug.Log("dims");
                Debug.Log(Vector2.Distance(player.transform.position, transform.position));
                if (Vector2.Distance(transform.position, player.transform.position) <= 0.6f)
                {
                    isMoving = false;
                }

            }
        }
    }

    /// <summary>
    /// If the Enemy is away from its original spawn position and far away from the Player, this method allow it to return to its spawn location.
    /// </summary>
    private void BackToSpawn()
    {
        Vector2 tempOwnPos = transform.position;
        GetComponent<Rigidbody2D>().AddForce((startPos - tempOwnPos) * MovementSpeed);
    }

    /// <summary>
    /// Method to make the Enemy attack
    /// </summary>
    private void Attack()
    {
        if (turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Realtime)
        {
            if (meleeCDTimer <= 0)
            {
                meleeCDTimer = meleeCD;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CurrentHealth -= 20;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UpdateStats();
            }
        }
        else
        {
            if (CurrentMovePoints >= 2)
            {
                CurrentMovePoints -= 2;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CurrentHealth -= 10;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UpdateStats();
            }
        }
    }
}
