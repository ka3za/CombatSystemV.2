using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public enum CombatMode { Realtime, Turnbased }

    [SerializeField]
    private GameObject player;

    [SerializeField]
    public List<GameObject> enemies = new List<GameObject>();

    private List<GameObject> enemiesSorted = new List<GameObject>();

    public bool playerTurn = true;

    private List<GameObject> enemySpeed = new List<GameObject>();

    private int temp;

    private bool isLarger;
    
    private bool isEqual;

    private bool listed = false;

    private int movedCount;

    private bool enemyActive = false;

    private CombatMode currentCombatMode;

    public CombatMode CurrentCombatMode
    {
        get { return currentCombatMode; }

        set { currentCombatMode = value; }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!listed)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemySpeed.Add(enemies[i]);
            }

            ListEnemy();
        }
        if (!playerTurn && !enemyActive)
        {
            ActivateEnemy();
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            for (int i = 0; i < enemiesSorted.Count; i++)
            {
                Debug.Log(enemiesSorted[i].ToString());
                Debug.Log(enemiesSorted[movedCount]);
                Debug.Log(movedCount);
            }
        }
    }

    public void SwitchTurn()
    {
        if (playerTurn)
        {
            playerTurn = false;
        }
        else
        {
            playerTurn = true;
        }
    }

    private void ListEnemy()
    {

        if (enemies.Count != 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                for (int j = 0; j < enemySpeed.Count; j++)
                {
                    temp = enemySpeed[j].GetComponent<Enemy>().MovementSpeed;
                    if (enemies[i].GetComponent<Enemy>().MovementSpeed < temp)
                    {
                        isLarger = false;
                        isEqual = false;
                    }
                    else if (enemies[i].GetComponent<Enemy>().MovementSpeed > temp)
                    {
                        isEqual = false;
                        isLarger = true;
                    }
                    else if(enemies[i].GetComponent<Enemy>().MovementSpeed == temp)
                    {
                        isEqual = true;
                        isLarger = false;
                    }

                }

                if (isLarger)
                {
                    enemiesSorted.Add(enemies[i]);
                    enemies.RemoveAt(i);

                }
                else if(isEqual)
                {
                    enemiesSorted.Add(enemies[i]);
                    enemies.RemoveAt(i);
                }
            }
        }
        else
        {
            listed = true;
        }
    }

    private void ActivateEnemy()
    {
        if (movedCount < enemiesSorted.Count)
        {
            if (!enemiesSorted[movedCount].GetComponent<Enemy>().Activated)
            {
                enemiesSorted[movedCount].GetComponent<Enemy>().Activated = true;
                enemyActive = true;
            }
        }
        else
        {
            GetComponent<ActionManager>().NewTurn();
        }

    }

    public void ReplenishActionPoints()
    {
        if(playerTurn)
        {
            player.GetComponent<Player>().CurrentMovePoints = player.GetComponent<Player>().MovePoints;
        }
        else
        {
            for (int i = 0; i < enemiesSorted.Count; i++)
            {
                enemiesSorted[i].GetComponent<Enemy>().CurrentMovePoints = enemiesSorted[0].GetComponent<Enemy>().MovePoints;
            }
            movedCount = 0;
        }
    }

    public void DeactivateEnemy()
    {
        movedCount += 1;
        enemyActive = false;
    }
}
