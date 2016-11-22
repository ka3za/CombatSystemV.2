using UnityEngine;
using System.Collections;

public class Weapon : Action {

    private Vector2 playerPos;

    private Vector2 mousePos;

    public Vector2 PlayerPos
    {
        set { playerPos = value; }
    }
    public Vector2 MousePos
    {
        set { mousePos = value; }
    }

   

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void Block()
    {

    }
}
