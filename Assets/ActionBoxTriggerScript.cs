using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBoxTriggerScript : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D c)
    {
        gameObject.GetComponentInParent<Action>().EnterTrigger(c);
    }
}
