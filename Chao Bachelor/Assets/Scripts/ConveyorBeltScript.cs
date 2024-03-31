using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1.0f;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 pos = rb.position;
        rb.position += transform.forward*speed*Time.fixedDeltaTime*-1;
        rb.MovePosition(pos);
    }
}
