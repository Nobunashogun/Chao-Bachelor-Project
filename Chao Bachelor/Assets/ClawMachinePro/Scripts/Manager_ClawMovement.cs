using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using System.Diagnostics;

public class Manager_ClawMovement : MonoBehaviour {

    [Header("Player Settings")]
    public bool freePlay = true;
    public int playerCoins = 10;

    

    
    

    [Space(5f)]

    [Header("Claw Settings")]

    // Our object that we move which, in turn, moves the claw and rope
    public Transform clawHolder;
    public Transform rayCastTransform;
    // X and Z movement speed
    public float movementSpeed = 1.0f; 

    // Y drop and raise speed
    public float dropSpeed = 1.0f;
    public float raiseSpeed = 1.0f;

    public float clawOpenSpeed = 1.0f;
    public float clawCloseSpeed = 1.0f;

    public float clawBottomWaitTime = 1.0f;
    // min distance to object down
    public float minDistanceToStop = 0.1f;
    public bool isDroppingForCatch = false;
    public bool isHoldingItem = false;
    public bool isDroppingItem = false;



    // This is false when we are droping / rasing the claw.
    public bool canMove = true;

    [HideInInspector]
    // This will stop any downward Y movement. Typically called from outside of this class. 
    // This case it is used from WallSensor_ClawMachine to tell the claw to stop moving since it hit a wall.
    public bool stopMovement = false;

    // The vertical limit of when our claw will stop moving down
    public float LimitY = 2.074f;

    // If enabled, the claw will automatically go back to the home / inital start position
    [Tooltip("If enabled, the claw will automatically go back to the home / inital start position")]
    public bool shouldReturnHomeAutomatically = false;

    // Position Variables
    [HideInInspector]
    public Vector3 clawHomePosition;

    [HideInInspector]
    public Vector3 clawDropFromPosition;

    // Animation Variables
    public Animation clawHeadAnimation;

    [Header("Claw Movement Boundary Limits")]

    // Used to add a buffer between the boundary and the center of the claw head (so we don't clip)
    public float clawHeadSizeX = 0.30f;
    public float clawHeadSizeZ = 0.13f;

    // Movement boundaries

    private float raydistance;
    

    

    [HideInInspector]
    public bool isDroppingBall = false;


    

    [Header("Misc Settings")]
    public PrizeCatcherDetector_ClawMachine prizeCatcherDetector;


    private Player player;
	// Use this for initialization
	void Start () {

       player = ReInput.players.GetPlayer(0);
        // Setup our inital positions
        clawHomePosition = clawHolder.transform.position;
        

        // Setup our boundaries
        
        canMove = true;
    }
	
	// Update is called once per frame
	void Update () {

        
	}

    void FixedUpdate()
    {
        // If movement is allowed
        if (canMove)
        {          

            // Press P key to drop the claw
            if (player.GetButtonDown("Confirm"))
            {
                dropClawButtonInput();
            }
            
            
            // Normal inputs below...
            if (player.GetAxisRaw("Vertical") > 0.05f)
            {
                clawMoveUp();
                //Debug.Log("Up Arrow Pressed");
            }

            if (player.GetAxisRaw("Vertical")< -0.05)
            {
                clawMoveDown();
            }

            if (player.GetAxisRaw("Horizontal")< -0.05)
            {
                clawMoveLeft();
            }

            if (player.GetAxisRaw("Horizontal") > 0.05)
            {
                clawMoveRight();
            }
            
            
        }
    }


    private void dropClawButtonInput()
    {
        // Make sure we're NOT above the prize catcher, we need to do a release for that, NOT a drop
        if (isHoldingItem)
        {
            StartCoroutine(DropBall());
        }
        else
        {
            clawDropFromPosition = clawHolder.transform.position;
            isDroppingForCatch = true;
            StartCoroutine(NewDropClaw());
            canMove = false;
        }


        // get current position of claw
        
            
            
        
    }

    public void openClawButtonInput()
    {
        // If we're not actively dropping the ball - This prevents from trying to drop multiple times
        if (!isDroppingBall)
        {
            // Drop the ball
            StartCoroutine(DropBall());
            OpenClaw();
            
        }
    }

    private void clawMoveUp()
    {
        //filler to test movement
        //clawHolder.Translate(0f, 0f, movementSpeed * 1 * Time.deltaTime);
        // + Z direction
        var camera = Camera.main;
        Ray ray = new Ray(clawHolder.transform.position, camera.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            if(hit.distance> 0.1)
            {
                //camera forward and right vectors:
                var forward = camera.transform.forward;
                var right = camera.transform.right;

                //steering.Normalize();
                //project forward and right vectors on the horizontal plane (y = 0)
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();


                //this is the direction in the world space we want to move:
                var desiredMoveDirection = forward * 1 + right * 0;
                desiredMoveDirection.Normalize();
                desiredMoveDirection *= -1;
                clawHolder.Translate(desiredMoveDirection * movementSpeed * Time.deltaTime);

                // Move our claw

                //Debug.Log("Claw Move Up Camera Correction");
            }
            else
            {
                //Debug.Log("collide with wall");
            }
        }
        
    }

