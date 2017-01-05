using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Player : Entity
{

    enum Classes { Tank, Hunter, Mage }

    [SerializeField]
    private Camera playerCam;


    #region visual
    [SerializeField]
    private Sprite[] playerSprites;
    [SerializeField]
    private Sprite[] playerSprites2;
    [SerializeField]
    private Sprite[] playerSprites3;
    [SerializeField]
    private Text classText;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text energySourceText;
    [SerializeField]
    private Text moveCostText;
    [SerializeField]
    private Text currentMovePointsUI;

    private bool isMoving = false;

    private float currentMovePoints;

    private float moveCost;

    private Vector3 currentMousePosition;

    private Vector3 moveTarget;

    [SerializeField]
    private Dropdown dropDown;

    [SerializeField]
    private Canvas canvas;

    private int lineCount = 100;

    private Material material;

    public float CurrentMovePoints
    {
        get { return currentMovePoints; }
        set { currentMovePoints = value; }
    }
    #endregion

    private float diagonalNerf = 0.75f;

    private float abilityCDTimer;

    #region ActionHandling
    [SerializeField]
    private GameObject[] Weapons;

    [SerializeField]
    private GameObject[] Abilities;

    [SerializeField]
    private float distance = 0.3f;

    private Vector3 mousePosition;

    //Action attach point
    private GameObject atchPoint;

    //Weapon Slot
    private GameObject actionReff;

    private GameObject curAction;

    //Free or contrained weapon
    private bool clamped = false;

    private bool actionSwitch = false;
    #endregion

    #region Stats

    private Classes currentClass;

    private int stamina;
    private int strength;
    private int intellect;
    private int agility;
    private int armor;
    private float currentEnergy;
    private float maxEnergy;
    private int movePoints;

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


    public float CurrentEnergy
    {
        get { return currentEnergy; }
        set { currentEnergy = value; }
    }

    public float MaxEnergy
    {
        get { return maxEnergy; }
        set { maxEnergy = value; }
    }

    public int MovePoints
    {
        get { return movePoints; }
        set { movePoints = value; }
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        dropDown.onValueChanged.AddListener(delegate
        {
            dropDownValueChangedHandler(dropDown);
        });

        turnManager.GetComponent<TurnManager>().CurrentCombatMode = TurnManager.CombatMode.Realtime;

        //Action attatch point
        atchPoint = new GameObject("ActionAttachPoint");
        IsSlowed = false;
        IsStunned = false;
        currentClass = Classes.Tank;
        ClassTank();
        ChangeAction();
        UpdateStats();


    }

    // Update is called once per frame
    void Update()
    {
        if (IsStunned == false)
        {
            MenuKeyHandling();
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Realtime)
                {
                    turnManager.GetComponent<TurnManager>().CurrentCombatMode = TurnManager.CombatMode.Turnbased;
                }
                else
                {
                    turnManager.GetComponent<TurnManager>().CurrentCombatMode = TurnManager.CombatMode.Realtime;
                }
            }
            ActionHandling();

            if (turnManager.GetComponent<TurnManager>().CurrentCombatMode != TurnManager.CombatMode.Turnbased)
            {
                currentMovePointsUI.text = "";
            }
        }

        if (CurrentHealth <= 0)
        {
            OnDeath();
        }

        ShowTravelLimit();
    }

    void FixedUpdate()
    {
        if (turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Realtime)
        {
            RealTimeMovement();
        }
        else
        {
            TurnBasedMovement();
        }
    }

    void Destroy()
    {
        dropDown.onValueChanged.RemoveAllListeners();
    }

    /// <summary>
    /// Depending on the target param. It will switch to one of the classes. And run the UpdateStats method.
    /// </summary>
    /// <param name="target"></param>
    private void dropDownValueChangedHandler(Dropdown target)
    {
        switch (target.value)
        {
            case 0:
                currentClass = Classes.Tank;
                ClassTank();
                ChangeAction();
                break;
            case 1:
                currentClass = Classes.Mage;
                ClassMage();
                ChangeAction();
                break;
            case 2:
                currentClass = Classes.Hunter;
                ClassHunter();
                ChangeAction();
                break;
            default:
                break;
        }
        UpdateStats();

    }

    public void SetDropdownIndex(int index)
    {
        dropDown.value = index;
    }

    /// <summary>
    /// Updates health and energy for the Player
    /// </summary>
    public void UpdateStats()
    {
        healthText.text = "Health : " + CurrentHealth + " / " + Health;
        classText.text = "Current Class: " + currentClass;
        switch (currentClass)
        {
            case Classes.Hunter:
                energySourceText.text = "Energy : " + currentEnergy + " / " + maxEnergy;
                break;
            case Classes.Mage:
                energySourceText.text = "Mana : " + currentEnergy + " / " + maxEnergy;
                break;
            case Classes.Tank:
                energySourceText.text = "Rage : " + currentEnergy + " / " + maxEnergy;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Update base stats for each class that the player can be
    /// </summary>
    /// 

    private void ClassTank()
    {
        stamina = 200;
        strength = 15;
        intellect = 0;
        agility = 0;
        armor = 50;
        BaseMovementSpeed = 120;
        MovementSpeed = BaseMovementSpeed;
        Health = (Stamina * 1.5f) + 10;
        CurrentHealth = Health;
        MaxEnergy = 100;
        CurrentEnergy = MaxEnergy;
        MovePoints = 8;
        CurrentMovePoints = MovePoints;
    }

    private void ClassMage()
    {
        stamina = 100;
        strength = 0;
        intellect = 10;
        agility = 0;
        armor = 0;
        BaseMovementSpeed = 160;
        MovementSpeed = BaseMovementSpeed;
        Health = (Stamina * 1.5f) + 100;
        CurrentHealth = Health;
        MaxEnergy = (Intellect * 1.1f) + 100;
        CurrentEnergy = MaxEnergy;
        MovePoints = 10;
        CurrentMovePoints = MovePoints;
    }

    private void ClassHunter()
    {
        stamina = 105;
        strength = 0;
        intellect = 0;
        agility = 25;
        armor = 10;
        BaseMovementSpeed = 170;
        MovementSpeed = BaseMovementSpeed;
        Health = (Stamina * 1.5f) + 10;
        CurrentHealth = Health;
        MaxEnergy = 100;
        CurrentEnergy = MaxEnergy;
        MovePoints = 12;
        CurrentMovePoints = MovePoints;
    }
    /// <summary>
    /// A method to handle keys to movement for the realtime combat mode
    /// </summary>
    private void RealTimeMovement()
    {
        abilityCDTimer -= Time.deltaTime;
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.left * MovementSpeed);
            switch (currentClass)
            {
                case Classes.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[2];
                    break;
                case Classes.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[2];
                    break;
                case Classes.Tank:
                    GetComponent<SpriteRenderer>().sprite = playerSprites[2];
                    break;
                default:
                    break;
            }

        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().AddForce((Vector2.left + Vector2.up) * (MovementSpeed * diagonalNerf));
            switch (currentClass)
            {
                case Classes.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[2];
                    break;
                case Classes.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[2];
                    break;
                case Classes.Tank:
                    GetComponent<SpriteRenderer>().sprite = playerSprites[2];
                    break;
                default:
                    break;
            }

        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().AddForce((Vector2.left + Vector2.down) * (MovementSpeed * diagonalNerf));
            switch (currentClass)
            {
                case Classes.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[2];
                    break;
                case Classes.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[2];
                    break;
                case Classes.Tank:
                    GetComponent<SpriteRenderer>().sprite = playerSprites[2];
                    break;
                default:
                    break;
            }

        }

        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * MovementSpeed);
            switch (currentClass)
            {
                case Classes.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[1];
                    break;
                case Classes.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[1];
                    break;
                case Classes.Tank:
                    GetComponent<SpriteRenderer>().sprite = playerSprites[1];
                    break;
                default:
                    break;
            }

        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().AddForce((Vector2.right + Vector2.up) * (MovementSpeed * diagonalNerf));
            switch (currentClass)
            {
                case Classes.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[1];
                    break;
                case Classes.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[1];
                    break;
                case Classes.Tank:
                    GetComponent<SpriteRenderer>().sprite = playerSprites[1];
                    break;
                default:
                    break;
            }

        }

        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().AddForce((Vector2.right + Vector2.down) * (MovementSpeed * diagonalNerf));
            switch (currentClass)
            {
                case Classes.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[1];
                    break;
                case Classes.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[1];
                    break;
                case Classes.Tank:
                    GetComponent<SpriteRenderer>().sprite = playerSprites[1];
                    break;
                default:
                    break;
            }

        }


        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * MovementSpeed);
            switch (currentClass)
            {
                case Classes.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[3];
                    break;
                case Classes.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[3];
                    break;
                case Classes.Tank:
                    GetComponent<SpriteRenderer>().sprite = playerSprites[3];
                    break;
                default:
                    break;
            }

        }

        if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.down * MovementSpeed);
            switch (currentClass)
            {
                case Classes.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[0];
                    break;
                case Classes.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[0];
                    break;
                case Classes.Tank:
                    GetComponent<SpriteRenderer>().sprite = playerSprites[0];
                    break;
                default:
                    break;
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseAbility();
        }

        playerCam.GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y, -10);
    }

    /// <summary>
    /// Will handle all keys for menus.
    /// </summary>
    private void MenuKeyHandling()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            canvas.enabled = true;
        }
    }

    private Vector3 GetMousePosition()
    {
        return playerCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, Input.mousePosition.z - playerCam.transform.position.z));
    }

    private void ActionHandling()
    {
        //gets the mouse position on the screen
        mousePosition = GetMousePosition();

        //mouseline
        Debug.DrawLine(transform.position, mousePosition, Color.red);

        //extrapoladed mouse position
        var end = (transform.position - mousePosition).normalized * 1000;

        if (!clamped)
        {
            //sets action attatch point at mouse position
            atchPoint.transform.position = mousePosition;
        }
        else
        {
            //sets action attatch point to a limited distance i the direction of mouse position
            atchPoint.transform.position = (mousePosition - transform.position).normalized * distance + transform.position;
        }

        //rotates action attatch point toward extrapoladed mouse position
        atchPoint.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((end.y - transform.position.y),
            (end.x - transform.position.x)) * Mathf.Rad2Deg + 90);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            actionSwitch = !actionSwitch;
            ChangeAction();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Realtime)
            {
                UseAction();
            }
            else
            {
                if (CurrentMovePoints >= 2)
                {
                    UseAction();
                    CurrentMovePoints -= 2;
                }
            }
        }

        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    clamped = !clamped;
        //}

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    AttachChildToPoint(actionReff);
        //}

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    DetachChildFromPoint(actionReff);
        //}
    }

    private void UseAction()
    {
        curAction.GetComponentInChildren<Action>().Use(strength, intellect, agility);
    }

    private void AttachChildToPoint(GameObject newChild)
    {
        if (newChild.transform.parent == null)
        {
            atchPoint.transform.rotation = Quaternion.identity;
            newChild.transform.rotation = Quaternion.identity;

            newChild.transform.position = atchPoint.transform.position;
            newChild.transform.parent = atchPoint.transform;
            Debug.Log(newChild.name + " is now child of action attatch point");
        }
        else
        {
            Debug.Log(newChild.name + " already is child of action attatch point");
        }
    }

    private void DetachChildFromPoint(GameObject exChild)
    {
        if (exChild.transform.parent != null)
        {
            exChild.transform.parent = null;
            Debug.Log(exChild.name + " is no longer the child of action attatch point");
        }
        else
        {
            Debug.Log(exChild.name + " is already not a child of action attatch point");
        }
    }

    private void TurnBasedMovement()
    {
        if (turnManager.GetComponent<TurnManager>().playerTurn)
        {
            currentMovePointsUI.text = "Current MP: " + CurrentMovePoints + " / " + MovePoints;
            if (Input.GetMouseButtonDown(1))
            {
                if (!isMoving)
                {
                    if (CurrentMovePoints >= moveCost)
                    {
                        moveTarget = currentMousePosition;
                        isMoving = true;
                        CurrentMovePoints -= moveCost;
                    }
                }

            }

            if (transform.position != moveTarget && isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, moveTarget, 5 * Time.deltaTime);
            }
            else
            {
                isMoving = false;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                turnManager.GetComponent<ActionManager>().NewTurn();
            }

        }
    }

    private void ChangeAction()
    {
        if (curAction != null)
        {
            DetachChildFromPoint(curAction);
            Destroy(curAction);
            curAction = null;
        }

        switch (currentClass)
        {
            case Classes.Hunter:
                if (actionSwitch)
                {
                    curAction = Instantiate(Abilities[2]);
                    clamped = false;
                }
                else
                {
                    curAction = Instantiate(Weapons[2]);
                    clamped = true;
                }
                AttachChildToPoint(curAction);
                break;
            case Classes.Mage:
                if (actionSwitch)
                {
                    curAction = Instantiate(Abilities[1]);
                    clamped = false;
                }
                else
                {
                    curAction = Instantiate(Weapons[1]);
                    clamped = true;
                }
                AttachChildToPoint(curAction);
                break;
            case Classes.Tank:
                if (actionSwitch)
                {
                    curAction = Instantiate(Abilities[0]);
                    clamped = true;
                }
                else
                {
                    curAction = Instantiate(Weapons[0]);
                    clamped = true;
                }
                AttachChildToPoint(curAction);
                break;
            default:
                break;
        }
    }

    private void UseAbility()
    {
        if (abilityCDTimer <= 0)
        {
            abilityCDTimer = 1;
            Vector3 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            switch (currentClass)
            {
                case Classes.Hunter:
                    break;
                case Classes.Mage:
                    tempPos.z = 0.1f;
                    GameObject fireExplosionPrefabTemp = Instantiate(Resources.Load("FireExplosionSprite") as GameObject, tempPos, Quaternion.identity) as GameObject;
                    fireExplosionPrefabTemp.GetComponent<FireExplosion>().UpdateAction(intellect);
                    break;
                case Classes.Tank:
                    tempPos.z = 0.1f;
                    GameObject shieldBashPrefabTemp = Instantiate(Resources.Load("ShieldBashSprite") as GameObject, transform.position, Quaternion.identity) as GameObject;
                    shieldBashPrefabTemp.GetComponent<ShieldBass>().UpdateAction(strength);
                    break;
                default:
                    break;
            }
        }


    }


    public override void OnDeath()
    {
        if (CurrentHealth <= 0)
        {
            SceneManager.LoadScene("Menu");
        }
        base.OnDeath();
    }

    private void ShowTravelLimit()
    {

        if (turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Turnbased && turnManager.GetComponent<TurnManager>().playerTurn)
        {
            currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePosition.z = 0;
            moveCost = Mathf.Ceil(Vector3.Distance(transform.position, currentMousePosition) * 2);

            if (Vector3.Distance(transform.position, currentMousePosition * 2) <= CurrentMovePoints)
            {

                moveCostText.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 20, Input.mousePosition.z);
                moveCostText.text = "MP COST: " + moveCost;
            }
            else
            {
                moveCostText.text = "";
            }
        }
        else
        {
            moveCostText.text = "";
        }
    }

    public void OnDrawGizmos()
    {

#if Unity_Editor
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, CurrentMovePoints / 2);
#endif
#if Unity_Editor
                UnityEditor.Handles.DrawLine(transform.position, currentMousePosition);
#endif  

    }
}
