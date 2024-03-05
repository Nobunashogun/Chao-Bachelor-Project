using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject UFO;
    Vector3 distance;
    void Start()
    {
        distance = transform.position - UFO.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = UFO.transform.position + distance;
    }
}
