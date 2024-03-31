using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSensorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Manager_ClawMovement manager;
    private void OnTriggerEnter(Collider other)
    {
        manager.isDropping = false;
        
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
