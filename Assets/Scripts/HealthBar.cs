using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [SerializeField]
    private List<Sprite> spriteList;

    private float savedEnemyHealth;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CheckHealth(float currentEnemyHealth)
    {
        if(currentEnemyHealth != savedEnemyHealth)
        {
            savedEnemyHealth = currentEnemyHealth;
            UpdateHealthBar();
        }


    }

    private void UpdateHealthBar()
    {
        float healthPercentage = (savedEnemyHealth / 180) * 100;
        int ceiledHealthPercentage = (Mathf.CeilToInt(healthPercentage / 10) * 10);

        switch (ceiledHealthPercentage)
        {
            case 10:
                GetComponent<SpriteRenderer>().sprite = spriteList[0];
                break;
            case 20:
                GetComponent<SpriteRenderer>().sprite = spriteList[1];
                break;
            case 30:
                GetComponent<SpriteRenderer>().sprite = spriteList[2];
                break;
            case 40:
                GetComponent<SpriteRenderer>().sprite = spriteList[3];
                break;
            case 50:
                GetComponent<SpriteRenderer>().sprite = spriteList[4];
                break;
            case 60:
                GetComponent<SpriteRenderer>().sprite = spriteList[5];
                break;
            case 70:
                GetComponent<SpriteRenderer>().sprite = spriteList[6];
                break;
            case 80:
                GetComponent<SpriteRenderer>().sprite = spriteList[7];
                break;
            case 90:
                GetComponent<SpriteRenderer>().sprite = spriteList[8];
                break;
            case 100:
                GetComponent<SpriteRenderer>().sprite = spriteList[9];
                break;

        }

    }
}
