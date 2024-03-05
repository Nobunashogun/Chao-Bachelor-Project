using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CameraRotater : MonoBehaviour
{
    
    [Header("Speed")]
    [Range(0f, 2f)]
    public float moveDuration;   
    private Player player;
    public bool ismoving = false;
    public  float targetangle;
    public float initialangle;
    float r;
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        targetangle = transform.rotation.eulerAngles.y;
        initialangle = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("LeftArrow")&&!ismoving)
        {
            //move 90 degrees to the left
            Debug.Log("LeftRot");
            ismoving = true;
            targetangle = targetangle+90;
            initialangle = transform.rotation.eulerAngles.y;
            
        }
        if (player.GetButtonDown("RightArrow")&&!ismoving)
        {
            //move 90 degrees to the left
            Debug.Log("RightRot");
            ismoving = true;
            targetangle = targetangle-90;
            initialangle = transform.rotation.eulerAngles.y;
            
        }
        if(ismoving)
        {
            float Angle = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, targetangle, ref r, moveDuration);
            transform.rotation = Quaternion.Euler(0, Angle, 0);            
        }

        // check if angle is reached
        if (Quaternion.Angle(transform.rotation, Quaternion.Euler(0, targetangle,0)) <= 0.01f)
        {
            transform.rotation = Quaternion.Euler(0, targetangle, 0);
            ismoving = false;
        }



    }
    
    
}
