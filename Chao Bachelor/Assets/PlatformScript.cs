using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform posA;
    public Transform posB;
    public GameObject platform;
    public float speed = 1.5f;
    float dist;
    void Start()
    {
        StartCoroutine(MoveToA());
    }

    IEnumerator MoveToA()
    {
        dist = Vector3.Distance(platform.transform.position, posA.position);
        
        Vector3 dir = posA.position - platform.transform.position ;  
        while(dist > 0.5)
        {
            platform.transform.Translate(dir * speed * Time.deltaTime);
            dist = Vector3.Distance(platform.transform.position, posA.position);
            //print(dist);
            yield return null;
        }
        StartCoroutine(MoveToB());
        yield return null;
        
    }
    IEnumerator MoveToB()
    {
        dist = Vector3.Distance(platform.transform.position, posB.position);
        Vector3 dir = posB.position - platform.transform.position;
        while (dist > 0.5)
        {
            platform.transform.Translate(dir * speed * Time.deltaTime);
            dist = Vector3.Distance(platform.transform.position, posB.position);
            yield return null;
        }
        StartCoroutine(MoveToA());
        yield return null;

    }
}
