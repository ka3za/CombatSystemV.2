using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : Entity {

    [SerializeField]
    private Camera playerCam;

    [SerializeField]
    private Sprite[] playerSprites;
    [SerializeField]
    private Sprite[] playerSprites2;
    [SerializeField]
    private Sprite[] playerSprites3;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text energySourceText;

    private Text moveCostText;

    private Text currentMovePositionUI;

    private GameObject turnManager;

    [SerializeField]
    private Dropdown dropDown;

    [SerializeField]
    private Canvas canvas;

    private float diagonalNerf = 0.75f;

    [SerializeField]
    private GameObject fireExplosionPrefab;

    [SerializeField]
    private float distance = 0.05f;

    private Vector3 mousePosition;
    private Vector3 direction;
    private float distanceFromPlayer;

    //Action attach point
    private GameObject atchPoint;

    //Weapon Slot
    private GameObject weaponReff;

    private bool clamped = false;

    // Use this for initialization
    void Start ()
    {
        dropDown.onValueChanged.AddListener(delegate
        {
            dropDownValueChangedHandler(dropDown);
        });

        CurrentClass = new Tank();
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
    }

    void FixedUpdate()
    {
        RealTimeMovement();

        //gets the mouse position on the screen
        mousePosition = playerCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 
            Input.mousePosition.y, Input.mousePosition.z - playerCam.transform.position.z));

        //raycast
        //RaycastHit2D ray = Physics2D.Raycast(transform.position, mousePosition, 1000);
        Debug.DrawRay(transform.position, mousePosition, Color.red);
        var end = transform.position + mousePosition.normalized * 1000;

        if (!clamped)
        {
            //sets action attatch point at mouse position
            atchPoint.transform.position = mousePosition; 
        }
        else
        {
            //NOTE: Jeg tror .normalized fucker det op.
            atchPoint.transform.position = (mousePosition- transform.position).normalized * distance + transform.position;
        }

        //rotates action attatch point toward extrapoladed mouse position
        atchPoint.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((end.y - transform.position.y),
            (end.x - transform.position.x)) * Mathf.Rad2Deg + 90);

        //gets distance between player position and mouse position
        //distanceFromPlayer = (Input.mousePosition - playerCam.WorldToScreenPoint(transform.position)).magnitude;

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

    void Destroy()
    {
        dropDown.onValueChanged.RemoveAllListeners();
    }

    public void AttachChildToPoint(GameObject newChild)
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

    public void DetachChildFromPoint(GameObject exChild)
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

    private void dropDownValueChangedHandler(Dropdown target)
    {
        switch (target.value)
        {
            case 0:
                CurrentClass = new Tank();
                Debug.Log("Changed to tank");
                break;
            case 1:
                CurrentClass = new Mage();
                Debug.Log("Changed to mage");
                break;
            case 2:
                CurrentClass = new Hunter();
                Debug.Log("Changed to hunter");
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

    public void UpdateStats()
    {
        if (CurrentClass != null)
        {
            healthText.text = "Health : " + CurrentClass.CurrentHealth + " / " + CurrentClass.Health;
            switch (CurrentClass.TheClassType)
            {
                case BaseClass.ClassType.Hunter:
                    energySourceText.text = "Energy : " + CurrentClass.CurrentEnergy + " / " + CurrentClass.MaxEnergy;
                    break;
                case BaseClass.ClassType.Mage:
                    energySourceText.text = "Mana : " + CurrentClass.CurrentEnergy + " / " + CurrentClass.MaxEnergy;
                    break;
                case BaseClass.ClassType.Tank:
                    energySourceText.text = "Rage : " + CurrentClass.CurrentEnergy + " / " + CurrentClass.MaxEnergy;
                    break;
                default:
                    break;
            }

        }
    }

    private void RealTimeMovement()
    {
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.left * CurrentClass.MovementSpeed);
            switch (CurrentClass.TheClassType)
            {
                case BaseClass.ClassType.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[2];
                    break;
                case BaseClass.ClassType.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[2];
                    break;
                case BaseClass.ClassType.Tank:
                    GetComponent<SpriteRenderer>().sprite = playerSprites[2];
                    break;
                default:
                    break;
            }

        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().AddForce((Vector2.left + Vector2.up) * (CurrentClass.MovementSpeed * diagonalNerf));
            switch (CurrentClass.TheClassType)
            {
                case BaseClass.ClassType.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[2];
                    break;
                case BaseClass.ClassType.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[2];
                    break;
                case BaseClass.ClassType.Tank:
                    GetComponent<SpriteRenderer>().sprite = playerSprites[2];
                    break;
                default:
                    break;
            }

        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().AddForce((Vector2.left + Vector2.down) * (CurrentClass.MovementSpeed * diagonalNerf));
            switch (CurrentClass.TheClassType)
            {
                case BaseClass.ClassType.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[2];
                    break;
                case BaseClass.ClassType.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[2];
                    break;
                case BaseClass.ClassType.Tank:
                    GetComponent<SpriteRenderer>().sprite = playerSprites[2];
                    break;
                default:
                    break;
            }

        }

        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * CurrentClass.MovementSpeed);
            switch (CurrentClass.TheClassType)
            {
                case BaseClass.ClassType.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[1];
                    break;
                case BaseClass.ClassType.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[1];
                    break;
                case BaseClass.ClassType.Tank:
                    GetComponent<SpriteRenderer>().sprite = playerSprites[1];
                    break;
                default:
                    break;
            }

        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().AddForce((Vector2.right + Vector2.up) * (CurrentClass.MovementSpeed * diagonalNerf));
            switch (CurrentClass.TheClassType)
            {
                case BaseClass.ClassType.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[1];
                    break;
                case BaseClass.ClassType.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[1];
                    break;
                case BaseClass.ClassType.Tank:
                    GetComponent<SpriteRenderer>().sprite = playerSprites[1];
                    break;
                default:
                    break;
            }

        }

        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().AddForce((Vector2.right + Vector2.down) * (CurrentClass.MovementSpeed * diagonalNerf));
            switch (CurrentClass.TheClassType)
            {
                case BaseClass.ClassType.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[1];
                    break;
                case BaseClass.ClassType.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[1];
                    break;
                case BaseClass.ClassType.Tank:
                    GetComponent<SpriteRenderer>().sprite = playerSprites[1];
                    break;
                default:
                    break;
            }

        }


        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * CurrentClass.MovementSpeed);
            switch (CurrentClass.TheClassType)
            {
                case BaseClass.ClassType.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[3];
                    break;
                case BaseClass.ClassType.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[3];
                    break;
                case BaseClass.ClassType.Tank:
                    GetComponent<SpriteRenderer>().sprite = playerSprites[3];
                    break;
                default:
                    break;
            }

        }

        if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.down * CurrentClass.MovementSpeed);
            switch (CurrentClass.TheClassType)
            {
                case BaseClass.ClassType.Hunter:
                    GetComponent<SpriteRenderer>().sprite = playerSprites3[0];
                    break;
                case BaseClass.ClassType.Mage:
                    GetComponent<SpriteRenderer>().sprite = playerSprites2[0];
                    break;
                case BaseClass.ClassType.Tank:
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

    private void TurnBasedMovement()
    {

    }

    private void EndTurn()
    {

    }

    private void ReplenishPoints()
    {

    }

    private void UseAbility()
    {
        switch (CurrentClass.TheClassType)
        {
            case BaseClass.ClassType.Hunter:
                break;
            case BaseClass.ClassType.Mage:
                Vector3 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                tempPos.z = 0.1f;
                GameObject fireExplosionPrefabTemp = Instantiate(fireExplosionPrefab, tempPos, Quaternion.identity) as GameObject;
                fireExplosionPrefabTemp.GetComponent<FireExplosion>().UpdateAction(CurrentClass.Intellect);
                break;
            case BaseClass.ClassType.Tank:
                break;
            default:
                break;
        }
        
    }

    private void UpdateWeaponPosAndDir()
    {

    }

    public override void OnDeath()
    {
        if (CurrentClass.CurrentHealth <= 0)
        {

        }
        base.OnDeath();
    }
}
