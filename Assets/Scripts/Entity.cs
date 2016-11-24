using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

    private BaseClass currentClass;

    public BaseClass CurrentClass
    {
        get { return currentClass; }
        set { currentClass = value; }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void SetClass(string className)
    {
        switch (className)
        {
            case "Tank":
                currentClass = new Tank();
                break;
            case "Mage":
                currentClass = new Mage();
                break;
            case "Hunter":
                currentClass = new Hunter();
                break;
            default:
                break;
        }
    }

    public virtual void OnDeath()
    {

    }
}
