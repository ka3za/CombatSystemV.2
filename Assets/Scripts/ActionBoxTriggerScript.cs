using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBoxTriggerScript : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Enemy")
        {
            gameObject.GetComponentInParent<Action>().EnterTrigger(c.gameObject); 
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.tag == "Enemy")
        {
            gameObject.GetComponentInParent<Action>().ExitTrigger(c.gameObject); 
        }
    }
}
