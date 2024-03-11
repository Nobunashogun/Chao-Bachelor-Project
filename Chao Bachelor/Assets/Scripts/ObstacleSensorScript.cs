using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSensorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Manager_ClawMovement wallSensor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DropItem"))
        {
            wallSensor.openClawButtonInput();
        }
    }
}
