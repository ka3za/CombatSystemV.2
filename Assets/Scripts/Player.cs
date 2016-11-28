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

    // Use this for initialization
    void Start () {
        dropDown.onValueChanged.AddListener(delegate
        {
            dropDownValueChangedHandler(dropDown);
        });

        CurrentClass = new Tank();
        UpdateStats();
	}
	
	// Update is called once per frame
	void Update () {
        MenuKeyHandling(); 

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
