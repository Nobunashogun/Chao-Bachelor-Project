using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetectorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Manager_ClawMovement manager;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            manager.isHoldingItem = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            manager.isHoldingItem = false;
        }
    }
}
