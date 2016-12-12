using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Player : Entity {

    enum Classes {Tank, Hunter, Mage}

    [SerializeField]
    private Camera playerCam;

    private GameObject turnManager;


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

    private Text moveCostText;

    private Text currentMovePositionUI;

    [SerializeField]
    private Dropdown dropDown;

    [SerializeField]
    private Canvas canvas;
    #endregion

    private float diagonalNerf = 0.75f;

    [SerializeField]
    private GameObject fireExplosionPrefab;

    [SerializeField]
    private GameObject[] Weapons;

    #region WeaponHandling
    [SerializeField]
    private float distance = 0.3f;

    private Vector3 mousePosition;

    //Action attach point
    private GameObject atchPoint;

    //Weapon Slot
    private GameObject weaponReff;

    private GameObject curWeapon;

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
    #endregion

    // Use this for initialization
    void Start ()
    {
        dropDown.onValueChanged.AddListener(delegate
        {
            dropDownValueChangedHandler(dropDown);
        });

        //Action attatch point
        atchPoint = new GameObject("ActionAttachPoint");

        currentClass = Classes.Tank;
        ClassTank();
        curWeapon = (GameObject)Instantiate(Weapons[0]);
        ChangeWeapon(curWeapon, true);
        UpdateStats();

        
	}

	// Update is called once per frame
	void Update ()
    {
        MenuKeyHandling();

        ActionHandling();
    }

    void FixedUpdate()
    {
        RealTimeMovement();
    }

    void Destroy()
    {
        dropDown.onValueChanged.RemoveAllListeners();
    }

    private void dropDownValueChangedHandler(Dropdown target)
    {
        Destroy(weaponReff);
        curWeapon = null;
        switch (target.value)
        {
            case 0:
                currentClass = Classes.Tank;
                ClassTank();
                curWeapon = (GameObject)Instantiate(Weapons[0]);
                ChangeWeapon(curWeapon, true);
                break;
            case 1:
                currentClass = Classes.Mage;
                ClassMage();
                curWeapon = (GameObject)Instantiate(Weapons[1]);
                ChangeWeapon(curWeapon, true);
                break;
            case 2:
                currentClass = Classes.Hunter;
                ClassHunter();
                curWeapon = (GameObject)Instantiate(Weapons[2]);
                ChangeWeapon(curWeapon, true);
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

    }

    private void EndTurn()
    {

    }

    private void ReplenishPoints()
    {

    }

    private void ChangeWeapon(GameObject newWeapon, bool isClamped)
    {
        if (weaponReff != null)
        {
            DetachChildFromPoint(weaponReff);
            weaponReff = null;
        }
        
        weaponReff = newWeapon;
        AttachChildToPoint(weaponReff);
        clamped = isClamped;
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
}
