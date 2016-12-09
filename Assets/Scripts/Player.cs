using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Player : Entity {

    enum Classes {Tank, Hunter, Mage}

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
    #endregion

    private float diagonalNerf = 0.75f;

    [SerializeField]
    private GameObject fireExplosionPrefab;



    #region WeaponHandling
    [SerializeField]
    private float distance = 0.3f;

    private Vector3 mousePosition;

    //Action attach point
    private GameObject atchPoint;

    //Weapon Slot
    private GameObject weaponReff;

    //Free or contrained weapon
    private bool clamped = false;
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
        set{ movePoints = value; }
    }

    #endregion

    // Use this for initialization
    void Start ()
    {
        dropDown.onValueChanged.AddListener(delegate
        {
            dropDownValueChangedHandler(dropDown);
        });

        turnManager.GetComponent<TurnManager>().CurrentCombatMode = TurnManager.CombatMode.Realtime;

        currentClass = Classes.Tank;
        ClassTank();
        currentMovePoints = MovePoints;

        UpdateStats();

        //Weapon Slot
        weaponReff = GameObject.Find("Weapon1");

        //Action attatch point
        atchPoint = new GameObject("ActionAttachPoint");
	}

	// Update is called once per frame
	void Update ()
    {
        MenuKeyHandling();
        if (Input.GetKeyDown(KeyCode.J))
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

        //gets the mouse position on the screen
        mousePosition = playerCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 
            Input.mousePosition.y, Input.mousePosition.z - playerCam.transform.position.z));

        if(Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log(transform.position.ToString());
        }

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
            atchPoint.transform.position = (mousePosition- transform.position).normalized * distance + transform.position;
        }

        //rotates action attatch point toward extrapoladed mouse position
        atchPoint.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((end.y - transform.position.y),
            (end.x - transform.position.x)) * Mathf.Rad2Deg + 90);

        if (Input.GetKeyDown(KeyCode.J))
        {
            clamped = !clamped;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            AttachChildToPoint(weaponReff);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            DetachChildFromPoint(weaponReff);
        }
        RealTimeMovement();
    }

    void Destroy()
    {
        dropDown.onValueChanged.RemoveAllListeners();
    }

    private void dropDownValueChangedHandler(Dropdown target)
    {
        switch (target.value)
        {
            case 0:
                currentClass = Classes.Tank;
                ClassTank();
                //NOTE: ChangeWeapon
                break;
            case 1:
                currentClass = Classes.Mage;
                ClassMage();
                break;
            case 2:
                currentClass = Classes.Hunter;
                ClassHunter();
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
    /// Update health and energy
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
        strength = 3;
        intellect = 0;
        agility = 0;
        armor = 10;
        BaseMovementSpeed = 120;
        MovementSpeed = BaseMovementSpeed;
        Health = (Stamina * 1.5f) + 10;
        CurrentHealth = Health;
        MaxEnergy = 100;
        CurrentEnergy = MaxEnergy;
        MovePoints = 8;
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
    }

    private void ClassHunter()
    {
        stamina = 105;
        strength = 0;
        intellect = 0;
        agility = 10;
        armor = 5;
        BaseMovementSpeed = 170;
        MovementSpeed = BaseMovementSpeed;
        Health = (Stamina * 1.5f) + 10;
        CurrentHealth = Health;
        MaxEnergy = 100;
        CurrentEnergy = MaxEnergy;
        MovePoints = 12;
    }

    /// <summary>
    /// A method to handle keys to movement for the realtime combat mode
    /// </summary>
    private void RealTimeMovement()
    {
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

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseAbility();
        }

        playerCam.GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y, -10);
    }

    private void MenuKeyHandling()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            canvas.enabled = true;
        }
    }

    private void ActionHandling()
    {
        //gets the mouse position on the screen
        mousePosition = playerCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, Input.mousePosition.z - playerCam.transform.position.z));

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

        if (Input.GetKeyDown(KeyCode.J))
        {
            clamped = !clamped;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            AttachChildToPoint(weaponReff);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            DetachChildFromPoint(weaponReff);
        }
    }

    private void AttachChildToPoint(GameObject newChild)
    {
        if (newChild.transform.parent == null)
        {
            atchPoint.transform.rotation = Quaternion.identity;
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
            currentMovePointsUI.text = "Current MP: " + currentMovePoints + " / " + MovePoints;
            if (Input.GetMouseButtonDown(1))
            {
                if (!isMoving)
                {
                    if (currentMovePoints >= moveCost)
                    {
                        moveTarget = currentMousePosition;
                        isMoving = true;
                        currentMovePoints -= moveCost;
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
                EndTurn();
            }

        }
    }

    private void EndTurn()
    {
        turnManager.GetComponent<TurnManager>().SwitchTurn();
        currentMovePoints = MovePoints;
    }

    private void ChangeWeapon()
    {
        //if weaponreff != null
        //detach
        //attatch weapon switch
        //remember clamped or not
        //what about ranger and raycast?
    }

    private void PrimeAbility()
    {
        //Sæt ability på action attacth point
    }

    private void UnprimeAbility()
    {
        //tag ability af action attach point
        //attacth weapon
    }

    private void UsePrimedAction()
    {
        //kald action attacth point child Use()
    }

    private void UseAbility()
    {
        switch (currentClass)
        {
            case Classes.Hunter:
                break;
            case Classes.Mage:
                Vector3 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                tempPos.z = 0.1f;
                GameObject fireExplosionPrefabTemp = Instantiate(fireExplosionPrefab, tempPos, Quaternion.identity) as GameObject;
                fireExplosionPrefabTemp.GetComponent<FireExplosion>().UpdateAction(intellect);
                break;
            case Classes.Tank:
                break;
            default:
                break;
        }
        
    }


    public override void OnDeath()
    {
        if (CurrentHealth <= 0)
        {

        }
        base.OnDeath();
    }

    public void OnDrawGizmos()
    {

        if (turnManager.GetComponent<TurnManager>().CurrentCombatMode == TurnManager.CombatMode.Turnbased && turnManager.GetComponent<TurnManager>().playerTurn)
        {
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, currentMovePoints / 2);
            currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePosition.z = 0;
            moveCost = Mathf.Ceil(Vector3.Distance(transform.position, currentMousePosition) * 2);

            if (Vector3.Distance(transform.position, currentMousePosition * 2) <= currentMovePoints)
            {

                moveCostText.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 20, Input.mousePosition.z);
                moveCostText.text = "MP COST: " + moveCost;
                UnityEditor.Handles.DrawLine(transform.position, currentMousePosition);
            }
            else
            {
                moveCostText.text = "";
            }
        }
    }
}
