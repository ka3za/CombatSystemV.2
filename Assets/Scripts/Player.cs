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

    private Text healthText;

    private Text energySourceText;

    private Text moveCostText;

    private Text currentMovePositionUI;

    private GameObject turnManager;

    // Use this for initialization
    void Start () {
        CurrentClass = new Tank();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        RealTimeMovement();
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
        if (Input.GetKey(KeyCode.A))
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
        if (Input.GetKey(KeyCode.D))
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
        if (Input.GetKey(KeyCode.W))
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
        if (Input.GetKey(KeyCode.S))
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
