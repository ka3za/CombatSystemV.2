using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : Entity {

    private Camera playerCam;

    private Text healthText;

    private Text energySourceText;

    private Text moveCostText;

    private Text currentMovePositionUI;

    private GameObject turnManager;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
}
