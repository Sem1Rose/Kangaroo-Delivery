using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Transform crate 
    {
        get
        {
            return transform.GetChild(1) != null? transform.GetChild(1) : null;
        }
    }

    public void OnAccepting()
    {
        GetComponent<BoxCollider>().enabled = false;
    }
}
