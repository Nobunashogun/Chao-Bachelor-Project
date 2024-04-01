using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSensorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Manager_ClawMovement manager;
    private void OnTriggerEnter(Collider other)
    {
        manager.isDroppingForCatch = false;
        if (other.gameObject.CompareTag("DropItem"))
        {
            //manager.isDroppingItem = true;
            StartCoroutine(manager.DropBall());
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            manager.isHoldingItem = true;
        }
        else
        {
            manager.isHoldingItem = false;
        }
    }
    
}
