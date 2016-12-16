using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items {

    public enum ItemType { Potion, Shoulder, Legs, Chest, Head, Hands, Feet}

    private bool stackAble;
    private int stackSize;
    private string itemDescription;

    public bool StackAble
    {
        get { return stackAble; }
        set { stackAble = value; }
    }

    public int StackSize
    {
        get { return stackSize; }
        set { stackSize = value; }
    }

    public string ItemDescription
    {
        get { return itemDescription; }
        set { itemDescription = value; }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