    private void clawMoveDown()
    {
        //filler to test movement
        //clawHolder.Translate(0f, 0f, movementSpeed * -1 * Time.deltaTime);
        // - Z direction
        var camera = Camera.main;
        Ray ray = new Ray(clawHolder.transform.position, camera.transform.forward*-1);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance > 0.1)
            {
                //camera forward and right vectors:
                var forward = camera.transform.forward;
                var right = camera.transform.right;

                //steering.Normalize();
                //project forward and right vectors on the horizontal plane (y = 0)
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();


                //this is the direction in the world space we want to move:
                var desiredMoveDirection = forward * -1 + right * 0;
                desiredMoveDirection.Normalize();
                desiredMoveDirection *= -1;
                clawHolder.Translate(desiredMoveDirection * movementSpeed * Time.deltaTime);
                // Move our claw
                //clawHolder.Translate(0f, 0f, movementSpeed * -1 * Time.deltaTime);
            }
        }   
    }

    private void clawMoveLeft()
    {
        //filler to test movement
        //clawHolder.Translate(movementSpeed * -1 * Time.deltaTime, 0f, 0f);
        // + X direction
        var camera = Camera.main;
        Ray ray = new Ray(clawHolder.transform.position, camera.transform.right * -1);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance > 0.1)
            {
                //camera forward and right vectors:
                var forward = camera.transform.forward;
                var right = camera.transform.right;

                //steering.Normalize();
                //project forward and right vectors on the horizontal plane (y = 0)
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();


                //this is the direction in the world space we want to move:
                var desiredMoveDirection = forward * 0 + right * -1;
                desiredMoveDirection.Normalize();
                desiredMoveDirection *= -1;
                clawHolder.Translate(desiredMoveDirection * movementSpeed * Time.deltaTime);

                // Move our claw
                //clawHolder.Translate(movementSpeed * -1 * Time.deltaTime, 0f, 0f);
            }
        }
                
    }

    private void clawMoveRight()
    {
        //filler to test movement
        //clawHolder.Translate(movementSpeed * 1 * Time.deltaTime, 0f, 0f);
        var camera = Camera.main;
        Ray ray = new Ray(clawHolder.transform.position, camera.transform.right);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance > 0.1)
            {
                //camera forward and right vectors:
                var forward = camera.transform.forward;
                var right = camera.transform.right;

                //steering.Normalize();
                //project forward and right vectors on the horizontal plane (y = 0)
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();


                //this is the direction in the world space we want to move:
                var desiredMoveDirection = forward * 0 + right * 1;
                desiredMoveDirection.Normalize();
                desiredMoveDirection *= -1;
                clawHolder.Translate(desiredMoveDirection * movementSpeed * Time.deltaTime);
            }
        }               
    }

    
    IEnumerator NewDropClaw()
    {
        OpenClaw();
        while (isDroppingForCatch)
        {
            Ray ray = new Ray(rayCastTransform.transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                
                //print(hit.transform.name + " "+ hit.distance);
                if(hit.distance > minDistanceToStop)
                {
                    clawHolder.Translate(0f, dropSpeed * -1 * Time.deltaTime, 0f);
                }
                else
                {
                    isDroppingForCatch = false;
                }
            }
            
            yield return null;
        }
        CloseClaw();
        yield return new WaitForSeconds(clawBottomWaitTime);

        while (!isDroppingForCatch)
        {
            
            if(clawHolder.transform.position.y < clawDropFromPosition.y)
            {
                clawHolder.Translate(0f, raiseSpeed * 1 * Time.deltaTime, 0f);
            }
            else
            {
                canMove = true;
                yield break;
            }
            

            yield return null;
        }
        

        

    }
    
    /// <summary>
    /// Used to drop the claw from a position. 
    /// </summary>
    /// <returns></returns>
    IEnumerator dropClaw()
    {
        // Save our drop position
        clawDropFromPosition = clawHomePosition;

        // Play opening animation
        OpenClaw();


        Ray ray = new Ray(clawHolder.transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            raydistance = hit.distance;
        }
        while (raydistance > minDistanceToStop)//while claw is further away than min distance and claw is open)
        {
                //if movement is stopped, breakout
                
                if (stopMovement)
                {
                    break;
                }

                clawHolder.Translate(0f, dropSpeed * -1 * Time.deltaTime, 0f);
                ray = new Ray(clawHolder.transform.position, Vector3.down);
                if (Physics.Raycast(ray, out hit))
                {
                    raydistance = hit.distance;
                }
            //Debug.Log(hit.transform.name + hit.distance);
            yield return null;
        }
        
        
        
        // Wait a few
        //yield return new WaitForSeconds(1.0f);

        // If movement was stopped
        if (stopMovement)
        {
            // Go right back up to where we dropped from
            while (clawHolder.transform.position.y <= clawDropFromPosition.y)
            { 
                // Move
                clawHolder.Translate(0f, dropSpeed * 1 * Time.deltaTime, 0f);

                yield return null;
            }

            yield return new WaitForSeconds(0.15f);

            // Close the claw
            CloseClaw();
        }
        else
        {
            /*
            // Implement some level of failure of closing the claw tight enough. Tricky!
            if (Random.Range(1, 10) <= failRate)
            {
                // Fire off the coroutine
                StartCoroutine(WeakClaws());
            }
            else
            {
                // Close claw head
                CloseClaw();
            }
            */
            //yield return new WaitForSeconds(1.0f);

            // First go back up
            while (clawHolder.transform.position.y <= clawDropFromPosition.y)
            {
                clawHolder.Translate(0f, dropSpeed * 1 * Time.deltaTime, 0f);

                yield return null;
            }

            if (shouldReturnHomeAutomatically)
            {
                yield return new WaitForSeconds(0.5f);

                // Return home now
                float startTime = Time.time;
                float journeyLength = Vector3.Distance(clawHolder.transform.position, clawHomePosition);

                while (Vector3.Distance(clawHolder.transform.position, clawHomePosition) > 0.05f)
                {
                    // Distance moved = time * speed.
                    float distCovered = (Time.time - startTime) * 0.03f;

                    // Fraction of journey completed = current distance divided by total distance.
                    float fracJourney = distCovered / journeyLength;

                    // Let's lerp the position closer to the home
                    clawHolder.transform.position = Vector3.Lerp(clawHolder.transform.position, clawHomePosition, fracJourney);                   
                    yield return null;
                }

                // Reset to exact position
                clawHolder.transform.position = clawHomePosition;

                // Play opening animation
                OpenClaw();

                yield return new WaitForSeconds(1.10f);

                CloseClaw();
            }
        }

        // We can move now
        canMove = true;

        // Allow movement again if stopped from collding with inside wall
        stopMovement = false;

        yield return null;

    }

    /// <summary>
    /// This is used to return the claw back to the STARTING / HOME position.
    /// The home position is where the claw STARTS when the game begins. Code found in Start() will set this location.
    /// </summary>
    /// <returns></returns>
    IEnumerator returnClawToHomePosition()
    {
        // First go back up
        while(clawHolder.transform.position.y < clawDropFromPosition.y)
        {
            clawHolder.Translate(0f, dropSpeed * 1 * Time.deltaTime, 0f);

            yield return null;
        }

        yield return null;
    }

    /// <summary>
    /// Open the claw, normally, using animations.
    /// </summary>
    private void OpenClaw()
    {
        // Play opening animation
        clawHeadAnimation["Claw_Open_New"].speed = clawOpenSpeed;
        clawHeadAnimation["Claw_Open_New"].time = 0f;
        clawHeadAnimation.CrossFade("Claw_Open_New");
    }

    /// <summary>
    /// Using animations, let's close the claws.
    /// </summary>
    private void CloseClaw()
    {
        clawHeadAnimation["Claw_Open_New"].speed = -clawCloseSpeed;
        clawHeadAnimation["Claw_Open_New"].time = clawHeadAnimation["Claw_Open_New"].length;
        clawHeadAnimation.CrossFade("Claw_Open_New");
    }

    /// <summary>
    /// This IEnumerator is used to create a "failed" state, which opens the claws only so far.
    /// </summary>
    /// <returns></returns>
    IEnumerator WeakClaws()
    {
        clawHeadAnimation["Claw_Open_Weak"].speed = -1.5f;
        clawHeadAnimation["Claw_Open_Weak"].time = clawHeadAnimation["Claw_Open_Weak"].length;
        clawHeadAnimation.CrossFade("Claw_Open_Weak");

        yield return new WaitForSeconds(Random.Range(2.15f, 2.85f));

        clawHeadAnimation["Claw_Close_From_Weak"].speed = -1.5f;
        clawHeadAnimation["Claw_Close_From_Weak"].time = clawHeadAnimation["Claw_Close_From_Weak"].length;
        clawHeadAnimation.CrossFade("Claw_Close_From_Weak");

    }

    /// <summary>
    /// Used to drop the ball
    /// </summary>
    /// <returns></returns>
    public IEnumerator DropBall()
    {
        // Flag we are dropping our balls
        isDroppingBall = true;

        OpenClaw();

        yield return new WaitForSeconds(0.55f);

        CloseClaw();

        // Flag we can drop a ball again
        isHoldingItem = false;
        isDroppingBall = false;
        canMove = true;
    }


    /// <summary>
    /// This is used to close the UI popup and give the player more coins.
    /// You can have your own IAP code here.
    /// </summary>
    

   

}
