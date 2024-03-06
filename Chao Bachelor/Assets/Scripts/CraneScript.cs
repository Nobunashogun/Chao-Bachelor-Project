using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class CraneScript : MonoBehaviour
{

    [Header("Arms")]
    public GameObject arm1;
    public GameObject arm2;
    public GameObject arm3;

    [Header("Movement")]
    public float downSpeed;
    public float rotationSpeed;
    public float rotationAmount;
    public float armSpeed;
    public float MaxArmSpeed;
    public float smoothDownSpeed;
    public float returnSpeed;
    public float raycastDistance;
    //----------------------------------------------------------------------------
    private bool rotateArms = false;
    private bool extractDown=false;
    //private bool extractUp = false;
    private bool canSteer = true;
    private Rigidbody rb;
    private Player player;
    //----------------------------------------------------------------------------
    private float startRot1;
    private float startRot2;
    private float startRot3;
    private float r1;
    private float r2;
    private float r3;
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        rb = GetComponent<Rigidbody>();
        startRot1 = arm1.transform.localRotation.eulerAngles.z;
        startRot2 = arm2.transform.localRotation.eulerAngles.z;
        startRot3 = arm3.transform.localRotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        MoveUfo();
        if(extractDown)
        {
            DownAndRaycast();
        }
        if (rotateArms)
        {
            RotateArms();
        }
        
    }
    void DownAndRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000f))
        {
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);
            if(hit.distance >= raycastDistance)
            {
                rb.velocity = Vector3.down*downSpeed*Time.fixedDeltaTime;
            }
            else
            {
                rb.velocity = Vector3.zero;
                rotateArms = true;
                extractDown = false;
            }
        }
    }
    void MoveUfo()
    {
        if (player.GetButtonDown("Confirm")&&canSteer)
        {
            extractDown = true;
            canSteer = false;
            
        }
        if (canSteer)
        {
            var camera = Camera.main;

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
            var desiredMoveDirection = forward * player.GetAxisRaw("Vertical") + right * player.GetAxisRaw("Horizontal");
            desiredMoveDirection.Normalize();
            //transform.position += desiredMoveDirection*armSpeed*Time.deltaTime;
            if(rb.velocity.magnitude <= MaxArmSpeed)
            {
                rb.velocity += desiredMoveDirection * armSpeed * Time.fixedDeltaTime;
            }
            
            
            if(rb.velocity.magnitude >= 0)
            {
                    rb.velocity -= rb.velocity * smoothDownSpeed;
            }
                
            
            
        }
    }

    void RotateArms()
    {
        
        float Angle1 = Mathf.SmoothDampAngle(arm1.transform.rotation.eulerAngles.z, startRot1+rotationAmount, ref r1, rotationSpeed);
        float Angle2 = Mathf.SmoothDampAngle(arm2.transform.rotation.eulerAngles.z, startRot2+rotationAmount, ref r2, rotationSpeed);
        float Angle3 = Mathf.SmoothDampAngle(arm3.transform.rotation.eulerAngles.z, startRot3+rotationAmount, ref r3, rotationSpeed);
        arm1.transform.rotation = Quaternion.Euler(arm1.transform.rotation.eulerAngles.x, arm1.transform.rotation.eulerAngles.y, Angle1);
        arm2.transform.rotation = Quaternion.Euler(arm2.transform.rotation.eulerAngles.x, arm2.transform.rotation.eulerAngles.y, Angle2);
        arm3.transform.rotation = Quaternion.Euler(arm3.transform.rotation.eulerAngles.x, arm3.transform.rotation.eulerAngles.y, Angle3);

        
        

        
    }
}
