using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;

public class CameraRotater : MonoBehaviour
{
    
    [Header("Move Duration")]
    [Range(0f, 0.5f)]
    public float moveDuration;   
    public bool ismoving = false;
    public bool enableOldStyle = false;


    private Player player;
    float r;
    private float targetangle;
    private float initialangle;
    [Header("New Style Rotation")]
    public int targetint = 0;
    public int currentint = 0;
    public int maxCommandChain = 3;
    public Transform[] CameraPositions; 
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        targetangle = transform.rotation.eulerAngles.y;
        initialangle = transform.rotation.eulerAngles.y;
        currentint = 0;
        targetint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (enableOldStyle)
        {
            OldRotate();
        }
        else
        {
            NewRotate();
        }


    }
    

    void OldRotate()
    {
        if (player.GetButtonDown("LeftArrow") && !ismoving)
        {
            //move 90 degrees to the left
            Debug.Log("LeftRot");
            ismoving = true;
            targetangle = targetangle + 90;
            initialangle = transform.rotation.eulerAngles.y;

        }
        if (player.GetButtonDown("RightArrow") && !ismoving)
        {
            //move 90 degrees to the left
            Debug.Log("RightRot");
            ismoving = true;
            targetangle = targetangle - 90;
            initialangle = transform.rotation.eulerAngles.y;

        }
        if (ismoving)
        {
            float Angle = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, targetangle, ref r, moveDuration);
            transform.rotation = Quaternion.Euler(0, Angle, 0);
        }

        // check if angle is reached
        if (Quaternion.Angle(transform.rotation, Quaternion.Euler(0, targetangle, 0)) <= 0.01f)
        {
            transform.rotation = Quaternion.Euler(0, targetangle, 0);
            ismoving = false;
        }
    }
    void NewRotate()
    {
        
        
            if (player.GetButtonDown("LeftArrow"))
            {
                //add 1 every time we press left
                if (targetint == CameraPositions.Length)
                {
                    targetint = 0;
                    //start Coroutine
                }
                else
                {
                    targetint += 1;
                    //start Coroutine
                }

            }
            if (player.GetButtonDown("RightArrow"))
            {
                //subtract 1 every time we press right
                if (targetint == 0)
                {
                    targetint = CameraPositions.Length;
                    //start Coroutine
                }
                else
                {
                    targetint -= 1;
                    //start Coroutine
                }

            }
        
        
        
        

       

    }
    IEnumerator LerpToNext(Vector3 targetPos, Quaternion targetRot, float moveTime)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        Quaternion startValue = transform.rotation;

        while (time < moveTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPos, time / moveTime);
            transform.rotation = Quaternion.Lerp(startValue, targetRot, time / moveTime);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        transform.rotation = targetRot;

        //start next coroutine
    }
    void MoveCam()
    {
        if (ismoving)
        {
            float Angle = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, targetangle, ref r, moveDuration);
            transform.rotation = Quaternion.Euler(0, Angle, 0);
            
        }

        // check if angle is reached
        if (Quaternion.Angle(transform.rotation, Quaternion.Euler(0, targetangle, 0)) <= 0.01f)
        {
            transform.rotation = Quaternion.Euler(0, targetangle, 0);
            ismoving = false;
        }
    }
}
