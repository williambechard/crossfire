using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableY : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.attachedRigidbody.gameObject.CompareTag("Target"))
        {
            
            // Set the Y constraint on the Rigidbody component
            other.attachedRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ|
                                                  RigidbodyConstraints.FreezePositionY;
        
        }
    }
}
